using TrajectoryAdvAIcer.Reporting.Contracts.Models;

namespace TrajectoryAdvAIcer.Reporting.Contracts.Interfaces;

/// <summary>
/// Фабрика экспортеров отчёта
/// </summary>
public interface IReportExporterFactory
{
    /// <summary>
    /// Получить экспортер
    /// </summary>
    IReportExporter GetReportExporter(ExportType type);
}
