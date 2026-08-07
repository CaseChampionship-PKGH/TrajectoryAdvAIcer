namespace TrajectoryAdvAIcer.Analysis.Contracts.Models;

/// <summary>
/// Результат анализа с генерацией итогового примечания для критерия программы
/// </summary>
public class CriterionAnalysis
{
    /// <summary>
    /// Сгенерированное примечание
    /// </summary>
    public string? Note { get; set; }
}
