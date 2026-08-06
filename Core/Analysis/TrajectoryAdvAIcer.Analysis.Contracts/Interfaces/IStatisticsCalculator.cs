using TrajectoryAdvAIcer.Analysis.Contracts.Models;
using TrajectoryAdvAIcer.Entities.Models;

namespace TrajectoryAdvAIcer.Analysis.Contracts.Interfaces;

/// <summary>
/// Калькулятор для подсчёта числовых и бинарных данных анекеты
/// </summary>
public interface IStatisticsCalculator
{
    /// <summary>
    /// Посчитать данные анекеты
    /// </summary>
    SurveyStatistics Calculate(LearningHistory parsedData);

    /// <summary>
    /// Посчитать данные одного вопроса
    /// </summary>
    QuestionStatistics? CalculateForQuestion(LearningHistory? question, IEnumerable<LearningHistory> responses);
}
