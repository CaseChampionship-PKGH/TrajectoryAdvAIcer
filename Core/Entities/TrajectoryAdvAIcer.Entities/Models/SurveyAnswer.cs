namespace TrajectoryAdvAIcer.Entities.Models;

/// <summary>
/// Ответ одного респондента на один вопрос
/// </summary>
public class SurveyAnswer
{
    /// <summary>
    /// Сгенерированный ID респондента
    /// </summary>
    public string RespondentId { get; set; } = string.Empty;

    /// <summary>
    /// Cсылка на вопрос
    /// </summary>
    public string QuestionId { get; set; } = string.Empty;

    /// <summary>
    /// Ответ для открытых ответов
    /// </summary>
    public string? TextValue { get; set; } = string.Empty;

    /// <summary>
    /// Ответ для числовых оценок (1-10)
    /// </summary>
    public double? NumericValue { get; set; }

    /// <summary>
    /// Ответ для бинарных ("да"/"нет")
    /// </summary>
    public string? BinaryValue { get; set; } = string.Empty;
}
