using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using AstrologyApiClient.Interfaces;

namespace AstrologyApiClient.Services;

/// <summary>
/// Wraps HttpClient for making API requests
/// </summary>
public class HttpClientWrapper : IHttpClientWrapper, IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;
    private bool _disposed = false;

    /// <summary>
    /// Initializes a new instance of HttpClientWrapper
    /// </summary>
    /// <param name="httpClient">HttpClient instance to use</param>
    /// <param name="baseUrl">Base URL for API requests</param>
    public HttpClientWrapper(HttpClient httpClient, string baseUrl)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _baseUrl = baseUrl ?? throw new ArgumentNullException(nameof(baseUrl));
    }

    /// <summary>
    /// Sends a POST request to the specified endpoint with form data
    /// </summary>
    /// <typeparam name="T">The response type</typeparam>
    /// <param name="endpoint">API endpoint path</param>
    /// <param name="formData">Form data to send</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Deserialized response object</returns>
    public async Task<T?> PostAsync<T>(string endpoint, Dictionary<string, string> formData, CancellationToken cancellationToken = default)
    {
        var url = $"{_baseUrl.TrimEnd('/')}/{endpoint.TrimStart('/')}";
        var content = new FormUrlEncodedContent(formData);
        
        var response = await _httpClient.PostAsync(url, content, cancellationToken);
        response.EnsureSuccessStatusCode();
        
        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

    /// <summary>
    /// Disposes the wrapper and releases resources
    /// </summary>
    public void Dispose()
    {
        if (!_disposed)
        {
            _disposed = true;
        }
    }
}

