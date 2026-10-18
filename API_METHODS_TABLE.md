# Astrology API Client Methods (v1.0.1)

## Complete Method Reference

| Method Name | URL | Exposed Method Signature |
|------------|-----|-------------------------|
| GetWesternHoroscopeAsync | `https://json.astrologyapi.com/v1/western_horoscope` | `Task<T?> GetWesternHoroscopeAsync<T>(BirthData birthData, CancellationToken cancellationToken = default)` |
| GetWesternChartDataAsync | `https://json.astrologyapi.com/v1/western_horoscope` | `Task<T?> GetWesternChartDataAsync<T>(BirthData birthData, CancellationToken cancellationToken = default)` |
| GetWesternChartImageAsync | `https://json.astrologyapi.com/v1/natal_wheel_chart` | `Task<T?> GetWesternChartImageAsync<T>(BirthData birthData, CancellationToken cancellationToken = default)` |
| GetPlanetPredictionAsync | `https://json.astrologyapi.com/v1/personalized_planet_prediction/daily/{planet}` | `Task<T?> GetPlanetPredictionAsync<T>(string planet, BirthData birthData, CancellationToken cancellationToken = default)` |
| GetTropicalTransitsDailyAsync | `https://json.astrologyapi.com/v1/tropical_transits/daily` | `Task<T?> GetTropicalTransitsDailyAsync<T>(BirthDataWithPredictionTimezone birthData, CancellationToken cancellationToken = default)` |
| GetTropicalTransitsWeeklyAsync | `https://json.astrologyapi.com/v1/tropical_transits/weekly` | `Task<T?> GetTropicalTransitsWeeklyAsync<T>(BirthDataWithPredictionTimezone birthData, CancellationToken cancellationToken = default)` |
| GetTropicalTransitsMonthlyAsync | `https://json.astrologyapi.com/v1/tropical_transits/monthly` | `Task<T?> GetTropicalTransitsMonthlyAsync<T>(BirthDataWithPredictionTimezone birthData, CancellationToken cancellationToken = default)` |
| GetSolarReturnDetailsAsync | `https://json.astrologyapi.com/v1/solar_return_details` | `Task<T?> GetSolarReturnDetailsAsync<T>(BirthData birthData, CancellationToken cancellationToken = default)` |
| GetSolarReturnPlanetsAsync | `https://json.astrologyapi.com/v1/solar_return_planets` | `Task<T?> GetSolarReturnPlanetsAsync<T>(BirthData birthData, CancellationToken cancellationToken = default)` |
| GetSolarReturnHouseCuspsAsync | `https://json.astrologyapi.com/v1/solar_return_house_cusps` | `Task<T?> GetSolarReturnHouseCuspsAsync<T>(BirthData birthData, CancellationToken cancellationToken = default)` |
| GetSolarReturnPlanetAspectsAsync | `https://json.astrologyapi.com/v1/solar_return_planet_aspects` | `Task<T?> GetSolarReturnPlanetAspectsAsync<T>(BirthData birthData, CancellationToken cancellationToken = default)` |
| GetLunarMetricsAsync | `https://json.astrologyapi.com/v1/lunar_metrics` | `Task<T?> GetLunarMetricsAsync<T>(BirthData birthData, CancellationToken cancellationToken = default)` |
| GetSynastryHoroscopeAsync | `https://json.astrologyapi.com/v1/synastry_horoscope` | `Task<T?> GetSynastryHoroscopeAsync<T>(CompositeBirthData compositeData, CancellationToken cancellationToken = default)` |
| GetPersonalityReportAsync | `https://json.astrologyapi.com/v1/personality_report/tropical` | `Task<T?> GetPersonalityReportAsync<T>(BirthData birthData, CancellationToken cancellationToken = default)` |
| GetRomanticPersonalityReportAsync | `https://json.astrologyapi.com/v1/romantic_personality_report/tropical` | `Task<T?> GetRomanticPersonalityReportAsync<T>(BirthData birthData, CancellationToken cancellationToken = default)` |
| GetLifeForecastReportAsync | `https://json.astrologyapi.com/v1/life_forecast_report/tropical` | `Task<T?> GetLifeForecastReportAsync<T>(BirthData birthData, CancellationToken cancellationToken = default)` |
| GetRomanticForecastReportAsync | `https://json.astrologyapi.com/v1/romantic_forecast_report/tropical` | `Task<T?> GetRomanticForecastReportAsync<T>(BirthData birthData, CancellationToken cancellationToken = default)` |
| GetFriendshipReportAsync | `https://json.astrologyapi.com/v1/friendship_report/tropical` | `Task<T?> GetFriendshipReportAsync<T>(CompositeBirthData compositeData, CancellationToken cancellationToken = default)` |
| GetKarmaDestinyReportAsync | `https://json.astrologyapi.com/v1/karma_destiny_report/tropical` | `Task<T?> GetKarmaDestinyReportAsync<T>(CompositeBirthData compositeData, CancellationToken cancellationToken = default)` |
| GetLoveCompatibilityReportAsync | `https://json.astrologyapi.com/v1/love_compatibility_report/tropical` | `Task<T?> GetLoveCompatibilityReportAsync<T>(CompositeBirthData compositeData, CancellationToken cancellationToken = default)` |
| GetRomanticForecastCoupleReportAsync | `https://json.astrologyapi.com/v1/romantic_forecast_couple_report/tropical` | `Task<T?> GetRomanticForecastCoupleReportAsync<T>(CompositeBirthData compositeData, CancellationToken cancellationToken = default)` |
| GetGeneralAscendantReportAsync (CompositeBirthData) | `https://json.astrologyapi.com/v1/general_ascendant_report/tropical` | `Task<T?> GetGeneralAscendantReportAsync<T>(CompositeBirthData compositeData, CancellationToken cancellationToken = default)` |
| GetGeneralAscendantReportAsync (BirthData overload) | `https://json.astrologyapi.com/v1/general_ascendant_report/tropical` | `Task<T?> GetGeneralAscendantReportAsync<T>(BirthData primary, BirthData secondary, double? orb = null, CancellationToken cancellationToken = default)` |
| GetGeneralSignReportAsync (CompositeBirthData) | `https://json.astrologyapi.com/v1/general_sign_report/tropical/{sign}` | `Task<T?> GetGeneralSignReportAsync<T>(string sign, CompositeBirthData compositeData, CancellationToken cancellationToken = default)` |
| GetGeneralSignReportAsync (BirthData overload) | `https://json.astrologyapi.com/v1/general_sign_report/tropical/{sign}` | `Task<T?> GetGeneralSignReportAsync<T>(string sign, BirthData primary, BirthData secondary, double? orb = null, CancellationToken cancellationToken = default)` |
| GetGeneralHouseReportAsync (CompositeBirthData) | `https://json.astrologyapi.com/v1/general_house_report/tropical` | `Task<T?> GetGeneralHouseReportAsync<T>(CompositeBirthData compositeData, CancellationToken cancellationToken = default)` |
| GetGeneralHouseReportAsync (BirthData overload) | `https://json.astrologyapi.com/v1/general_house_report/tropical` | `Task<T?> GetGeneralHouseReportAsync<T>(BirthData primary, BirthData secondary, double? orb = null, CancellationToken cancellationToken = default)` |
| GetZodiacCompatibilityAsync | `https://json.astrologyapi.com/v1/zodiac_compatibility/{zodiacName}/{partnerZodiacName}` | `Task<T?> GetZodiacCompatibilityAsync<T>(string zodiacName, string partnerZodiacName, CompositeBirthData compositeData, CancellationToken cancellationToken = default)` |
| GetCompatibilityAsync | `https://json.astrologyapi.com/v1/compatibility/{sunSign}/{risingSign}/{partnerSunSign}/{partnerRisingSign}` | `Task<T?> GetCompatibilityAsync<T>(string sunSign, string risingSign, string partnerSunSign, string partnerRisingSign, CompositeBirthData compositeData, CancellationToken cancellationToken = default)` |
| GetTarotPredictionsAsync | `https://json.astrologyapi.com/v1/tarot_predictions` | `Task<T?> GetTarotPredictionsAsync<T>(TarotPredictionData tarotData, CancellationToken cancellationToken = default)` |

## Notes

- All methods are generic (`<T>`) and return `Task<T?>` to support flexible response deserialization
- Methods with `{planet}`, `{sign}`, `{zodiacName}`, etc. in the URL require these values to be provided as method parameters
- Methods accepting `CompositeBirthData` use standard composite format (p_day, p_month, etc. and s_day, s_month, etc.)
- General report methods (GetGeneralAscendantReportAsync, GetGeneralSignReportAsync, GetGeneralHouseReportAsync) support two overloads:
  - One accepting `CompositeBirthData` 
  - One accepting two separate `BirthData` objects with optional `orb` parameter
- General report methods use a hybrid format where primary person uses simple field names (day, month, year) and secondary person uses s_ prefix (s_day, s_month, s_year)
- All methods support optional `CancellationToken` for cancellation support

