using TrajectoryAdvAIcer.Entities.Models;

namespace TrajectoryAdvAIcer.Analysis.Contracts.Models;

/// <summary>
/// Результат анализа с генерацией итогового примечания для критерия программы
/// </summary>
public class AnalysisResult
{
    /// <summary>
    /// Метаданные программы полученные из SurveyParseResult
    /// </summary>
    public LearningHistory LearningHistory { get; set; } = null!;
}
