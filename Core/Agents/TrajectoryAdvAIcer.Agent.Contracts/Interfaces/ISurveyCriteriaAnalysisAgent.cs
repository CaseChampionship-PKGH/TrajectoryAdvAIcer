using TrajectoryAdvAIcer.Agent.Contracts.Enums;
using TrajectoryAdvAIcer.Analysis.Contracts.Models;
using TrajectoryAdvAIcer.Analysis.Contracts.Models.Aggregation;

namespace TrajectoryAdvAIcer.Agent.Contracts.Interfaces;

/// <summary>
/// ИИ-Агент для анализа программы по критерию
/// </summary>
public interface ISurveyCriteriaAnalysisAgent
{
    /// <summary>
    /// Проанализировать критерий
    /// </summary>
    Task<string> AnalyzeAndGenerateCriterionNoteAsync(CriterionPromptData criterionData, LlmVariant llmVariant);

    /// <summary>
    /// Проанализировать траекторию развития по всем данным
    /// </summary>
    Task<Trajectory> AnalyzeTrajectoryAsync(AggregatedCriteriaData allData, List<string> criterionNotes, LlmVariant llmVariant);
}
