using TrajectoryAdvAIcer.Entities.Models;

namespace TrajectoryAdvAIcer.Analysis.Contracts.Models.Aggregation;

/// <summary>
/// Рекомендуемый для сотрудника курс
/// </summary>
public class RecommendedCourse
{
    /// <summary>
    /// Курс
    /// </summary>
    public required Course Course { get; set; }

    /// <summary>
    /// Количество сотрудников с такой же должностью и ИОГВ, прошедших этот курс
    /// </summary>
    public int Popularity { get; set; }

    /// <summary>
    /// Общее количество сотрудников с такой же должностью и ИОГВ
    /// </summary>
    public int TotalSimilarEmployees { get; set; }
}
