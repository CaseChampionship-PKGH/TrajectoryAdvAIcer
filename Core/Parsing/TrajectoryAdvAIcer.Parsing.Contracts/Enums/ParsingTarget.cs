namespace TrajectoryAdvAIcer.Parsing.Contracts.Enums;

/// <summary>
/// Цель парсинга
/// </summary>
public enum ParsingTarget
{
    /// <summary>
    /// История обучения
    /// </summary>
    LearningHistory,

    /// <summary>
    /// Реестр электронных курсов
    /// </summary>
    CourseCatalog,

    /// <summary>
    /// Ответы от LLM-агентов
    /// </summary>
    AgentResponse
}
