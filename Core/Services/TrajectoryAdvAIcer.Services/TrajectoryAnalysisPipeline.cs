using System.Runtime.CompilerServices;
using TrajectoryAdvAIcer.Agent.Contracts.Enums;
using TrajectoryAdvAIcer.Agent.Contracts.Interfaces;
using TrajectoryAdvAIcer.Analysis.Contracts.Enums;
using TrajectoryAdvAIcer.Analysis.Contracts.Interfaces;
using TrajectoryAdvAIcer.Analysis.Contracts.Models;
using TrajectoryAdvAIcer.Analysis.Contracts.Models.Aggregation;
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

    async Task<PipelineResult> IPipelineService.RunAsync(PipelineContext context, CancellationToken cancellationToken)
    {
        var (history, catalog, warnings) = await ParseAndValidateAsync(context, cancellationToken);

        var employeesToProcess = history.Employees;
        var results = new List<EmployeeTrajectoryAdvice>();

        foreach (var employee in employeesToProcess)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var advice = await ProcessSingleEmployeeAsync(employee, history, catalog, context.AnalysisMethod, cancellationToken);
            results.Add(advice);
        }

        return new PipelineResult
        {
            AnalysisResult = new AnalysisResult { TrajectoryAdvices = results },
            Errors = warnings
        };
    }

    async IAsyncEnumerable<PipelineEvent> IPipelineService.RunStreamAsync(
        PipelineContext context,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var (history, catalog, warnings) = await ParseAndValidateAsync(context, cancellationToken);

        yield return new PipelineEvent
        {
            Type = "errors",
            Errors = warnings
        };

        var employeesToProcess = history.Employees;

        foreach (var employee in employeesToProcess)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var advice = await ProcessSingleEmployeeAsync(employee, history, catalog, context.AnalysisMethod, cancellationToken);
            yield return new PipelineEvent
            {
                Type = "employee",
                EmployeeAdvice = advice
            };
        }

        yield return new PipelineEvent
        {
            Type = "complete"
        };
    }

    Task<byte[]> IPipelineService.ExportStatsExcel(AnalysisResult result)
    {
        var reportExporter = reportExporterFactory.GetReportExporter(ExportType.Excel);
        return Task.FromResult(reportExporter.Export(result));
    }

    private async Task<(LearningHistory History, CourseCatalog Catalog, List<string> Warnings)>
       ParseAndValidateAsync(PipelineContext context, CancellationToken cancellationToken)
    {
        var historyFormat = formatDetector.DetectFormat(context.LearningHistoryFileName, context.LearningHistoryStream);
        var historyParser = parserFactory.GetParser(historyFormat, ParsingTarget.LearningHistory);
        var parseHistoryResult = await historyParser.ParseAsync<LearningHistory>(context.LearningHistoryStream);

        var catalogFormat = formatDetector.DetectFormat(context.CourseCatalogFileName, context.CourseCatalogStream);
        var catalogParser = parserFactory.GetParser(catalogFormat, ParsingTarget.CourseCatalog);
        var parsedCatalogResult = await catalogParser.ParseAsync<CourseCatalog>(context.CourseCatalogStream);

        var validationResult = dataValidator.Validate(parseHistoryResult.Data, parsedCatalogResult.Data);

        var warnings = new List<string>();
        warnings.AddRange(parseHistoryResult.Warnings);
        warnings.AddRange(parsedCatalogResult.Warnings);
        warnings.AddRange(validationResult.Warnings);

        return (validationResult.ValidatedLearningHistory, parsedCatalogResult.Data, warnings);
    }

    private async Task<EmployeeTrajectoryAdvice> ProcessSingleEmployeeAsync(
        Employee employee,
        LearningHistory history,
        CourseCatalog catalog,
        AnalysisMethod analysisMethod,
        CancellationToken cancellationToken)
    {
        var popularityMap = collaborativeFilteringService.BuildPopularityMapForEmployee(history, employee.Id);
        var similarCount = history.Employees.Count(e =>
            e.Position == employee.Position && e.IOGV == employee.IOGV) - 1;

        var candidates = courseCandidateSelector.SelectCandidates(history, employee.Id);
        var recommendedIds = topCourseSelector.SelectTopCourses(candidates, similarCount);

        var recommendedCourses = recommendedIds
            .Select(id => catalog.Courses.FirstOrDefault(c => c.Id == id))
            .Where(c => c != null)
            .Select(c => new RecommendedCourse
            {
                Course = c!,
                Popularity = popularityMap.GetValueOrDefault(c!.Id, 0),
                TotalSimilarEmployees = similarCount
            })
            .ToList();

        var passedCourses = PassedCoursesResolver.ResolvePassedCourses(employee.Id, history.Records, catalog);

        var llmVariant = analysisMethod switch
        {
            AnalysisMethod.RussianAiAgent => LlmVariant.Russian,
            AnalysisMethod.ForeignAiAgent => LlmVariant.Foreign,
            AnalysisMethod.LocalAiAgent => LlmVariant.Local,
            _ => LlmVariant.Russian
        };

        var trajectory = await trajectoryAdvicerAnalysisAgent.GenerateTrajectoryForEmployeeAsync(
            employee, passedCourses, recommendedCourses, llmVariant, cancellationToken);

        return new EmployeeTrajectoryAdvice
        {
            Profile = employee,
            PassedCourseNames = passedCourses,
            Trajectory = trajectory.Trajectory
        };
    }
}
