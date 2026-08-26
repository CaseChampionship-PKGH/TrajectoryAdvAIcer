using System.Text;
using TrajectoryAdvAIcer.Agent.Contracts.Interfaces;
using TrajectoryAdvAIcer.Entities.Models;

namespace TrajectoryAdvAIcer.Agent;

/// <inheritdoc cref="IPromptProvider"/>
public class DefaultPromptProvider : IPromptProvider
{
    string IPromptProvider.BuildCriterionNotePrompt(LearningHistory learningHistory, IEnumerable<string> recommendedIds, CourseCatalog courseCatalog)
    {
        var sb = new StringBuilder();
        sb.AppendLine("Ты — аналитик образовательных программ.");
        //sb.AppendLine($"Проанализируй ответы слушателей по критерию «{criterionData.CriterionName}».");
        //sb.AppendLine();

        //var stats = criterionData.Statistics;
        //if (stats != null)
        //{
        //    if (stats.Average.HasValue)
        //    {
        //        sb.AppendLine($"Статистика: средний балл — {stats.Average:F1} из 10.");
        //        if (stats.Distribution != null)
        //        {
        //            sb.AppendLine($"Распределение оценок: 1-3: {stats.PercentLow:F1}%, " +
        //                          $"4-7: {stats.PercentMedium:F1}%, " +
        //                          $"8-10: {stats.PercentHigh:F1}%.");
        //        }
        //    }
        //    else if (stats.YesCount.HasValue)
        //    {
        //        sb.AppendLine($"Статистика: Вопрос - «{stats.Question.QuestionText}»: \"Да\" — {stats.YesCount} чел. ({stats.YesPercent:F1}%), " +
        //                      $"\"Нет\" — {stats.NoCount} чел. ({stats.NoPercent:F1}%).");
        //    }
        //}

        //sb.AppendLine();
        //sb.AppendLine("Ответы на вопросы:");
        //foreach (var qa in criterionData.Questions)
        //{
        //    if (qa.Answers.Count == 0)
        //    {
        //        continue;
        //    }

        //    sb.AppendLine($"Вопрос: «{qa.Question.QuestionText}»");
        //    foreach (var ans in qa.Answers)
        //    {
        //        sb.AppendLine($"- «{ans}»");
        //    }

        //    sb.AppendLine();
        //}

        sb.AppendLine("На основе этих данных напиши Примечание на 3-8 предложений деловым стилем. " +
                      "Указывай точные цифры. Верни ответ в чистом тексте без markdown. Не придумывай факты, опирайся только на предоставленные ответы.");
        return sb.ToString();
    }
}

