namespace TrajectoryAdvAIcer.Analysis.Contracts.Models.Aggregation;

/// <summary>
/// Данные анкеты по критерию
/// </summary>
public class CriterionPromptData
{
    /// <summary>
    /// "Полезность", "Практико-ориентированность", и т.д.
    /// </summary>
    public string CriterionName { get; set; } = string.Empty;

    /// <summary>
    /// Метрики из SurveyStatistics
    /// </summary>
    public QuestionStatistics Statistics { get; set; } = null!;

    /// <summary>
    /// Вопросы и ответы к ним
    /// </summary>
    public List<QuestionWithAnswers> Questions { get; set; } = [];
}
