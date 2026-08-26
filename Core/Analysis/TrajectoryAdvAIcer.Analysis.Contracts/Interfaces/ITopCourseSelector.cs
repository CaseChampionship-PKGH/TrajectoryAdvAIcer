using TrajectoryAdvAIcer.Analysis.Contracts.Models.Aggregation;

namespace TrajectoryAdvAIcer.Analysis.Contracts.Interfaces;

/// <summary>
/// Сервис фильтрации рекомандательных курсов по их популярности среди коллег
/// </summary>
public interface ITopCourseSelector
{
    /// <summary>
    /// Выбрать топ рекомандательных курсов
    /// </summary>
    List<string> SelectTopCourses(
        List<CourseCandidate> candidates,
        int similarEmployeesCount,
        double popularityThreshold = 0.2,
        int maxCourses = 5);
}

