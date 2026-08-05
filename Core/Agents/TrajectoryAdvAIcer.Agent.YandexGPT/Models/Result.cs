using System.Text.Json.Serialization;

namespace TrajectoryAdvAIcer.Agent.YandexGPT.Models;

/// <summary>
/// Результат ллм модели от YandexGPT
/// </summary>
internal class Result
{
    [JsonPropertyName("alternatives")]
    public required List<Alternative> Alternatives { get; set; }
}
