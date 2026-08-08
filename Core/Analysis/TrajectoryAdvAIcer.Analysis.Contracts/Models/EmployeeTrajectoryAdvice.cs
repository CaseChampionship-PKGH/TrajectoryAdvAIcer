using TrajectoryAdvAIcer.Analysis.Contracts.Models.Trajectory;
using TrajectoryAdvAIcer.Entities.Models;

namespace TrajectoryAdvAIcer.Analysis.Contracts.Models;

/// <summary>
/// Сгенерированная траектория обучения для сотрудника
/// </summary>
public class EmployeeTrajectoryAdvice
{
    /// <summary>
    /// Сотрудник
    /// </summary>
    public Employee Profile { get; set; } = null!;

    /// <summary>
    /// Имена прошедших курсов
    /// </summary>
    public required List<string> PassedCourseNames { get; set; }

    /// <summary>
    /// Траектория обучения
    /// </summary>
    public required List<TrajectoryRecommendation> Trajectory { get; set; }
}
