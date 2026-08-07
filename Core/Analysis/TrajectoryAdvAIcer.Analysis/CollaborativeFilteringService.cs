using TrajectoryAdvAIcer.Analysis.Contracts.Interfaces;
using TrajectoryAdvAIcer.Entities.Enums;
using TrajectoryAdvAIcer.Entities.Models;

namespace TrajectoryAdvAIcer.Analysis;

/// <inheritdoc cref="ICollaborativeFilteringService"/>
public class CollaborativeFilteringService : ICollaborativeFilteringService
{
    Dictionary<string, Dictionary<string, int>> ICollaborativeFilteringService.BuildPopularityMaps(LearningHistory history)
    {
        var result = new Dictionary<string, Dictionary<string, int>>();
        foreach (var employee in history.Employees)
        {
            result[employee.Id] = BuildPopularityMapForEmployee(history, employee.Id);
        }
        return result;
    }

    /// <inheritdoc />
    public Dictionary<string, int> BuildPopularityMapForEmployee(LearningHistory history, string employeeId)
    {
        var targetEmployee = history.Employees.FirstOrDefault(e => e.Id == employeeId);
        if (targetEmployee == null)
        {
            return [];
        }

        var similarEmployees = history.Employees
            .Where(e => e.Id != employeeId &&
                        e.Position == targetEmployee.Position &&
                        e.IOGV == targetEmployee.IOGV)
            .Select(e => e.Id)
            .ToHashSet();

        var coursePopularity = new Dictionary<string, int>();
        foreach (var record in history.Records)
        {
            if (similarEmployees.Contains(record.EmployeeId) && record.Status == CompletionStatus.Passed)
            {
                if (coursePopularity.TryGetValue(record.CourseId, out var value))
                {
                    coursePopularity[record.CourseId] = ++value;
                }
                else
                {
                    coursePopularity[record.CourseId] = 1;
                }
            }
        }

        return coursePopularity;
    }
}
