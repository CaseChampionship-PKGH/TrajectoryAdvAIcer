namespace TrajectoryAdvAIcer.Agent.Contracts.Models;

/// <summary>
/// Запрос к LLM
/// </summary>
public class LlmRequest
{
    /// <summary>
    /// Сырые данные промпта
    /// </summary>
    public string RawPrompt { get; set; } = string.Empty;
}
