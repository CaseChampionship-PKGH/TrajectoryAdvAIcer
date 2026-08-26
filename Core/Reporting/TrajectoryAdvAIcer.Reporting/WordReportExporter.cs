using TrajectoryAdvAIcer.Analysis.Contracts.Models;
using TrajectoryAdvAIcer.Reporting.Contracts.Interfaces;
using TrajectoryAdvAIcer.Reporting.Contracts.Models;

namespace TrajectoryAdvAIcer.Reporting;

/// <inheritdoc cref="IReportExporter"/>
public class WordReportExporter : IReportExporter
{
    ExportType IReportExporter.ExportType => ExportType.Word;

    byte[] IReportExporter.Export(AnalysisResult analysisResult)
    {
        //    using var stream = new MemoryStream();
        //    using (var doc = WordprocessingDocument.Create(stream, WordprocessingDocumentType.Document))
        //    {
        //        var mainPart = doc.AddMainDocumentPart();
        //        mainPart.Document = new Document();
        //        var body = mainPart.Document.AppendChild(new Body());

        //        // Главный заголовок
        //        body.AppendChild(CreateParagraph("АНАЛИТИЧЕСКАЯ СПРАВКА ПО ИТОГАМ РЕАЛИЗАЦИИ ПРОГРАММЫ ПОВЫШЕНИЯ КВАЛИФИКАЦИИ",
        //            bold: true, fontSize: 12, center: true));

        //        // Создаём единую таблицу и сразу добавляем границы
        //        var table = new Table();
        //        // Свойства таблицы с границами
        //        var tblProps = new TableProperties();
        //        tblProps.AppendChild(new TableBorders(
        //            new TopBorder { Val = BorderValues.Single, Color = "000000", Size = 4 },
        //            new BottomBorder { Val = BorderValues.Single, Color = "000000", Size = 4 },
        //            new LeftBorder { Val = BorderValues.Single, Color = "000000", Size = 4 },
        //            new RightBorder { Val = BorderValues.Single, Color = "000000", Size = 4 },
        //            new InsideHorizontalBorder { Val = BorderValues.Single, Color = "000000", Size = 4 },
        //            new InsideVerticalBorder { Val = BorderValues.Single, Color = "000000", Size = 4 }));
        //        table.AppendChild(tblProps);

        //        var tblGrid = new TableGrid();
        //        for (var i = 0; i < 13; i++)
        //        {
        //            tblGrid.AppendChild(new GridColumn());
        //        }

        //        table.AppendChild(tblGrid);

        //        // *** Общая информация ***
        //        table.AppendChild(CreateSectionHeaderRow("Общая информация о программе", 13));
        //        table.AppendChild(CreateInfoRow("Наименование программы", analysisResult.ProgramInfo.Title ?? ""));
        //        table.AppendChild(CreateInfoRow("Период обучения", analysisResult.ProgramInfo.Period ?? ""));
        //        table.AppendChild(CreateInfoRow("Форма обучения", analysisResult.ProgramInfo.EducationForm ?? ""));
        //        table.AppendChild(CreateInfoRow("Количество слушателей, принявших участие в опросе",
        //            analysisResult.ProgramInfo.ListenersCount.ToString()));
        //        table.AppendChild(CreateInfoRow("Преподаватели программы", analysisResult.ProgramInfo.Teachers ?? ""));

        //        // *** Ключевые показатели ***
        //        var criteria = analysisResult.AllCriteriaAnalysisData;
        //        var numericCriteria = criteria
        //            .Where(c => c.CriterionData.Statistics?.Average.HasValue == true)
        //            .ToList();
        //        var engagementCriterion = criteria
        //            .FirstOrDefault(c => c.CriterionData.CriterionName == SurveyAnalysisConstants.Engagement);

        //        table.AppendChild(CreateSectionHeaderRow("Ключевые показатели по программе", 13));
        //        table.AppendChild(CreateKeyMetricsHeader());
        //        table.AppendChild(CreateKeyMetricsSecondHeader());

        //        foreach (var crit in numericCriteria)
        //        {
        //            table.AppendChild(CreateKeyMetricDataRow(crit));
        //        }

        //        if (engagementCriterion != null)
        //        {
        //            var engagementRows = CreateEngagementRows(engagementCriterion);
        //            foreach (var r in engagementRows)
        //            {
        //                table.AppendChild(r);
        //            }
        //        }

        //        // *** Предложения слушателей ***
        //        table.AppendChild(CreateSectionHeaderRow("Предложения слушателей", 13));

        //        // Строка заголовков
        //        var propHeaderRow = new TableRow();
        //        propHeaderRow.AppendChild(CreateCell("Темы, которые оказались неактуальны для слушателей", gridSpan: 6,
        //            lightBlue: true, bold: true, center: true, fontSize: 12));
        //        propHeaderRow.AppendChild(CreateCell("Темы, которыми можно дополнить программу", gridSpan: 7,
        //            lightBlue: true, bold: true, center: true, fontSize: 12));
        //        table.AppendChild(propHeaderRow);

        //        // Строка содержимого
        //        var propDataRow = new TableRow();
        //        var excluded = analysisResult.Trajectory?.ExcludedTopicsSummary ?? "";
        //        var suggested = analysisResult.Trajectory?.SuggestedTopicsSummary ?? "";
        //        propDataRow.AppendChild(CreateCell(excluded, gridSpan: 6, lightBlue: false, bold: false, fontSize: 11));
        //        propDataRow.AppendChild(CreateCell(suggested, gridSpan: 7, lightBlue: false, bold: false, fontSize: 11));
        //        table.AppendChild(propDataRow);

        //        // *** Предпочтительная форма обучения ***
        //        table.AppendChild(CreateSectionHeaderRow("Предпочтительная форма обучения", 13));

        //        // Заголовки трёх колонок
        //        var fmtHeaderRow = new TableRow();
        //        fmtHeaderRow.AppendChild(CreateCell("Очное обучение в аудиториях МРЦ (Корпоративного университета)",
        //            gridSpan: 5, lightBlue: false, bold: false, center: true, fontSize: 11));
        //        fmtHeaderRow.AppendChild(CreateCell("Смешанное обучение: частично очно, частично дистанционно",
        //            gridSpan: 5, lightBlue: false, bold: false, center: true, fontSize: 11));
        //        fmtHeaderRow.AppendChild(CreateCell("Обучение с применением дистанционных образовательных технологий на своем рабочем месте",
        //            gridSpan: 3, lightBlue: false, bold: false, center: true, fontSize: 11));
        //        table.AppendChild(fmtHeaderRow);

        //        // Значения с ранжированием цветом
        //        var fmt = analysisResult.FormatDistribution;
        //        var totalFmt = fmt.FullTime + fmt.Mixed + fmt.Remote;
        //        var rankings = new[] { fmt.FullTime, fmt.Mixed, fmt.Remote };
        //        var sorted = rankings.OrderByDescending(x => x).ToList();
        //        var fullRank = sorted.IndexOf(fmt.FullTime);
        //        var mixedRank = sorted.IndexOf(fmt.Mixed);
        //        var remoteRank = sorted.IndexOf(fmt.Remote);

        //        var fullColor = GetRankColor(fullRank);
        //        var mixedColor = GetRankColor(mixedRank);
        //        var remoteColor = GetRankColor(remoteRank);

        //        var fmtDataRow = new TableRow();
        //        fmtDataRow.AppendChild(CreateCell(
        //            $"{fmt.FullTime} чел. ({Percent(fmt.FullTime, totalFmt)})",
        //            gridSpan: 5, lightBlue: false, bold: false, center: true, fontSize: 11,
        //            customFill: fullColor));
        //        fmtDataRow.AppendChild(CreateCell(
        //            $"{fmt.Mixed} чел. ({Percent(fmt.Mixed, totalFmt)})",
        //            gridSpan: 5, lightBlue: false, bold: false, center: true, fontSize: 11,
        //            customFill: mixedColor));
        //        fmtDataRow.AppendChild(CreateCell(
        //            $"{fmt.Remote} чел. ({Percent(fmt.Remote, totalFmt)})",
        //            gridSpan: 3, lightBlue: false, bold: false, center: true, fontSize: 11,
        //            customFill: remoteColor));
        //        table.AppendChild(fmtDataRow);

        //        // *** Траектория изменения программы ***
        //        table.AppendChild(CreateSectionHeaderRow("Траектория изменения программы по результатам итогового опроса слушателей", 13));
        //        if (analysisResult.Trajectory != null)
        //        {
        //            table.AppendChild(CreateTrajectoryRow("Потребность в дальнейшей реализации программы", analysisResult.Trajectory.NeedForProgram));
        //            table.AppendChild(CreateTrajectoryRow("Корректировка отбора слушателей", analysisResult.Trajectory.AdmissionCorrection));
        //            table.AppendChild(CreateTrajectoryRow("Дополнение программы учебными вопросами", analysisResult.Trajectory.ProgramSupplement));
        //            table.AppendChild(CreateTrajectoryRow("Изменение количества часов в программе", analysisResult.Trajectory.HoursChange));
        //            table.AppendChild(CreateTrajectoryRow("Изменение формы обучения", analysisResult.Trajectory.FormChange));
        //        }

        //        body.AppendChild(table);
        //        var sectionProps = new SectionProperties();
        //        sectionProps.AppendChild(new PageSize
        //        {
        //            Width = (uint)(297 * 56.69),
        //            Height = (uint)(210 * 56.69),
        //            Orient = PageOrientationValues.Landscape
        //        });
        //        sectionProps.AppendChild(new PageMargin
        //        {
        //            Left = 1134,    // 2 см
        //            Right = 1134,   // 2 см
        //            Top = 567,      // 1 см
        //            Bottom = 567    // 1 см
        //        });
        //        body.AppendChild(sectionProps);

        //        mainPart.Document.Save();
        //    }
        //    return stream.ToArray();
        return [];
    }

    //// Цвет для рейтинга
    //private static string GetRankColor(int rank)
    //{
    //    return rank switch
    //    {
    //        0 => "C5E0B3", // светло-зелёный
    //        1 => "FFE599", // жёлтый
    //        _ => "FFFFFF"  // белый
    //    };
    //}

    //private static string GetColorForScore(double score)
    //{
    //    if (score >= 1 && score <= 4)
    //    {
    //        return "F7CAAC";   // оранжевый
    //    }

    //    if (score >= 5 && score <= 7)
    //    {
    //        return "FFE599";   // жёлтый
    //    }

    //    if (score >= 8 && score < 10)
    //    {
    //        return "C5E0B3";   // светло-зелёный
    //    }

    //    if (score >= 10)
    //    {
    //        return "A8D08D";                // тёмно-зелёный
    //    }

    //    return "FFFFFF";
    //}

    //// Универсальное создание ячейки с расширенными параметрами
    //private static TableCell CreateCell(string text, int gridSpan = 1, bool lightBlue = false, bool bold = false,
    //    bool verticalMerge = false, bool center = false, int fontSize = 12, string? customFill = null)
    //{
    //    var cell = new TableCell();
    //    if (gridSpan > 1)
    //    {
    //        cell.AppendChild(new GridSpan { Val = gridSpan });
    //    }

    //    var props = new TableCellProperties();
    //    if (!string.IsNullOrEmpty(customFill))
    //    {
    //        props.AppendChild(new Shading { Fill = customFill, Color = "auto", Val = ShadingPatternValues.Clear });
    //    }
    //    else if (lightBlue)
    //    {
    //        props.AppendChild(new Shading { Fill = "DEEAF6", Color = "auto", Val = ShadingPatternValues.Clear });
    //    }
    //    if (center)
    //    {
    //        props.AppendChild(new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center });
    //    }
    //    if (verticalMerge)
    //    {
    //        props.AppendChild(new VerticalMerge { Val = MergedCellValues.Restart });
    //    }
    //    cell.AppendChild(props);

    //    var para = new Paragraph();
    //    if (center)
    //    {
    //        para.AppendChild(new ParagraphProperties(new Justification { Val = JustificationValues.Center }));
    //    }
    //    var run = new Run();
    //    var runProps = new RunProperties();
    //    if (bold)
    //    {
    //        runProps.AppendChild(new Bold());
    //    }

    //    runProps.AppendChild(new RunFonts { Ascii = "Times New Roman", HighAnsi = "Times New Roman", ComplexScript = "Times New Roman" });
    //    runProps.AppendChild(new FontSize { Val = (fontSize * 2).ToString() });
    //    run.AppendChild(runProps);
    //    run.AppendChild(new Text(text ?? ""));
    //    para.AppendChild(run);
    //    cell.AppendChild(para);
    //    return cell;
    //}

    //// Ячейка продолжения вертикального объединения
    //private static TableCell CreateVerticalContinueCell()
    //{
    //    var cell = new TableCell();
    //    cell.AppendChild(new VerticalMerge { Val = MergedCellValues.Continue });
    //    cell.AppendChild(new Paragraph()); // обязательный элемент
    //    return cell;

    //}

    //// Секционный заголовок
    //private static TableRow CreateSectionHeaderRow(string text, int gridSpan) =>
    //    CreateSingleCellRow(text, gridSpan, lightBlue: true, bold: true, fontSize: 12, center: true);

    //private static TableRow CreateSingleCellRow(string text, int gridSpan, bool lightBlue, bool bold, int fontSize, bool center)
    //{
    //    var row = new TableRow();
    //    row.AppendChild(CreateCell(text, gridSpan, lightBlue, bold, center: center, fontSize: fontSize));
    //    return row;
    //}

    //private static TableRow CreateInfoRow(string label, string value)
    //{
    //    var row = new TableRow();
    //    row.AppendChild(CreateCell(label, gridSpan: 6, lightBlue: false, bold: false, fontSize: 11));
    //    row.AppendChild(CreateCell(value, gridSpan: 7, lightBlue: false, bold: false, fontSize: 11));
    //    return row;
    //}

    //private static TableRow CreateKeyMetricsHeader()
    //{
    //    var row = new TableRow();
    //    row.AppendChild(CreateCell("Ключевые показатели", gridSpan: 1, lightBlue: true, bold: true,
    //        verticalMerge: true, center: true, fontSize: 12));
    //    row.AppendChild(CreateCell("Баллы по шкале/кол-во оценок", gridSpan: 10, lightBlue: true, bold: true,
    //        center: true, fontSize: 12));
    //    row.AppendChild(CreateCell("Средний балл по показателю", gridSpan: 1, lightBlue: true, bold: true,
    //        verticalMerge: true, center: true, fontSize: 12));
    //    row.AppendChild(CreateCell("Примечание", gridSpan: 1, lightBlue: true, bold: true,
    //        verticalMerge: true, center: true, fontSize: 12));
    //    return row;
    //}

    //private static TableRow CreateKeyMetricsSecondHeader()
    //{
    //    var row = new TableRow();
    //    row.AppendChild(CreateVerticalContinueCell());
    //    for (var i = 1; i <= 10; i++)
    //    {
    //        row.AppendChild(CreateCell(i.ToString(), gridSpan: 1, lightBlue: true, bold: true,
    //            center: true, fontSize: 11));
    //    }
    //    row.AppendChild(CreateVerticalContinueCell());
    //    row.AppendChild(CreateVerticalContinueCell());
    //    return row;
    //}

    //private static TableRow CreateKeyMetricDataRow(CriterionAnalysis crit)
    //{
    //    var stats = crit.CriterionData.Statistics;
    //    var dist = stats?.Distribution ?? [];
    //    var row = new TableRow();
    //    row.AppendChild(CreateCell(crit.CriterionData.CriterionName, gridSpan: 1, lightBlue: false, bold: false, fontSize: 11));
    //    for (var i = 1; i <= 10; i++)
    //    {
    //        var value = dist.ContainsKey(i) ? dist[i] : 0;
    //        var color = value > 0 ? GetColorForScore(i) : "FFFFFF";
    //        row.AppendChild(CreateCell(value.ToString(), gridSpan: 1, lightBlue: false, bold: false,
    //            center: true, fontSize: 11, customFill: color));
    //    }
    //    var avgColor = GetColorForScore(stats?.Average ?? 0);
    //    row.AppendChild(CreateCell(stats?.Average?.ToString("F2") ?? "—", gridSpan: 1, lightBlue: false, bold: true,
    //        center: true, fontSize: 11, customFill: avgColor));
    //    row.AppendChild(CreateCell(crit.Note ?? "", gridSpan: 1, lightBlue: false, bold: false, fontSize: 12));
    //    return row;
    //}

    //private static List<TableRow> CreateEngagementRows(CriterionAnalysis engagementCriterion)
    //{
    //    var rows = new List<TableRow>();
    //    var stats = engagementCriterion.CriterionData.Statistics;
    //    var yes = stats?.YesCount ?? 0;
    //    var no = stats?.NoCount ?? 0;
    //    var yesColor = yes > no ? "F7CAAC" : "FFFFFF";
    //    var noColor = no > yes ? "A8D08D" : "FFFFFF";
    //    var engagementPercent = stats?.NoPercent ?? 0;
    //    var engColor = engagementPercent >= 80 ? "C5E0B3" : (engagementPercent >= 50 ? "FFEB9C" : "FFE599");

    //    // Строка 1
    //    var row1 = new TableRow();
    //    row1.AppendChild(CreateCell("Вовлеченность в образовательный процесс", gridSpan: 1, lightBlue: false, bold: false,
    //        verticalMerge: true, fontSize: 11));
    //    row1.AppendChild(CreateCell("Чувствовалась ли отстранённость от образовательного процесса?",
    //        gridSpan: 10, lightBlue: true, bold: true, center: true, fontSize: 11));
    //    row1.AppendChild(CreateCell("Уровень вовлеченности", gridSpan: 1, lightBlue: true, bold: true,
    //        verticalMerge: true, center: true, fontSize: 11));
    //    row1.AppendChild(CreateCell(engagementCriterion.Note ?? "", gridSpan: 1, lightBlue: false, bold: false,
    //        verticalMerge: true, fontSize: 12));
    //    rows.Add(row1);

    //    // Строка 2 (Да / Нет)
    //    var row2 = new TableRow();
    //    row2.AppendChild(CreateVerticalContinueCell());
    //    row2.AppendChild(CreateCell("Да", gridSpan: 5, lightBlue: true, bold: true, center: true, fontSize: 12));
    //    row2.AppendChild(CreateCell("Нет", gridSpan: 5, lightBlue: true, bold: true, center: true, fontSize: 12));
    //    row2.AppendChild(CreateVerticalContinueCell());
    //    row2.AppendChild(CreateVerticalContinueCell());
    //    rows.Add(row2);

    //    // Строка 3 (значения)
    //    var row3 = new TableRow();
    //    row3.AppendChild(CreateVerticalContinueCell());
    //    row3.AppendChild(CreateCell(yes.ToString(), gridSpan: 5, lightBlue: false, bold: false, center: true, fontSize: 12, customFill: yesColor));
    //    row3.AppendChild(CreateCell(no.ToString(), gridSpan: 5, lightBlue: false, bold: false, center: true, fontSize: 12, customFill: noColor));
    //    row3.AppendChild(CreateCell($"{engagementPercent:F1}%", gridSpan: 1, lightBlue: false, bold: false, center: true, fontSize: 12, customFill: engColor));
    //    row3.AppendChild(CreateVerticalContinueCell());
    //    rows.Add(row3);

    //    return rows;
    //}

    //private static TableRow CreateTrajectoryRow(string label, string content)
    //{
    //    var row = new TableRow();
    //    row.AppendChild(CreateCell(label, gridSpan: 6, lightBlue: false, bold: false, fontSize: 11));
    //    row.AppendChild(CreateCell(content, gridSpan: 7, lightBlue: false, bold: false, fontSize: 11));
    //    return row;
    //}

    //private static Paragraph CreateParagraph(string text, bool bold, int fontSize, bool center = false)
    //{
    //    var para = new Paragraph();
    //    if (center)
    //    {
    //        para.AppendChild(new ParagraphProperties(new Justification { Val = JustificationValues.Center }));
    //    }
    //    var run = new Run();
    //    var runProps = new RunProperties();
    //    if (bold)
    //    {
    //        runProps.AppendChild(new Bold());
    //    }

    //    runProps.AppendChild(new RunFonts { Ascii = "Times New Roman", HighAnsi = "Times New Roman", ComplexScript = "Times New Roman" });
    //    runProps.AppendChild(new FontSize { Val = (fontSize * 2).ToString() });
    //    run.AppendChild(runProps);
    //    run.AppendChild(new Text(text));
    //    para.AppendChild(run);
    //    return para;
    //}

    //private static string Percent(int count, int total) =>
    //    total > 0 ? $"{100.0 * count / total:F1}%" : "0%";
}
