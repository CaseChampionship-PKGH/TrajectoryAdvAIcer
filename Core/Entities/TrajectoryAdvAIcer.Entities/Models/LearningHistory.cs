namespace TrajectoryAdvAIcer.Entities.Models;

/// <summary>
/// История обучения всеми сотрудниками
/// </summary>
public class LearningHistory
{
    /// <summary>
    /// Список сотрудников
    /// </summary>
    public List<Employee> Employees { get; set; } = [];

    /// <summary>
    /// Список их прохождений курсов
    /// </summary>
    public List<LearningRecord> Records { get; set; } = [];
}
