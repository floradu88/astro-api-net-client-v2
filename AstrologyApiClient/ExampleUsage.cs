using AstrologyApiClient;
using AstrologyApiClient.Models;

namespace AstrologyApiClient.Examples;

/// <summary>
/// Example usage of the AstrologyApiClient
/// Demonstrates how credentials are set once and reused for all requests
/// </summary>
public class ExampleUsage
{
    /// <summary>
    /// Example method demonstrating various API client usage patterns
    /// </summary>
    public static async Task Example()
    {
        // Set credentials once when creating the client
        // These credentials will be used for all subsequent API calls
        var client = new AstrologyApiClient(
            userId: "your_user_id",
            apiKey: "your_api_key"
        );

        try
        {
            // Example 1: Get western horoscope
            var birthData = new BirthData
            {
                Day = 10,
                Month = 5,
                Year = 1990,
                Hour = 19,
                Min = 55,
                Lat = 19.20,
                Lon = 25.20,
                Tzone = 5.5
            };

            // No need to pass credentials again - they're already set!
            var horoscope = await client.GetWesternHoroscopeAsync<object>(birthData);

            // Example 2: Get planet prediction
            var planetPrediction = await client.GetPlanetPredictionAsync<object>("mars", birthData);

            // Example 3: Get tropical transits
            var transitsData = new BirthDataWithPredictionTimezone
            {
                Day = 10,
                Month = 5,
                Year = 1990,
                Hour = 19,
                Min = 55,
                Lat = 19.20,
                Lon = 25.20,
                Tzone = 5.5,
                PredictionTimezone = 0
            };
            var dailyTransits = await client.GetTropicalTransitsDailyAsync<object>(transitsData);

            // Example 4: Get synastry horoscope (composite)
            var compositeData = new CompositeBirthData
            {
                Primary = new PrimaryBirthData
                {
                    PDay = 10,
                    PMonth = 5,
                    PYear = 1990,
                    PHour = 11,
                    PMin = 55,
                    PLat = 19.20,
                    PLon = 25.20,
                    PTzone = 5.5
                },
                Secondary = new SecondaryBirthData
                {
                    SDay = 10,
                    SMonth = 5,
                    SYear = 1990,
                    SHour = 15,
                    SMin = 22,
                    SLat = 19.33,
                    SLon = 25.20,
                    STzone = 5.5
                },
                Orb = 1
            };
            var synastry = await client.GetSynastryHoroscopeAsync<object>(compositeData);

            // Example 5: Get tarot predictions
            var tarotData = new TarotPredictionData
            {
                Love = 57,
                Career = 32,
                Finance = 54
            };
            var tarot = await client.GetTarotPredictionsAsync<object>(tarotData);
        }
        finally
        {
            // Dispose the client when done
            client.Dispose();
        }
    }
}

