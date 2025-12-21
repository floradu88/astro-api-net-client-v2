using AstrologyApiClient.Models;

namespace AstrologyApiClient.Interfaces;

/// <summary>
/// Interface for the Astrology API Client
/// </summary>
public interface IAstrologyApiClient : IDisposable
{
    /// <summary>
    /// Gets the western horoscope data for the given birth data
    /// </summary>
    /// <typeparam name="T">The response type</typeparam>
    /// <param name="birthData">Birth data for the horoscope</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The horoscope data</returns>
    Task<T?> GetWesternHoroscopeAsync<T>(BirthData birthData, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets the western chart data for the given birth data
    /// </summary>
    /// <typeparam name="T">The response type</typeparam>
    /// <param name="birthData">Birth data for the chart</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The chart data</returns>
    Task<T?> GetWesternChartDataAsync<T>(BirthData birthData, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets the natal wheel chart image (Western wheel chart).
    /// Note: Western wheel chart is the same as natal wheel chart.
    /// Uses the natal_wheel_chart API endpoint.
    /// </summary>
    /// <typeparam name="T">The response type</typeparam>
    /// <param name="birthData">Birth data for the chart</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The chart image response containing the chart URL</returns>
    Task<T?> GetWesternChartImageAsync<T>(BirthData birthData, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets personalized planet prediction for the specified planet
    /// </summary>
    /// <typeparam name="T">The response type</typeparam>
    /// <param name="planet">The planet name (e.g., "mars", "moon", "sun")</param>
    /// <param name="birthData">Birth data for the prediction</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The planet prediction data</returns>
    Task<T?> GetPlanetPredictionAsync<T>(string planet, BirthData birthData, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets daily tropical transits for the given birth data
    /// </summary>
    /// <typeparam name="T">The response type</typeparam>
    /// <param name="birthData">Birth data with prediction timezone</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The daily transits data</returns>
    Task<T?> GetTropicalTransitsDailyAsync<T>(BirthDataWithPredictionTimezone birthData, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets weekly tropical transits for the given birth data
    /// </summary>
    /// <typeparam name="T">The response type</typeparam>
    /// <param name="birthData">Birth data with prediction timezone</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The weekly transits data</returns>
    Task<T?> GetTropicalTransitsWeeklyAsync<T>(BirthDataWithPredictionTimezone birthData, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets monthly tropical transits for the given birth data
    /// </summary>
    /// <typeparam name="T">The response type</typeparam>
    /// <param name="birthData">Birth data with prediction timezone</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The monthly transits data</returns>
    Task<T?> GetTropicalTransitsMonthlyAsync<T>(BirthDataWithPredictionTimezone birthData, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets solar return details for the given birth data
    /// </summary>
    /// <typeparam name="T">The response type</typeparam>
    /// <param name="birthData">Birth data for the solar return</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The solar return details</returns>
    Task<T?> GetSolarReturnDetailsAsync<T>(BirthData birthData, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets solar return planets for the given birth data
    /// </summary>
    /// <typeparam name="T">The response type</typeparam>
    /// <param name="birthData">Birth data for the solar return</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The solar return planets data</returns>
    Task<T?> GetSolarReturnPlanetsAsync<T>(BirthData birthData, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets solar return house cusps for the given birth data
    /// </summary>
    /// <typeparam name="T">The response type</typeparam>
    /// <param name="birthData">Birth data for the solar return</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The solar return house cusps data</returns>
    Task<T?> GetSolarReturnHouseCuspsAsync<T>(BirthData birthData, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets solar return planet aspects for the given birth data
    /// </summary>
    /// <typeparam name="T">The response type</typeparam>
    /// <param name="birthData">Birth data for the solar return</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The solar return planet aspects data</returns>
    Task<T?> GetSolarReturnPlanetAspectsAsync<T>(BirthData birthData, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets lunar metrics for the given birth data
    /// </summary>
    /// <typeparam name="T">The response type</typeparam>
    /// <param name="birthData">Birth data for the lunar metrics</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The lunar metrics data</returns>
    Task<T?> GetLunarMetricsAsync<T>(BirthData birthData, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets synastry horoscope for two people
    /// </summary>
    /// <typeparam name="T">The response type</typeparam>
    /// <param name="compositeData">Composite birth data for both people</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The synastry horoscope data</returns>
    Task<T?> GetSynastryHoroscopeAsync<T>(CompositeBirthData compositeData, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets personality report for the given birth data
    /// </summary>
    /// <typeparam name="T">The response type</typeparam>
    /// <param name="birthData">Birth data for the personality report</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The personality report data</returns>
    Task<T?> GetPersonalityReportAsync<T>(BirthData birthData, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets romantic personality report for the given birth data
    /// </summary>
    /// <typeparam name="T">The response type</typeparam>
    /// <param name="birthData">Birth data for the romantic personality report</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The romantic personality report data</returns>
    Task<T?> GetRomanticPersonalityReportAsync<T>(BirthData birthData, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets life forecast report for the given birth data
    /// </summary>
    /// <typeparam name="T">The response type</typeparam>
    /// <param name="birthData">Birth data for the life forecast report</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The life forecast report data</returns>
    Task<T?> GetLifeForecastReportAsync<T>(BirthData birthData, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets romantic forecast report for the given birth data
    /// </summary>
    /// <typeparam name="T">The response type</typeparam>
    /// <param name="birthData">Birth data for the romantic forecast report</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The romantic forecast report data</returns>
    Task<T?> GetRomanticForecastReportAsync<T>(BirthData birthData, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets friendship report for two people
    /// </summary>
    /// <typeparam name="T">The response type</typeparam>
    /// <param name="compositeData">Composite birth data for both people</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The friendship report data</returns>
    Task<T?> GetFriendshipReportAsync<T>(CompositeBirthData compositeData, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets karma destiny report for two people
    /// </summary>
    /// <typeparam name="T">The response type</typeparam>
    /// <param name="compositeData">Composite birth data for both people</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The karma destiny report data</returns>
    Task<T?> GetKarmaDestinyReportAsync<T>(CompositeBirthData compositeData, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets love compatibility report for two people
    /// </summary>
    /// <typeparam name="T">The response type</typeparam>
    /// <param name="compositeData">Composite birth data for both people</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The love compatibility report data</returns>
    Task<T?> GetLoveCompatibilityReportAsync<T>(CompositeBirthData compositeData, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets romantic forecast couple report for two people
    /// </summary>
    /// <typeparam name="T">The response type</typeparam>
    /// <param name="compositeData">Composite birth data for both people</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The romantic forecast couple report data</returns>
    Task<T?> GetRomanticForecastCoupleReportAsync<T>(CompositeBirthData compositeData, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets general ascendant report for two people using composite birth data
    /// Uses hybrid format: primary person as day/month/year, secondary as s_day/s_month/s_year
    /// </summary>
    /// <typeparam name="T">The response type</typeparam>
    /// <param name="compositeData">Composite birth data for both people</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The general ascendant report data</returns>
    Task<T?> GetGeneralAscendantReportAsync<T>(CompositeBirthData compositeData, CancellationToken cancellationToken = default);
    
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
    Task<T?> GetGeneralAscendantReportAsync<T>(BirthData primary, BirthData secondary, double? orb = null, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets general sign report for the specified sign using composite birth data
    /// Uses hybrid format: primary person as day/month/year, secondary as s_day/s_month/s_year
    /// </summary>
    /// <typeparam name="T">The response type</typeparam>
    /// <param name="sign">The zodiac sign (e.g., "sun", "moon", "mars")</param>
    /// <param name="compositeData">Composite birth data for both people</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The general sign report data</returns>
    Task<T?> GetGeneralSignReportAsync<T>(string sign, CompositeBirthData compositeData, CancellationToken cancellationToken = default);
    
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
    Task<T?> GetGeneralSignReportAsync<T>(string sign, BirthData primary, BirthData secondary, double? orb = null, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets general house report for two people using composite birth data
    /// Uses hybrid format: primary person as day/month/year, secondary as s_day/s_month/s_year
    /// </summary>
    /// <typeparam name="T">The response type</typeparam>
    /// <param name="compositeData">Composite birth data for both people</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The general house report data</returns>
    Task<T?> GetGeneralHouseReportAsync<T>(CompositeBirthData compositeData, CancellationToken cancellationToken = default);
    
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
    Task<T?> GetGeneralHouseReportAsync<T>(BirthData primary, BirthData secondary, double? orb = null, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets zodiac compatibility between two zodiac signs
    /// </summary>
    /// <typeparam name="T">The response type</typeparam>
    /// <param name="zodiacName">The first zodiac sign name</param>
    /// <param name="partnerZodiacName">The partner zodiac sign name</param>
    /// <param name="compositeData">Composite birth data for both people</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The zodiac compatibility data</returns>
    Task<T?> GetZodiacCompatibilityAsync<T>(string zodiacName, string partnerZodiacName, CompositeBirthData compositeData, CancellationToken cancellationToken = default);
    
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
    Task<T?> GetCompatibilityAsync<T>(string sunSign, string risingSign, string partnerSunSign, string partnerRisingSign, CompositeBirthData compositeData, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets tarot predictions for love, career, and finance
    /// </summary>
    /// <typeparam name="T">The response type</typeparam>
    /// <param name="tarotData">Tarot prediction data with love, career, and finance scores</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The tarot predictions data</returns>
    Task<T?> GetTarotPredictionsAsync<T>(TarotPredictionData tarotData, CancellationToken cancellationToken = default);
}

