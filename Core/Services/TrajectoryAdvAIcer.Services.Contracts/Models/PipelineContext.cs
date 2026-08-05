using TrajectoryAdvAIcer.Analysis.Contracts.Enums;

namespace TrajectoryAdvAIcer.Services.Contracts.Models;

/// <summary>
/// Результат выполнения pipeline
/// </summary>
public record PipelineContext
{
    /// <summary>
    /// Файл с тестами
    /// </summary>
    public Stream UserAnswersStream { get; set; } = null!;

    /// <summary>
    /// Имя файла с тестом
    /// </summary>
    public string UserAnswersFileName { get; set; } = string.Empty;

    /// <summary>
    /// Метод аналиа
    /// </summary>
    public AnalysisMethod AnalysisMethod { get; set; }
}
