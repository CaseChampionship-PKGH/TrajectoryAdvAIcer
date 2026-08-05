using TrajectoryAdvAIcer.Analysis.Contracts.Models;
using TrajectoryAdvAIcer.Reporting.Contracts.Models;

namespace TrajectoryAdvAIcer.Reporting.Contracts.Interfaces;

/// <summary>
/// Экспортер отчёта
/// </summary>
public interface IReportExporter
{
    /// <summary>
    /// Тип экспортера
    /// </summary>
    ExportType ExportType { get; }

    /// <summary>
    /// Экспортировать статистику
    /// </summary>
    byte[] Export(AnalysisResult analysisData);
}
