using TrajectoryAdvAIcer.Analysis.Contracts.Models;
using TrajectoryAdvAIcer.Analysis.Contracts.Models.Aggregation;
using TrajectoryAdvAIcer.Entities.Models;

namespace TrajectoryAdvAIcer.Analysis.Contracts.Interfaces;

/// <summary>
/// Cервис-преобразователь данных для промптов
/// </summary>
public interface ICriterionAggregator
{
    /// <summary>
    /// Выделить вопросы по критериям
    /// </summary>
    AggregatedCriteriaData Aggregate(LearningHistory parsedResult, SurveyStatistics statistics);
}
