namespace AstrologyApiClient.Interfaces;

/// <summary>
/// Interface for HTTP client operations
/// </summary>
public interface IHttpClientWrapper : IDisposable
{
    /// <summary>
    /// Sends a POST request to the specified endpoint with form data
    /// </summary>
    /// <typeparam name="T">The response type</typeparam>
    /// <param name="endpoint">API endpoint path</param>
    /// <param name="formData">Form data to send</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Deserialized response object</returns>
    Task<T?> PostAsync<T>(string endpoint, Dictionary<string, string> formData, CancellationToken cancellationToken = default);
}

