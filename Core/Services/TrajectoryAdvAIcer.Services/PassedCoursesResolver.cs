using TrajectoryAdvAIcer.Entities.Enums;
using TrajectoryAdvAIcer.Entities.Models;

namespace TrajectoryAdvAIcer.Services;

/// <summary>
/// Резолвит названия пройденных сотрудником курсов.
/// </summary>
public static class PassedCoursesResolver
{
    /// <summary>
    /// Возвращает названия пройденных курсов сотрудника.
    /// Название берётся из справочника курсов, а при отсутствии курса
    /// в справочнике — из записи истории обучения.
    /// </summary>
    /// <param name="employeeId">Идентификатор сотрудника.</param>
    /// <param name="records">Записи истории обучения всех сотрудников.</param>
    /// <param name="catalog">Справочник курсов.</param>
    public static List<string> ResolvePassedCourses(
        string employeeId,
        IEnumerable<LearningRecord> records,
        CourseCatalog catalog)
    {
        var catalogCourseById = catalog.Courses.ToDictionary(c => c.Id);
        var result = new List<string>();
        var seen = new HashSet<string>();

        foreach (var record in records)
        {
            if (record.EmployeeId != employeeId || record.Status != CompletionStatus.Passed)
            {
                continue;
            }

            var title = catalogCourseById.TryGetValue(record.CourseId, out var course)
                ? course.Title
                : record.CourseTitle;

            if (string.IsNullOrWhiteSpace(title) || !seen.Add(title))
            {
                continue;
            }

            result.Add(title!);
        }

        return result;
    }
}