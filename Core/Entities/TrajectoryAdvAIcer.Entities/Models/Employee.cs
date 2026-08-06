namespace TrajectoryAdvAIcer.Entities.Models;

/// <summary>
/// Сотрудник, проходящий курс
/// </summary>
public class Employee
{
    /// <summary>
    /// Идентификатор сотрудника
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// ФИО сотрудника
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Должность сотрудника
    /// </summary>
    public string Position { get; set; } = string.Empty;

    /// <summary>
    /// Подраздение, где работает сотрудник
    /// </summary>
    public string IOGV { get; set; } = string.Empty;
}
