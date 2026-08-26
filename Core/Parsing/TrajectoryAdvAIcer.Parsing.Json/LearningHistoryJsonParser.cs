using System.Text.Json;
using TrajectoryAdvAIcer.Entities.Models;
using TrajectoryAdvAIcer.Parsing.Contracts.Enums;
using TrajectoryAdvAIcer.Parsing.Contracts.Exceptions;
using TrajectoryAdvAIcer.Parsing.Contracts.Interfaces;

namespace TrajectoryAdvAIcer.Parsing.Json;

/// <summary>
/// Json парсер истории обучения
/// </summary>
public class LearningHistoryJsonParser : IDataParser
{
    private readonly static JsonSerializerOptions serializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    /// <inheritdoc />
    public InputFormat Format => InputFormat.Json;

    /// <inheritdoc />
    public ParsingTarget Target => ParsingTarget.LearningHistory;

    /// <inheritdoc />
    public async Task<T> ParseAsync<T>(Stream input)
    {
        if (typeof(T) != typeof(LearningHistory))
        {
            throw new InvalidOperationException("LearningHistoryJsonParser ожидает тип LearningHistory");
        }

        if (input == null || input.Length == 0)
        {
            throw new ParsingException("Поток с истории обучения пуст.");
        }

        string rawJson;
        using (var reader = new StreamReader(input, leaveOpen: true))
        {
            rawJson = await reader.ReadToEndAsync();
        }

        if (string.IsNullOrWhiteSpace(rawJson))
        {
            throw new ParsingException("Истории обучения не содержит данных.");
        }

        try
        {
            var response = JsonSerializer.Deserialize<LearningHistory>(rawJson, serializerOptions);

            return response == null
                ? throw new ParsingException("Десериализованный история обучения равен null или не содержит результатов.")
                : (T)(object)response;
        }
        catch (JsonException ex)
        {
            throw new ParsingException($"Ошибка десериализации JSON-истории обучения: {ex.Message}");
        }
    }
}
