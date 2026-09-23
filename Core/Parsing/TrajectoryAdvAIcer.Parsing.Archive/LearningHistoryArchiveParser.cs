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
/// Парсер истории обучения по архиву папки
/// </summary>
public class LearningHistoryArchiveParser : IDataParser
{
    private readonly LearningHistoryCsvParser csvParser;
    private readonly LearningHistoryExcelParser excelParser;
    private readonly LearningHistoryJsonParser jsonParser;
    private readonly IFormatDetector formatDetector;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="LearningHistoryArchiveParser "/>
    /// </summary>
    public LearningHistoryArchiveParser(LearningHistoryCsvParser csvParser,
        LearningHistoryExcelParser excelParser,
        LearningHistoryJsonParser jsonParser,
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
    public ParsingTarget Target => ParsingTarget.LearningHistory;

    async Task<ParseResult<T>> IDataParser.ParseAsync<T>(Stream input)
    {
        if (typeof(T) != typeof(LearningHistory))
        {
            throw new InvalidOperationException("LearningHistoryArchiveParser ожидает тип LearningHistory");
        }

        var allResults = new LearningHistory();
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

            ParseResult<LearningHistory>? result = null;

            if (format == InputFormat.Csv)
            {
                result = await csvParser.ParseAsync<LearningHistory>(ms);
            }
            else if (format == InputFormat.Excel)
            {
                result = await excelParser.ParseAsync<LearningHistory>(ms);
            }
            else if (format == InputFormat.Json)
            {
                result = await jsonParser.ParseAsync<LearningHistory>(ms);
            }

            if (result != null)
            {
                warnings.AddRange(result.Warnings);
                allResults.Employees.AddRange(result.Data.Employees);
                allResults.Records.AddRange(result.Data.Records);
            }
        }

        return new ParseResult<T>((T)(object)allResults, warnings);
    }
}
