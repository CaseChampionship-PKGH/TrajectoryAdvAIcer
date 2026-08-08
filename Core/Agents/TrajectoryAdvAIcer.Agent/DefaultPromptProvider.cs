using System.Text;
using TrajectoryAdvAIcer.Agent.Contracts.Interfaces;
using TrajectoryAdvAIcer.Analysis.Contracts.Models.Aggregation;
using TrajectoryAdvAIcer.Entities.Models;

namespace TrajectoryAdvAIcer.Agent;

/// <inheritdoc cref="IPromptProvider"/>
public class DefaultPromptProvider : IPromptProvider
{
    string IPromptProvider.BuildTrajectoryPrompt(
        Employee profile,
        List<string> passedCourses,
        List<RecommendedCourse> recommendedCourses)
    {
        var sb = new StringBuilder();

        sb.AppendLine("Ты — консультант по обучению государственных гражданских служащих.");
        sb.AppendLine("Составь индивидуальную траекторию обучения для следующего сотрудника:");
        sb.AppendLine();
        sb.AppendLine($"Должность: {profile.Position}");
        sb.AppendLine($"ИОГВ: {profile.IOGV}");
        sb.AppendLine();

        sb.AppendLine("Уже пройденные курсы:");
        foreach (var courseTitle in passedCourses)
        {
            sb.AppendLine($"- {courseTitle}");
        }

        sb.AppendLine();

        sb.AppendLine("На основе опыта коллег с такой же должностью и ИОГВ рекомендуются следующие курсы:");
        foreach (var rec in recommendedCourses)
        {
            sb.AppendLine();
            sb.AppendLine($"Курс: \"{rec.Course.Title}\"");
            sb.AppendLine($"Популярность среди коллег: {rec.Popularity} из {rec.TotalSimilarEmployees}");
            sb.AppendLine($"Аннотация: {rec.Course.Annotation}");
            sb.AppendLine($"Цели: {rec.Course.Goals}");
            sb.AppendLine($"Результаты: {rec.Course.Results}");
        }
        sb.AppendLine();

        sb.AppendLine("Сформируй персональную траекторию обучения:");
        sb.AppendLine("- Перечисли курсы в логическом порядке.");
        sb.AppendLine("- Для каждого дай краткое обоснование (1-2 предложения), почему этот курс важен именно для этой должности.");
        sb.AppendLine("- Укажи, какие конкретные навыки и знания сотрудник получит.");
        sb.AppendLine();
        sb.AppendLine("Верни ответ строго в формате JSON без markdown-обёртки:");
        sb.AppendLine("{");
        sb.AppendLine("  \"trajectory\": [");
        sb.AppendLine("    {");
        sb.AppendLine("      \"courseTitle\": \"Название курса\",");
        sb.AppendLine("      \"rationale\": \"Обоснование рекомендации\"");
        sb.AppendLine("    }");
        sb.AppendLine("  ]");
        sb.AppendLine("}");

        return sb.ToString();
    }
}

