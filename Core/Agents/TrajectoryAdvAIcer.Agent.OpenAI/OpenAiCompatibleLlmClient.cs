using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using TrajectoryAdvAIcer.Agent.Contracts.Enums;
using TrajectoryAdvAIcer.Agent.Contracts.Interfaces;
using TrajectoryAdvAIcer.Agent.Contracts.Models;
using TrajectoryAdvAIcer.Agent.OpenAI.Models;
using Microsoft.Extensions.Configuration;

namespace TrajectoryAdvAIcer.Agent.OpenAI;

/// <inheritdoc cref="ILlmClient"/>, поддерживающий протокол OpenAI
public class OpenAiCompatibleLlmClient : ILlmClient
{
    private readonly HttpClient httpClient;
    private readonly string apiKey;
    private readonly string requestUri;
    private readonly string model;

    LlmVariant ILlmClient.LlmVariant => LlmVariant.Foreign;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="OpenAiCompatibleLlmClient "/>
    /// </summary>
    public OpenAiCompatibleLlmClient(IHttpClientFactory httpClientFactory, IConfiguration config)
    {
        httpClient = httpClientFactory.CreateClient("ForeignLLM");
        apiKey = config.GetRequiredSection("ForeignLLM").GetValue<string>("ApiKey")!;
        requestUri = config.GetRequiredSection("ForeignLLM").GetValue<string>("RequestUri")!;
        model = config.GetRequiredSection("ForeignLLM").GetValue<string>("Model")!;
    }

    async Task<LlmResponse> ILlmClient.SendRequestAsync(LlmRequest llmRequest, string targetTest)
    {
        var request = new OpenAICompatibleRequest
        {
            Model = model,
            Messages =
            [
                new ChatMessage { Role = "system", Content = "Ты — эксперт по проверке тестовых заданий." },
                new ChatMessage { Role = "user", Content = llmRequest.RawPrompt }
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
        var result = JsonSerializer.Deserialize<OpenAICompatibleResponse>(body);

        return new LlmResponse()
        {
            RawResponse = result?.Choices?.FirstOrDefault()?.Message?.Content
               ?? "ОШИБКА: Пустой ответ от внешнего LLM"
        };
    }
}
