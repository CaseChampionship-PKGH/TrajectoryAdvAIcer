using TrajectoryAdvAIcer.Analysis.Contracts.Models;
using TrajectoryAdvAIcer.Reporting.Contracts.Interfaces;
using TrajectoryAdvAIcer.Reporting.Contracts.Models;
using TrajectoryAdvAIcer.Services.Contracts.Constants;

namespace TrajectoryAdvAIcer.Reporting;

/// <inheritdoc cref="IReportExporter"/>
public class ExcelReportExporter : IReportExporter
{
    private readonly static string[] headers = SurveyAnalysisConstants.CriteriaNames.Concat(["Общая оценка удовлетворённости"]).ToArray();

    ExportType IReportExporter.ExportType => ExportType.Excel;

    byte[] IReportExporter.Export(AnalysisResult analysisResult)
    {
        //using var workbook = new XLWorkbook();
        //var ws = workbook.Worksheets.Add("Анализ программы");

        //ws.Cell(2, 2).Value = "Количественные показатели по программе";
        //ws.Cell(2, 2).Style.Font.Bold = true;
        //ws.Cell(2, 2).Style.Font.FontSize = 11;
        //var headerRange = ws.Range(2, 2, 2, 7).Merge();
        //headerRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
        //headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        //headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#DEEBF6");

        //for (var i = 0; i < headers.Length; i++)
        //{
        //    var cell = ws.Cell(3, i + 2);
        //    cell.Value = headers[i];
        //    cell.Style.Font.FontSize = 11;

        //    cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#DEEBF6");
        //    cell.Style.Alignment.WrapText = true;
        //}

        //var criteria = analysisResult.AllCriteriaAnalysisData;
        //var usefulness = GetAverage(criteria, SurveyAnalysisConstants.Usefulness);
        //var practicality = GetAverage(criteria, SurveyAnalysisConstants.Practicality);
        //var accessibility = GetAverage(criteria, SurveyAnalysisConstants.Accessibility);
        //var engagement = GetEngagementPercent(criteria);
        //var interaction = GetAverage(criteria, SurveyAnalysisConstants.Interaction);

        //double? overall = null;
        //if (usefulness.HasValue && practicality.HasValue && accessibility.HasValue && interaction.HasValue && engagement.HasValue)
        //{
        //    overall = (usefulness.Value + practicality.Value + accessibility.Value + interaction.Value) / 4.0;
        //}

        //var dataRow = 4;
        //ws.Cell(dataRow, 2).Value = usefulness?.ToString("F2") ?? "—";
        //ws.Cell(dataRow, 3).Value = practicality?.ToString("F2") ?? "—";
        //ws.Cell(dataRow, 4).Value = accessibility?.ToString("F2") ?? "—";
        //ws.Cell(dataRow, 5).Value = engagement.HasValue ? $"{engagement.Value:F2}%" : "—";
        //ws.Cell(dataRow, 6).Value = interaction?.ToString("F2") ?? "—";
        //ws.Cell(dataRow, 7).Value = overall?.ToString("F10") ?? "—";

        //var criterianCells = ws.Range(dataRow - 1, 2, dataRow - 1, 7);
        //criterianCells.Style.Font.FontSize = 11;
        //criterianCells.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        //criterianCells.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

        //var dataCells = ws.Range(dataRow, 2, dataRow, 7);
        //dataCells.Style.Font.FontSize = 11;
        //dataCells.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        //dataCells.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

        //var tableRange = ws.Range(2, 2, dataRow, 7);
        //tableRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        //tableRange.Style.Border.OutsideBorderColor = XLColor.Black;
        //tableRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        //tableRange.Style.Border.InsideBorderColor = XLColor.Black;

        //tableRange.Style.Alignment.WrapText = true;

        //for (var col = 2; col <= 7; col++)
        //{
        //    ws.Column(col).Width = 25;
        //}

        //ws.Row(2).Height = 25;
        //ws.Row(3).Height = 50;
        //ws.Row(4).Height = 50;

        //using var stream = new MemoryStream();
        //workbook.SaveAs(stream);
        //return stream.ToArray();
        return [];
    }
}
