using TrajectoryAdvAIcer.Analysis.Contracts.Models.Aggregation;
using TrajectoryAdvAIcer.Entities.Models;

namespace TrajectoryAdvAIcer.Agent.Contracts.Interfaces;

/// <summary>
/// Помошник в преобразовании данных в промпт
/// </summary>
public interface IPromptProvider
{
    /// <summary>
    /// Формирует промпт для генерации индивидуальной траектории обучения.
    /// </summary>
    /// <param name="profile">Профиль сотрудника (должность, ИОГВ).</param>
    /// <param name="passedCourses">Пройденные сотрудником курсы (имена).</param>
    /// <param name="recommendedCourses">Рекомендованные курсы с популярностью и описаниями.</param>
    /// <returns>Текстовый промпт для LLM.</returns>
    string BuildTrajectoryPrompt(
        Employee profile,
        List<string> passedCourses,
        List<RecommendedCourse> recommendedCourses);
}
