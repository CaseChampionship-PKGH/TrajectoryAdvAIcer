using System.Text.Json.Serialization;

namespace TrajectoryAdvAIcer.Agent.YandexGPT.Models;

/// <summary>
/// Модель запроса к YandexGPT
/// </summary>
internal class YandexGptRequest
{
    [JsonPropertyName("modelUri")]
    public required string ModelUri { get; set; }

    [JsonPropertyName("completionOptions")]
    public required CompletionOptions CompletionOptions { get; set; }

    [JsonPropertyName("messages")]
    public required List<Message> Messages { get; set; }
}
