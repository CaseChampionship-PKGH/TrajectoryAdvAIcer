using TrajectoryAdvAIcer.Entities.Models;

namespace TrajectoryAdvAIcer.Analysis.Contracts.Models;

/// <summary>
/// Данные анализа
/// </summary>
public class QuestionStatistics
{
    /// <summary>
    /// Связанный вопрос с статистикой
    /// </summary>
    public SurveyQuestion Question { get; set; } = null!;

    // Для Numeric

    /// <summary>
    /// Среднее арифметическое
    /// </summary>
    public double? Average { get; set; }

    /// <summary>
    /// Медиана
    /// </summary>
    public double? Median { get; set; }

    /// <summary>
    /// Стандартное 
    /// </summary>
    public double? StandardDeviation { get; set; }

    /// <summary>
    /// Распределение оценка → количество
    /// </summary>
    public Dictionary<int, int> Distribution { get; set; } = [];

    /// <summary>
    /// Процент низких баллов
    /// </summary>
    public double? PercentLow { get; set; }   // 1-3

    /// <summary>
    /// Процент средених баллов
    /// </summary>
    public double? PercentMedium { get; set; } // 4-7

    /// <summary>
    /// Процент высоких баллов
    /// </summary>
    public double? PercentHigh { get; set; }   // 8-10

    // Для Binary

    /// <summary>
    /// Количество "Да"
    /// </summary>
    public int? YesCount { get; set; }

    /// <summary>
    /// Количество "Нет"
    /// </summary>
    public int? NoCount { get; set; }

    /// <summary>
    /// Процент  "Да"
    /// </summary>
    public double? YesPercent { get; set; }

    /// <summary>
    /// Процент  "Нет"
    /// </summary>
    public double? NoPercent { get; set; }

    // Для OpenText

    /// <summary>
    /// Количество открытых ответов
    /// </summary>
    public int? AnswerCount { get; set; }
}
