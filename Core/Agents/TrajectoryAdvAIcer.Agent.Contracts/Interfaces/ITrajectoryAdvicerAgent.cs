using TrajectoryAdvAIcer.Agent.Contracts.Enums;
using TrajectoryAdvAIcer.Analysis.Contracts.Models.Aggregation;
using TrajectoryAdvAIcer.Analysis.Contracts.Models.Trajectory;
using TrajectoryAdvAIcer.Entities.Models;

namespace TrajectoryAdvAIcer.Agent.Contracts.Interfaces;

/// <summary>
/// ИИ-Агент для анализа истории обучения и генерации траектории обучения
/// </summary>
public interface ITrajectoryAdvicerAgent
{
    /// <summary>
    /// Сгенерировать траекторию обучения
    /// </summary>
    Task<TrajectoryAnalysisResult> GenerateTrajectoryForEmployeeAsync(Employee profile,
        List<string> passedCourses,
        List<RecommendedCourse> recommendedCourses,
        LlmVariant llmVariant, CancellationToken cancellationToken);
}
