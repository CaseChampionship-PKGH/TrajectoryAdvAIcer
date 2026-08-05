namespace TrajectoryAdvAIcer.Analysis.Contracts.Models;

/// <summary>
/// Траектория изменения программы по результатам итогового опроса слушателей
/// </summary>
public class Trajectory
{
    /// <summary>
    /// Потребность в дальнейшей реализации программы
    /// </summary>
    public string NeedForProgram { get; set; } = string.Empty;

    /// <summary>
    /// Корректировка отбора слушателей
    /// </summary>
    public string AdmissionCorrection { get; set; } = string.Empty;

    /// <summary>
    /// Дополнение программы учебными вопросами
    /// </summary>
    public string ProgramSupplement { get; set; } = string.Empty;

    /// <summary>
    /// Изменение количества часов в программе
    /// </summary>
    public string HoursChange { get; set; } = string.Empty;

    /// <summary>
    /// Изменение формы обучения
    /// </summary>
    public string FormChange { get; set; } = string.Empty;

    /// <summary>
    /// Изложение по анализу исключённых тем
    /// </summary>
    public string ExcludedTopicsSummary { get; set; } = string.Empty;

    /// <summary>
    /// Изложение по анализу дополнений тем
    /// </summary>
    public string SuggestedTopicsSummary { get; set; } = string.Empty;
}
