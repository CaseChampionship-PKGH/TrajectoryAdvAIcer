namespace TrajectoryAdvAIcer.Agent.Contracts.Models;

/// <summary>
/// Ответ LLM
/// </summary>
public class LlmResponse
{
    /// <summary>
    /// Сырые данные ответа от ИИ-агента
    /// </summary>
    public string RawResponse { get; set; } = string.Empty;
}
