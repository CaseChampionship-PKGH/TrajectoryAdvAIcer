using TrajectoryAdvAIcer.Entities.Models;

namespace TrajectoryAdvAIcer.Analysis.Contracts.Interfaces;

/// <summary>
/// Cервис-преобразователь коллаборативной фильтрации
/// </summary>
public interface ICollaborativeFilteringService
{
    /// <summary>
    /// Для каждого сотрудника строит словарь популярности курсов
    /// (CourseId → количество прохождений) среди сотрудников с такой же должностью и ИОГВ.
    /// </summary>
    Dictionary<string, Dictionary<string, int>> BuildPopularityMaps(LearningHistory history);

    /// <summary>
    /// Строит словарь популярности курсов для одного сотрудника.
    /// </summary>
    Dictionary<string, int> BuildPopularityMapForEmployee(LearningHistory history, string employeeId);
}
