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

    /// <summary>
    /// Gets the western horoscope data for the given birth data
    /// </summary>
    /// <typeparam name="T">The response type</typeparam>
    /// <param name="birthData">Birth data for the horoscope</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The horoscope data</returns>
    public async Task<T?> GetWesternHoroscopeAsync<T>(BirthData birthData, CancellationToken cancellationToken = default)
    {
        var formData = _formDataConverter.Convert(birthData);
        return await _httpClientWrapper.PostAsync<T>("western_horoscope", formData, cancellationToken);
    }

    /// <summary>
    /// Gets the western chart data for the given birth data
    /// </summary>
    /// <typeparam name="T">The response type</typeparam>
    /// <param name="birthData">Birth data for the chart</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The chart data</returns>
    public async Task<T?> GetWesternChartDataAsync<T>(BirthData birthData, CancellationToken cancellationToken = default)
    {
        var formData = _formDataConverter.Convert(birthData);
        return await _httpClientWrapper.PostAsync<T>("western_horoscope", formData, cancellationToken);
    }

    /// <summary>
    /// Gets the natal wheel chart image (Western wheel chart).
    /// Note: Western wheel chart is the same as natal wheel chart.
    /// Uses the natal_wheel_chart API endpoint.
    /// </summary>
    /// <typeparam name="T">The response type</typeparam>
    /// <param name="birthData">Birth data for the chart</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The chart image response containing the chart URL</returns>
    public async Task<T?> GetWesternChartImageAsync<T>(BirthData birthData, CancellationToken cancellationToken = default)
    {
        var formData = _formDataConverter.Convert(birthData);
        return await _httpClientWrapper.PostAsync<T>("natal_wheel_chart", formData, cancellationToken);
    }

    // Planet Prediction Methods

    /// <summary>
    /// Gets personalized planet prediction for the specified planet
    /// </summary>
    /// <typeparam name="T">The response type</typeparam>
    /// <param name="planet">The planet name (e.g., "mars", "moon", "sun")</param>
    /// <param name="birthData">Birth data for the prediction</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The planet prediction data</returns>
    public async Task<T?> GetPlanetPredictionAsync<T>(string planet, BirthData birthData, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(planet))
            throw new ArgumentException("Planet cannot be null or empty", nameof(planet));

        var formData = _formDataConverter.Convert(birthData);
        return await _httpClientWrapper.PostAsync<T>($"personalized_planet_prediction/daily/{planet}", formData, cancellationToken);
    }

    // Tropical Transits Methods

    /// <summary>
    /// Gets daily tropical transits for the given birth data
    /// </summary>
    /// <typeparam name="T">The response type</typeparam>
    /// <param name="birthData">Birth data with prediction timezone</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The daily transits data</returns>
    public async Task<T?> GetTropicalTransitsDailyAsync<T>(BirthDataWithPredictionTimezone birthData, CancellationToken cancellationToken = default)
    {
        var formData = _formDataConverter.Convert(birthData);
        return await _httpClientWrapper.PostAsync<T>("tropical_transits/daily", formData, cancellationToken);
    }

    /// <summary>
    /// Gets weekly tropical transits for the given birth data
    /// </summary>
    /// <typeparam name="T">The response type</typeparam>
    /// <param name="birthData">Birth data with prediction timezone</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The weekly transits data</returns>
    public async Task<T?> GetTropicalTransitsWeeklyAsync<T>(BirthDataWithPredictionTimezone birthData, CancellationToken cancellationToken = default)
    {
        var formData = _formDataConverter.Convert(birthData);
        return await _httpClientWrapper.PostAsync<T>("tropical_transits/weekly", formData, cancellationToken);
    }

    /// <summary>
    /// Gets monthly tropical transits for the given birth data
    /// </summary>
    /// <typeparam name="T">The response type</typeparam>
    /// <param name="birthData">Birth data with prediction timezone</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The monthly transits data</returns>
    public async Task<T?> GetTropicalTransitsMonthlyAsync<T>(BirthDataWithPredictionTimezone birthData, CancellationToken cancellationToken = default)
    {
        var formData = _formDataConverter.Convert(birthData);
        return await _httpClientWrapper.PostAsync<T>("tropical_transits/monthly", formData, cancellationToken);
    }

    // Solar Return Methods

    /// <summary>
    /// Gets solar return details for the given birth data
    /// </summary>
    /// <typeparam name="T">The response type</typeparam>
    /// <param name="birthData">Birth data for the solar return</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The solar return details</returns>
    public async Task<T?> GetSolarReturnDetailsAsync<T>(BirthData birthData, CancellationToken cancellationToken = default)
    {
        var formData = _formDataConverter.Convert(birthData);
        return await _httpClientWrapper.PostAsync<T>("solar_return_details", formData, cancellationToken);
    }

    /// <summary>
    /// Gets solar return planets for the given birth data
    /// </summary>
    /// <typeparam name="T">The response type</typeparam>
    /// <param name="birthData">Birth data for the solar return</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The solar return planets data</returns>
    public async Task<T?> GetSolarReturnPlanetsAsync<T>(BirthData birthData, CancellationToken cancellationToken = default)
    {
        var formData = _formDataConverter.Convert(birthData);
        return await _httpClientWrapper.PostAsync<T>("solar_return_planets", formData, cancellationToken);
    }

    /// <summary>
    /// Gets solar return house cusps for the given birth data
    /// </summary>
    /// <typeparam name="T">The response type</typeparam>
    /// <param name="birthData">Birth data for the solar return</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The solar return house cusps data</returns>
    public async Task<T?> GetSolarReturnHouseCuspsAsync<T>(BirthData birthData, CancellationToken cancellationToken = default)
    {
        var formData = _formDataConverter.Convert(birthData);
        return await _httpClientWrapper.PostAsync<T>("solar_return_house_cusps", formData, cancellationToken);
    }

    /// <summary>
    /// Gets solar return planet aspects for the given birth data
    /// </summary>
    /// <typeparam name="T">The response type</typeparam>
    /// <param name="birthData">Birth data for the solar return</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The solar return planet aspects data</returns>
    public async Task<T?> GetSolarReturnPlanetAspectsAsync<T>(BirthData birthData, CancellationToken cancellationToken = default)
    {
        var formData = _formDataConverter.Convert(birthData);
        return await _httpClientWrapper.PostAsync<T>("solar_return_planet_aspects", formData, cancellationToken);
    }

    // Lunar Metrics

    /// <summary>
    /// Gets lunar metrics for the given birth data
    /// </summary>
    /// <typeparam name="T">The response type</typeparam>
    /// <param name="birthData">Birth data for the lunar metrics</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The lunar metrics data</returns>
    public async Task<T?> GetLunarMetricsAsync<T>(BirthData birthData, CancellationToken cancellationToken = default)
    {
        var formData = _formDataConverter.Convert(birthData);
        return await _httpClientWrapper.PostAsync<T>("lunar_metrics", formData, cancellationToken);
    }

    // Synastry/Composite Methods

    /// <summary>
    /// Gets synastry horoscope for two people
    /// </summary>
    /// <typeparam name="T">The response type</typeparam>
    /// <param name="compositeData">Composite birth data for both people</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The synastry horoscope data</returns>
    public async Task<T?> GetSynastryHoroscopeAsync<T>(CompositeBirthData compositeData, CancellationToken cancellationToken = default)
    {
        var formData = _formDataConverter.Convert(compositeData);
        return await _httpClientWrapper.PostAsync<T>("synastry_horoscope", formData, cancellationToken);
    }

    // Report Methods

    /// <summary>
    /// Gets personality report for the given birth data
    /// </summary>
    /// <typeparam name="T">The response type</typeparam>
    /// <param name="birthData">Birth data for the personality report</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The personality report data</returns>
    public async Task<T?> GetPersonalityReportAsync<T>(BirthData birthData, CancellationToken cancellationToken = default)
    {
        var formData = _formDataConverter.Convert(birthData);
        return await _httpClientWrapper.PostAsync<T>("personality_report/tropical", formData, cancellationToken);
    }

    /// <summary>
    /// Gets romantic personality report for the given birth data
    /// </summary>
    /// <typeparam name="T">The response type</typeparam>
    /// <param name="birthData">Birth data for the romantic personality report</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The romantic personality report data</returns>
    public async Task<T?> GetRomanticPersonalityReportAsync<T>(BirthData birthData, CancellationToken cancellationToken = default)
    {
        var formData = _formDataConverter.Convert(birthData);
        return await _httpClientWrapper.PostAsync<T>("romantic_personality_report/tropical", formData, cancellationToken);
    }

    /// <summary>
    /// Gets life forecast report for the given birth data
    /// </summary>
    /// <typeparam name="T">The response type</typeparam>
    /// <param name="birthData">Birth data for the life forecast report</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The life forecast report data</returns>
    public async Task<T?> GetLifeForecastReportAsync<T>(BirthData birthData, CancellationToken cancellationToken = default)
    {
        var formData = _formDataConverter.Convert(birthData);
        return await _httpClientWrapper.PostAsync<T>("life_forecast_report/tropical", formData, cancellationToken);
    }

    /// <summary>
    /// Gets romantic forecast report for the given birth data
    /// </summary>
    /// <typeparam name="T">The response type</typeparam>
    /// <param name="birthData">Birth data for the romantic forecast report</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The romantic forecast report data</returns>
    public async Task<T?> GetRomanticForecastReportAsync<T>(BirthData birthData, CancellationToken cancellationToken = default)
    {
        var formData = _formDataConverter.Convert(birthData);
        return await _httpClientWrapper.PostAsync<T>("romantic_forecast_report/tropical", formData, cancellationToken);
    }

    /// <summary>
    /// Gets friendship report for two people
    /// </summary>
    /// <typeparam name="T">The response type</typeparam>
    /// <param name="compositeData">Composite birth data for both people</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The friendship report data</returns>
    public async Task<T?> GetFriendshipReportAsync<T>(CompositeBirthData compositeData, CancellationToken cancellationToken = default)
    {
        var formData = _formDataConverter.Convert(compositeData);
        return await _httpClientWrapper.PostAsync<T>("friendship_report/tropical", formData, cancellationToken);
    }

    /// <summary>
    /// Gets karma destiny report for two people
    /// </summary>
    /// <typeparam name="T">The response type</typeparam>
    /// <param name="compositeData">Composite birth data for both people</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The karma destiny report data</returns>
    public async Task<T?> GetKarmaDestinyReportAsync<T>(CompositeBirthData compositeData, CancellationToken cancellationToken = default)
    {
        var formData = _formDataConverter.Convert(compositeData);
        return await _httpClientWrapper.PostAsync<T>("karma_destiny_report/tropical", formData, cancellationToken);
    }

    /// <summary>
    /// Gets love compatibility report for two people
    /// </summary>
    /// <typeparam name="T">The response type</typeparam>
    /// <param name="compositeData">Composite birth data for both people</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The love compatibility report data</returns>
    public async Task<T?> GetLoveCompatibilityReportAsync<T>(CompositeBirthData compositeData, CancellationToken cancellationToken = default)
    {
        var formData = _formDataConverter.Convert(compositeData);
        return await _httpClientWrapper.PostAsync<T>("love_compatibility_report/tropical", formData, cancellationToken);
    }

    /// <summary>
    /// Gets romantic forecast couple report for two people
    /// </summary>
    /// <typeparam name="T">The response type</typeparam>
    /// <param name="compositeData">Composite birth data for both people</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The romantic forecast couple report data</returns>
    public async Task<T?> GetRomanticForecastCoupleReportAsync<T>(CompositeBirthData compositeData, CancellationToken cancellationToken = default)
    {
        var formData = _formDataConverter.Convert(compositeData);
        return await _httpClientWrapper.PostAsync<T>("romantic_forecast_couple_report/tropical", formData, cancellationToken);
    }

    /// <summary>
    /// Gets general ascendant report for two people using composite birth data
    /// Uses hybrid format: primary person as day/month/year, secondary as s_day/s_month/s_year
    /// </summary>
    /// <typeparam name="T">The response type</typeparam>
    /// <param name="compositeData">Composite birth data for both people</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The general ascendant report data</returns>
    public async Task<T?> GetGeneralAscendantReportAsync<T>(CompositeBirthData compositeData, CancellationToken cancellationToken = default)
    {
        // These endpoints require hybrid format: primary person as day/month/year (not p_day), secondary as s_day/s_month/s_year
        var formData = _formDataConverter.ConvertToHybridFormat(compositeData);
        return await _httpClientWrapper.PostAsync<T>("general_ascendant_report/tropical", formData, cancellationToken);
    }

    /// <summary>
    /// Gets general ascendant report for two people using separate birth data objects
    /// Uses hybrid format: primary person as day/month/year, secondary as s_day/s_month/s_year
    /// </summary>
    /// <typeparam name="T">The response type</typeparam>
    /// <param name="primary">Birth data for the primary person</param>
    /// <param name="secondary">Birth data for the secondary person</param>
    /// <param name="orb">Optional orb value for aspect calculations</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The general ascendant report data</returns>
    public async Task<T?> GetGeneralAscendantReportAsync<T>(BirthData primary, BirthData secondary, double? orb = null, CancellationToken cancellationToken = default)
    {
        // These endpoints require hybrid format: primary person as day/month/year (not p_day), secondary as s_day/s_month/s_year
        var formData = _formDataConverter.ConvertToHybridFormat(primary, secondary, orb);
        return await _httpClientWrapper.PostAsync<T>("general_ascendant_report/tropical", formData, cancellationToken);
    }

    /// <summary>
    /// Gets general sign report for the specified sign using composite birth data
    /// Uses hybrid format: primary person as day/month/year, secondary as s_day/s_month/s_year
    /// </summary>
    /// <typeparam name="T">The response type</typeparam>
    /// <param name="sign">The zodiac sign (e.g., "sun", "moon", "mars")</param>
    /// <param name="compositeData">Composite birth data for both people</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The general sign report data</returns>
    public async Task<T?> GetGeneralSignReportAsync<T>(string sign, CompositeBirthData compositeData, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(sign))
            throw new ArgumentException("Sign cannot be null or empty", nameof(sign));

        // These endpoints require hybrid format: primary person as day/month/year (not p_day), secondary as s_day/s_month/s_year
        var formData = _formDataConverter.ConvertToHybridFormat(compositeData);
        return await _httpClientWrapper.PostAsync<T>($"general_sign_report/tropical/{sign}", formData, cancellationToken);
    }

    /// <summary>
    /// Gets general sign report for the specified sign using separate birth data objects
    /// Uses hybrid format: primary person as day/month/year, secondary as s_day/s_month/s_year
    /// </summary>
    /// <typeparam name="T">The response type</typeparam>
    /// <param name="sign">The zodiac sign (e.g., "sun", "moon", "mars")</param>
    /// <param name="primary">Birth data for the primary person</param>
    /// <param name="secondary">Birth data for the secondary person</param>
    /// <param name="orb">Optional orb value for aspect calculations</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The general sign report data</returns>
    public async Task<T?> GetGeneralSignReportAsync<T>(string sign, BirthData primary, BirthData secondary, double? orb = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(sign))
            throw new ArgumentException("Sign cannot be null or empty", nameof(sign));

        // These endpoints require hybrid format: primary person as day/month/year (not p_day), secondary as s_day/s_month/s_year
        var formData = _formDataConverter.ConvertToHybridFormat(primary, secondary, orb);
        return await _httpClientWrapper.PostAsync<T>($"general_sign_report/tropical/{sign}", formData, cancellationToken);
    }

    /// <summary>
    /// Gets general house report for two people using composite birth data
    /// Uses hybrid format: primary person as day/month/year, secondary as s_day/s_month/s_year
    /// </summary>
    /// <typeparam name="T">The response type</typeparam>
    /// <param name="compositeData">Composite birth data for both people</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The general house report data</returns>
    public async Task<T?> GetGeneralHouseReportAsync<T>(CompositeBirthData compositeData, CancellationToken cancellationToken = default)
    {
        // These endpoints require hybrid format: primary person as day/month/year (not p_day), secondary as s_day/s_month/s_year
        var formData = _formDataConverter.ConvertToHybridFormat(compositeData);
        return await _httpClientWrapper.PostAsync<T>("general_house_report/tropical", formData, cancellationToken);
    }

    /// <summary>
    /// Gets general house report for two people using separate birth data objects
    /// Uses hybrid format: primary person as day/month/year, secondary as s_day/s_month/s_year
    /// </summary>
    /// <typeparam name="T">The response type</typeparam>
    /// <param name="primary">Birth data for the primary person</param>
    /// <param name="secondary">Birth data for the secondary person</param>
    /// <param name="orb">Optional orb value for aspect calculations</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The general house report data</returns>
    public async Task<T?> GetGeneralHouseReportAsync<T>(BirthData primary, BirthData secondary, double? orb = null, CancellationToken cancellationToken = default)
    {
        // These endpoints require hybrid format: primary person as day/month/year (not p_day), secondary as s_day/s_month/s_year
        var formData = _formDataConverter.ConvertToHybridFormat(primary, secondary, orb);
        return await _httpClientWrapper.PostAsync<T>("general_house_report/tropical", formData, cancellationToken);
    }

    // Compatibility Methods

    /// <summary>
    /// Gets zodiac compatibility between two zodiac signs
    /// </summary>
    /// <typeparam name="T">The response type</typeparam>
    /// <param name="zodiacName">The first zodiac sign name</param>
    /// <param name="partnerZodiacName">The partner zodiac sign name</param>
    /// <param name="compositeData">Composite birth data for both people</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The zodiac compatibility data</returns>
    public async Task<T?> GetZodiacCompatibilityAsync<T>(string zodiacName, string partnerZodiacName, CompositeBirthData compositeData, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(zodiacName))
            throw new ArgumentException("Zodiac name cannot be null or empty", nameof(zodiacName));
        if (string.IsNullOrWhiteSpace(partnerZodiacName))
            throw new ArgumentException("Partner zodiac name cannot be null or empty", nameof(partnerZodiacName));

        var formData = _formDataConverter.Convert(compositeData);
        return await _httpClientWrapper.PostAsync<T>($"zodiac_compatibility/{zodiacName}/{partnerZodiacName}", formData, cancellationToken);
    }

    /// <summary>
    /// Gets compatibility report based on sun and rising signs for both people
    /// </summary>
    /// <typeparam name="T">The response type</typeparam>
    /// <param name="sunSign">The first person's sun sign</param>
    /// <param name="risingSign">The first person's rising sign</param>
    /// <param name="partnerSunSign">The partner's sun sign</param>
    /// <param name="partnerRisingSign">The partner's rising sign</param>
    /// <param name="compositeData">Composite birth data for both people</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The compatibility report data</returns>
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

    /// <summary>
    /// Gets tarot predictions for love, career, and finance
    /// </summary>
    /// <typeparam name="T">The response type</typeparam>
    /// <param name="tarotData">Tarot prediction data with love, career, and finance scores</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The tarot predictions data</returns>
    public async Task<T?> GetTarotPredictionsAsync<T>(TarotPredictionData tarotData, CancellationToken cancellationToken = default)
    {
        var formData = _formDataConverter.Convert(tarotData);
        return await _httpClientWrapper.PostAsync<T>("tarot_predictions", formData, cancellationToken);
    }

    /// <summary>
    /// Disposes the client and releases all resources
    /// </summary>
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
