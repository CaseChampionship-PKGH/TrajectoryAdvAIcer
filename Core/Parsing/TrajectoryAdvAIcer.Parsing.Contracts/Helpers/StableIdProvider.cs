using System.Security.Cryptography;
using System.Text;

namespace TrajectoryAdvAIcer.Parsing.Contracts.Helpers;

/// <summary>
/// Провайдер идентификтора
/// </summary>
public static class StableIdProvider
{
    /// <summary>
    /// Сгенерировать идентификатор для строки
    /// </summary>
    public static string GenerateStableId(string input)
        => Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(input)))[..12];
}
