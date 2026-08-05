namespace TrajectoryAdvAIcer.Services.Contracts.Constants;

/// <summary>
/// Константы для анализа анкет
/// </summary>
public class SurveyAnalysisConstants
{
    /// <summary>
    /// Критерий "Полезность программы"
    /// </summary>
    public readonly static string Usefulness = "Полезность программы";

    /// <summary>
    /// Критерий "Практико-ориентированность программы"
    /// </summary>
    public readonly static string Practicality = "Практико-ориентированность программы";

    /// <summary>
    /// Критерий "Доступность материалов по программе"
    /// </summary>
    public readonly static string Accessibility = "Доступность материалов по программе";

    /// <summary>
    /// Критерий "Взаимодействие с командой КУ"
    /// </summary>
    public readonly static string Interaction = "Взаимодействие с командой КУ";

    /// <summary>
    /// Критерий "Вовлеченность в образовательный процесс"
    /// </summary>
    public readonly static string Engagement = "Вовлеченность в образовательный процесс";

    /// <summary>
    /// Имена критериев
    /// </summary>
    public readonly static string[] CriteriaNames = [Usefulness, Practicality, Accessibility, Interaction, Engagement];

    /// <summary>
    /// Ключевые слова для распределения вопросов по критериям
    /// </summary>
    public readonly static Dictionary<string, string> CriterionKeywords = new()
    {
        [Usefulness] = "полезност|наиболее актуальны|ожидаемый эффект|исключить из программы|дополнить программу|почему.*решили пройти",
        [Practicality] = "практико-ориентированность|практических заданий|практической отработки|организации практических занятий",
        [Accessibility] = "доступность|доступным и понятным|последовательность тем и логик|задать интересующие Вас вопросы",
        [Interaction] = "взаимодействие с командой|Корпоративного университета",
        [Engagement] = "отстраненность|потерю интереса|вовлеченность|повысить Вашу вовлеченность"
    };
}
