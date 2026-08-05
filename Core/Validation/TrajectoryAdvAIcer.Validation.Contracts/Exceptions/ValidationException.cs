namespace TrajectoryAdvAIcer.Validation.Contracts.Exceptions;

/// <summary>
/// Ошибка парсинга данных
/// </summary>
public class ValidationException : Exception
{
    /// <summary>
    /// Список ошибок
    /// </summary>
    public IEnumerable<string> Errors { get; set; } = null!;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="ValidationException"/>
    /// </summary>
    public ValidationException(string message) : base(message) { }
}
