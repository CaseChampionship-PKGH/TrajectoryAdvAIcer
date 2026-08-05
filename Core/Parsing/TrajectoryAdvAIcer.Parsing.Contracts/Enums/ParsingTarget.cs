namespace TrajectoryAdvAIcer.Parsing.Contracts.Enums;

/// <summary>
/// Цель парсинга
/// </summary>
public enum ParsingTarget
{
    /// <summary>
    /// Анкеты обратной связи (новый кейс)
    /// </summary>
    Survey,

    /// <summary>
    /// Ответы от LLM-агентов
    /// </summary>
    AgentResponse
}
