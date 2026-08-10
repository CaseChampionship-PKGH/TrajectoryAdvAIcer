using TrajectoryAdvAIcer.Agent.Contracts.Enums;
using TrajectoryAdvAIcer.Agent.Contracts.Interfaces;
using TrajectoryAdvAIcer.Analysis.Contracts.Enums;
using TrajectoryAdvAIcer.Analysis.Contracts.Interfaces;
using TrajectoryAdvAIcer.Analysis.Contracts.Models;
using TrajectoryAdvAIcer.Analysis.Contracts.Models.Aggregation;
using TrajectoryAdvAIcer.Entities.Enums;
using TrajectoryAdvAIcer.Entities.Models;
using TrajectoryAdvAIcer.Parsing.Contracts.Enums;
using TrajectoryAdvAIcer.Parsing.Contracts.Interfaces;
using TrajectoryAdvAIcer.Reporting.Contracts.Interfaces;
using TrajectoryAdvAIcer.Reporting.Contracts.Models;
using TrajectoryAdvAIcer.Services.Contracts.Interfaces;
using TrajectoryAdvAIcer.Services.Contracts.Models;
using TrajectoryAdvAIcer.Validation.Contracts.Interfaces;

namespace TrajectoryAdvAIcer.Services;

/// <summary>
/// <inheritdoc cref="IPipelineService"/>
/// </summary>
public class TrajectoryAnalysisPipeline : IPipelineService
{
    private readonly IFormatDetector formatDetector;
    private readonly IParserFactory parserFactory;
    private readonly IDataValidator dataValidator;
    private readonly ICollaborativeFilteringService collaborativeFilteringService;
    private readonly ICourseCandidateSelector courseCandidateSelector;
    private readonly ITopCourseSelector topCourseSelector;
    private readonly ITrajectoryAdvicerAgent trajectoryAdvicerAnalysisAgent;

    private readonly IReportExporterFactory reportExporterFactory;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="TrajectoryAnalysisPipeline"/>
    /// </summary>
    public TrajectoryAnalysisPipeline(IFormatDetector formatDetector,
        IParserFactory parserFactory,
        IDataValidator dataValidator,
        ICollaborativeFilteringService collaborativeFilteringService,
        ICourseCandidateSelector courseCandidateSelector,
        ITopCourseSelector topCourseSelector,
        ITrajectoryAdvicerAgent trajectoryAdvicerAnalysisAgent,
        IReportExporterFactory reportExporterFactory)
    {
        this.formatDetector = formatDetector;
        this.parserFactory = parserFactory;
        this.dataValidator = dataValidator;
        this.collaborativeFilteringService = collaborativeFilteringService;
        this.courseCandidateSelector = courseCandidateSelector;
        this.topCourseSelector = topCourseSelector;
        this.trajectoryAdvicerAnalysisAgent = trajectoryAdvicerAnalysisAgent;
        this.reportExporterFactory = reportExporterFactory;
    }

    async Task<PipelineResult> IPipelineService.RunAsync(PipelineContext context)
    {
        var learningHistortyFormat = formatDetector.DetectFormat(context.LearningHistoryFileName, context.LearningHistoryStream);
        var learningHistortyParser = parserFactory.GetParser(learningHistortyFormat, ParsingTarget.LearningHistory);

        var parsedLearningHistory = await learningHistortyParser.ParseAsync<LearningHistory>(context.LearningHistoryStream);

        var courseCatalogFormat = formatDetector.DetectFormat(context.CourseCatalogFileName, context.CourseCatalogStream);
        var courseCatalogParser = parserFactory.GetParser(courseCatalogFormat, ParsingTarget.CourseCatalog);

        var courseCatalog = await courseCatalogParser.ParseAsync<CourseCatalog>(context.CourseCatalogStream);

        var validationResult = dataValidator.Validate(parsedLearningHistory, courseCatalog);

        var history = validationResult.ValidatedLearningHistory;

        var results = new List<EmployeeTrajectoryAdvice>();

        foreach (var employee in history.Employees)
        {
            var popularityMap = collaborativeFilteringService.BuildPopularityMapForEmployee(history, employee.Id);
            var similarCount = history.Employees.Count(e => e.Position == employee.Position && e.IOGV == employee.IOGV) - 1;
            var candidates = courseCandidateSelector.SelectCandidates(history, employee.Id);
            var recommendedIds = topCourseSelector.SelectTopCourses(candidates, similarCount);

            var recommendedCourses = recommendedIds
                .Select(id => courseCatalog.Courses.FirstOrDefault(c => c.Id == id))
                .Where(c => c != null)
                .Select(c => new RecommendedCourse
                {
                    Course = c!,
                    Popularity = popularityMap.GetValueOrDefault(c!.Id, 0),
                    TotalSimilarEmployees = similarCount
                })
                .ToList();

            var passedCourseIds = history.Records
                .Where(r => r.EmployeeId == employee.Id && r.Status == CompletionStatus.Passed)
                .Select(r => r.CourseId)
                .ToHashSet();

            var passedCourses = courseCatalog.Courses
                .Where(c => passedCourseIds.Contains(c.Id))
                .Select(c => c.Title)
                .ToList();

            var llmVariant = context.AnalysisMethod switch
            {
                AnalysisMethod.RussianAiAgent => LlmVariant.Russian,
                AnalysisMethod.ForeignAiAgent => LlmVariant.Foreign,
                AnalysisMethod.LocalAiAgent => LlmVariant.Local,
                _ => LlmVariant.Russian,
            };

            var trajectory = await trajectoryAdvicerAnalysisAgent.GenerateTrajectoryForEmployeeAsync(employee,
                passedCourses,
                recommendedCourses,
                llmVariant);

            results.Add(new EmployeeTrajectoryAdvice
            {
                Profile = employee,
                PassedCourseNames = passedCourses,
                Trajectory = trajectory.Trajectory
            });
        }

        var result = new AnalysisResult()
        {
            TrajectoryAdvices = results
        };

        return new PipelineResult()
        {
            AnalysisResult = result,
            Errors = validationResult.Warnings.ToList(),
        };
    }

    async Task<byte[]> IPipelineService.ExportStatsExcel(AnalysisResult result)
    {
        var reportExporter = reportExporterFactory.GetReportExporter(ExportType.Excel);
        var excelBytes = reportExporter.Export(result);
        return excelBytes;
    }
}
