using System.Globalization;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using TrajectoryAdvAIcer.Entities.Enums;
using TrajectoryAdvAIcer.Entities.Models;
using TrajectoryAdvAIcer.Parsing.Contracts.Enums;
using TrajectoryAdvAIcer.Parsing.Contracts.Helpers;
using TrajectoryAdvAIcer.Parsing.Contracts.Interfaces;
using TrajectoryAdvAIcer.Parsing.Contracts.Models;

namespace TrajectoryAdvAIcer.Parsing.Csv;

/// <summary>
/// Csv парсер ответов тестируемых в анкете
/// </summary>
public class SurveyCsvParser : IDataParser
{
    /// <inheritdoc />
    public InputFormat Format => InputFormat.Csv;

    /// <inheritdoc />
    public ParsingTarget Target => ParsingTarget.Survey;

    /// <inheritdoc />
    public async Task<T> ParseAsync<T>(Stream input, string fileName)
    {
        if (typeof(T) != typeof(Survey))
        {
            throw new InvalidOperationException("SurveyCsvParser ожидает тип SurveyParseResult");
        }

        var encoding = Encoding.GetEncoding("windows-1251");
        using var reader = new StreamReader(input, encoding, detectEncodingFromByteOrderMarks: false);
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ";",
            HasHeaderRecord = true,
            TrimOptions = TrimOptions.Trim,
            BadDataFound = null
        };

        using var csv = new CsvReader(reader, config);
        csv.Read();
        csv.ReadHeader();
        var headers = csv.HeaderRecord;
        if (headers == null || headers.Length < 5)
        {
            throw new InvalidDataException("CSV-файл анкеты должен содержать хотя бы 5 столбцов.");
        }

        var questionIndexes = new Dictionary<int, SurveyQuestion>();
        for (var i = 3; i < headers.Length; i++)
        {
            var headerText = headers[i]?.Trim();
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

        while (csv.Read())
        {
            var position = csv.GetField(2)?.Trim();

            var hasData = !string.IsNullOrWhiteSpace(position);
            if (!hasData)
            {
                for (var i = 4; i < headers.Length; i++)
                {
                    if (!string.IsNullOrWhiteSpace(csv.GetField(i)))
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
                var rawValue = csv.GetField(colIndex)?.Trim();
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
                    case QuestionType.Numeric:
                        if (double.TryParse(rawValue, out var numVal))
                        {
                            answer.NumericValue = numVal;
                        }

                        break;
                    case QuestionType.Binary:
                        var lower = rawValue.ToLowerInvariant();
                        answer.BinaryValue = lower switch
                        {
                            "да" or "yes" => "да",
                            "нет" or "no" => "нет",
                            _ => null
                        };
                        break;
                    case QuestionType.OpenText:
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

        return (T)(object)result;
    }
}
