using System.IO.Compression;
using System.Text;
using LearnAInalytics.Parsing.Contracts.Enums;
using LearnAInalytics.Parsing.Contracts.Exceptions;
using LearnAInalytics.Parsing.Contracts.Interfaces;
using LearnAInalytics.Parsing.Contracts.Models;
using LearnAInalytics.Parsing.Csv;
using LearnAInalytics.Parsing.Excel;

namespace LearnAInalytics.Parsing.Archive;

/// <summary>
/// Парсер ответов тестируемых по архиву папки
/// </summary>
public class SurveyArchiveParser : IDataParser
{
    private readonly SurveyCsvParser csvParser;
    private readonly SurveyExcelParser excelParser;
    private readonly IFormatDetector formatDetector;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="SurveyArchiveParser"/>
    /// </summary>
    public SurveyArchiveParser(SurveyCsvParser csvParser, SurveyExcelParser excelParser, IFormatDetector formatDetector)
    {
        this.csvParser = csvParser;
        this.excelParser = excelParser;
        this.formatDetector = formatDetector;
    }

    /// <inheritdoc />
    public InputFormat Format => InputFormat.Archive;

    /// <inheritdoc />
    public ParsingTarget Target => ParsingTarget.Survey;

    async Task<T> IDataParser.ParseAsync<T>(Stream input, string fileName)
    {
        if (typeof(T) != typeof(List<SurveyParseResult>))
        {
            throw new ParsingException(
                $"Парсер папки не поддерживает тип {typeof(T).Name}. Ожидался List<SurveyParseResult>).");
        }

        var allResults = new List<SurveyParseResult>();
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

            List<SurveyParseResult>? parsed = null;

            if (format == InputFormat.Excel)
            {
                parsed = await excelParser.ParseAsync<List<SurveyParseResult>>(ms, FixEntryName(entry.Name));
            }
            else if (format == InputFormat.Csv)
            {
                parsed = await csvParser.ParseAsync<List<SurveyParseResult>>(ms, FixEntryName(entry.Name));
            }

            if (parsed != null)
            {
                allResults.AddRange(parsed);
            }
        }

        return (T)(object)allResults;
    }

    private static string FixEntryName(string corruptedName)
    {
        if (string.IsNullOrWhiteSpace(corruptedName))
        {
            return corruptedName;
        }

        // Если в имени нет вопросительных знаков или других явных признаков порчи,
        // считаем, что оно уже в UTF-8 (или корректной кодировке)
        if (!ContainsReplacementCharacters(corruptedName))
        {
            return corruptedName;
        }

        // Пытаемся перекодировать через распространённые русские кодовые страницы
        var possibleEncodings = new[]
        {
            Encoding.GetEncoding(866),   // OEM Cyrillic (DOS)
            Encoding.GetEncoding(1251),  // Windows-1251
            Encoding.GetEncoding(20866), // KOI8-R
            Encoding.GetEncoding(28595), // ISO 8859-5
            Encoding.UTF8
        };

        foreach (var enc in possibleEncodings)
        {
            // Получаем байты "как есть" в предположении, что исходная кодировка была enc
            var bytes = enc.GetBytes(corruptedName);
            // Конвертируем эти байты в UTF-8
            var decoded = Encoding.UTF8.GetString(bytes);
            // Если результат содержит кириллицу и не содержит знаков вопроса, считаем успешным
            if (!ContainsReplacementCharacters(decoded) && ContainsCyrillic(decoded))
            {
                return decoded;
            }
        }

        static bool ContainsReplacementCharacters(string text)
            => text.Contains('\uFFFD') || text.Contains('?');

        static bool ContainsCyrillic(string text)
            => text.Any(c => c >= 'а' && c <= 'я' || c >= 'А' && c <= 'Я' || c == 'ё' || c == 'Ё');

        // Если ничего не помогло, возвращаем исходное (пусть с вопросами)
        return corruptedName;
    }
}
