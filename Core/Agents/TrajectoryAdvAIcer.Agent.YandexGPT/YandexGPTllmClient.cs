using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using TrajectoryAdvAIcer.Agent.Contracts.Enums;
using TrajectoryAdvAIcer.Agent.Contracts.Interfaces;
using TrajectoryAdvAIcer.Agent.Contracts.Models;
using TrajectoryAdvAIcer.Agent.YandexGPT.Models;
using Microsoft.Extensions.Configuration;

namespace TrajectoryAdvAIcer.Agent.YandexGPT;

/// <inheritdoc cref="ILlmClient"/> на основе YandexGPT
public class YandexGPTllmClient : ILlmClient
{
    private readonly HttpClient httpClient;
    private readonly string apiKey;
    private readonly string model;
    private readonly string requestUri;
    private readonly string folderId;

    LlmVariant ILlmClient.LlmVariant => LlmVariant.Russian;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="YandexGPTllmClient"/>
    /// </summary>
    public YandexGPTllmClient(IHttpClientFactory httpClientFactory, IConfiguration config)
    {
        httpClient = httpClientFactory.CreateClient("RussianLLM");
        model = config.GetRequiredSection("RussianLLM").GetValue<string>("Model")!;
        requestUri = config.GetRequiredSection("RussianLLM").GetValue<string>("RequestUri")!;
        apiKey = config.GetRequiredSection("RussianLLM").GetValue<string>("ApiKey")!;
        folderId = config.GetRequiredSection("RussianLLM").GetValue<string>("FolderId")!;
    }

    async Task<LlmResponse> ILlmClient.SendRequestAsync(LlmRequest llmRequest, string targetTest)
    {
        var request = new YandexGptRequest
        {
            ModelUri = $"gpt://{folderId}/{model}",
            CompletionOptions = new CompletionOptions
            {
                Stream = false,
                Temperature = 0,
                MaxTokens = "2000"
            },
            Messages =
            [
                new Message { Role = "system", Text = "Ты — эксперт по проверке тестовых заданий." },
                new Message { Role = "user", Text = llmRequest.RawPrompt }
            ]
        };

        var json = JsonSerializer.Serialize(request);
        using var requestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };
        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Api-Key", apiKey);

        var response = await httpClient.SendAsync(requestMessage);

        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<YandexGptResponse>(responseBody);

        return new LlmResponse()
        {
            RawResponse = result?.Result?.Alternatives?.FirstOrDefault()?.Message?.Text
                ?? throw new InvalidOperationException("Пустой ответ от YandexGPT")
        };
    }
}
