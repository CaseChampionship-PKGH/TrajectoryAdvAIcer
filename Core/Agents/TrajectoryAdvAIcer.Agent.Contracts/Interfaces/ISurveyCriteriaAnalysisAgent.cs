using TrajectoryAdvAIcer.Agent.Contracts.Enums;
using TrajectoryAdvAIcer.Entities.Models;

namespace TrajectoryAdvAIcer.Agent.Contracts.Interfaces;

/// <summary>
/// ИИ-Агент для анализа истории обучения
/// </summary>
public interface ISurveyCriteriaAnalysisAgent
{
    /// <summary>
    /// Сгенерировать траекторию обучения
    /// </summary>
    Task<string> GenerateLearningTrajectoryAsync(LearningHistory learningHistory, IEnumerable<string> recommendedIds, CourseCatalog courseCatalog, LlmVariant llmVariant);
}
