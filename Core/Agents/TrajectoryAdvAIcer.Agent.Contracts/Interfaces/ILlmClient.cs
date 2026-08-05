using TrajectoryAdvAIcer.Agent.Contracts.Enums;
using TrajectoryAdvAIcer.Agent.Contracts.Models;

namespace TrajectoryAdvAIcer.Agent.Contracts.Interfaces;

/// <summary>
/// Клиент для обращение к LLM
/// </summary>
public interface ILlmClient
{
    /// <summary>
    /// Группа LLM
    /// </summary>
    LlmVariant LlmVariant { get; }

    /// <summary>
    /// Отправить запрос 
    /// </summary>
    Task<LlmResponse> SendRequestAsync(LlmRequest request, string targetTest);
}
