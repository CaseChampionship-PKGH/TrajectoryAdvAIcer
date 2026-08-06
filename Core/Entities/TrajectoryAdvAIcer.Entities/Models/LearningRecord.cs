using TrajectoryAdvAIcer.Entities.Enums;

namespace TrajectoryAdvAIcer.Entities.Models;

/// <summary>
/// Информация о прохождении программы сотрудником
/// </summary>
public class LearningRecord
{
    /// <summary>
    /// Идентификатор сотрудника
    /// </summary>
    public string EmployeeId { get; set; } = string.Empty;

    /// <summary>
    /// Идентификатор курса
    /// </summary>
    public string CourseId { get; set; } = string.Empty;

    /// <summary>
    /// Тип прохождения (как проходил)
    /// </summary>
    public CourseType Type { get; set; }

    /// <summary>
    /// Статус прохождения курса
    /// </summary>
    public CompletionStatus Status { get; set; }
}
