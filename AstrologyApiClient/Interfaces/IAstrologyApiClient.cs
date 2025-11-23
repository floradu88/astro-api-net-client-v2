using AstrologyApiClient.Models;

namespace AstrologyApiClient.Interfaces;

/// <summary>
/// Interface for the Astrology API Client
/// </summary>
public interface IAstrologyApiClient : IDisposable
{
    Task<T?> GetWesternHoroscopeAsync<T>(BirthData birthData, CancellationToken cancellationToken = default);
    Task<T?> GetWesternChartDataAsync<T>(BirthData birthData, CancellationToken cancellationToken = default);
    Task<T?> GetWesternChartImageAsync<T>(BirthData birthData, CancellationToken cancellationToken = default);
    Task<T?> GetPlanetPredictionAsync<T>(string planet, BirthData birthData, CancellationToken cancellationToken = default);
    Task<T?> GetTropicalTransitsDailyAsync<T>(BirthDataWithPredictionTimezone birthData, CancellationToken cancellationToken = default);
    Task<T?> GetTropicalTransitsWeeklyAsync<T>(BirthDataWithPredictionTimezone birthData, CancellationToken cancellationToken = default);
    Task<T?> GetTropicalTransitsMonthlyAsync<T>(BirthDataWithPredictionTimezone birthData, CancellationToken cancellationToken = default);
    Task<T?> GetSolarReturnDetailsAsync<T>(BirthData birthData, CancellationToken cancellationToken = default);
    Task<T?> GetSolarReturnPlanetsAsync<T>(BirthData birthData, CancellationToken cancellationToken = default);
    Task<T?> GetSolarReturnHouseCuspsAsync<T>(BirthData birthData, CancellationToken cancellationToken = default);
    Task<T?> GetSolarReturnPlanetAspectsAsync<T>(BirthData birthData, CancellationToken cancellationToken = default);
    Task<T?> GetLunarMetricsAsync<T>(BirthData birthData, CancellationToken cancellationToken = default);
    Task<T?> GetSynastryHoroscopeAsync<T>(CompositeBirthData compositeData, CancellationToken cancellationToken = default);
    Task<T?> GetPersonalityReportAsync<T>(BirthData birthData, CancellationToken cancellationToken = default);
    Task<T?> GetRomanticPersonalityReportAsync<T>(BirthData birthData, CancellationToken cancellationToken = default);
    Task<T?> GetLifeForecastReportAsync<T>(BirthData birthData, CancellationToken cancellationToken = default);
    Task<T?> GetRomanticForecastReportAsync<T>(BirthData birthData, CancellationToken cancellationToken = default);
    Task<T?> GetFriendshipReportAsync<T>(CompositeBirthData compositeData, CancellationToken cancellationToken = default);
    Task<T?> GetKarmaDestinyReportAsync<T>(CompositeBirthData compositeData, CancellationToken cancellationToken = default);
    Task<T?> GetLoveCompatibilityReportAsync<T>(CompositeBirthData compositeData, CancellationToken cancellationToken = default);
    Task<T?> GetRomanticForecastCoupleReportAsync<T>(CompositeBirthData compositeData, CancellationToken cancellationToken = default);
    Task<T?> GetGeneralAscendantReportAsync<T>(CompositeBirthData compositeData, CancellationToken cancellationToken = default);
    Task<T?> GetGeneralSignReportAsync<T>(string sign, CompositeBirthData compositeData, CancellationToken cancellationToken = default);
    Task<T?> GetGeneralHouseReportAsync<T>(CompositeBirthData compositeData, CancellationToken cancellationToken = default);
    Task<T?> GetZodiacCompatibilityAsync<T>(string zodiacName, string partnerZodiacName, CompositeBirthData compositeData, CancellationToken cancellationToken = default);
    Task<T?> GetCompatibilityAsync<T>(string sunSign, string risingSign, string partnerSunSign, string partnerRisingSign, CompositeBirthData compositeData, CancellationToken cancellationToken = default);
    Task<T?> GetTarotPredictionsAsync<T>(TarotPredictionData tarotData, CancellationToken cancellationToken = default);
}

