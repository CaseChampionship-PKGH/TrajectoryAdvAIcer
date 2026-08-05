namespace TrajectoryAdvAIcer.Entities.Models;

/// <summary>
/// Метаданные программы
/// </summary>
public class ProgramInfo
{
    /// <summary>
    /// Название программы
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Период обучения
    /// </summary>
    public string Period { get; set; } = string.Empty;

    /// <summary>
    /// Форма обучения
    /// </summary>
    public string EducationForm { get; set; } = string.Empty;

    /// <summary>
    /// Преподаватели
    /// </summary>
    public string Teachers { get; set; } = string.Empty;

    /// <summary>
    /// Количество слушателей
    /// </summary>
    public int ListenersCount { get; set; }
}
