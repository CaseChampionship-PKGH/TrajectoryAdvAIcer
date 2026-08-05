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
                                ""needForProgram"": ""потребность в дальнейшей реализации программы"",
                                ""admissionCorrection"": ""нужна ли корректировка отбора слушателей"",
                                ""programSupplement"": ""что нужно добавить в программу"",
                                ""hoursChange"": ""нужно ли изменение количества часов"",
                                ""formChange"": ""нужно ли изменение формы обучения"",
                                ""excludedTopicsSummary"": ""обобщение предложений об исключении тем"",
                                ""suggestedTopicsSummary"": ""обобщение предложений о добавлении тем""
                            }
                            ```";
        }
        else if (targetTest == "note")
        {
            mockResponse = "Рекомендуется пересмотреть формулировку вопроса и добавить пояснения в материал.";
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
