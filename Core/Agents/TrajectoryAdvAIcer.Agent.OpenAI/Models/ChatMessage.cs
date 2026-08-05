using System.Text.Json.Serialization;

namespace TrajectoryAdvAIcer.Agent.OpenAI.Models;

/// <summary>
/// Сообщение к модели
/// </summary>
internal class ChatMessage
{
    [JsonPropertyName("role")]
    public string Role { get; set; } = string.Empty;

    [JsonPropertyName("content")]
    public string Content { get; set; } = string.Empty;
}
