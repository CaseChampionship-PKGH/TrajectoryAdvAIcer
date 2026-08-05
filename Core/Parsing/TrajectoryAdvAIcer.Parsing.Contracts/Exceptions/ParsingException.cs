namespace TrajectoryAdvAIcer.Parsing.Contracts.Exceptions;

/// <summary>
/// Ошибка парсинга данных
/// </summary>
public class ParsingException : Exception
{
    /// <summary>
    /// Список ошибок
    /// </summary>
    public IEnumerable<string> Errors { get; set; } = null!;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="ParsingException"/>
    /// </summary>
    public ParsingException(string message) : base(message) { }
}
