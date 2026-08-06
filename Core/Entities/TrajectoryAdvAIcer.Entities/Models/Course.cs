namespace TrajectoryAdvAIcer.Entities.Models;

/// <summary>
/// Курс подготовки
/// </summary>
public class Course
{
    /// <summary>
    /// Идентификатор курса
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Название курса
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Аннотация
    /// </summary>
    public string Annotation { get; set; } = string.Empty;

    /// <summary>
    /// Цели, задачи курса
    /// </summary>
    public string Goals { get; set; } = string.Empty;

    /// <summary>
    /// Результаты (чем будет владеть)
    /// </summary>
    public string Results { get; set; } = string.Empty;
}
