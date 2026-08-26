using System.Globalization;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using TrajectoryAdvAIcer.Entities.Enums;
using TrajectoryAdvAIcer.Entities.Models;
using TrajectoryAdvAIcer.Parsing.Contracts.Enums;
using TrajectoryAdvAIcer.Parsing.Contracts.Exceptions;
using TrajectoryAdvAIcer.Parsing.Contracts.Helpers;
using TrajectoryAdvAIcer.Parsing.Contracts.Interfaces;

namespace TrajectoryAdvAIcer.Parsing.Csv;

/// <summary>
/// Csv парсер истории обучения
/// </summary>
public class LearningHistoryCsvParser : IDataParser
{
    /// <inheritdoc />
    public InputFormat Format => InputFormat.Csv;

    /// <inheritdoc />
    public ParsingTarget Target => ParsingTarget.LearningHistory;

    /// <inheritdoc />
    public async Task<T> ParseAsync<T>(Stream input)
    {
        if (typeof(T) != typeof(LearningHistory))
        {
            throw new InvalidOperationException("LearningHistoryCsvParser ожидает тип LearningHistory");
        }

        var encoding = Encoding.GetEncoding("windows-1251");
        using var reader = new StreamReader(input, encoding, detectEncodingFromByteOrderMarks: false);
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ";",
            HasHeaderRecord = true,
            TrimOptions = TrimOptions.Trim,
            BadDataFound = null
        };

        using var csv = new CsvReader(reader, config);
        csv.Read();
        csv.ReadHeader();
        var headers = csv.HeaderRecord;
        if (headers == null || headers.Length < 5)
        {
            throw new InvalidDataException("CSV-файл истории обучения должен содержать хотя бы 5 столбцов.");
        }

        var employees = new Dictionary<string, Employee>();
        var records = new List<LearningRecord>();

        while (csv.Read())
        {
            var fio = csv.GetField(0);
            if (string.IsNullOrWhiteSpace(fio))
            {
                continue;
            }

            var position = csv.GetField(1)?.Trim();
            var iogv = csv.GetField(2)?.Trim();
            var typeStr = csv.GetField(3)?.Trim();
            var courseTitle = csv.GetField(4)?.Trim();
            var statusStr = csv.GetField(5)?.Trim();

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
        return (T)(object)history;
    }
}
