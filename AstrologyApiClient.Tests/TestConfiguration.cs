namespace AstrologyApiClient.Tests;

/// <summary>
/// Configuration for integration tests
/// Loads credentials from environment variables or appsettings.json
/// </summary>
public static class TestConfiguration
{
    private static string? _userId;
    private static string? _apiKey;

    /// <summary>
    /// Gets the User ID for API authentication
    /// Priority: Environment Variable > appsettings.json > Default
    /// </summary>
    public static string UserId
    {
        get
        {
            if (_userId != null)
                return _userId;

            // Try environment variable first
            _userId = Environment.GetEnvironmentVariable("ASTROLOGY_API_USER_ID");
            
            if (string.IsNullOrWhiteSpace(_userId))
            {
                // Try appsettings.json
                _userId = LoadFromAppSettings("AstrologyApi:UserId");
            }

            if (string.IsNullOrWhiteSpace(_userId))
            {
                throw new InvalidOperationException(
                    "User ID not configured. Set ASTROLOGY_API_USER_ID environment variable or add AstrologyApi:UserId to appsettings.json");
            }

            return _userId;
        }
    }

    /// <summary>
    /// Gets the API Key for API authentication
    /// Priority: Environment Variable > appsettings.json > Default
    /// </summary>
    public static string ApiKey
    {
        get
        {
            if (_apiKey != null)
                return _apiKey;

            // Try environment variable first
            _apiKey = Environment.GetEnvironmentVariable("ASTROLOGY_API_KEY");
            
            if (string.IsNullOrWhiteSpace(_apiKey))
            {
                // Try appsettings.json
                _apiKey = LoadFromAppSettings("AstrologyApi:ApiKey");
            }

            if (string.IsNullOrWhiteSpace(_apiKey))
            {
                throw new InvalidOperationException(
                    "API Key not configured. Set ASTROLOGY_API_KEY environment variable or add AstrologyApi:ApiKey to appsettings.json");
            }

            return _apiKey;
        }
    }

    /// <summary>
    /// Resets the cached credentials (useful for testing different configurations)
    /// </summary>
    public static void Reset()
    {
        _userId = null;
        _apiKey = null;
    }

    /// <summary>
    /// Sets credentials explicitly (useful for testing)
    /// </summary>
    public static void SetCredentials(string userId, string apiKey)
    {
        _userId = userId ?? throw new ArgumentNullException(nameof(userId));
        _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
    }

    private static string? LoadFromAppSettings(string key)
    {
        try
        {
            var appSettingsPath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
            if (!File.Exists(appSettingsPath))
                return null;

            var json = File.ReadAllText(appSettingsPath);
            var doc = System.Text.Json.JsonDocument.Parse(json);
            
            var parts = key.Split(':');
            var current = doc.RootElement;
            
            foreach (var part in parts)
            {
                if (current.TryGetProperty(part, out var property))
                {
                    current = property;
                }
                else
                {
                    return null;
                }
            }

            return current.GetString();
        }
        catch
        {
            return null;
        }
    }
}

