using System.Text.Json.Serialization;

namespace TrajectoryAdvAIcer.Agent.OpenAI.Models;

/// <summary>
/// Модель запроса к OpenAI
/// </summary>
internal class OpenAICompatibleRequest
{
    [JsonPropertyName("model")]
    public string Model { get; set; } = string.Empty;

    [JsonPropertyName("messages")]
    public List<ChatMessage> Messages { get; set; } = null!;

    [JsonPropertyName("temperature")]
    public double Temperature { get; set; } = 0;

    [JsonPropertyName("max_tokens")]
    public int MaxTokens { get; set; } = 2000;
}
