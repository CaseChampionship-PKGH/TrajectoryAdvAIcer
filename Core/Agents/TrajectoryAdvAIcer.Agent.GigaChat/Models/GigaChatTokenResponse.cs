using System.Text.Json.Serialization;

namespace TrajectoryAdvAIcer.Agent.GigaChat.Models;

/// <summary>
/// Модель ответа access token к Gigachat
/// </summary>
internal class GigaChatTokenResponse
{
    /// <summary>
    /// Токен доступа
    /// </summary>
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; } = string.Empty;

    /// <summary>
    /// Срок жизни в секундах
    /// </summary>
    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; } // в секундах
}
