using TrajectoryAdvAIcer.Entities.Models;

namespace TrajectoryAdvAIcer.Validation.Contracts.Models;

/// <summary>
/// Результат валидации
/// </summary>
public record ValidationResult
{
    /// <summary>
    /// Список найдённых ошибок
    /// </summary>
    public IEnumerable<string> Warnings { get; set; } = [];

    /// <summary>
    /// Успешно ли прошла валидация
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Валидные данные, готовые к анализу
    /// </summary>
    public LearningHistory ValidatedResults { get; set; } = null!;
}
