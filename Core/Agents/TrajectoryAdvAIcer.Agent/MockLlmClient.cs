using TrajectoryAdvAIcer.Agent.Contracts.Enums;
using TrajectoryAdvAIcer.Agent.Contracts.Interfaces;
using TrajectoryAdvAIcer.Agent.Contracts.Models;

namespace TrajectoryAdvAIcer.Agent;

/// <summary>
/// Заглушка для <see cref="ILlmClient"/>
/// </summary>
public class MockLlmClient : ILlmClient
{
    LlmVariant ILlmClient.LlmVariant => LlmVariant.Russian;

    Task<LlmResponse> ILlmClient.SendRequestAsync(LlmRequest prompt, string targetTest)
    {
        string mockResponse;

        if (targetTest == "trajectory")
        {
            mockResponse = @"```json
                {
                    ""trajectory"": [
                        {
                            ""courseTitle"": ""Название курса 1"",
                            ""rationale"": ""Обоснование рекомендации""
                        },
                        {
                            ""courseTitle"": ""Название курса 2"",
                            ""rationale"": ""Обоснование рекомендации""
                        },
                        {
                            ""courseTitle"": ""Название курса 3"",
                            ""rationale"": ""Обоснование рекомендации""
                        },
                        {
                            ""courseTitle"": ""Название курса 4"",
                            ""rationale"": ""Обоснование рекомендации""
                        },
                        {
                            ""courseTitle"": ""Название курса 5"",
                            ""rationale"": ""Обоснование рекомендации""
                        },
                        {
                            ""courseTitle"": ""Название курса 6"",
                            ""rationale"": ""Обоснование рекомендации""
                        },
                        {
                            ""courseTitle"": ""Название курса 7"",
                            ""rationale"": ""Обоснование рекомендации""
                        },
                        {
                            ""courseTitle"": ""Название курса 8"",
                            ""rationale"": ""Обоснование рекомендации""
                        }
                    ]
                }
                ```";
        }
        else
        {
            // На случай дополнительных запросов
            mockResponse = "{}";
        }

        return Task.FromResult(new LlmResponse()
        {
            RawResponse = mockResponse
        });
    }
}
