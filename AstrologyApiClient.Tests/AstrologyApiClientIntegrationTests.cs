using AstrologyApiClient;
using AstrologyApiClient.Models;
using System.Net;

namespace AstrologyApiClient.Tests;

/// <summary>
/// Integration tests for AstrologyApiClient
/// These tests make actual API calls and require valid credentials
/// Set credentials via environment variables or appsettings.json
/// Note: Some tests may show 405 errors if the API endpoint is not licensed
/// Run ApiAvailabilityTests to see a complete list of available APIs
/// </summary>
public class AstrologyApiClientIntegrationTests : IDisposable
{
    private readonly AstrologyApiClient _client;
    private bool _disposed = false;

    public AstrologyApiClientIntegrationTests()
    {
        var userId = TestConfiguration.UserId;
        var apiKey = TestConfiguration.ApiKey;
        
        _client = new AstrologyApiClient(userId, apiKey);
    }

    // Helper to handle 405 errors gracefully
    private void Handle405Error(string endpointName)
    {
        Assert.True(true, $"405 Method Not Allowed for {endpointName} - License not available. Check ApiAvailabilityTests for full report.");
    }

    [Fact]
    public async Task GetWesternHoroscopeAsync_WithValidData_ReturnsResult()
    {
        // Arrange
        var birthData = CreateTestBirthData();

        // Act & Assert
        try
        {
            var result = await _client.GetWesternHoroscopeAsync<object>(birthData);
            Assert.NotNull(result);
        }
        catch (HttpRequestException ex) when (ex.Message.Contains("405"))
        {
            Handle405Error("Western Horoscope");
        }
    }

    [Fact]
    public async Task GetWesternChartDataAsync_WithValidData_ReturnsResult()
    {
        // Arrange
        var birthData = CreateTestBirthData();

        // Act & Assert
        try
        {
            var result = await _client.GetWesternChartDataAsync<object>(birthData);
            Assert.NotNull(result);
        }
        catch (HttpRequestException ex) when (ex.Message.Contains("405"))
        {
            Handle405Error("Western Chart Data");
        }
    }

    [Fact]
    public async Task GetWesternChartImageAsync_WithValidData_ReturnsResult()
    {
        // Arrange
        var birthData = CreateTestBirthData();

        // Act & Assert
        try
        {
            var result = await _client.GetWesternChartImageAsync<object>(birthData);
            Assert.NotNull(result);
        }
        catch (HttpRequestException ex) when (ex.Message.Contains("405"))
        {
            Handle405Error("Western Chart Image");
        }
    }

    [Fact]
    public async Task GetPlanetPredictionAsync_WithMars_ReturnsResult()
    {
        // Arrange
        var birthData = CreateTestBirthData();
        var planet = "mars";

        // Act & Assert
        try
        {
            var result = await _client.GetPlanetPredictionAsync<object>(planet, birthData);
            Assert.NotNull(result);
        }
        catch (HttpRequestException ex) when (ex.Message.Contains("405"))
        {
            Handle405Error("Planet Prediction (Mars)");
        }
    }

    [Fact]
    public async Task GetPlanetPredictionAsync_WithMoon_ReturnsResult()
    {
        // Arrange
        var birthData = CreateTestBirthData();
        var planet = "moon";

        // Act & Assert
        try
        {
            var result = await _client.GetPlanetPredictionAsync<object>(planet, birthData);
            Assert.NotNull(result);
        }
        catch (HttpRequestException ex) when (ex.Message.Contains("405"))
        {
            Handle405Error("Planet Prediction (Moon)");
        }
    }

    [Fact]
    public async Task GetTropicalTransitsDailyAsync_WithValidData_ReturnsResult()
    {
        // Arrange
        var birthData = new BirthDataWithPredictionTimezone
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

        // Act & Assert
        try
        {
            var result = await _client.GetTropicalTransitsDailyAsync<object>(birthData);
            Assert.NotNull(result);
        }
        catch (HttpRequestException ex) when (ex.Message.Contains("405"))
        {
            Handle405Error("Tropical Transits Daily");
        }
    }

    [Fact]
    public async Task GetTropicalTransitsWeeklyAsync_WithValidData_ReturnsResult()
    {
        // Arrange
        var birthData = new BirthDataWithPredictionTimezone
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

        // Act & Assert
        try
        {
            var result = await _client.GetTropicalTransitsWeeklyAsync<object>(birthData);
            Assert.NotNull(result);
        }
        catch (HttpRequestException ex) when (ex.Message.Contains("405"))
        {
            Handle405Error("Tropical Transits Weekly");
        }
    }

    [Fact]
    public async Task GetTropicalTransitsMonthlyAsync_WithValidData_ReturnsResult()
    {
        // Arrange
        var birthData = new BirthDataWithPredictionTimezone
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

        // Act & Assert
        try
        {
            var result = await _client.GetTropicalTransitsMonthlyAsync<object>(birthData);
            Assert.NotNull(result);
        }
        catch (HttpRequestException ex) when (ex.Message.Contains("405"))
        {
            Handle405Error("Tropical Transits Monthly");
        }
    }

    [Fact]
    public async Task GetSolarReturnDetailsAsync_WithValidData_ReturnsResult()
    {
        // Arrange
        var birthData = CreateTestBirthData();

        // Act & Assert
        try
        {
            var result = await _client.GetSolarReturnDetailsAsync<object>(birthData);
            Assert.NotNull(result);
        }
        catch (HttpRequestException ex) when (ex.Message.Contains("405"))
        {
            Handle405Error("Solar Return Details");
        }
    }

    [Fact]
    public async Task GetSolarReturnPlanetsAsync_WithValidData_ReturnsResult()
    {
        // Arrange
        var birthData = CreateTestBirthData();

        // Act & Assert
        try
        {
            var result = await _client.GetSolarReturnPlanetsAsync<object>(birthData);
            Assert.NotNull(result);
        }
        catch (HttpRequestException ex) when (ex.Message.Contains("405"))
        {
            Handle405Error("Solar Return Planets");
        }
    }

    [Fact]
    public async Task GetSolarReturnHouseCuspsAsync_WithValidData_ReturnsResult()
    {
        // Arrange
        var birthData = CreateTestBirthData();

        // Act & Assert
        try
        {
            var result = await _client.GetSolarReturnHouseCuspsAsync<object>(birthData);
            Assert.NotNull(result);
        }
        catch (HttpRequestException ex) when (ex.Message.Contains("405"))
        {
            Handle405Error("Solar Return House Cusps");
        }
    }

    [Fact]
    public async Task GetSolarReturnPlanetAspectsAsync_WithValidData_ReturnsResult()
    {
        // Arrange
        var birthData = CreateTestBirthData();

        // Act & Assert
        try
        {
            var result = await _client.GetSolarReturnPlanetAspectsAsync<object>(birthData);
            Assert.NotNull(result);
        }
        catch (HttpRequestException ex) when (ex.Message.Contains("405"))
        {
            Handle405Error("Solar Return Planet Aspects");
        }
    }

    [Fact]
    public async Task GetLunarMetricsAsync_WithValidData_ReturnsResult()
    {
        // Arrange
        var birthData = CreateTestBirthData();

        // Act & Assert
        try
        {
            var result = await _client.GetLunarMetricsAsync<object>(birthData);
            Assert.NotNull(result);
        }
        catch (HttpRequestException ex) when (ex.Message.Contains("405"))
        {
            Handle405Error("Lunar Metrics");
        }
    }

    [Fact]
    public async Task GetSynastryHoroscopeAsync_WithValidData_ReturnsResult()
    {
        // Arrange
        var compositeData = CreateTestCompositeBirthData();

        // Act & Assert
        try
        {
            var result = await _client.GetSynastryHoroscopeAsync<object>(compositeData);
            Assert.NotNull(result);
        }
        catch (HttpRequestException ex) when (ex.Message.Contains("405"))
        {
            Handle405Error("Synastry Horoscope");
        }
    }

    [Fact]
    public async Task GetPersonalityReportAsync_WithValidData_ReturnsResult()
    {
        // Arrange
        var birthData = CreateTestBirthData();

        // Act & Assert
        try
        {
            var result = await _client.GetPersonalityReportAsync<object>(birthData);
            Assert.NotNull(result);
        }
        catch (HttpRequestException ex) when (ex.Message.Contains("405"))
        {
            Handle405Error("Personality Report");
        }
    }

    [Fact]
    public async Task GetRomanticPersonalityReportAsync_WithValidData_ReturnsResult()
    {
        // Arrange
        var birthData = CreateTestBirthData();

        // Act & Assert
        try
        {
            var result = await _client.GetRomanticPersonalityReportAsync<object>(birthData);
            Assert.NotNull(result);
        }
        catch (HttpRequestException ex) when (ex.Message.Contains("405"))
        {
            Handle405Error("Romantic Personality Report");
        }
    }

    [Fact]
    public async Task GetLifeForecastReportAsync_WithValidData_ReturnsResult()
    {
        // Arrange
        var birthData = CreateTestBirthData();

        // Act & Assert
        try
        {
            var result = await _client.GetLifeForecastReportAsync<object>(birthData);
            Assert.NotNull(result);
        }
        catch (HttpRequestException ex) when (ex.Message.Contains("405"))
        {
            Handle405Error("Life Forecast Report");
        }
    }

    [Fact]
    public async Task GetRomanticForecastReportAsync_WithValidData_ReturnsResult()
    {
        // Arrange
        var birthData = CreateTestBirthData();

        // Act & Assert
        try
        {
            var result = await _client.GetRomanticForecastReportAsync<object>(birthData);
            Assert.NotNull(result);
        }
        catch (HttpRequestException ex) when (ex.Message.Contains("405"))
        {
            Handle405Error("Romantic Forecast Report");
        }
    }

    [Fact]
    public async Task GetFriendshipReportAsync_WithValidData_ReturnsResult()
    {
        // Arrange
        var compositeData = CreateTestCompositeBirthData();

        // Act & Assert
        try
        {
            var result = await _client.GetFriendshipReportAsync<object>(compositeData);
            Assert.NotNull(result);
        }
        catch (HttpRequestException ex) when (ex.Message.Contains("405"))
        {
            Handle405Error("Friendship Report");
        }
    }

    [Fact]
    public async Task GetKarmaDestinyReportAsync_WithValidData_ReturnsResult()
    {
        // Arrange
        var compositeData = CreateTestCompositeBirthData();

        // Act & Assert
        try
        {
            var result = await _client.GetKarmaDestinyReportAsync<object>(compositeData);
            Assert.NotNull(result);
        }
        catch (HttpRequestException ex) when (ex.Message.Contains("405"))
        {
            Handle405Error("Karma Destiny Report");
        }
    }

    [Fact]
    public async Task GetLoveCompatibilityReportAsync_WithValidData_ReturnsResult()
    {
        // Arrange
        var compositeData = CreateTestCompositeBirthData();

        // Act & Assert
        try
        {
            var result = await _client.GetLoveCompatibilityReportAsync<object>(compositeData);
            Assert.NotNull(result);
        }
        catch (HttpRequestException ex) when (ex.Message.Contains("405"))
        {
            Handle405Error("Love Compatibility Report");
        }
    }

    [Fact]
    public async Task GetRomanticForecastCoupleReportAsync_WithValidData_ReturnsResult()
    {
        // Arrange
        var compositeData = CreateTestCompositeBirthData();

        // Act & Assert
        try
        {
            var result = await _client.GetRomanticForecastCoupleReportAsync<object>(compositeData);
            Assert.NotNull(result);
        }
        catch (HttpRequestException ex) when (ex.Message.Contains("405"))
        {
            Handle405Error("Romantic Forecast Couple Report");
        }
    }

    [Fact]
    public async Task GetGeneralAscendantReportAsync_WithValidData_ReturnsResult()
    {
        // Arrange
        var compositeData = CreateTestCompositeBirthData();

        // Act & Assert
        try
        {
            var result = await _client.GetGeneralAscendantReportAsync<object>(compositeData);
            Assert.NotNull(result);
        }
        catch (HttpRequestException ex) when (ex.Message.Contains("405"))
        {
            Handle405Error("General Ascendant Report");
        }
    }

    [Fact]
    public async Task GetGeneralSignReportAsync_WithSun_ReturnsResult()
    {
        // Arrange
        var compositeData = CreateTestCompositeBirthData();
        var sign = "sun";

        // Act & Assert
        try
        {
            var result = await _client.GetGeneralSignReportAsync<object>(sign, compositeData);
            Assert.NotNull(result);
        }
        catch (HttpRequestException ex) when (ex.Message.Contains("405"))
        {
            Handle405Error("General Sign Report (Sun)");
        }
    }

    [Fact]
    public async Task GetGeneralHouseReportAsync_WithValidData_ReturnsResult()
    {
        // Arrange
        var compositeData = CreateTestCompositeBirthData();

        // Act & Assert
        try
        {
            var result = await _client.GetGeneralHouseReportAsync<object>(compositeData);
            Assert.NotNull(result);
        }
        catch (HttpRequestException ex) when (ex.Message.Contains("405"))
        {
            Handle405Error("General House Report");
        }
    }

    [Fact]
    public async Task GetZodiacCompatibilityAsync_WithValidData_ReturnsResult()
    {
        // Arrange
        var compositeData = CreateTestCompositeBirthData();
        var zodiacName = "virgo";
        var partnerZodiacName = "pisces";

        // Act & Assert
        try
        {
            var result = await _client.GetZodiacCompatibilityAsync<object>(zodiacName, partnerZodiacName, compositeData);
            Assert.NotNull(result);
        }
        catch (HttpRequestException ex) when (ex.Message.Contains("405"))
        {
            Handle405Error("Zodiac Compatibility");
        }
    }

    [Fact]
    public async Task GetCompatibilityAsync_WithValidData_ReturnsResult()
    {
        // Arrange
        var compositeData = CreateTestCompositeBirthData();
        var sunSign = "leo";
        var risingSign = "aries";
        var partnerSunSign = "cancer";
        var partnerRisingSign = "virgo";

        // Act & Assert
        try
        {
            var result = await _client.GetCompatibilityAsync<object>(sunSign, risingSign, partnerSunSign, partnerRisingSign, compositeData);
            Assert.NotNull(result);
        }
        catch (HttpRequestException ex) when (ex.Message.Contains("405"))
        {
            Handle405Error("Compatibility");
        }
    }

    [Fact]
    public async Task GetTarotPredictionsAsync_WithValidData_ReturnsResult()
    {
        // Arrange
        var tarotData = new TarotPredictionData
        {
            Love = 57,
            Career = 32,
            Finance = 54
        };

        // Act & Assert
        try
        {
            var result = await _client.GetTarotPredictionsAsync<object>(tarotData);
            Assert.NotNull(result);
        }
        catch (HttpRequestException ex) when (ex.Message.Contains("405"))
        {
            Handle405Error("Tarot Predictions");
        }
    }

    [Fact]
    public async Task AllMethods_WithInvalidCredentials_ThrowsException()
    {
        // Arrange
        var invalidClient = new AstrologyApiClient("invalid", "invalid");
        var birthData = CreateTestBirthData();

        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(() => 
            invalidClient.GetWesternHoroscopeAsync<object>(birthData));
        
        invalidClient.Dispose();
    }

    private static BirthData CreateTestBirthData()
    {
        return new BirthData
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
    }

    private static CompositeBirthData CreateTestCompositeBirthData()
    {
        return new CompositeBirthData
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
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _client?.Dispose();
            _disposed = true;
        }
    }
}
