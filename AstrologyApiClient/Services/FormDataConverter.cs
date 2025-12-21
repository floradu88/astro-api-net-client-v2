using AstrologyApiClient.Interfaces;
using AstrologyApiClient.Models;

namespace AstrologyApiClient.Services;

/// <summary>
/// Converts model objects to form data dictionaries
/// </summary>
public class FormDataConverter : IFormDataConverter
{
    /// <summary>
    /// Converts BirthData to form data dictionary
    /// </summary>
    /// <param name="data">Birth data to convert</param>
    /// <returns>Dictionary containing form data</returns>
    public Dictionary<string, string> Convert(BirthData data)
    {
        if (data == null)
            throw new ArgumentNullException(nameof(data));

        return new Dictionary<string, string>
        {
            { "day", data.Day.ToString() },
            { "month", data.Month.ToString() },
            { "year", data.Year.ToString() },
            { "hour", data.Hour.ToString() },
            { "min", data.Min.ToString() },
            { "lat", data.Lat.ToString() },
            { "lon", data.Lon.ToString() },
            { "tzone", data.Tzone.ToString() }
        };
    }

    /// <summary>
    /// Converts CompositeBirthData to form data dictionary using standard format (p_ prefix for primary)
    /// </summary>
    /// <param name="data">Composite birth data to convert</param>
    /// <returns>Dictionary containing form data</returns>
    public Dictionary<string, string> Convert(CompositeBirthData data)
    {
        if (data == null)
            throw new ArgumentNullException(nameof(data));
        if (data.Primary == null)
            throw new ArgumentNullException(nameof(data.Primary));
        if (data.Secondary == null)
            throw new ArgumentNullException(nameof(data.Secondary));

        var formData = new Dictionary<string, string>
        {
            { "p_day", data.Primary.PDay.ToString() },
            { "p_month", data.Primary.PMonth.ToString() },
            { "p_year", data.Primary.PYear.ToString() },
            { "p_hour", data.Primary.PHour.ToString() },
            { "p_min", data.Primary.PMin.ToString() },
            { "p_lat", data.Primary.PLat.ToString() },
            { "p_lon", data.Primary.PLon.ToString() },
            { "p_tzone", data.Primary.PTzone.ToString() },
            { "s_day", data.Secondary.SDay.ToString() },
            { "s_month", data.Secondary.SMonth.ToString() },
            { "s_year", data.Secondary.SYear.ToString() },
            { "s_hour", data.Secondary.SHour.ToString() },
            { "s_min", data.Secondary.SMin.ToString() },
            { "s_lat", data.Secondary.SLat.ToString() },
            { "s_lon", data.Secondary.SLon.ToString() },
            { "s_tzone", data.Secondary.STzone.ToString() }
        };

        if (data.Orb.HasValue)
        {
            formData.Add("orb", data.Orb.Value.ToString());
        }

        return formData;
    }

    /// <summary>
    /// Converts BirthDataWithPredictionTimezone to form data dictionary
    /// </summary>
    /// <param name="data">Birth data with prediction timezone to convert</param>
    /// <returns>Dictionary containing form data</returns>
    public Dictionary<string, string> Convert(BirthDataWithPredictionTimezone data)
    {
        if (data == null)
            throw new ArgumentNullException(nameof(data));

        var formData = Convert((BirthData)data);
        formData.Add("prediction_timezone", data.PredictionTimezone.ToString());
        return formData;
    }

    /// <summary>
    /// Converts TarotPredictionData to form data dictionary
    /// </summary>
    /// <param name="data">Tarot prediction data to convert</param>
    /// <returns>Dictionary containing form data</returns>
    public Dictionary<string, string> Convert(TarotPredictionData data)
    {
        if (data == null)
            throw new ArgumentNullException(nameof(data));

        return new Dictionary<string, string>
        {
            { "love", data.Love.ToString() },
            { "career", data.Career.ToString() },
            { "finance", data.Finance.ToString() }
        };
    }

    /// <summary>
    /// Converts CompositeBirthData to hybrid format for general report endpoints
    /// Primary person uses simple format (day, month, year) instead of (p_day, p_month, p_year)
    /// Secondary person uses s_ prefix format (s_day, s_month, s_year)
    /// </summary>
    public Dictionary<string, string> ConvertToHybridFormat(CompositeBirthData data)
    {
        if (data == null)
            throw new ArgumentNullException(nameof(data));
        if (data.Primary == null)
            throw new ArgumentNullException(nameof(data.Primary));
        if (data.Secondary == null)
            throw new ArgumentNullException(nameof(data.Secondary));

        var formData = new Dictionary<string, string>
        {
            // Primary person in simple format (not p_day, but day)
            { "day", data.Primary.PDay.ToString() },
            { "month", data.Primary.PMonth.ToString() },
            { "year", data.Primary.PYear.ToString() },
            { "hour", data.Primary.PHour.ToString() },
            { "min", data.Primary.PMin.ToString() },
            { "lat", data.Primary.PLat.ToString() },
            { "lon", data.Primary.PLon.ToString() },
            { "tzone", data.Primary.PTzone.ToString() },
            // Secondary person in s_ format
            { "s_day", data.Secondary.SDay.ToString() },
            { "s_month", data.Secondary.SMonth.ToString() },
            { "s_year", data.Secondary.SYear.ToString() },
            { "s_hour", data.Secondary.SHour.ToString() },
            { "s_min", data.Secondary.SMin.ToString() },
            { "s_lat", data.Secondary.SLat.ToString() },
            { "s_lon", data.Secondary.SLon.ToString() },
            { "s_tzone", data.Secondary.STzone.ToString() }
        };

        if (data.Orb.HasValue)
        {
            formData.Add("orb", data.Orb.Value.ToString());
        }

        return formData;
    }

    /// <summary>
    /// Converts two BirthData objects to hybrid format for general report endpoints
    /// Primary person uses simple format (day, month, year) instead of (p_day, p_month, p_year)
    /// Secondary person uses s_ prefix format (s_day, s_month, s_year)
    /// </summary>
    /// <param name="primary">Birth data for the primary person</param>
    /// <param name="secondary">Birth data for the secondary person</param>
    /// <param name="orb">Optional orb value for aspect calculations</param>
    public Dictionary<string, string> ConvertToHybridFormat(BirthData primary, BirthData secondary, double? orb = null)
    {
        if (primary == null)
            throw new ArgumentNullException(nameof(primary));
        if (secondary == null)
            throw new ArgumentNullException(nameof(secondary));

        var formData = new Dictionary<string, string>
        {
            // Primary person in simple format (day, not p_day)
            { "day", primary.Day.ToString() },
            { "month", primary.Month.ToString() },
            { "year", primary.Year.ToString() },
            { "hour", primary.Hour.ToString() },
            { "min", primary.Min.ToString() },
            { "lat", primary.Lat.ToString() },
            { "lon", primary.Lon.ToString() },
            { "tzone", primary.Tzone.ToString() },
            // Secondary person in s_ format
            { "s_day", secondary.Day.ToString() },
            { "s_month", secondary.Month.ToString() },
            { "s_year", secondary.Year.ToString() },
            { "s_hour", secondary.Hour.ToString() },
            { "s_min", secondary.Min.ToString() },
            { "s_lat", secondary.Lat.ToString() },
            { "s_lon", secondary.Lon.ToString() },
            { "s_tzone", secondary.Tzone.ToString() }
        };

        if (orb.HasValue)
        {
            formData.Add("orb", orb.Value.ToString());
        }

        return formData;
    }
}

