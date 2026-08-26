using TrajectoryAdvAIcer.Entities.Models;
using TrajectoryAdvAIcer.Validation.Contracts.Interfaces;
using TrajectoryAdvAIcer.Validation.Contracts.Models;

namespace TrajectoryAdvAIcer.Validation;

/// <summary>
/// Валидатор данных анкетирования
/// </summary>
public class DataValidator : IDataValidator
{
    ValidationResult IDataValidator.Validate(LearningHistory parsedData)
    {
        //var warnings = new List<string>();

        //var cleanedResponses = new List<LearningHistory>();

        //foreach (var response in parsedData.Responses)
        //{
        //    var cleanedAnswers = new List<LearningHistory>();

        //    foreach (var answer in response.Answers)
        //    {
        //        var question = parsedData.Questions.FirstOrDefault(q => q.QuestionId == answer.QuestionId);
        //        if (question == null)
        //        {
        //            warnings.Add($"Ответ с ID вопроса {answer.QuestionId} не найден в справочнике.");
        //            continue;
        //        }

        //        if (answer.TextValue != null && AnswerQuality.IsPotentialInjection(answer.TextValue))
        //        {
        //            warnings.Add($"Потенциальный Prompt Injection: '{answer.TextValue}' ответ проигнорен.");
        //            continue;
        //        }

        //        switch (question.Type)
        //        {
        //            case QuestionType.Numeric:
        //                if (answer.NumericValue.HasValue)
        //                {
        //                    if (answer.NumericValue < 1 || answer.NumericValue > 10)
        //                    {
        //                        warnings.Add($"Оценка {answer.NumericValue} для вопроса '{question.QuestionText}' вне диапазона 1-10. Ответ будет исключён.");
        //                        continue;
        //                    }
        //                    cleanedAnswers.Add(answer);
        //                }
        //                else
        //                {
        //                    warnings.Add($"Пустая числовая оценка для вопроса '{question.QuestionText}'.");
        //                }
        //                break;

        //            case QuestionType.Binary:
        //                var binVal = answer.BinaryValue;
        //                if (string.IsNullOrWhiteSpace(binVal))
        //                {
        //                    warnings.Add($"Пустой бинарный ответ для вопроса '{question.QuestionText}'.");
        //                    continue;
        //                }
        //                if (!IsValidBinary(binVal))
        //                {
        //                    if (AnswerQuality.IsNonInformative(binVal))
        //                    {
        //                        warnings.Add($"Бинарный ответ '{binVal}' для вопроса '{question.QuestionText}' интерпретирован как пустой.");
        //                        continue;
        //                    }
        //                    warnings.Add($"Неожиданное значение '{binVal}' для бинарного вопроса '{question.QuestionText}'. Ответ исключён.");
        //                    continue;
        //                }
        //                cleanedAnswers.Add(answer);
        //                break;

        //            case QuestionType.OpenText:
        //                if (AnswerQuality.IsNonInformativeForQuestion(question.QuestionText, answer.TextValue))
        //                {
        //                    continue;
        //                }
        //                cleanedAnswers.Add(answer);
        //                break;
        //        }
        //    }

        //    if (cleanedAnswers.Count > 0)
        //    {
        //        cleanedResponses.Add(new SurveyResponse
        //        {
        //            RespondentId = response.RespondentId,
        //            Position = response.Position,
        //            Answers = cleanedAnswers
        //        });
        //    }
        //    else
        //    {
        //        warnings.Add($"Анкета респондента {response.RespondentId} полностью пуста после фильтрации.");
        //    }
        //}

        //var usedQuestionIds = cleanedResponses
        //    .SelectMany(r => r.Answers.Select(a => a.QuestionId))
        //    .Distinct()
        //    .ToHashSet();

        //var cleanedQuestions = parsedData.Questions
        //    .Where(q => usedQuestionIds.Contains(q.QuestionId))
        //    .ToList();

        //var result = new Survey()
        //{
        //    ProgramInfo = parsedData.ProgramInfo,
        //    Questions = cleanedQuestions,
        //    Responses = cleanedResponses
        //};

        //return new ValidationResult
        //{
        //    ValidatedResults = result,
        //    Success = result.Questions.Count > 0,
        //    Warnings = warnings
        //};

        return new ValidationResult();
    }

    private static bool IsValidBinary(string value)
    {
        return value.Equals("да", StringComparison.OrdinalIgnoreCase) ||
               value.Equals("нет", StringComparison.OrdinalIgnoreCase);
    }
}
