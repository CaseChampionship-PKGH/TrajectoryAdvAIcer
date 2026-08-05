using System.Text.RegularExpressions;
using TrajectoryAdvAIcer.Analysis.Contracts.Interfaces;
using TrajectoryAdvAIcer.Analysis.Contracts.Models;
using TrajectoryAdvAIcer.Analysis.Contracts.Models.Aggregation;
using TrajectoryAdvAIcer.Entities.Enums;
using TrajectoryAdvAIcer.Entities.Models;
using TrajectoryAdvAIcer.Services.Contracts.Constants;
using TrajectoryAdvAIcer.Services.Contracts.Helpers;

namespace TrajectoryAdvAIcer.Analysis;

/// <inheritdoc cref="ICriterionAggregator"/>
public class SurveyAggregator : ICriterionAggregator
{
    private readonly IStatisticsCalculator calculator;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="SurveyAggregator"/>
    /// </summary>
    public SurveyAggregator(IStatisticsCalculator calculator)
    {
        this.calculator = calculator;
    }

    AggregatedCriteriaData ICriterionAggregator.Aggregate(Survey survey, SurveyStatistics statistics)
    {
        var result = new AggregatedCriteriaData();
        var questions = survey.Questions;
        var responses = survey.Responses;

        // 1. Сопоставление вопросов критериям с учётом уточняющих
        var questionCriterionMap = new Dictionary<string, string>(); // QuestionId -> CriterionName
        string? lastNonClarifyingCriterion = null;

        foreach (var q in questions)
        {
            if (q.QuestionText.Contains("формат обучения", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            var criterion = DetermineCriterion(q.QuestionText);
            if (criterion == null)
            {
                // Уточняющий вопрос – привязываем к предыдущему критерию
                if (lastNonClarifyingCriterion != null)
                {
                    criterion = lastNonClarifyingCriterion;
                }
                else
                {
                    continue; // не можем определить, пропускаем
                }
            }
            else
            {
                lastNonClarifyingCriterion = criterion;
            }

            questionCriterionMap[q.QuestionId] = criterion;
        }

        // 2. Формирование данных по каждому критерию
        foreach (var name in SurveyAnalysisConstants.CriteriaNames)
        {
            // Находим все вопросы этого критерия
            var criterionQuestions = questions
                .Where(q => questionCriterionMap.TryGetValue(q.QuestionId, out var c) && c == name)
                .ToList();

            var statQuestion = FindMainStatQuestion(criterionQuestions, name);
            var calculatedStatQuestion = statistics.Questions.FirstOrDefault(x => x.Question.QuestionId == statQuestion?.QuestionId);
            var criterionStat = calculatedStatQuestion ?? calculator.CalculateForQuestion(statQuestion, responses);

            var promptData = new CriterionPromptData
            {
                CriterionName = name,
                Statistics = criterionStat ?? new QuestionStatistics()
            };

            foreach (var q in criterionQuestions)
            {
                var questionWithAnswers = new QuestionWithAnswers { Question = q };

                // Собираем ответы
                foreach (var response in responses)
                {
                    var answer = response.Answers.FirstOrDefault(a => a.QuestionId == q.QuestionId);
                    if (answer == null)
                    {
                        continue;
                    }

                    string? textToAdd = null;
                    if (q.Type == QuestionType.OpenText && !string.IsNullOrWhiteSpace(answer.TextValue))
                    {
                        textToAdd = answer.TextValue;
                    }
                    else if (q.Type == QuestionType.Binary && !string.IsNullOrWhiteSpace(answer.TextValue))
                    {
                        // Уточняющий открытый ответ после бинарного: проверяем условие
                        // Родительский бинарный вопрос можно найти по предыдущему вопросу того же критерия
                        // Упростим: для уточняющих вопросов мы уже знаем, что они привязаны к тому же критерию,
                        // и их ответы нужно фильтровать по значению бинарного ответа.
                        // Определим родительский бинарный вопрос: в порядке следования вопросов ищем последний бинарный
                        var parentBinary = FindParentBinaryQuestion(criterionQuestions, q);
                        if (parentBinary != null)
                        {
                            var parentAnswer = response.Answers.FirstOrDefault(a => a.QuestionId == parentBinary.QuestionId);
                            if (parentAnswer != null && ShouldIncludeClarification(q.QuestionText, parentAnswer.BinaryValue))
                            {
                                textToAdd = answer.TextValue;
                            }
                        }
                        else
                        {
                            textToAdd = answer.TextValue; // нет родителя – добавляем всегда
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(textToAdd))
                    {
                        questionWithAnswers.Answers.Add(textToAdd);
                    }
                }

                if (questionWithAnswers.Answers.Count > 0)
                {
                    promptData.Questions.Add(questionWithAnswers);
                }
            }

            result.AllCriteriaData.Add(promptData);
        }

        // 3. Извлечение ExcludedTopics и SuggestedTopics
        result.ExcludedTopics = FilterDashResponses(ExtractOpenResponses(questions, responses, "исключить из"));
        result.SuggestedTopics = FilterDashResponses(ExtractOpenResponses(questions, responses, "дополнить программу"));

        // 4. Распределение форматов обучения
        result.FormatDistribution = ExtractFormatDistribution(questions, responses);

        return result;
    }

    private static SurveyQuestion? FindMainStatQuestion(List<SurveyQuestion> questions, string criterionName)
    {
        // Для критериев полезность, практика, доступность, взаимодействие – ищем числовой вопрос
        if (criterionName != "Вовлеченность в образовательный процесс")
        {
            return questions.FirstOrDefault(q => q.Type == QuestionType.Numeric);
        }
        else
        {
            // Для вовлечённости – бинарный вопрос об отстранённости
            return questions.FirstOrDefault(q => q.Type == QuestionType.Binary &&
                q.QuestionText.Contains("отстраненность"));
        }
    }

    private static string? DetermineCriterion(string questionText)
    {
        foreach (var kv in SurveyAnalysisConstants.CriterionKeywords)
        {
            if (Regex.IsMatch(questionText, kv.Value, RegexOptions.IgnoreCase))
            {
                return kv.Key;
            }
        }
        return null; // уточняющий или не идентифицирован
    }

    private static SurveyQuestion? FindParentBinaryQuestion(List<SurveyQuestion> criterionQuestions, SurveyQuestion clarifying)
    {
        // Ищем предыдущий вопрос, который не является уточняющим
        var index = criterionQuestions.IndexOf(clarifying);
        for (var i = index - 1; i >= 0; i--)
        {
            if (criterionQuestions[i].Type == QuestionType.Binary)
            {
                return criterionQuestions[i];
            }
        }
        return null;
    }

    private static bool ShouldIncludeClarification(string clarifyingText, string? binaryValue)
    {
        // Если вопрос о "нет" и бинарный ответ "нет", то включаем
        var asksForNo = clarifyingText.Contains("нет", StringComparison.OrdinalIgnoreCase) ||
                         clarifyingText.Contains("напишите, пожалуйста, почему", StringComparison.OrdinalIgnoreCase);
        var asksForYes = clarifyingText.Contains("да", StringComparison.OrdinalIgnoreCase) &&
                          !clarifyingText.Contains("нет", StringComparison.OrdinalIgnoreCase);

        return asksForNo && binaryValue?.ToLower() == "нет" || asksForYes && binaryValue?.ToLower() == "да";
    }

    private static List<string> ExtractOpenResponses(List<SurveyQuestion> questions, List<SurveyResponse> responses, string keyword)
    {
        var targetQuestion = questions.FirstOrDefault(q => q.QuestionText.Contains(keyword, StringComparison.OrdinalIgnoreCase));
        return targetQuestion == null
            ? []
            : responses
                .SelectMany(r => r.Answers)
                .Where(a => a.QuestionId == targetQuestion.QuestionId && !string.IsNullOrWhiteSpace(a.TextValue))
                .Select(a => a.TextValue!)
                .ToList();
    }

    private List<string> FilterDashResponses(List<string> rawList) =>
        rawList.Where(s => !AnswerQuality.IsNonInformative(s)).ToList();

    private static LearningFormatDistribution ExtractFormatDistribution(List<SurveyQuestion> questions, List<SurveyResponse> responses)
    {
        var formatQuestion = questions.FirstOrDefault(q => q.QuestionText.Contains("формат обучения", StringComparison.OrdinalIgnoreCase));
        if (formatQuestion == null)
        {
            return new LearningFormatDistribution();
        }

        var answers = responses
            .SelectMany(r => r.Answers)
            .Where(a => a.QuestionId == formatQuestion.QuestionId && !string.IsNullOrWhiteSpace(a.TextValue))
            .Select(a => a.TextValue!);

        int full = 0, mixed = 0, remote = 0;
        foreach (var a in answers)
        {
            if (a.Contains("очное", StringComparison.OrdinalIgnoreCase) && !a.Contains("смешанное", StringComparison.OrdinalIgnoreCase))
            {
                full++;
            }
            else if (a.Contains("смешанное", StringComparison.OrdinalIgnoreCase))
            {
                mixed++;
            }
            else if (a.Contains("дистанцион", StringComparison.OrdinalIgnoreCase))
            {
                remote++;
            }
        }
        return new LearningFormatDistribution { FullTime = full, Mixed = mixed, Remote = remote };
    }
}
