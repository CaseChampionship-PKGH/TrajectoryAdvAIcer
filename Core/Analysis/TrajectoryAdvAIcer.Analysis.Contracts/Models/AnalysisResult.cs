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
    public LearningHistory ProgramInfo { get; set; } = null!;

    /// <summary>
    /// Результат анализа с генерацией итогового примечания для каждого критерия программы
    /// </summary>
    public List<CriterionAnalysis> AllCriteriaAnalysisData { get; set; } = null!;

    /// <summary>
    /// Траектория изменения программы по результатам итогового опроса слушателей
    /// </summary>
    public Trajectory? Trajectory { get; set; }
}
