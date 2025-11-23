using AstrologyApiClient.Interfaces;
using AstrologyApiClient.Models;

namespace AstrologyApiClient.Services;

/// <summary>
/// Converts model objects to form data dictionaries
/// </summary>
public class FormDataConverter : IFormDataConverter
{
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

    public Dictionary<string, string> Convert(BirthDataWithPredictionTimezone data)
    {
        if (data == null)
            throw new ArgumentNullException(nameof(data));

        var formData = Convert((BirthData)data);
        formData.Add("prediction_timezone", data.PredictionTimezone.ToString());
        return formData;
    }

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
}

