using TrajectoryAdvAIcer.Agent.Contracts.Enums;
using TrajectoryAdvAIcer.Agent.Contracts.Interfaces;
using TrajectoryAdvAIcer.Entities.Models;
using TrajectoryAdvAIcer.Parsing.Contracts.Interfaces;

namespace TrajectoryAdvAIcer.Agent;

/// <inheritdoc cref="ISurveyCriteriaAnalysisAgent"/> на базе искуственного интелекта LLM
public class LlmSurveyAnalysisAgent : ISurveyCriteriaAnalysisAgent
{
    private readonly IPromptProvider promptProvider;
    private readonly IParserFactory parserFactory;
    private readonly ILlmFactory llmFactory;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="LlmSurveyAnalysisAgent"/>
    /// </summary>
    public LlmSurveyAnalysisAgent(IPromptProvider promptProvider,
        IParserFactory parserFactory,
        ILlmFactory llmFactory)
    {
        this.promptProvider = promptProvider;
        this.parserFactory = parserFactory;
        this.llmFactory = llmFactory;
    }

    async Task<string> ISurveyCriteriaAnalysisAgent.GenerateLearningTrajectoryAsync(LearningHistory learningHistory, IEnumerable<string> recommendedIds, CourseCatalog courseCatalog, LlmVariant llmVariant)
    {
        //var prompt = promptProvider.BuildCriterionNotePrompt(criterionData);
        //var llmClient = llmFactory.CreateLLmClient(llmVariant);
        //var response = await llmClient.SendRequestAsync(new LlmRequest { RawPrompt = prompt }, "note");

        //var raw = response.RawResponse;
        //var cleaned = raw.Replace("```json", "").Replace("```", "").Trim();
        //return cleaned;
        return string.Empty;
    }
}
