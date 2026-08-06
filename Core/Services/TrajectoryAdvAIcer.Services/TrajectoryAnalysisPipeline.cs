using TrajectoryAdvAIcer.Analysis.Contracts.Models;
using TrajectoryAdvAIcer.Entities.Models;
using TrajectoryAdvAIcer.Parsing.Contracts.Enums;
using TrajectoryAdvAIcer.Parsing.Contracts.Interfaces;
using TrajectoryAdvAIcer.Services.Contracts.Interfaces;
using TrajectoryAdvAIcer.Services.Contracts.Models;

namespace TrajectoryAdvAIcer.Services;

/// <summary>
/// <inheritdoc cref="IPipelineService"/>
/// </summary>
public class TrajectoryAnalysisPipeline : IPipelineService
{
    private readonly IFormatDetector formatDetector;
    private readonly IParserFactory parserFactory;
    //private readonly IDataValidator dataValidator;
    //private readonly IStatisticsCalculator statisticsCalculator;
    //private readonly ICriterionAggregator criterionAggregator;
    //private readonly ISurveyCriteriaAnalysisAgent surveyCriteriaAnalysisAgent;
    //private readonly IReportExporterFactory reportExporterFactory;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="TrajectoryAnalysisPipeline"/>
    /// </summary>
    public TrajectoryAnalysisPipeline(IFormatDetector formatDetector,
        IParserFactory parserFactory)
    //IDataValidator dataValidator,
    //IStatisticsCalculator statisticsCalculator,
    //ICriterionAggregator criterionAggregator,
    //ISurveyCriteriaAnalysisAgent surveyCriteriaAnalysisAgent,
    //IReportExporterFactory reportExporterFactory)
    {
        this.formatDetector = formatDetector;
        this.parserFactory = parserFactory;
        //this.dataValidator = dataValidator;
        //this.statisticsCalculator = statisticsCalculator;
        //this.criterionAggregator = criterionAggregator;
        //this.surveyCriteriaAnalysisAgent = surveyCriteriaAnalysisAgent;
        //this.reportExporterFactory = reportExporterFactory;
    }

    async Task<PipelineResult> IPipelineService.RunAsync(PipelineContext context)
    {
        var learningHistortyFormat = formatDetector.DetectFormat(context.LearningHistoryFileName, context.LearningHistoryStream);
        var learningHistortyParser = parserFactory.GetParser(learningHistortyFormat, ParsingTarget.LearningHistory);

        var learningHistory = await learningHistortyParser.ParseAsync<LearningHistory>(context.LearningHistoryStream);

        var courseCatalogFormat = formatDetector.DetectFormat(context.CourseCatalogFileName, context.CourseCatalogStream);
        var courseCatalogParser = parserFactory.GetParser(courseCatalogFormat, ParsingTarget.CourseCatalog);

        var courseCatalog = await courseCatalogParser.ParseAsync<CourseCatalog>(context.CourseCatalogStream);

        //var validationResult = dataValidator.Validate(surveyParseResult);

        //var statistics = statisticsCalculator.Calculate(validationResult.ValidatedResults);
        //var aggregationResult = criterionAggregator.Aggregate(validationResult.ValidatedResults, statistics);

        //var criterionAnalysisList = new List<CriterionAnalysis>();

        //foreach (var promptData in aggregationResult.AllCriteriaData)
        //{
        //    var note = await surveyCriteriaAnalysisAgent.AnalyzeAndGenerateCriterionNoteAsync(promptData, context.AnalysisMethod == AnalysisMethod.RussianAiAgent
        //            ? LlmVariant.Russian
        //            : LlmVariant.Foreign);

        //    criterionAnalysisList.Add(new CriterionAnalysis
        //    {
        //        CriterionData = promptData,
        //        Note = note
        //    });
        //}

        //var trajectory = await surveyCriteriaAnalysisAgent.AnalyzeTrajectoryAsync(aggregationResult,
        //    criterionAnalysisList.Select(x => x.Note ?? string.Empty).ToList(),
        //    context.AnalysisMethod == AnalysisMethod.RussianAiAgent
        //            ? LlmVariant.Russian
        //            : LlmVariant.Foreign);

        //var result = new AnalysisResult()
        //{
        //    ProgramInfo = surveyParseResult.ProgramInfo,
        //    AllCriteriaAnalysisData = criterionAnalysisList,
        //    FormatDistribution = aggregationResult.FormatDistribution,
        //    Trajectory = trajectory
        //};

        return new PipelineResult()
        {
            LearningHistory = learningHistory,
            CourseCatalog = courseCatalog,
            Errors = [],
            //Errors = validationResult.Warnings.ToList(),
        };
    }

    async Task<byte[]> IPipelineService.ExportStatsExcel(AnalysisResult result)
    {
        //var reportExporter = reportExporterFactory.GetReportExporter(ExportType.Excel);
        //var excelBytes = reportExporter.Export(result);
        //return excelBytes;
        return [];
    }
}
