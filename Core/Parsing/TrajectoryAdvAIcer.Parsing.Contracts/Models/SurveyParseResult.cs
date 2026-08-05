using TrajectoryAdvAIcer.Entities.Models;

namespace TrajectoryAdvAIcer.Parsing.Contracts.Models;

/// <summary>
/// Результат парсинга анекты
/// </summary>
public class SurveyParseResult
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
