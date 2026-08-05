namespace TrajectoryAdvAIcer.Entities.Models;

/// <summary>
/// Полученная анкета
/// </summary>
public class Survey
{
    /// <summary>
    /// Распарсенная программа
    /// </summary>
    public required ProgramInfo ProgramInfo { get; set; }

    /// <summary>
    /// Справочник вопросов анкеты
    /// </summary>
    public required List<SurveyQuestion> Questions { get; set; }

    /// <summary>
    /// Ответы всех респондентов
    /// </summary>
    public required List<SurveyResponse> Responses { get; set; }
}
