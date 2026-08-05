using TrajectoryAdvAIcer.Analysis.Contracts.Models.Aggregation;

namespace TrajectoryAdvAIcer.Analysis.Contracts.Models;

/// <summary>
/// Результат анализа с генерацией итогового примечания для критерия программы
/// </summary>
public class CriterionAnalysis
{
    /// <summary>
    /// Исходные данные
    /// </summary>
    public CriterionPromptData CriterionData { get; set; } = null!;

    /// <summary>
    /// Сгенерированное примечание
    /// </summary>
    public string? Note { get; set; }
}
