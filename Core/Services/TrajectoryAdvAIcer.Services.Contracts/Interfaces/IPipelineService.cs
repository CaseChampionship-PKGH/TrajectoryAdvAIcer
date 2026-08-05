using TrajectoryAdvAIcer.Analysis.Contracts.Models;
using TrajectoryAdvAIcer.Services.Contracts.Models;

namespace TrajectoryAdvAIcer.Services.Contracts.Interfaces;

/// <summary>
/// Сервис - оркестратор: вызывает последовательно парсинг, валидацию, временной анализ, агентов сравнения, сбор статистики и генерацию отчёта.
/// </summary>
public interface IPipelineService
{
    /// <summary>
    /// Валидировать результаты теста
    /// </summary>
    Task<PipelineResult> RunAsync(PipelineContext context);

    /// <summary>
    /// Экспортировать статистику в Excel
    /// </summary>
    Task<byte[]> ExportStatsExcel(AnalysisResult analysisResult);

    /// <summary>
    /// Экспортировать отчёт в Word
    /// </summary>
    Task<byte[]> ExportReportWord(AnalysisResult analysisResult);
}
