using TrajectoryAdvAIcer.Parsing.Contracts.Enums;
using TrajectoryAdvAIcer.Parsing.Contracts.Exceptions;
using TrajectoryAdvAIcer.Parsing.Contracts.Interfaces;

namespace TrajectoryAdvAIcer.Parsing;

/// <summary>
/// <inheritdoc cref="IParserFactory"/>
/// </summary>
public class FormatDetector : IFormatDetector
{
    private readonly static byte[] jsonStartBytes = { 0x5B };  // '['
    private readonly static byte[] jsonStartBytes2 = { 0x7B }; // '{'
    private readonly static byte[] zipSignature = { 0x50, 0x4B, 0x03, 0x04 }; // PK..
    private readonly static byte[] gZipSignature = { 0x1F, 0x8B }; // GZip

    private readonly static HashSet<string> csvExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".csv", ".tsv", ".txt"
    };

    private readonly static HashSet<string> jsonExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".json"
    };

    private readonly static HashSet<string> excelExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".xlsx", ".xlsm", ".xls"
    };

    private readonly static HashSet<string> archiveExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".zip", ".gz", ".tar", ".7z"
    };

    /// <summary>
    /// Определяет формат входных данных.
    /// Приоритет: расширение файла -> сигнатура содержимого -> эвристики.
    /// </summary>
    /// <param name="fileName">Имя файла с расширением.</param>
    /// <param name="content">Поток с содержимым (позиция будет сброшена).</param>
    /// <returns>Определённый формат.</returns>
    /// <exception cref="ParsingException">Если формат не удалось определить.</exception>
    public InputFormat DetectFormat(string fileName, Stream content)
    {
        if (string.IsNullOrWhiteSpace(fileName) && content is not { Length: > 0 })
        {
            throw new ParsingException("Не указаны ни имя файла, ни содержимое для определения формата.");
        }

        var extension = Path.GetExtension(fileName)?.ToLowerInvariant() ?? string.Empty;

        if (jsonExtensions.Contains(extension))
        {
            return InputFormat.Json;
        }

        if (csvExtensions.Contains(extension))
        {
            return InputFormat.Csv;
        }

        if (excelExtensions.Contains(extension))
        {
            return InputFormat.Excel;
        }

        if (archiveExtensions.Contains(extension))
        {
            return InputFormat.Archive;
        }

        if (content is { Length: > 0 })
        {
            content.Position = 0;
            var signature = ReadSignature(content, 10);

            if (IsJsonContent(signature))
            {
                return InputFormat.Json;
            }

            if (IsArchiveContent(signature))
            {
                return InputFormat.Archive;
            }

            if (IsLikelyCsvContent(signature))
            {
                return InputFormat.Csv;
            }
        }

        // 3. Последняя попытка по тексту
        if (string.IsNullOrWhiteSpace(fileName) && content is { Length: > 0 })
        {
            content.Position = 0;
            using var reader = new StreamReader(content, leaveOpen: true);
            var firstLine = reader.ReadLine()?.Trim() ?? string.Empty;
            content.Position = 0;

            if (firstLine.StartsWith('[') || firstLine.StartsWith('{'))
            {
                return InputFormat.Json;
            }

            if (firstLine.Contains(',') || firstLine.Contains(';'))
            {
                return InputFormat.Csv;
            }
        }

        throw new ParsingException(
            $"Не удалось определить формат данных. Имя файла: '{fileName ?? "отсутствует"}'. " +
            $"Поддерживаемые расширения: .json, .csv, .xlsx, .zip.");
    }

    /// <summary>
    /// Читает первые N байт из потока для анализа сигнатуры.
    /// </summary>
    private static byte[] ReadSignature(Stream stream, int length)
    {
        var buffer = new byte[length];
        var bytesRead = stream.Read(buffer, 0, length);
        stream.Position = 0;

        if (bytesRead < length)
        {
            var trimmed = new byte[bytesRead];
            Array.Copy(buffer, trimmed, bytesRead);
            return trimmed;
        }

        return buffer;
    }

    /// <summary>
    /// Проверяет, является ли содержимое JSON-документом.
    /// </summary>
    private static bool IsJsonContent(byte[] signature)
    {
        if (signature.Length == 0)
        {
            return false;
        }

        // JSON массив: начинается с '['
        if (signature[0] == jsonStartBytes[0])
        {
            return true;
        }

        // JSON объект: начинается с '{'
        if (signature[0] == jsonStartBytes2[0])
        {
            return true;
        }

        // Может быть с BOM (UTF-8 BOM: EF BB BF, затем [ или {)
        if (signature.Length >= 4 &&
            signature[0] == 0xEF && signature[1] == 0xBB && signature[2] == 0xBF &&
            (signature[3] == 0x5B || signature[3] == 0x7B))
        {
            return true;
        }

        // UTF-16 LE BOM: FF FE
        if (signature.Length >= 3 &&
            signature[0] == 0xFF && signature[1] == 0xFE)
        {
            return true;
        }

        // UTF-16 BE BOM: FE FF
        if (signature.Length >= 3 &&
            signature[0] == 0xFE && signature[1] == 0xFF)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Проверяет, является ли содержимое архивом.
    /// </summary>
    private static bool IsArchiveContent(byte[] signature)
    {
        if (signature.Length < 4)
        {
            return false;
        }

        // ZIP (PK..)
        if (signature[0] == zipSignature[0] &&
            signature[1] == zipSignature[1] &&
            signature[2] == zipSignature[2] &&
            signature[3] == zipSignature[3])
        {
            return true;
        }

        // GZip (1F 8B)
        if (signature.Length >= 2 &&
            signature[0] == gZipSignature[0] &&
            signature[1] == gZipSignature[1])
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Эвристическая проверка на CSV-контент.
    /// </summary>
    private static bool IsLikelyCsvContent(byte[] signature)
    {
        if (signature.Length == 0)
        {
            return false;
        }

        // Конвертируем в строку для анализа первой строки
        var firstBytes = signature.Take(Math.Min(signature.Length, 200)).ToArray();
        var text = System.Text.Encoding.UTF8.GetString(firstBytes);

        // Убираем BOM если есть
        if (text.StartsWith('\uFEFF'))
        {
            text = text[1..];
        }

        var firstLine = text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();

        if (string.IsNullOrWhiteSpace(firstLine))
        {
            return false;
        }

        // Признаки CSV: содержит разделители (запятые, точки с запятой, табы)
        var commaCount = firstLine.Count(c => c == ',');
        var semicolonCount = firstLine.Count(c => c == ';');
        var tabCount = firstLine.Count(c => c == '\t');

        // Если есть хотя бы один разделитель — вероятно CSV
        if (commaCount > 0 || semicolonCount > 0 || tabCount > 0)
        {
            return true;
        }

        // Если строка в кавычках и внутри разделители — тоже CSV
        if (firstLine.StartsWith('"') && firstLine.Contains("\",\""))
        {
            return true;
        }

        return false;
    }
}
