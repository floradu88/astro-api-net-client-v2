using AstrologyApiClient.Interfaces;
using AstrologyApiClient.Models;
using AstrologyApiClient.Services;

namespace AstrologyApiClient;

/// <summary>
/// Client for interacting with the Astrology API
/// Credentials are set once during initialization and reused for all requests
/// </summary>
public class AstrologyApiClient : IAstrologyApiClient, IDisposable
{
    private readonly IHttpClientWrapper _httpClientWrapper;
    private readonly IFormDataConverter _formDataConverter;
    private readonly HttpClient? _httpClient;
    private bool _disposed = false;

    /// <summary>
    /// Initializes a new instance of the AstrologyApiClient
    /// </summary>
    /// <param name="userId">User ID for authentication</param>
    /// <param name="apiKey">API Key for authentication</param>
    public AstrologyApiClient(string userId, string apiKey)
        : this(new HttpClient(), userId, apiKey, "https://json.astrologyapi.com/v1")
    {
    }

    /// <summary>
    /// Initializes a new instance with custom HttpClient (for testing or advanced scenarios)
    /// </summary>
    /// <param name="httpClient">HttpClient instance</param>
    /// <param name="userId">User ID for authentication</param>
    /// <param name="apiKey">API Key for authentication</param>
    public AstrologyApiClient(HttpClient httpClient, string userId, string apiKey)
        : this(httpClient, userId, apiKey, "https://json.astrologyapi.com/v1")
    {
    }

    /// <summary>
    /// Internal constructor for dependency injection
    /// </summary>
    internal AstrologyApiClient(
        HttpClient httpClient,
        string userId,
        string apiKey,
        string baseUrl,
        IFormDataConverter? formDataConverter = null,
        IHttpClientWrapper? httpClientWrapper = null)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        
        AuthenticationService.ConfigureBasicAuth(_httpClient, userId, apiKey);
        
        _formDataConverter = formDataConverter ?? new FormDataConverter();
        _httpClientWrapper = httpClientWrapper ?? new HttpClientWrapper(_httpClient, baseUrl);
    }

    // Western Horoscope Methods

    public async Task<T?> GetWesternHoroscopeAsync<T>(BirthData birthData, CancellationToken cancellationToken = default)
    {
        var formData = _formDataConverter.Convert(birthData);
        return await _httpClientWrapper.PostAsync<T>("western_horoscope", formData, cancellationToken);
    }

    public async Task<T?> GetWesternChartDataAsync<T>(BirthData birthData, CancellationToken cancellationToken = default)
    {
        var formData = _formDataConverter.Convert(birthData);
        return await _httpClientWrapper.PostAsync<T>("western_horoscope", formData, cancellationToken);
    }

    public async Task<T?> GetWesternChartImageAsync<T>(BirthData birthData, CancellationToken cancellationToken = default)
    {
        var formData = _formDataConverter.Convert(birthData);
        return await _httpClientWrapper.PostAsync<T>("natal_wheel_chart", formData, cancellationToken);
    }

    // Planet Prediction Methods

    public async Task<T?> GetPlanetPredictionAsync<T>(string planet, BirthData birthData, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(planet))
            throw new ArgumentException("Planet cannot be null or empty", nameof(planet));

        var formData = _formDataConverter.Convert(birthData);
        return await _httpClientWrapper.PostAsync<T>($"personalized_planet_prediction/daily/{planet}", formData, cancellationToken);
    }

    // Tropical Transits Methods

    public async Task<T?> GetTropicalTransitsDailyAsync<T>(BirthDataWithPredictionTimezone birthData, CancellationToken cancellationToken = default)
    {
        var formData = _formDataConverter.Convert(birthData);
        return await _httpClientWrapper.PostAsync<T>("tropical_transits/daily", formData, cancellationToken);
    }

    public async Task<T?> GetTropicalTransitsWeeklyAsync<T>(BirthDataWithPredictionTimezone birthData, CancellationToken cancellationToken = default)
    {
        var formData = _formDataConverter.Convert(birthData);
        return await _httpClientWrapper.PostAsync<T>("tropical_transits/weekly", formData, cancellationToken);
    }

    public async Task<T?> GetTropicalTransitsMonthlyAsync<T>(BirthDataWithPredictionTimezone birthData, CancellationToken cancellationToken = default)
    {
        var formData = _formDataConverter.Convert(birthData);
        return await _httpClientWrapper.PostAsync<T>("tropical_transits/monthly", formData, cancellationToken);
    }

    // Solar Return Methods

    public async Task<T?> GetSolarReturnDetailsAsync<T>(BirthData birthData, CancellationToken cancellationToken = default)
    {
        var formData = _formDataConverter.Convert(birthData);
        return await _httpClientWrapper.PostAsync<T>("solar_return_details", formData, cancellationToken);
    }

    public async Task<T?> GetSolarReturnPlanetsAsync<T>(BirthData birthData, CancellationToken cancellationToken = default)
    {
        var formData = _formDataConverter.Convert(birthData);
        return await _httpClientWrapper.PostAsync<T>("solar_return_planets", formData, cancellationToken);
    }

    public async Task<T?> GetSolarReturnHouseCuspsAsync<T>(BirthData birthData, CancellationToken cancellationToken = default)
    {
        var formData = _formDataConverter.Convert(birthData);
        return await _httpClientWrapper.PostAsync<T>("solar_return_house_cusps", formData, cancellationToken);
    }

    public async Task<T?> GetSolarReturnPlanetAspectsAsync<T>(BirthData birthData, CancellationToken cancellationToken = default)
    {
        var formData = _formDataConverter.Convert(birthData);
        return await _httpClientWrapper.PostAsync<T>("solar_return_planet_aspects", formData, cancellationToken);
    }

    // Lunar Metrics

    public async Task<T?> GetLunarMetricsAsync<T>(BirthData birthData, CancellationToken cancellationToken = default)
    {
        var formData = _formDataConverter.Convert(birthData);
        return await _httpClientWrapper.PostAsync<T>("lunar_metrics", formData, cancellationToken);
    }

    // Synastry/Composite Methods

    public async Task<T?> GetSynastryHoroscopeAsync<T>(CompositeBirthData compositeData, CancellationToken cancellationToken = default)
    {
        var formData = _formDataConverter.Convert(compositeData);
        return await _httpClientWrapper.PostAsync<T>("synastry_horoscope", formData, cancellationToken);
    }

    // Report Methods

    public async Task<T?> GetPersonalityReportAsync<T>(BirthData birthData, CancellationToken cancellationToken = default)
    {
        var formData = _formDataConverter.Convert(birthData);
        return await _httpClientWrapper.PostAsync<T>("personality_report/tropical", formData, cancellationToken);
    }

    public async Task<T?> GetRomanticPersonalityReportAsync<T>(BirthData birthData, CancellationToken cancellationToken = default)
    {
        var formData = _formDataConverter.Convert(birthData);
        return await _httpClientWrapper.PostAsync<T>("romantic_personality_report/tropical", formData, cancellationToken);
    }

    public async Task<T?> GetLifeForecastReportAsync<T>(BirthData birthData, CancellationToken cancellationToken = default)
    {
        var formData = _formDataConverter.Convert(birthData);
        return await _httpClientWrapper.PostAsync<T>("life_forecast_report/tropical", formData, cancellationToken);
    }

    public async Task<T?> GetRomanticForecastReportAsync<T>(BirthData birthData, CancellationToken cancellationToken = default)
    {
        var formData = _formDataConverter.Convert(birthData);
        return await _httpClientWrapper.PostAsync<T>("romantic_forecast_report/tropical", formData, cancellationToken);
    }

    public async Task<T?> GetFriendshipReportAsync<T>(CompositeBirthData compositeData, CancellationToken cancellationToken = default)
    {
        var formData = _formDataConverter.Convert(compositeData);
        return await _httpClientWrapper.PostAsync<T>("friendship_report/tropical", formData, cancellationToken);
    }

    public async Task<T?> GetKarmaDestinyReportAsync<T>(CompositeBirthData compositeData, CancellationToken cancellationToken = default)
    {
        var formData = _formDataConverter.Convert(compositeData);
        return await _httpClientWrapper.PostAsync<T>("karma_destiny_report/tropical", formData, cancellationToken);
    }

    public async Task<T?> GetLoveCompatibilityReportAsync<T>(CompositeBirthData compositeData, CancellationToken cancellationToken = default)
    {
        var formData = _formDataConverter.Convert(compositeData);
        return await _httpClientWrapper.PostAsync<T>("love_compatibility_report/tropical", formData, cancellationToken);
    }

    public async Task<T?> GetRomanticForecastCoupleReportAsync<T>(CompositeBirthData compositeData, CancellationToken cancellationToken = default)
    {
        var formData = _formDataConverter.Convert(compositeData);
        return await _httpClientWrapper.PostAsync<T>("romantic_forecast_couple_report/tropical", formData, cancellationToken);
    }

    public async Task<T?> GetGeneralAscendantReportAsync<T>(CompositeBirthData compositeData, CancellationToken cancellationToken = default)
    {
        var formData = _formDataConverter.Convert(compositeData);
        return await _httpClientWrapper.PostAsync<T>("general_ascendant_report/tropical", formData, cancellationToken);
    }

    public async Task<T?> GetGeneralSignReportAsync<T>(string sign, CompositeBirthData compositeData, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(sign))
            throw new ArgumentException("Sign cannot be null or empty", nameof(sign));

        var formData = _formDataConverter.Convert(compositeData);
        return await _httpClientWrapper.PostAsync<T>($"general_sign_report/tropical/{sign}", formData, cancellationToken);
    }

    public async Task<T?> GetGeneralHouseReportAsync<T>(CompositeBirthData compositeData, CancellationToken cancellationToken = default)
    {
        var formData = _formDataConverter.Convert(compositeData);
        return await _httpClientWrapper.PostAsync<T>("general_house_report", formData, cancellationToken);
    }

    // Compatibility Methods

    public async Task<T?> GetZodiacCompatibilityAsync<T>(string zodiacName, string partnerZodiacName, CompositeBirthData compositeData, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(zodiacName))
            throw new ArgumentException("Zodiac name cannot be null or empty", nameof(zodiacName));
        if (string.IsNullOrWhiteSpace(partnerZodiacName))
            throw new ArgumentException("Partner zodiac name cannot be null or empty", nameof(partnerZodiacName));

        var formData = _formDataConverter.Convert(compositeData);
        return await _httpClientWrapper.PostAsync<T>($"zodiac_compatibility/{zodiacName}/{partnerZodiacName}", formData, cancellationToken);
    }

    public async Task<T?> GetCompatibilityAsync<T>(string sunSign, string risingSign, string partnerSunSign, string partnerRisingSign, CompositeBirthData compositeData, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(sunSign))
            throw new ArgumentException("Sun sign cannot be null or empty", nameof(sunSign));
        if (string.IsNullOrWhiteSpace(risingSign))
            throw new ArgumentException("Rising sign cannot be null or empty", nameof(risingSign));
        if (string.IsNullOrWhiteSpace(partnerSunSign))
            throw new ArgumentException("Partner sun sign cannot be null or empty", nameof(partnerSunSign));
        if (string.IsNullOrWhiteSpace(partnerRisingSign))
            throw new ArgumentException("Partner rising sign cannot be null or empty", nameof(partnerRisingSign));

        var formData = _formDataConverter.Convert(compositeData);
        return await _httpClientWrapper.PostAsync<T>($"compatibility/{sunSign}/{risingSign}/{partnerSunSign}/{partnerRisingSign}", formData, cancellationToken);
    }

    // Tarot Methods

    public async Task<T?> GetTarotPredictionsAsync<T>(TarotPredictionData tarotData, CancellationToken cancellationToken = default)
    {
        var formData = _formDataConverter.Convert(tarotData);
        return await _httpClientWrapper.PostAsync<T>("tarot_predictions", formData, cancellationToken);
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _httpClientWrapper?.Dispose();
            _httpClient?.Dispose();
            _disposed = true;
        }
    }
}
