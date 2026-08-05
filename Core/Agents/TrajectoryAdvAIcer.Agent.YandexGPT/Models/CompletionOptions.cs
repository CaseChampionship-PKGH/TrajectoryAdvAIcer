using System.Text.Json.Serialization;

namespace TrajectoryAdvAIcer.Agent.YandexGPT.Models;

/// <summary>
/// Параметры запроса
/// </summary>
internal class CompletionOptions
{
    [JsonPropertyName("stream")]
    public bool Stream { get; set; } = false;

    [JsonPropertyName("temperature")]
    public double Temperature { get; set; } = 0;

    [JsonPropertyName("maxTokens")]
    public string MaxTokens { get; set; } = "2000";
}
