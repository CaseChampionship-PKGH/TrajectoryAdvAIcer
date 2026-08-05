using TrajectoryAdvAIcer.Analysis.Contracts.Models;
using TrajectoryAdvAIcer.Analysis.Contracts.Models.Aggregation;
using TrajectoryAdvAIcer.Entities.Models;

namespace TrajectoryAdvAIcer.Api.Models;

/// <summary>
/// Api модель резульата анализа
/// </summary>
public class AnalysisResultApiModel
{
    /// <summary>
    /// Метаданные программы полученные из SurveyParseResult
    /// </summary>
    public ProgramInfo ProgramInfo { get; set; } = null!;

    /// <summary>
    /// Результат анализа с генерацией итогового примечания для каждого критерия программы
    /// </summary>
    public List<CriterionAnalysis> AllCriteriaAnalysisData { get; set; } = null!;

    /// <summary>
    /// Предпочтительная форма обучения
    /// </summary>
    public LearningFormatDistribution FormatDistribution { get; set; } = null!;

    /// <summary>
    /// Траектория изменения программы по результатам итогового опроса слушателей
    /// </summary>
    public Trajectory? Trajectory { get; set; }

    /// <summary>
    /// Список возникших в процессе валидации ошибок
    /// </summary>
    public List<string> Errors { get; set; } = null!;
}
