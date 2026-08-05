namespace TrajectoryAdvAIcer.Analysis.Contracts.Models.Aggregation;

/// <summary>
/// Подсчёт предпочтительной форма обучения
/// </summary>
public class LearningFormatDistribution
{
    /// <summary>
    /// Очное обучение в аудиториях МРЦ 
    /// </summary>
    public int FullTime { get; set; }

    /// <summary>
    /// Смешанное обучение: частично очно, частично дистанционно
    /// </summary>
    public int Mixed { get; set; }

    /// <summary>
    /// Обучение с применением дистанционных образовательных технологий на своем рабочем месте
    /// </summary>
    public int Remote { get; set; }
}
