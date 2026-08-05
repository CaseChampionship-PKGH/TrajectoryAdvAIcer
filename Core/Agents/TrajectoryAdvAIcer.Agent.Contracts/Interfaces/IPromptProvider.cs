using TrajectoryAdvAIcer.Analysis.Contracts.Models.Aggregation;

namespace TrajectoryAdvAIcer.Agent.Contracts.Interfaces;

/// <summary>
/// Помошник в преобразовании данных в промпт
/// </summary>
public interface IPromptProvider
{
    /// <summary>
    /// Формирует промпт для получения примечания по критерию
    /// </summary>
    string BuildCriterionNotePrompt(CriterionPromptData criterionData);

    /// <summary>
    /// Формирует промпт для генерации раздела "Траектория изменения программы"
    /// </summary>
    string BuildTrajectoryPrompt(AggregatedCriteriaData allData, List<string> notes);
}
