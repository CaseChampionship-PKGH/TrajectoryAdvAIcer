using ClosedXML.Excel;
using TrajectoryAdvAIcer.Entities.Enums;
using TrajectoryAdvAIcer.Entities.Models;
using TrajectoryAdvAIcer.Parsing.Contracts.Enums;
using TrajectoryAdvAIcer.Parsing.Contracts.Exceptions;
using TrajectoryAdvAIcer.Parsing.Contracts.Helpers;
using TrajectoryAdvAIcer.Parsing.Contracts.Interfaces;
using TrajectoryAdvAIcer.Parsing.Contracts.Models;

namespace TrajectoryAdvAIcer.Parsing.Excel;

/// <summary>
/// Excel парсер истории обучения
/// </summary>
public class LearningHistoryExcelParser : IDataParser
{
    private static readonly IReadOnlyList<string> RequiredHeaders = new[]
    {
        "ФИО", "Должность", "ИОГВ", "Курс", "Статус"
    };

    private static readonly List<string> OptionalHeaders = new()
    {
        "Тип"
    };

    /// <inheritdoc />
    public InputFormat Format => InputFormat.Excel;

    /// <inheritdoc />
    public ParsingTarget Target => ParsingTarget.LearningHistory;

    /// <inheritdoc />
    public Task<ParseResult<T>> ParseAsync<T>(Stream input)
    {
        if (typeof(T) != typeof(LearningHistory))
        {
            throw new InvalidOperationException("LearningHistoryExcelParser ожидает тип LearningHistory");
        }

        using var workbook = new XLWorkbook(input);
        var (ws, columns) = ResolveDataSheet(workbook);

        var employees = new Dictionary<string, Employee>();
        var records = new List<LearningRecord>();
        var warnings = new List<string>();

        if (!columns.ContainsKey("Тип"))
        {
            warnings.Add("В файле истории обучения отсутствует столбец «Тип», тип курса всех записей установлен как «ЭК».");
        }

        var lastRow = ws.LastRowUsed()?.RowNumber() ?? 1;
        for (var row = 2; row <= lastRow; row++)
        {
            var fio = GetString(ws, row, columns, "ФИО");
            if (string.IsNullOrWhiteSpace(fio))
            {
                continue;
            }

            var position = GetString(ws, row, columns, "Должность");
            var iogv = GetString(ws, row, columns, "ИОГВ");
            var courseTitle = GetString(ws, row, columns, "Курс");
            var statusStr = GetString(ws, row, columns, "Статус");

            if (string.IsNullOrWhiteSpace(position) || string.IsNullOrWhiteSpace(iogv) ||
                string.IsNullOrWhiteSpace(courseTitle) || string.IsNullOrWhiteSpace(statusStr))
            {
                continue;
            }

            var employeeKey = $"{fio}|{position}|{iogv}";
            if (!employees.TryGetValue(employeeKey, out var employee))
            {
                employee = new Employee
                {
                    Id = StableIdProvider.GenerateStableId(employeeKey),
                    FullName = fio,
                    Position = position,
                    IOGV = iogv
                };
                employees[employeeKey] = employee;
            }

            var courseId = StableIdProvider.GenerateStableId(courseTitle);

            var typeStr = GetString(ws, row, columns, "Тип");
            var type = string.IsNullOrWhiteSpace(typeStr) ? CourseType.EK : typeStr switch
            {
                "ЭК" => CourseType.EK,
                "ППК" => CourseType.PPK,
                _ => throw new ParsingException($"Неизвестный тип курса: {typeStr}")
            };

            var status = statusStr switch
            {
                "Пройден" => CompletionStatus.Passed,
                "Не пройден" => CompletionStatus.NotPassed,
                _ => CompletionStatus.InProgress
            };

            records.Add(new LearningRecord
            {
                EmployeeId = employee.Id,
                CourseId = courseId,
                CourseTitle = courseTitle,
                Type = type,
                Status = status
            });
        }

        var history = new LearningHistory
        {
            Employees = employees.Values.ToList(),
            Records = records
        };

        return Task.FromResult(new ParseResult<T>((T)(object)history, warnings));
    }

    private static (IXLWorksheet Sheet, IReadOnlyDictionary<string, int> Columns) ResolveDataSheet(XLWorkbook workbook)
    {
        foreach (var worksheet in workbook.Worksheets)
        {
            var columns = DetectColumns(worksheet);
            if (RequiredHeaders.All(h => columns.ContainsKey(h)))
            {
                return (worksheet, columns);
            }
        }

        throw new ParsingException(
            $"Не удалось найти лист с данными истории обучения. " +
            $"Требуются столбцы: {string.Join(", ", RequiredHeaders)}.");
    }

    private static IReadOnlyDictionary<string, int> DetectColumns(IXLWorksheet ws)
    {
        var columns = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        var lastColumn = ws.LastColumnUsed()?.ColumnNumber() ?? 1;
        var lastRow = ws.LastRowUsed()?.RowNumber() ?? 1;

        for (var col = 1; col <= lastColumn; col++)
        {
            var header = ws.Cell(1, col).GetString().Trim();
            if (string.IsNullOrWhiteSpace(header))
            {
                continue;
            }

            if (!columns.ContainsKey(header) &&
                (RequiredHeaders.Contains(header, StringComparer.OrdinalIgnoreCase) ||
                 OptionalHeaders.Contains(header, StringComparer.OrdinalIgnoreCase)))
            {
                columns[header] = col;
            }
        }

        if (!columns.ContainsKey("Тип"))
        {
            var typeColumn = DetectTypeColumnByValues(ws, lastRow, lastColumn);
            if (typeColumn.HasValue)
            {
                columns["Тип"] = typeColumn.Value;
            }
        }

        return columns;
    }

    private static int? DetectTypeColumnByValues(IXLWorksheet ws, int lastRow, int lastColumn)
    {
        var sampleEnd = Math.Min(lastRow, 100);
        for (var col = 1; col <= lastColumn; col++)
        {
            var seenTypeValue = false;
            var allTypeValues = true;

            for (var row = 2; row <= sampleEnd; row++)
            {
                var value = ws.Cell(row, col).GetString().Trim();
                if (string.IsNullOrWhiteSpace(value))
                {
                    continue;
                }

                seenTypeValue = true;
                if (value is not ("ЭК" or "ППК"))
                {
                    allTypeValues = false;
                    break;
                }
            }

            if (seenTypeValue && allTypeValues)
            {
                return col;
            }
        }

        return null;
    }

    private static string GetString(IXLWorksheet ws, int row, IReadOnlyDictionary<string, int> columns, string key)
    {
        if (!columns.TryGetValue(key, out var col))
        {
            return string.Empty;
        }

        return ws.Cell(row, col).GetString().Trim();
    }
}