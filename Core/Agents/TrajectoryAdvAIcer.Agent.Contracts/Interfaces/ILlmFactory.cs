using TrajectoryAdvAIcer.Agent.Contracts.Enums;

namespace TrajectoryAdvAIcer.Agent.Contracts.Interfaces;

/// <summary>
/// Фабрика получения LLM
/// </summary>
public interface ILlmFactory
{
    /// <summary>
    /// Получить LLM по его группе
    /// </summary>
    ILlmClient CreateLLmClient(LlmVariant variant);
}
