using ClosedXML.Excel;
using TrajectoryAdvAIcer.Analysis.Contracts.Models;
using TrajectoryAdvAIcer.Reporting.Contracts.Interfaces;
using TrajectoryAdvAIcer.Reporting.Contracts.Models;

namespace TrajectoryAdvAIcer.Reporting;

/// <inheritdoc cref="IReportExporter"/>
public class ExcelReportExporter : IReportExporter
{
    private readonly static string[] headers = { "ФИО", "Должность", "ИОГВ", "Рекомендованная траектория обучения" };

    ExportType IReportExporter.ExportType => ExportType.Excel;

    byte[] IReportExporter.Export(AnalysisResult analysisResult)
    {
        using var workbook = new XLWorkbook();
        var ws = workbook.Worksheets.Add("Рекомендации");

        ws.Row(1).Height = 10;

        var titleCell = ws.Cell(2, 1);
        titleCell.Value = "Рекомендации траекторий обучений для сотрудников";
        titleCell.Style.Font.Bold = true;
        titleCell.Style.Font.FontSize = 14;
        titleCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        titleCell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
        ws.Range(2, 1, 2, 4).Merge();
        ws.Row(2).Height = 25;

        ws.Row(3).Height = 10;

        for (var i = 0; i < headers.Length; i++)
        {
            var cell = ws.Cell(4, i + 1);
            cell.Value = headers[i];
            cell.Style.Font.Bold = true;
            cell.Style.Fill.BackgroundColor = XLColor.LightGray;
            cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
        }

        var row = 5;
        foreach (var rec in analysisResult.TrajectoryAdvices)
        {
            ws.Cell(row, 1).Value = rec.Profile.FullName;
            ws.Cell(row, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell(row, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            ws.Cell(row, 2).Value = rec.Profile.Position;
            ws.Cell(row, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell(row, 2).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            ws.Cell(row, 3).Value = rec.Profile.IOGV;
            ws.Cell(row, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell(row, 3).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            var trajectoryText = string.Join("\n", rec.Trajectory.Select((t, index) =>
                $"{index + 1}. «{t.CourseTitle}» – {t.Rationale}"));

            var trajectoryCell = ws.Cell(row, 4);
            trajectoryCell.Value = trajectoryText;
            trajectoryCell.Style.Alignment.WrapText = true;
            trajectoryCell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;
            trajectoryCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

            row++;
        }

        ws.Columns("A:C").AdjustToContents();
        ws.Column("D").Width = 80;
        ws.Rows().AdjustToContents();

        var range = ws.Range(1, 1, row - 1, 4);
        range.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }
}
