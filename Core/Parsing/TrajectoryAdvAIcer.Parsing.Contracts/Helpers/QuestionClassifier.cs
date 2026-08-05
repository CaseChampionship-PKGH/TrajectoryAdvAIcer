using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using TrajectoryAdvAIcer.Entities.Enums;

namespace TrajectoryAdvAIcer.Parsing.Contracts.Helpers;

/// <summary>
/// Классификатор вопросов анкеты
/// </summary>
public static class QuestionClassifier
{
    private readonly static Regex liWordRegex = new(@"\bли\b", RegexOptions.IgnoreCase);

    /// <summary>
    /// Сгенерировать id для вопроса
    /// </summary>
    public static string GenerateQuestionId(string questionText)
    {
        var hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(questionText));
        return Convert.ToBase64String(hashBytes)[..12];
    }

    /// <summary>
    /// Отклассифицировать вопрос
    /// </summary>
    public static QuestionType ClassifyQuestionType(string questionText)
    {
        if (string.IsNullOrWhiteSpace(questionText))
        {
            return QuestionType.OpenText;
        }

        var lower = questionText.ToLowerInvariant();

        if (lower.Contains("10-балльной")
            || lower.Contains("10 балльной")
            || lower.Contains("оцените")
            || lower.Contains("10 — балльной"))
        {
            return QuestionType.Numeric;
        }

        if (liWordRegex.IsMatch(lower) ||
            lower.Contains("да/нет") ||
            lower.Contains("да или нет") ||
            lower.Contains("бинарный"))
        {
            return QuestionType.Binary;
        }

        return QuestionType.OpenText;
    }
}
