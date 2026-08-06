using System.IO.Compression;
using TrajectoryAdvAIcer.Entities.Models;
using TrajectoryAdvAIcer.Parsing.Contracts.Enums;
using TrajectoryAdvAIcer.Parsing.Contracts.Exceptions;
using TrajectoryAdvAIcer.Parsing.Contracts.Interfaces;
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

    async Task<T> IDataParser.ParseAsync<T>(Stream input)
    {
        if (typeof(T) != typeof(LearningHistory))
        {
            throw new InvalidOperationException("LearningHistoryArchiveParser ожидает тип LearningHistory");
        }

        var allResults = new LearningHistory();
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

            LearningHistory? parsed = null;

            if (format == InputFormat.Csv)
            {
                parsed = await csvParser.ParseAsync<LearningHistory>(ms);
            }
            else if (format == InputFormat.Excel)
            {
                parsed = await excelParser.ParseAsync<LearningHistory>(ms);
            }
            else if (format == InputFormat.Json)
            {
                parsed = await jsonParser.ParseAsync<LearningHistory>(ms);
            }

            if (parsed != null)
            {
                allResults.Employees.AddRange(parsed.Employees);
                allResults.Records.AddRange(parsed.Records);
            }
        }

        return (T)(object)allResults;
    }
}
