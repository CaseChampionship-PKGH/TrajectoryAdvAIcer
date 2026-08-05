namespace TrajectoryAdvAIcer.Analysis.Contracts.Models;

/// <summary>
/// Результат подсчёта числовых и бинарных данных анекеты
/// </summary>
public class SurveyStatistics
{
    /// <summary>
    /// Вопрос
    /// </summary>
    public List<QuestionStatistics> Questions { get; set; } = [];

    /// <summary>
    /// Количество респондентов
    /// </summary>
    public int TotalRespondents { get; set; }

    /// <summary>
    /// Общий средний балл по всем числовым вопросам
    /// </summary>
    public double? OverallNumericAverage { get; set; }
}
