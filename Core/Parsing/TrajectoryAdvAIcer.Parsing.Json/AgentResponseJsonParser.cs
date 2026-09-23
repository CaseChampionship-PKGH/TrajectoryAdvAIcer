using System.Text.Json;
using TrajectoryAdvAIcer.Analysis.Contracts.Models.Trajectory;
using TrajectoryAdvAIcer.Parsing.Contracts.Enums;
using TrajectoryAdvAIcer.Parsing.Contracts.Exceptions;
using TrajectoryAdvAIcer.Parsing.Contracts.Interfaces;
using TrajectoryAdvAIcer.Parsing.Contracts.Models;

namespace TrajectoryAdvAIcer.Parsing.Json;

/// <inheritdoc cref="IDataParser"/> для ответов агента
public class AgentResponseJsonParser : IDataParser
{
    private readonly static JsonSerializerOptions serializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    /// <inheritdoc />
    public InputFormat Format => InputFormat.Json;

    /// <inheritdoc />
    public ParsingTarget Target => ParsingTarget.AgentResponse;

    async Task<ParseResult<T>> IDataParser.ParseAsync<T>(Stream input)
    {
        if (typeof(T) != typeof(TrajectoryAnalysisResult))
        {
            throw new ParsingException(
                $"Парсер AgentResponse не поддерживает тип {typeof(T).Name}. Ожидался TrajectoryAnalysisResult.");
        }

        if (input == null || input.Length == 0)
        {
            throw new ParsingException("Поток с ответом агента пуст.");
        }

        string rawJson;
        using (var reader = new StreamReader(input, leaveOpen: true))
        {
            rawJson = await reader.ReadToEndAsync();
        }

        if (string.IsNullOrWhiteSpace(rawJson))
        {
            throw new ParsingException("Ответ агента не содержит данных.");
        }

        try
        {
            var response = JsonSerializer.Deserialize<TrajectoryAnalysisResult>(rawJson, serializerOptions);

            return response == null
                ? throw new ParsingException("Десериализованный ответ агента равен null или не содержит результатов.")
                : new ParseResult<T>((T)(object)response);
        }
        catch (JsonException ex)
        {
            throw new ParsingException($"Ошибка десериализации JSON-ответа агента: {ex.Message}");
        }
    }
}
