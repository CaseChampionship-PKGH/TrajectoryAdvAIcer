using ClosedXML.Excel;
using TrajectoryAdvAIcer.Entities.Enums;
using TrajectoryAdvAIcer.Entities.Models;
using TrajectoryAdvAIcer.Parsing.Contracts.Enums;
using TrajectoryAdvAIcer.Parsing.Contracts.Exceptions;
using TrajectoryAdvAIcer.Parsing.Contracts.Helpers;
using TrajectoryAdvAIcer.Parsing.Contracts.Interfaces;

namespace TrajectoryAdvAIcer.Parsing.Excel;

/// <summary>
/// Excel парсер истории обучения
/// </summary>
public class LearningHistoryExcelParser : IDataParser
{
    /// <inheritdoc />
    public InputFormat Format => InputFormat.Excel;

    /// <inheritdoc />
    public ParsingTarget Target => ParsingTarget.LearningHistory;

    /// <inheritdoc />
    public Task<T> ParseAsync<T>(Stream input)
    {
        if (typeof(T) != typeof(LearningHistory))
        {
            throw new InvalidOperationException("LearningHistoryExcelParser ожидает тип LearningHistory");
        }

        using var workbook = new XLWorkbook(input);
        var ws = workbook.Worksheet(1);

        var employees = new Dictionary<string, Employee>();
        var records = new List<LearningRecord>();

        var lastRow = ws.LastRowUsed()?.RowNumber() ?? 1;
        for (var row = 2; row <= lastRow; row++)
        {
            var fio = ws.Cell(row, 1).GetString().Trim();
            if (string.IsNullOrWhiteSpace(fio))
            {
                continue;
            }

            var position = ws.Cell(row, 2).GetString().Trim();
            var iogv = ws.Cell(row, 3).GetString().Trim();
            var typeStr = ws.Cell(row, 4).GetString().Trim();
            var courseTitle = ws.Cell(row, 5).GetString().Trim();
            var statusStr = ws.Cell(row, 6).GetString().Trim();

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

            var type = typeStr switch
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
                Type = type,
                Status = status
            });
        }

        var history = new LearningHistory
        {
            Employees = employees.Values.ToList(),
            Records = records
        };

        return Task.FromResult((T)(object)history);
    }
}
