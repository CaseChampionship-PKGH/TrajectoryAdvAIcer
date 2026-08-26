namespace TrajectoryAdvAIcer.Analysis.Contracts.Models.Trajectory;

/// <summary>
/// Сгенерированная тракетория обучения
/// </summary>
public class TrajectoryAnalysisResult
{
    /// <summary>
    /// Список рекомендуеммых курсов с обоснованиями
    /// </summary>
    public List<TrajectoryRecommendation> Trajectory { get; set; } = new();
}
