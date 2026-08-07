using TrajectoryAdvAIcer.Entities.Models;
using TrajectoryAdvAIcer.Validation.Contracts.Interfaces;
using TrajectoryAdvAIcer.Validation.Contracts.Models;

namespace TrajectoryAdvAIcer.Validation;

/// <summary>
/// Валидатор данных анкетирования
/// </summary>
public class DataValidator : IDataValidator
{
    ValidationResult IDataValidator.Validate(LearningHistory parsedLearningHistory, CourseCatalog parsedCourseCatalog)
    {
        var warnings = new List<string>();
        var validRecords = new List<LearningRecord>();
        var missingCoursesReported = new HashSet<string>();

        var employeeNames = parsedLearningHistory.Employees.ToDictionary(e => e.Id, e => e.FullName);

        foreach (var record in parsedLearningHistory.Records)
        {
            if (string.IsNullOrWhiteSpace(record.EmployeeId))
            {
                warnings.Add($"Запись с пустым EmployeeId удалена. Курс: {record.CourseTitle ?? record.CourseId}");
                continue;
            }
            if (string.IsNullOrWhiteSpace(record.CourseId))
            {
                warnings.Add($"Запись с пустым CourseId удалена. Сотрудник: {record.EmployeeId}");
                continue;
            }
            if (!parsedCourseCatalog.Courses.Any(c => c.Id == record.CourseId))
            {
                if (missingCoursesReported.Add(record.CourseId))
                {
                    warnings.Add($"Курс \"{record.CourseTitle ?? record.CourseId}\" отсутствует в справочнике, записи сохранены, но требуют проверки.");
                }
            }
            validRecords.Add(record);
        }

        var uniqueRecords = validRecords
            .GroupBy(r => (r.EmployeeId, r.CourseId))
            .Select(g =>
            {
                if (g.Count() > 1)
                {
                    var first = g.First();
                    var employeeName = employeeNames.TryGetValue(first.EmployeeId, out var name) ? name : first.EmployeeId;
                    warnings.Add($"Найдены дубликаты для сотрудника {employeeName}, курс \"{first.CourseTitle ?? first.CourseId}\". Оставлена последняя запись.");
                }
                return g.Last();
            })
            .ToList();

        var validEmployeeIds = uniqueRecords.Select(r => r.EmployeeId).Distinct().ToHashSet();
        var validEmployees = parsedLearningHistory.Employees.Where(e => validEmployeeIds.Contains(e.Id)).ToList();

        var cleanedHistory = new LearningHistory
        {
            Employees = validEmployees,
            Records = uniqueRecords
        };

        return new ValidationResult
        {
            ValidatedLearningHistory = cleanedHistory,
            Warnings = warnings
        };
    }
}
