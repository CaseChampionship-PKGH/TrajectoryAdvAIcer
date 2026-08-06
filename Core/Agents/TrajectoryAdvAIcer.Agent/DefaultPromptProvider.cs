using System.Text;
using TrajectoryAdvAIcer.Agent.Contracts.Interfaces;
using TrajectoryAdvAIcer.Analysis.Contracts.Models.Aggregation;
using TrajectoryAdvAIcer.Services.Contracts.Constants;

namespace TrajectoryAdvAIcer.Agent;

/// <inheritdoc cref="IPromptProvider"/>
public class DefaultPromptProvider : IPromptProvider
{
    string IPromptProvider.BuildCriterionNotePrompt(CriterionPromptData criterionData)
    {
        var sb = new StringBuilder();
        sb.AppendLine("Ты — аналитик образовательных программ.");
        sb.AppendLine($"Проанализируй ответы слушателей по критерию «{criterionData.CriterionName}».");
        sb.AppendLine();

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


    string IPromptProvider.BuildTrajectoryPrompt(AggregatedCriteriaData allData, List<string> notes)
    {
        var sb = new StringBuilder();
        sb.AppendLine("Ты — методист образовательных программ.");
        sb.AppendLine("Составь раздел «Траектория изменения программы» и краткие обобщения предложений слушателей.");
        sb.AppendLine("На основе следующих данных:");
        sb.AppendLine();

        // Примечания по критериям
        var criterianNames = SurveyAnalysisConstants.CriteriaNames;
        for (var i = 0; i < criterianNames.Length && i < notes.Count; i++)
        {
            sb.AppendLine($"Примечание по критерию «{criterianNames[i]}»: {notes[i]}");
            sb.AppendLine();
        }

        // Сводная статистика
        //sb.AppendLine("Сводная статистика:");
        //foreach (var c in allData.AllCriteriaData)
        //{
        //    var s = c.Statistics;
        //    if (s == null)
        //    {
        //        continue;
        //    }

        //    if (s.Average.HasValue)
        //    {
        //        sb.AppendLine($"- {c.CriterionName}: средний балл {s.Average:F1}");
        //    }
        //    else if (s.YesCount.HasValue)
        //    {
        //        sb.AppendLine($"- {c.CriterionName}, Вопрос - «{s.Question.QuestionText}»: Да — {s.YesCount}, Нет — {s.NoCount} ({s.YesPercent:F1}%)");
        //    }
        //}

        // Сырые предложения по исключению и добавлению тем
        if (allData.ExcludedTopics.Count > 0)
        {
            sb.AppendLine("Предложения исключить темы (сырые ответы):");
            foreach (var t in allData.ExcludedTopics)
            {
                sb.AppendLine($"- {t}");
            }
        }
        else
        {
            sb.AppendLine("Предложений об исключении тем нет.");
        }

        if (allData.SuggestedTopics.Count > 0)
        {
            sb.AppendLine("Предложения добавить темы (сырые ответы):");
            foreach (var t in allData.SuggestedTopics)
            {
                sb.AppendLine($"- {t}");
            }
        }
        else
        {
            sb.AppendLine("Предложений о добавлении тем нет.");
        }

        // Распределение форматов
        var dist = allData.FormatDistribution;
        int total = dist.FullTime + dist.Mixed + dist.Remote;
        sb.AppendLine($"Предпочтения по форме обучения: очно — {dist.FullTime} чел. ({(total > 0 ? 100.0 * dist.FullTime / total : 0):F1}%), " +
                      $"смешанно — {dist.Mixed} чел. ({(total > 0 ? 100.0 * dist.Mixed / total : 0):F1}%), " +
                      $"дистанционно — {dist.Remote} чел. ({(total > 0 ? 100.0 * dist.Remote / total : 0):F1}%).");

        sb.AppendLine();
        sb.AppendLine("Сформируй JSON-ответ со следующими полями:");
        sb.AppendLine("1. needForProgram — потребность в дальнейшей реализации программы (1-2 предложения).");
        sb.AppendLine("2. admissionCorrection — нужна ли корректировка отбора слушателей (1 предложение).");
        sb.AppendLine("3. programSupplement — что нужно добавить в программу (1-2 предложения).");
        sb.AppendLine("4. hoursChange — нужно ли изменение количества часов (1 предложение).");
        sb.AppendLine("5. formChange — нужно ли изменение формы обучения (1 предложение, учитывая распределение).");
        sb.AppendLine("6. excludedTopicsSummary — обобщение предложений об исключении тем: 1-2 предложения, сгруппировав похожие.");
        sb.AppendLine("7. suggestedTopicsSummary — обобщение предложений о добавлении тем: 1-2 предложения, сгруппировав похожие.");
        sb.AppendLine();
        sb.AppendLine("Верни строго JSON без markdown:");
        sb.AppendLine("{");
        sb.AppendLine("  \"needForProgram\": \"...\",");
        sb.AppendLine("  \"admissionCorrection\": \"...\",");
        sb.AppendLine("  \"programSupplement\": \"...\",");
        sb.AppendLine("  \"hoursChange\": \"...\",");
        sb.AppendLine("  \"formChange\": \"...\",");
        sb.AppendLine("  \"excludedTopicsSummary\": \"...\",");
        sb.AppendLine("  \"suggestedTopicsSummary\": \"...\"");
        sb.AppendLine("}");
        return sb.ToString();
    }
}

