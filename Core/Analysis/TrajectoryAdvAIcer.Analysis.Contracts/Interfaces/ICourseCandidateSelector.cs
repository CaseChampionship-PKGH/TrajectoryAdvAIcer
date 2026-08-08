using TrajectoryAdvAIcer.Analysis.Contracts.Models.Aggregation;
using TrajectoryAdvAIcer.Entities.Models;

namespace TrajectoryAdvAIcer.Analysis.Contracts.Interfaces;

/// <summary>
/// Сервис фильтрации рекомендательных курсов по должности и ИОГВ коллег
/// </summary>
public interface ICourseCandidateSelector
{
    /// <summary>
    /// Отбирает курсы-кандидаты для сотрудника:
    /// - курсы, популярные среди коллег с такой же должностью и ИОГВ,
    /// - исключая уже пройденные сотрудником.
    /// Возвращает список курсов с показателем популярности (количество прохождений).
    /// </summary>
    List<CourseCandidate> SelectCandidates(LearningHistory history, string employeeId);
}
