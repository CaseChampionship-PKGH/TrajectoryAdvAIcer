using System.Globalization;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using TrajectoryAdvAIcer.Entities.Models;
using TrajectoryAdvAIcer.Parsing.Contracts.Enums;
using TrajectoryAdvAIcer.Parsing.Contracts.Helpers;
using TrajectoryAdvAIcer.Parsing.Contracts.Interfaces;

namespace TrajectoryAdvAIcer.Parsing.Csv;

/// <summary>
/// Csv парсер реестра курсов
/// </summary>
public class CourseCatalogCsvParser : IDataParser
{
    /// <inheritdoc />
    public InputFormat Format => InputFormat.Csv;

    /// <inheritdoc />
    public ParsingTarget Target => ParsingTarget.CourseCatalog;

    /// <inheritdoc />
    public async Task<T> ParseAsync<T>(Stream input)
    {
        if (typeof(T) != typeof(CourseCatalog))
        {
            throw new InvalidOperationException("CourseCatalogCsvParser ожидает тип CourseCatalog");
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
        if (headers == null || headers.Length < 4)
        {
            throw new InvalidDataException("CSV-файл реестра курсов должен содержать хотя бы 4 столбца.");
        }

        var courses = new List<Course>();
        while (csv.Read())
        {
            var title = csv.GetField(0)?.Trim();
            if (string.IsNullOrWhiteSpace(title))
            {
                continue;
            }

            var course = new Course
            {
                Id = StableIdProvider.GenerateStableId(title),
                Title = title,
                Annotation = csv.GetField(1)?.Trim() ?? "",
                Goals = csv.GetField(2)?.Trim() ?? "",
                Results = csv.GetField(3)?.Trim() ?? ""
            };
            courses.Add(course);
        }
        return (T)(object)new CourseCatalog { Courses = courses };
    }
}
