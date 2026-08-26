using System.Text;
using TrajectoryAdvAIcer.Agent.Contracts.Enums;
using TrajectoryAdvAIcer.Agent.Contracts.Interfaces;
using TrajectoryAdvAIcer.Agent.Contracts.Models;
using TrajectoryAdvAIcer.Analysis.Contracts.Models.Aggregation;
using TrajectoryAdvAIcer.Analysis.Contracts.Models.Trajectory;
using TrajectoryAdvAIcer.Entities.Models;
using TrajectoryAdvAIcer.Parsing.Contracts.Enums;
using TrajectoryAdvAIcer.Parsing.Contracts.Interfaces;

namespace TrajectoryAdvAIcer.Agent;

/// <inheritdoc cref="ITrajectoryAdvicerAgent"/> на базе искуственного интелекта LLM
public class LlmTrajectoryAdvicerAgent : ITrajectoryAdvicerAgent
{
    private readonly IPromptProvider promptProvider;
    private readonly IParserFactory parserFactory;
    private readonly ILlmFactory llmFactory;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="LlmTrajectoryAdvicerAgent"/>
    /// </summary>
    public LlmTrajectoryAdvicerAgent(IPromptProvider promptProvider,
        IParserFactory parserFactory,
        ILlmFactory llmFactory)
    {
        this.promptProvider = promptProvider;
        this.parserFactory = parserFactory;
        this.llmFactory = llmFactory;
    }

    async Task<TrajectoryAnalysisResult> ITrajectoryAdvicerAgent.GenerateTrajectoryForEmployeeAsync(Employee profile,
        List<string> passedCourses,
        List<RecommendedCourse> recommendedCourses,
        LlmVariant llmVariant)
    {
        var prompt = promptProvider.BuildTrajectoryPrompt(profile, passedCourses, recommendedCourses);
        var llmClient = llmFactory.CreateLLmClient(llmVariant);
        var response = await llmClient.SendRequestAsync(new LlmRequest { RawPrompt = prompt }, "trajectory");

        var raw = response.RawResponse;
        var cleaned = raw.Replace("```json", "").Replace("```", "").Trim();
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(cleaned));
        var parser = parserFactory.GetParser(InputFormat.Json, ParsingTarget.AgentResponse);

        return await parser.ParseAsync<TrajectoryAnalysisResult>(stream);
    }
}
