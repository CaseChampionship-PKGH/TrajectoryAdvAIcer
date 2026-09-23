using ClosedXML.Excel;
using TrajectoryAdvAIcer.Entities.Models;
using TrajectoryAdvAIcer.Parsing.Contracts.Enums;
using TrajectoryAdvAIcer.Parsing.Contracts.Helpers;
using TrajectoryAdvAIcer.Parsing.Contracts.Interfaces;
using TrajectoryAdvAIcer.Parsing.Contracts.Models;

namespace TrajectoryAdvAIcer.Parsing.Excel;

/// <summary>
/// Excel парсер реестра курсов
/// </summary>
public class CourseCatalogExcelParser : IDataParser
{
    /// <inheritdoc />
    public InputFormat Format => InputFormat.Excel;

    /// <inheritdoc />
    public ParsingTarget Target => ParsingTarget.CourseCatalog;

    /// <inheritdoc />
    public Task<ParseResult<T>> ParseAsync<T>(Stream input)
    {
        if (typeof(T) != typeof(CourseCatalog))
        {
            throw new InvalidOperationException("CourseCatalogExcelParser ожидает тип CourseCatalog");
        }

        using var workbook = new XLWorkbook(input);
        var ws = workbook.Worksheet(1);

        var courses = new List<Course>();
        var lastRow = ws.LastRowUsed()?.RowNumber() ?? 1;
        for (var row = 2; row <= lastRow; row++)
        {
            var title = ws.Cell(row, 1).GetString().Trim();
            if (string.IsNullOrWhiteSpace(title))
            {
                continue;
            }

            var course = new Course
            {
                Id = StableIdProvider.GenerateStableId(title),
                Title = title,
                Annotation = ws.Cell(row, 2).GetString().Trim(),
                Goals = ws.Cell(row, 3).GetString().Trim(),
                Results = ws.Cell(row, 4).GetString().Trim()
            };
            courses.Add(course);
        }

        var catalog = new CourseCatalog { Courses = courses };

        return Task.FromResult(new ParseResult<T>((T)(object)catalog));
    }
}
