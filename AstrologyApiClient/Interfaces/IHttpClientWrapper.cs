namespace AstrologyApiClient.Interfaces;

/// <summary>
/// Interface for HTTP client operations
/// </summary>
public interface IHttpClientWrapper : IDisposable
{
    Task<T?> PostAsync<T>(string endpoint, Dictionary<string, string> formData, CancellationToken cancellationToken = default);
}

