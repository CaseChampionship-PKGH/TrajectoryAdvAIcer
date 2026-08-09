using TrajectoryAdvAIcer.Analysis.Contracts.Models;

namespace TrajectoryAdvAIcer.Api.Models;

/// <summary>
/// Api модель результата анализа
/// </summary>
public class AnalysisResultApiModel
{
    /// <summary>
    /// Список сотрудников с их историей и траекторией обучения
    /// </summary>
    public List<EmployeeTrajectoryAdvice> TrajectoryAdvices { get; set; } = [];

    /// <summary>
    /// Список возникших в процессе валидации ошибок
    /// </summary>
    public List<string> Errors { get; set; } = null!;
}
