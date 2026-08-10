using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using TrajectoryAdvAIcer.Agent.Contracts.Enums;
using TrajectoryAdvAIcer.Agent.Contracts.Interfaces;
using TrajectoryAdvAIcer.Agent.Contracts.Models;
using TrajectoryAdvAIcer.Agent.Ollama.Models;

namespace TrajectoryAdvAIcer.Agent.Ollama;

/// <inheritdoc cref="ILlmClient"/>, на основе Ollama
public class OllamaLlmClient : ILlmClient
{
    private readonly HttpClient httpClient;
    private readonly string apiKey;
    private readonly string requestUri;
    private readonly string model;

    LlmVariant ILlmClient.LlmVariant => LlmVariant.Local;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="OllamaLlmClient"/>
    /// </summary>
    public OllamaLlmClient(IHttpClientFactory httpClientFactory, IConfiguration config)
    {
        httpClient = httpClientFactory.CreateClient("LocalLLM");
        apiKey = config.GetRequiredSection("LocalLLM").GetValue<string>("ApiKey")!;
        requestUri = config.GetRequiredSection("LocalLLM").GetValue<string>("RequestUri")!;
        model = config.GetRequiredSection("LocalLLM").GetValue<string>("Model")!;
    }

    async Task<LlmResponse> ILlmClient.SendRequestAsync(LlmRequest llmRequest, string targetTest)
    {
        var request = new OllamaCompatibleRequest
        {
            Model = model,
            Messages =
            [
                new() { Role = "system", Content = "Ты — консультант по обучению государственных гражданских служащих." },
                new() { Role = "user", Content = llmRequest.RawPrompt }
            ],
            Temperature = 0,
            MaxTokens = 2000
        };

        var json = JsonSerializer.Serialize(request);
        using var requestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };
        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

        var response = await httpClient.SendAsync(requestMessage);
        response.EnsureSuccessStatusCode();

        var body = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<OllamaCompatibleResponse>(body);

        return new LlmResponse
        {
            RawResponse = result?.Choices?.FirstOrDefault()?.Message?.Content
               ?? "ОШИБКА: Пустой ответ от локальной LLM"
        };
    }
}
