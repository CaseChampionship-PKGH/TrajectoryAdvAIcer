using TrajectoryAdvAIcer.Analysis.Contracts.Models;

namespace TrajectoryAdvAIcer.Services.Contracts.Models;

/// <summary>
/// Результат выполнения pipeline
/// </summary>
public record PipelineResult
{
    /// <summary>
    /// Список возникших в процессе валидации ошибок
    /// </summary>
    public List<string> Errors { get; set; } = null!;

    /// <summary>
    /// Статистика
    /// </summary>
    public AnalysisResult AnalysisResult { get; set; } = null!;
}
