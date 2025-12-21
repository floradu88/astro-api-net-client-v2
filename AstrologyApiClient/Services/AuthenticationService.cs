using System.Net.Http.Headers;
using System.Text;

namespace AstrologyApiClient.Services;

/// <summary>
/// Handles authentication setup for HTTP client
/// </summary>
public static class AuthenticationService
{
    /// <summary>
    /// Configures Basic Authentication on the HTTP client
    /// </summary>
    public static void ConfigureBasicAuth(HttpClient httpClient, string userId, string apiKey)
    {
        if (httpClient == null)
            throw new ArgumentNullException(nameof(httpClient));
        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentException("User ID cannot be null or empty", nameof(userId));
        if (string.IsNullOrWhiteSpace(apiKey))
            throw new ArgumentException("API Key cannot be null or empty", nameof(apiKey));

        var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{userId}:{apiKey}"));
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }
}

