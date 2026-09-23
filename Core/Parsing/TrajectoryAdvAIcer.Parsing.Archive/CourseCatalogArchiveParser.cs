using System.IO.Compression;
using TrajectoryAdvAIcer.Entities.Models;
using TrajectoryAdvAIcer.Parsing.Contracts.Enums;
using TrajectoryAdvAIcer.Parsing.Contracts.Exceptions;
using TrajectoryAdvAIcer.Parsing.Contracts.Interfaces;
using TrajectoryAdvAIcer.Parsing.Contracts.Models;
using TrajectoryAdvAIcer.Parsing.Csv;
using TrajectoryAdvAIcer.Parsing.Excel;
using TrajectoryAdvAIcer.Parsing.Json;

namespace TrajectoryAdvAIcer.Parsing.Archive;

/// <summary>
/// Парсер реестра курсов по архиву папки
/// </summary>
public class CourseCatalogArchiveParser : IDataParser
{
    private readonly CourseCatalogCsvParser csvParser;
    private readonly CourseCatalogExcelParser excelParser;
    private readonly CourseCatalogJsonParser jsonParser;
    private readonly IFormatDetector formatDetector;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="CourseCatalogArchiveParser "/>
    /// </summary>
    public CourseCatalogArchiveParser(CourseCatalogCsvParser csvParser,
        CourseCatalogExcelParser excelParser,
        CourseCatalogJsonParser jsonParser,
        IFormatDetector formatDetector)
    {
        this.csvParser = csvParser;
        this.excelParser = excelParser;
        this.jsonParser = jsonParser;
        this.formatDetector = formatDetector;
    }

    /// <inheritdoc />
    public InputFormat Format => InputFormat.Archive;

    /// <inheritdoc />
    public ParsingTarget Target => ParsingTarget.CourseCatalog;

    async Task<ParseResult<T>> IDataParser.ParseAsync<T>(Stream input)
    {
        if (typeof(T) != typeof(CourseCatalog))
        {
            throw new InvalidOperationException("CourseCatalogArchiveParser ожидает тип CourseCatalog");
        }

        var allResults = new CourseCatalog();
        var warnings = new List<string>();
        using var archive = new ZipArchive(input, ZipArchiveMode.Read, leaveOpen: true);

        foreach (var entry in archive.Entries)
        {
            if (entry.Length == 0 || entry.Name.EndsWith('/'))
            {
                continue;
            }

            using var entryStream = entry.Open();
            using var ms = new MemoryStream();
            await entryStream.CopyToAsync(ms);
            ms.Position = 0;

            InputFormat format;
            try
            {
                format = formatDetector.DetectFormat(entry.Name, ms);
            }
            catch (ParsingException)
            {
                continue;
            }

            if (format == InputFormat.Archive)
            {
                continue;
            }

            ParseResult<CourseCatalog>? result = null;

            if (format == InputFormat.Csv)
            {
                result = await csvParser.ParseAsync<CourseCatalog>(ms);
            }
            else if (format == InputFormat.Excel)
            {
                result = await excelParser.ParseAsync<CourseCatalog>(ms);
            }
            else if (format == InputFormat.Json)
            {
                result = await jsonParser.ParseAsync<CourseCatalog>(ms);
            }

            if (result != null)
            {
                warnings.AddRange(result.Warnings);
                allResults.Courses.AddRange(result.Data.Courses);
            }
        }

        return new ParseResult<T>((T)(object)allResults, warnings);
    }
}
