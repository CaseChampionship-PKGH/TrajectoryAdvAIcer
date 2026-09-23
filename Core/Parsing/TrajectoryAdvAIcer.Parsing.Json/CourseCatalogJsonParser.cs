using System.Text.Json;
using TrajectoryAdvAIcer.Entities.Models;
using TrajectoryAdvAIcer.Parsing.Contracts.Enums;
using TrajectoryAdvAIcer.Parsing.Contracts.Exceptions;
using TrajectoryAdvAIcer.Parsing.Contracts.Interfaces;
using TrajectoryAdvAIcer.Parsing.Contracts.Models;

namespace TrajectoryAdvAIcer.Parsing.Json;

/// <summary>
/// Json парсер реестра курсов
/// </summary>
public class CourseCatalogJsonParser : IDataParser
{
    private readonly static JsonSerializerOptions serializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    /// <inheritdoc />
    public InputFormat Format => InputFormat.Json;

    /// <inheritdoc />
    public ParsingTarget Target => ParsingTarget.CourseCatalog;

    /// <inheritdoc />
    public async Task<ParseResult<T>> ParseAsync<T>(Stream input)
    {
        if (typeof(T) != typeof(CourseCatalog))
        {
            throw new InvalidOperationException("CourseCatalogJsonParser ожидает тип CourseCatalog");
        }

        if (input == null || input.Length == 0)
        {
            throw new ParsingException("Поток с реестром курсов пуст.");
        }

        string rawJson;
        using (var reader = new StreamReader(input, leaveOpen: true))
        {
            rawJson = await reader.ReadToEndAsync();
        }

        if (string.IsNullOrWhiteSpace(rawJson))
        {
            throw new ParsingException("Реестр курсов не содержит данных.");
        }

        try
        {
            var response = JsonSerializer.Deserialize<CourseCatalog>(rawJson, serializerOptions);

            return response == null
                ? throw new ParsingException("Десериализованный реестр курсов равен null или не содержит результатов.")
                : new ParseResult<T>((T)(object)response);
        }
        catch (JsonException ex)
        {
            throw new ParsingException($"Ошибка десериализации JSON-реестра курсов: {ex.Message}");
        }
    }
}
