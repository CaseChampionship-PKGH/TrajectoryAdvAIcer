using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using TrajectoryAdvAIcer.Agent.Contracts.Enums;
using TrajectoryAdvAIcer.Agent.Contracts.Interfaces;
using TrajectoryAdvAIcer.Agent.Contracts.Models;
using TrajectoryAdvAIcer.Agent.GigaChat.Models;
using Microsoft.Extensions.Configuration;

namespace TrajectoryAdvAIcer.Agent.GigaChat;

/// <inheritdoc cref="ILlmClient"/> на основе Gigachat
public class GigaChatLlmClient : ILlmClient
{
    private const int AccessTokenOffsetSeconds = 60;

    private readonly HttpClient agentHttpClient;
    private readonly HttpClient tokenHttpClient;
    private readonly string apiKey;
    private readonly string requestUri;
    private readonly string tokenUrl;
    private readonly string model;
    private readonly string scope;

    private string? cachedToken;
    private DateTime tokenExpiry = DateTime.MinValue;

    LlmVariant ILlmClient.LlmVariant => LlmVariant.Russian;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="GigaChatLlmClient"/>
    /// </summary>
    public GigaChatLlmClient(IHttpClientFactory httpClientFactory, IConfiguration config)
    {
        agentHttpClient = httpClientFactory.CreateClient("RussianLLM");
        tokenHttpClient = httpClientFactory.CreateClient("RussianLLMAccessToken");
        apiKey = config.GetRequiredSection("RussianLLM").GetValue<string>("ApiKey")!;
        tokenUrl = config.GetRequiredSection("RussianLLM").GetValue<string>("TokenUrl")!;
        requestUri = config.GetRequiredSection("RussianLLM").GetValue<string>("RequestUri")!;
        model = config.GetRequiredSection("RussianLLM").GetValue<string>("Model")!;
        scope = config.GetRequiredSection("RussianLLM").GetValue<string>("Scope")!;
    }

    async Task<LlmResponse> ILlmClient.SendRequestAsync(LlmRequest llmRequest, string targetTest)
    {
        var token = await GetAccessToken();

        var request = new GigaChatCompatibleRequest
        {
            Model = model,
            Messages =
            [
                new() { Role = "system", Content = "Ты — аналитик образовательных программ." },
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
        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await agentHttpClient.SendAsync(requestMessage);
        response.EnsureSuccessStatusCode();

        var body = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<GigaChatCompatibleResponse>(body);

        return new LlmResponse
        {
            RawResponse = result?.Choices?.FirstOrDefault()?.Message?.Content
                          ?? "ОШИБКА: Пустой ответ от GigaChat"
        };
    }

    private async Task<string> GetAccessToken()
    {
        if (!string.IsNullOrEmpty(cachedToken) && DateTime.UtcNow < tokenExpiry)
        {
            return cachedToken;
        }

        using var tokenRequest = new HttpRequestMessage(HttpMethod.Post, tokenUrl);
        tokenRequest.Headers.Authorization = new AuthenticationHeaderValue("Basic", apiKey);
        tokenRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        tokenRequest.Headers.Add("RqUID", Guid.NewGuid().ToString());

        tokenRequest.Content = new StringContent($"scope={scope}", Encoding.UTF8, "application/x-www-form-urlencoded");

        var tokenResponse = await tokenHttpClient.SendAsync(tokenRequest);
        tokenResponse.EnsureSuccessStatusCode();

        var tokenBody = await tokenResponse.Content.ReadAsStringAsync();
        var tokenData = JsonSerializer.Deserialize<GigaChatTokenResponse>(tokenBody);
        if (tokenData == null || string.IsNullOrWhiteSpace(tokenData.AccessToken))
        {
            throw new InvalidOperationException("Не удалось получить access_token от GigaChat");
        }

        cachedToken = tokenData!.AccessToken;
        tokenExpiry = DateTime.UtcNow.AddSeconds(tokenData.ExpiresIn - AccessTokenOffsetSeconds);
        return cachedToken;
    }
}
