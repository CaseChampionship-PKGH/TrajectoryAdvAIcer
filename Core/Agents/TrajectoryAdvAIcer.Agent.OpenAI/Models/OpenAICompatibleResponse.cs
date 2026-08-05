using System.Text.Json.Serialization;

namespace TrajectoryAdvAIcer.Agent.OpenAI.Models;

/// <summary>
/// Ответ ллм модели от OpenAI
/// </summary>
internal class OpenAICompatibleResponse
{
    [JsonPropertyName("choices")]
    public List<Choice> Choices { get; set; } = null!;
}
