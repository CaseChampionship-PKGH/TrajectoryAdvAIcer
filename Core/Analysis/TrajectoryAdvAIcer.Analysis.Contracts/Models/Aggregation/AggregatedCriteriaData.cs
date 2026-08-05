namespace TrajectoryAdvAIcer.Analysis.Contracts.Models.Aggregation;

/// <summary>
/// Данные по всем критериям
/// </summary>
public class AggregatedCriteriaData
{
    /// <summary>
    /// Данные для промптов по каждому из 5 критериев
    /// </summary>
    public List<CriterionPromptData> AllCriteriaData { get; set; } = [];

    /// <summary>
    /// Темы, предложенные к исключению (на основе вопроса "Какие темы можно исключить")
    /// </summary>
    public List<string> ExcludedTopics { get; set; } = [];

    /// <summary>
    /// Темы, предложенные к добавлению (на основе вопроса "Какими темами дополнить")
    /// </summary>
    public List<string> SuggestedTopics { get; set; } = [];

    /// <summary>
    /// Распределение предпочтений по формату обучения
    /// </summary>
    public LearningFormatDistribution FormatDistribution { get; set; } = null!;
}
