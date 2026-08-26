using System.Text.Json.Serialization;

namespace TrajectoryAdvAIcer.Agent.Ollama.Models;

/// <summary>
/// Результат ллм модели от OpenAI
/// </summary>
internal class Choice
{
    [JsonPropertyName("message")]
    public ChatMessage Message { get; set; } = null!;
}
