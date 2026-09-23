using System.Text;
using System.Text.Json;
using AutoMapper;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using TrajectoryAdvAIcer.Analysis.Contracts.Enums;
using TrajectoryAdvAIcer.Analysis.Contracts.Models;
using TrajectoryAdvAIcer.Api.Models;
using TrajectoryAdvAIcer.Parsing.Contracts.Exceptions;
using TrajectoryAdvAIcer.Services.Contracts.Interfaces;
using TrajectoryAdvAIcer.Services.Contracts.Models;

namespace TrajectoryAdvAIcer.Api.Controllers;

/// <summary>
/// Управление анализом анкет
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AnalysisController : ControllerBase
{
    private readonly IPipelineService pipeline;
    private readonly IMapper mapper;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AnalysisController"/>
    /// </summary>
    public AnalysisController(IPipelineService pipeline, IMapper mapper)
    {
        this.pipeline = pipeline;
        this.mapper = mapper;
    }

    /// <summary>
    /// Запуск анализа и получение отчёта
    /// </summary>
    [HttpPost("run")]
    public async Task<IActionResult> RunAnalysis(
        IFormFile learningHistory,
        IFormFile courseCatalog,
        [FromQuery] AnalysisMethod analysisMethod = AnalysisMethod.RussianAiAgent)
    {
        if (learningHistory == null || learningHistory.Length == 0)
        {
            return BadRequest("Файл с историей обучений обязателен.");
        }

        if (courseCatalog == null || courseCatalog.Length == 0)
        {
            return BadRequest("Файл с реестром курсов обязателен.");
        }

        var context = await BuildContext(learningHistory, courseCatalog, analysisMethod);

        try
        {
            var result = await pipeline.RunAsync(context);
            var mappedResult = mapper.Map<AnalysisResultApiModel>(result.AnalysisResult);
            mappedResult.Errors = result.Errors;
            return Ok(mappedResult);
        }
        catch (ParsingException ex)
        {
            return UnprocessableEntity(new { error = ex.Message, details = ex.Errors });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Внутренняя ошибка сервера.", detail = ex.Message });
        }
    }

    /// <summary>
    /// Запуск анализа в виде потока и получение структурированного отчёта
    /// </summary>
    [HttpPost("run-stream")]
    public async Task RunAnalysisStream(
        IFormFile learningHistory,
        IFormFile courseCatalog,
        [FromQuery] AnalysisMethod analysisMethod = AnalysisMethod.ForeignAiAgent)
    {
        var context = await BuildContext(learningHistory, courseCatalog, analysisMethod);

        Response.ContentType = "application/x-ndjson";
        Response.Headers.Append("Cache-Control", "no-cache");
        Response.Headers.Append("X-Content-Type-Options", "nosniff");
        var compressionFeature = HttpContext.Features.Get<IHttpsCompressionFeature>();
        if (compressionFeature != null)
        {
            compressionFeature.Mode = HttpsCompressionMode.DoNotCompress;
        }
        await Response.Body.FlushAsync();
        await using var writer = new StreamWriter(Response.Body, Encoding.UTF8, leaveOpen: true);

        await foreach (var pipelineEvent in pipeline.RunStreamAsync(context,
            HttpContext.RequestAborted))
        {
            var json = JsonSerializer.Serialize(pipelineEvent);
            await writer.WriteLineAsync(json);
            await writer.FlushAsync();
        }
    }

    private static async Task<PipelineContext> BuildContext(IFormFile learningHistory,
        IFormFile courseCatalog,
        AnalysisMethod analysisMethod)
    {
        if (learningHistory == null || learningHistory.Length == 0)
        {
            throw new BadHttpRequestException("Файл с историей обучений обязателен.");
        }

        if (courseCatalog == null || courseCatalog.Length == 0)
        {
            throw new BadHttpRequestException("Файл с реестром курсов обязателен.");
        }

        using var historyStream = new MemoryStream();
        await learningHistory.CopyToAsync(historyStream);
        historyStream.Position = 0;

        using var catalogStream = new MemoryStream();
        await courseCatalog.CopyToAsync(catalogStream);
        catalogStream.Position = 0;

        return new PipelineContext
        {
            LearningHistoryStream = learningHistory.OpenReadStream(),
            LearningHistoryFileName = learningHistory.FileName,
            CourseCatalogStream = courseCatalog.OpenReadStream(),
            CourseCatalogFileName = courseCatalog.FileName,
            AnalysisMethod = analysisMethod
        };
    }

    /// <summary>
    /// Экспорт отчёта в Excel (.excel)
    /// </summary>
    [HttpPost("export/excel")]
    public async Task<IActionResult> ExportToExcel([FromBody] AnalysisResultApiModel analysisResultModel)
    {
        var domainResult = mapper.Map<AnalysisResult>(analysisResultModel);
        var fileBytes = await pipeline.ExportStatsExcel(domainResult);
        var fileName = $"Рекомендации_траекторий_обучения_{DateTime.Now:yyyy-MM-dd}.xlsx";
        HttpContext.Response.Headers["X-Filename"] = Uri.EscapeDataString(fileName);
        return File(fileBytes,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            fileName);
    }

    /// <summary>
    /// Проверка работоспособности сервиса
    /// </summary>
    [HttpGet("health")]
    public IActionResult Health() => Ok(new { status = "healthy", timestamp = DateTime.UtcNow });
}
