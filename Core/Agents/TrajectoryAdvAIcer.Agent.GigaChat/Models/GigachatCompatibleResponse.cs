using System.Text.Json.Serialization;

namespace TrajectoryAdvAIcer.Agent.GigaChat.Models;

/// <summary>
/// Ответ ллм модели от Gigachat
/// </summary>
internal class GigaChatCompatibleResponse
{
    [JsonPropertyName("choices")]
    public List<Choice> Choices { get; set; } = null!;
}
