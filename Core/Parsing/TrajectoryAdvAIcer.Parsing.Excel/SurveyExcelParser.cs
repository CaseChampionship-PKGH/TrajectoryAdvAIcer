using ClosedXML.Excel;
using TrajectoryAdvAIcer.Entities.Models;
using TrajectoryAdvAIcer.Parsing.Contracts.Enums;
using TrajectoryAdvAIcer.Parsing.Contracts.Helpers;
using TrajectoryAdvAIcer.Parsing.Contracts.Interfaces;
using TrajectoryAdvAIcer.Parsing.Contracts.Models;

namespace TrajectoryAdvAIcer.Parsing.Excel;

/// <summary>
/// Excel парсер ответов тестируемых в анкете
/// </summary>
public class SurveyExcelParser : IDataParser
{
    /// <inheritdoc />
    public InputFormat Format => InputFormat.Excel;

    /// <inheritdoc />
    public ParsingTarget Target => ParsingTarget.Survey;

    /// <inheritdoc />
    public Task<T> ParseAsync<T>(Stream input, string fileName)
    {
        if (typeof(T) != typeof(Survey))
        {
            throw new InvalidOperationException("SurveyExcelParser ожидает тип SurveyParseResult");
        }

        using var workbook = new XLWorkbook(input);
        var ws = workbook.Worksheet(1);

        var headers = new List<string>();
        var firstRow = ws.Row(1);
        for (var col = 1; col <= firstRow.CellCount(); col++)
        {
            headers.Add(firstRow.Cell(col).GetString().Trim());
        }

        var questionIndexes = new Dictionary<int, SurveyQuestion>();
        for (var i = 3; i < headers.Count; i++)
        {
            var headerText = headers[i];
            if (string.IsNullOrWhiteSpace(headerText))
            {
                continue;
            }

            var questionId = QuestionClassifier.GenerateQuestionId(headerText);
            var questionType = QuestionClassifier.ClassifyQuestionType(headerText);
            questionIndexes[i] = new SurveyQuestion
            {
                QuestionId = questionId,
                QuestionText = headerText,
                Type = questionType
            };
        }

        var questions = questionIndexes.Values.ToList();
        var responses = new List<SurveyResponse>();

        // Используем LastRowUsed, чтобы не обходить весь лист (1 млн строк)
        var lastRow = ws.LastRowUsed()?.RowNumber() ?? 1;
        for (var row = 2; row <= lastRow; row++)
        {
            var position = ws.Cell(row, 3).GetString().Trim();
            var hasData = !string.IsNullOrWhiteSpace(position);
            if (!hasData)
            {
                for (int col = 4; col < headers.Count; col++)
                {
                    if (!string.IsNullOrWhiteSpace(ws.Cell(row, col + 1).GetString().Trim()))
                    {
                        hasData = true;
                        break;
                    }
                }
            }
            if (!hasData)
            {
                continue;
            }

            var respondentId = Guid.NewGuid().ToString();
            var answers = new List<SurveyAnswer>();
            foreach (var (colIndex, question) in questionIndexes)
            {
                var rawValue = ws.Cell(row, colIndex + 1).GetString().Trim();
                if (string.IsNullOrWhiteSpace(rawValue))
                {
                    continue;
                }

                var answer = new SurveyAnswer
                {
                    RespondentId = respondentId,
                    QuestionId = question.QuestionId
                };
                switch (question.Type)
                {
                    case Entities.Enums.QuestionType.Numeric:
                        if (double.TryParse(rawValue, out var numVal))
                        {
                            answer.NumericValue = numVal;
                        }

                        break;
                    case Entities.Enums.QuestionType.Binary:
                        var lower = rawValue.ToLowerInvariant();
                        answer.BinaryValue = lower switch
                        {
                            "да" or "yes" => "да",
                            "нет" or "no" => "нет",
                            _ => null
                        };
                        break;
                    case Entities.Enums.QuestionType.OpenText:
                        answer.TextValue = rawValue;
                        break;
                }
                answers.Add(answer);
            }

            responses.Add(new SurveyResponse
            {
                RespondentId = respondentId,
                Position = position,
                Answers = answers
            });
        }

        var programInfo = ProgramInfoParser.Parse(fileName);
        programInfo.ListenersCount = responses.Count;

        var result = new Survey()
        {
            Questions = questions,
            Responses = responses,
            ProgramInfo = programInfo
        };

        return Task.FromResult((T)(object)result);
    }
}
