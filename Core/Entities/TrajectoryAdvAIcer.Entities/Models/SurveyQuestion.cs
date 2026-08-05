using TrajectoryAdvAIcer.Entities.Enums;

namespace TrajectoryAdvAIcer.Entities.Models;

/// <summary>
/// Вопрос анкеты
/// </summary>
public class SurveyQuestion
{
    /// <summary>
    /// Уникальный ID (хэш от QuestionText)
    /// </summary>
    public string QuestionId { get; set; } = string.Empty;

    /// <summary>
    /// Полный текст вопроса
    /// </summary>
    public string QuestionText { get; set; } = string.Empty;

    /// <summary>
    /// Тип вопроса
    /// </summary>
    public QuestionType Type { get; set; }
}
