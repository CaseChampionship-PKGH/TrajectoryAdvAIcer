using TrajectoryAdvAIcer.Entities.Models;

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
    /// История обучения
    /// </summary>
    public LearningHistory LearningHistory { get; set; } = null!;

    /// <summary>
    /// Реестр курсов
    /// </summary>
    public CourseCatalog CourseCatalog { get; set; } = null!;
}
