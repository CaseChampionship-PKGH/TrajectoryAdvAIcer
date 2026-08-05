using System.Text.Json.Serialization;

namespace TrajectoryAdvAIcer.Agent.YandexGPT.Models;

/// <summary>
/// Сообщение к модели
/// </summary>
internal class Message
{
    [JsonPropertyName("role")]
    public required string Role { get; set; } // "system", "user"

    [JsonPropertyName("text")]
    public required string Text { get; set; }
}
