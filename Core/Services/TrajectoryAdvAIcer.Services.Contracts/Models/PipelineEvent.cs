using TrajectoryAdvAIcer.Analysis.Contracts.Models;

namespace TrajectoryAdvAIcer.Services.Contracts.Models;

/// <summary>
/// Результат выполнения действия pipeline
/// </summary>
public record PipelineEvent
{
    /// <summary>
    /// Тип выполнения
    /// </summary>
    public string? Type { get; set; }

    /// <summary>
    /// Ошибки
    /// </summary>
    /// <remarks>
    /// если Type = "errors"
    /// </remarks>
    public List<string>? Errors { get; set; }

    /// <summary>
    /// Траектория сотрудника
    /// </summary>
    /// <remarks>
    /// если Type = "employee"
    /// </remarks>
    public EmployeeTrajectoryAdvice? EmployeeAdvice { get; set; }

    /// <summary>
    /// Флаг окончания работы
    /// </summary>
    public bool IsComplete { get; set; }
}

