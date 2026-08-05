using System.Text.RegularExpressions;

namespace TrajectoryAdvAIcer.Services.Contracts.Helpers;

/// <summary>
/// Отсеиватель неинформативных ответов
/// </summary>
public static partial class AnswerQuality
{
    /// <summary>
    /// Эквиваленты отсутствия ответа для отсекания всех не информативных ответов
    /// </summary>
    private readonly static HashSet<string> dashEquivalents = new(StringComparer.OrdinalIgnoreCase)
    {
        "нет ответа", "отсутствует", "отсутствуют", "прочерк", "нет ответа, все ок",
        "все норм", "все хорошо", "все отлично", "все раскрыто", "всего достаточно",
        "все супер", "достаточно по объему", "программа достаточно полная", "все актуально",
        "пока достаточно", "затрудняюсь ответить", "затрудняюсь ответить.",
        "не знаю", "не готов ответить", "не готов ответить.", "нет ответа, все ок",
        "всё раскрыто", "всего достаточно", "информации достаточно",
        "курс полон", "все полезно", "всё полезно", "все оставить", "все норм",
        "все хорошо организовано", "организация достаточна",
        "Никакие", "ни какие", "Ничего не нужно исключать", "Ничего исключать не надо", "все полезны",
        "Оставить все.", "Оставить все", "затрудняюсь с ответом",
        "все темы актуальны", "таких нет", "все темы важны", "Все вопросы важны", "все нужны",
    };

    private static readonly string[] injectionPatterns =
    [
        "ignore previous instructions",
        "ignore all instructions",
        "new instructions:",
        "system prompt:",
        "ты должен",
        "твоя задача",
        "теперь ты",
        "действуй как",
        "ответь мне как",
        "следуй инструкции",
        "отвечай как"
    ];

    /// <summary>
    /// Проверить на потенциальный prompt injection
    /// </summary>
    public static bool IsPotentialInjection(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return false;
        }

        var lower = text.ToLowerInvariant();
        return injectionPatterns.Any(lower.Contains);
    }

    /// <summary>
    /// Паттерн нахождения эквивалентов прочерков
    /// </summary>
    private readonly static Regex dashPattern = DashEquivalent();

    private readonly static Dictionary<string, HashSet<string>> questionSpecificFilters = new()
    {
        // Вопросы о применении знаний/навыков
        ["знания.*примен|сможете применить|умения.*навыки.*полученные"] = new(StringComparer.OrdinalIgnoreCase)
        {
            "все", "всё", "абсолютно все", "практически все", "большинство",
            "нового ничего", "никакие", "не знаю", "ничего", "ничего нового",
            "все знания применяю", "все полезны и применяемы",
            "все навыки", "все навыки.", "все", "все, очень интересно"
        },

        // Вопрос "Какие вопросы были наиболее актуальны?" – убираем только совсем общие фразы
        ["наиболее актуальны"] = new(StringComparer.OrdinalIgnoreCase)
        {
            "все", "всё", "все вопросы", "все вопросы актуальны",
            "все темы актуальны", "все актуально", "абсолютно все"
        },

        // Вопрос "Что бы Вы хотели изменить в организации практических занятий?" – часто пишут "ничего"
        ["изменить в организации"] = new(StringComparer.OrdinalIgnoreCase)
        {
            "ничего", "ничего менять не нужно", "все устраивает", "ничего не менять",
            "все хорошо", "все отлично", "организация достаточна", "замечаний нет"
        }
    };

    /// <summary>
    /// Универсальная проверка (без контекста вопроса)
    /// </summary>
    public static bool IsNonInformative(string? text) => IsNonInformativeForQuestion(null, text);

    /// <summary>
    /// Проверка с учётом контекста вопроса
    /// </summary>
    public static bool IsNonInformativeForQuestion(string? questionText, string? text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return true;
        }

        // Проверка на прочерки и общие паттерны
        if (text.StartsWith('-') || text.StartsWith('–') || text.StartsWith('—') || text.StartsWith('_'))
        {
            return true;
        }

        if (dashEquivalents.Contains(text))
        {
            return true;
        }

        if (dashPattern.IsMatch(text))
        {
            return true;
        }

        // Дополнительные фильтры по контексту вопроса
        if (!string.IsNullOrWhiteSpace(questionText))
        {
            foreach (var kvp in questionSpecificFilters)
            {
                if (Regex.IsMatch(questionText, kvp.Key, RegexOptions.IgnoreCase))
                {
                    if (kvp.Value.Contains(text))
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    [GeneratedRegex(@"^(затрудняюсь\s*ответит[ь]?|не\s*готов[а]?\s*ответит[ь]?|не\s*готова?\s*ответит[ь]?|вс[её]\s*(норм|хорошо|отлично|раскрыто|супер|полезно|актуально)|всего\s*(достаточно|хватило)|информации\s*достаточно|программа\s*достаточно\s*полная|пока\s*достаточно|курс\s*полон|организация\s*достаточна|пожеланий\s*нет|нет\s*ответа|нет\s*пожеланий|ничего\s*менять\s*не\s*нужно)", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline, "ru-RU")]
    private static partial Regex DashEquivalent();
}
