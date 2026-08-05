using System.Security.Cryptography;
using System.Text;
using TrajectoryAdvAIcer.Entities.Enums;

namespace TrajectoryAdvAIcer.Parsing;

/// <summary>
/// Классификатор вопросов анкеты
/// </summary>
public static class QuestionClassifier
{
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
        var lower = questionText.ToLowerInvariant();

        if (lower.Contains("оцените") && lower.Contains("10-балльной шкале"))
        {
            return QuestionType.Numeric;
        }

        if (lower.Contains("чувствовали ли вы отстраненность"))
        {
            return QuestionType.Binary;
        }

        if (lower.Contains("вовлеченность") || lower.Contains("вовлечённость"))
        {
            return QuestionType.Binary;
        }

        return QuestionType.OpenText;
    }
}
