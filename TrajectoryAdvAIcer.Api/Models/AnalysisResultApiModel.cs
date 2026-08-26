using TrajectoryAdvAIcer.Entities.Models;

namespace TrajectoryAdvAIcer.Api.Models;

/// <summary>
/// Api модель резульата анализа
/// </summary>
public class AnalysisResultApiModel
{
    /// <summary>
    /// Метаданные программы полученные из SurveyParseResult
    /// </summary>
    public LearningHistory ProgramInfo { get; set; } = null!;

    /// <summary>
    /// Список возникших в процессе валидации ошибок
    /// </summary>
    public List<string> Errors { get; set; } = null!;
}
