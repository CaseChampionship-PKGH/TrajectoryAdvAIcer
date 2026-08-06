namespace TrajectoryAdvAIcer.Entities.Enums;

/// <summary>
/// Процесс обучения
/// </summary>
/// <remarks>
/// не bool потому что может быть в скоре "в процессе"
/// </remarks>
public enum CompletionStatus
{
    /// <summary>
    /// Пройден
    /// </summary>
    Passed,

    /// <summary>
    /// Не пройден
    /// </summary>
    NotPassed
}
