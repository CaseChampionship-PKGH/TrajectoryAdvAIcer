using System.Text.RegularExpressions;
using TrajectoryAdvAIcer.Entities.Models;

namespace TrajectoryAdvAIcer.Parsing.Contracts.Helpers;

/// <summary>
/// Парсер программы
/// </summary>
public static class ProgramInfoParser
{
    /// <summary>
    /// Распарсить программу
    /// </summary>
    public static ProgramInfo Parse(string fileName)
    {
        var info = new ProgramInfo();

        var nameWithoutExt = Path.GetFileNameWithoutExtension(fileName);

        var match = Regex.Match(nameWithoutExt, @"^[\d\.\-\s/_]+(?=[А-ЯЁA-Zа-яёa-z])");
        if (match.Success)
        {
            info.Period = match.Value.Trim(' ', '_', '-');
            info.Title = nameWithoutExt.Substring(match.Index + match.Length).TrimStart(' ', '_', '-');
        }
        else
        {
            info.Title = nameWithoutExt;
        }

        return info;
    }
}
