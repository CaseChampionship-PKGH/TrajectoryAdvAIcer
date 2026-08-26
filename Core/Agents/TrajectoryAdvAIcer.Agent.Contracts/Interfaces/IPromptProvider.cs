using TrajectoryAdvAIcer.Entities.Models;

namespace TrajectoryAdvAIcer.Agent.Contracts.Interfaces;

/// <summary>
/// Помошник в преобразовании данных в промпт
/// </summary>
public interface IPromptProvider
{
    /// <summary>
    /// Формирует промпт для получения примечания по критерию
    /// </summary>
    string BuildCriterionNotePrompt(LearningHistory learningHistory, IEnumerable<string> recommendedIds, CourseCatalog courseCatalog);
}
