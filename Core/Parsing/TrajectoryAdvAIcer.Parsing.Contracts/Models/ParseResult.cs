namespace TrajectoryAdvAIcer.Parsing.Contracts.Models;

/// <summary>
/// Результат парсинга: распарсенные данные и предупреждения.
/// </summary>
/// <typeparam name="T">Тип распарсенных данных.</typeparam>
public sealed record ParseResult<T>(T Data, IReadOnlyList<string> Warnings)
{
    /// <summary>
    /// Создаёт результат без предупреждений.
    /// </summary>
    public ParseResult(T data) : this(data, Array.Empty<string>())
    {
    }
}