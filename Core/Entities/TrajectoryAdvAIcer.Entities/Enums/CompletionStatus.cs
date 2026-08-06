namespace TrajectoryAdvAIcer.Entities.Enums;

/// <summary>
/// Процесс обучения
/// </summary>
public enum CompletionStatus
{
    /// <summary>
    /// Пройден
    /// </summary>
    Passed,

    /// <summary>
    /// Не пройден
    /// </summary>
    NotPassed,

    /// <summary>
    /// В процессе (на будущее)
    /// </summary>
    InProgress
}
