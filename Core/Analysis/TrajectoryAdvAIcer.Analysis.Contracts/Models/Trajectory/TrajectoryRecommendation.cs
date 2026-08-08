namespace TrajectoryAdvAIcer.Analysis.Contracts.Models.Trajectory;

/// <summary>
/// Рекомендация для одного курса по тракетории обучения
/// </summary>
public class TrajectoryRecommendation
{
    /// <summary>
    /// Название курса
    /// </summary>
    public string CourseTitle { get; set; } = string.Empty;

    /// <summary>
    /// Обоснование
    /// </summary>
    public string Rationale { get; set; } = string.Empty;
}
