namespace TrajectoryAdvAIcer.Web.Models;

/// <summary>
/// Данные Dashboard
/// </summary>
public class DashboardData
{
    /// <summary>
    /// Словарь критерий -> средний балл
    /// </summary>
    public Dictionary<string, double> AverageScores { get; set; } = [];

    /// <summary>
    /// Словарь критерий -> средний балл но для лепестковой
    /// </summary>
    public Dictionary<string, double> SpiderValues { get; set; } = [];

    /// <summary>
    /// Матрица корреляции 5x5
    /// </summary>
    public double[][] HeatMapData { get; set; } = null!;

    /// <summary>
    /// Названия критериев
    /// </summary>
    public string[] CriteriaLabels { get; set; } = null!;

    /// <summary>
    /// Распределние оценок по { низкий, средний, высокий }
    /// </summary>
    public int[] DistributionCounts { get; set; } = new int[3];

    /// <summary>
    /// Полная картина
    /// </summary>
    public double OverallSatisfaction { get; set; }
}
