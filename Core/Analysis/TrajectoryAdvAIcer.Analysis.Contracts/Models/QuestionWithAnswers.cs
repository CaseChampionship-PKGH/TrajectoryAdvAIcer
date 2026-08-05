using TrajectoryAdvAIcer.Entities.Models;

namespace TrajectoryAdvAIcer.Analysis.Contracts.Models;

/// <summary>
/// Сжатое представление открытых ответов на вопрос для анализа
/// </summary>
public record QuestionWithAnswers
{
    /// <summary>
    /// Вопрос
    /// </summary>
    public SurveyQuestion Question { get; set; } = null!;

    /// <summary>
    /// Ответы
    /// </summary>
    public List<string> Answers { get; set; } = [];
}
