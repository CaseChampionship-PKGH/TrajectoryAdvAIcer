using TrajectoryAdvAIcer.Analysis.Contracts.Enums;

namespace TrajectoryAdvAIcer.Services.Contracts.Models;

/// <summary>
/// Результат выполнения pipeline
/// </summary>
public record PipelineContext
{
    /// <summary>
    /// Файл с историей обучения
    /// </summary>
    public Stream LearningHistoryStream { get; set; } = null!;

    /// <summary>
    /// Имя файла с историей обучения
    /// </summary>
    public string LearningHistoryFileName { get; set; } = string.Empty;

    /// <summary>
    /// Файл с реестром курсов
    /// </summary>
    public Stream CourseCatalogStream { get; set; } = null!;

    /// <summary>
    /// Имя файла с реестром курсов
    /// </summary>
    public string CourseCatalogFileName { get; set; } = string.Empty;

    /// <summary>
    /// Метод аналиа
    /// </summary>
    public AnalysisMethod AnalysisMethod { get; set; }
}
