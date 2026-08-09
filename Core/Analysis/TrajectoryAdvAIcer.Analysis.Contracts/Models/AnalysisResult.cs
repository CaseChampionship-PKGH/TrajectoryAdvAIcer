namespace TrajectoryAdvAIcer.Analysis.Contracts.Models;

/// <summary>
/// Результат анализа с генерацией траекторий обучений всех сотрудников
/// </summary>
public class AnalysisResult
{
    /// <summary>
    /// Список сотрудников и их траекторий
    /// </summary>
    public List<EmployeeTrajectoryAdvice> TrajectoryAdvices { get; set; } = [];
}
