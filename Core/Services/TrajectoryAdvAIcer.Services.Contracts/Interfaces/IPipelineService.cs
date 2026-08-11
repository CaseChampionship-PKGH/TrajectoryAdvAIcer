using TrajectoryAdvAIcer.Analysis.Contracts.Models;
using TrajectoryAdvAIcer.Services.Contracts.Models;

namespace TrajectoryAdvAIcer.Services.Contracts.Interfaces;

/// <summary>
/// Сервис - оркестратор: вызывает последовательно парсинг, валидацию, временной анализ, агентов сравнения, сбор статистики и генерацию отчёта.
/// </summary>
public interface IPipelineService
{
    /// <summary>
    /// Запустить анализ траектории
    /// </summary>
    Task<PipelineResult> RunAsync(PipelineContext context, CancellationToken cancellationToken = default);

    /// <summary>
    /// Запустить анализ траектории поточно
    /// </summary>
    IAsyncEnumerable<PipelineEvent> RunStreamAsync(PipelineContext context, CancellationToken cancellationToken = default);

    /// <summary>
    /// Экспортировать отчёт в Excel
    /// </summary>
    Task<byte[]> ExportStatsExcel(AnalysisResult analysisResult);
}
