namespace TrajectoryAdvAIcer.Entities.Models;

/// <summary>
/// Контейнер для ответов одного респондента
/// </summary>
public class SurveyResponse
{
    /// <summary>
    /// Идентификатор респондента
    /// </summary>
    public string RespondentId { get; set; } = string.Empty;

    /// <summary>
    /// Должность
    /// </summary>
    public string? Position { get; set; }

    /// <summary>
    /// Ответы на вопросы
    /// </summary>
    public List<SurveyAnswer> Answers { get; set; } = null!;
}
