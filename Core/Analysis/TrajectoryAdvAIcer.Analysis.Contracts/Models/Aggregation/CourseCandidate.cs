namespace TrajectoryAdvAIcer.Analysis.Contracts.Models.Aggregation;

/// <summary>
/// Данные по всем критериям
/// </summary>
public class CourseCandidate
{
    /// <summary>
    /// Идентификатор курса
    /// </summary>
    public string CourseId { get; set; } = string.Empty;

    /// <summary>
    /// Популярность
    /// </summary>
    public int Popularity { get; set; }
}
