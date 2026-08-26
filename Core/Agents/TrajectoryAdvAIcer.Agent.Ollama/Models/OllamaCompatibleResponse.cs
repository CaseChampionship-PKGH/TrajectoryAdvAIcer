using System.Text.Json.Serialization;

namespace TrajectoryAdvAIcer.Agent.Ollama.Models;

/// <summary>
/// Ответ ллм модели от Ollama
/// </summary>
internal class OllamaCompatibleResponse
{
    [JsonPropertyName("choices")]
    public List<Choice> Choices { get; set; } = null!;
}
