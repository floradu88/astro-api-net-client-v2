using AstrologyApiClient;
using AstrologyApiClient.Interfaces;
using AstrologyApiClient.Models;
using AstrologyApiClient.Services;
using Moq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace AstrologyApiClient.Tests;

public class AstrologyApiClientTests : IDisposable
{
    private readonly Mock<IHttpClientWrapper> _mockHttpClientWrapper;
    private readonly Mock<IFormDataConverter> _mockFormDataConverter;
    private readonly HttpClient _httpClient;
    private readonly AstrologyApiClient _client;
    private bool _disposed = false;

    public AstrologyApiClientTests()
    {
        _mockHttpClientWrapper = new Mock<IHttpClientWrapper>();
        _mockFormDataConverter = new Mock<IFormDataConverter>();
        _httpClient = new HttpClient();
        
        // Use reflection to access internal constructor for testing
        var constructor = typeof(AstrologyApiClient).GetConstructors(
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .First(c => c.GetParameters().Length == 6);
        
        _client = (AstrologyApiClient)constructor.Invoke(new object[]
        {
            _httpClient,
            "test_user_id",
            "test_api_key",
            "https://json.astrologyapi.com/v1",
            _mockFormDataConverter.Object,
            _mockHttpClientWrapper.Object
        });
    }

    [Fact]
    public void Constructor_WithValidCredentials_SetsAuthentication()
    {
        // Arrange & Act
        var client = new AstrologyApiClient("user123", "key456");
        
        // Assert
        Assert.NotNull(client);
        client.Dispose();
    }

    [Fact]
    public void Constructor_WithNullUserId_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new AstrologyApiClient(null!, "key"));
    }

    [Fact]
    public void Constructor_WithEmptyUserId_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new AstrologyApiClient("", "key"));
    }

    [Fact]
    public void Constructor_WithNullApiKey_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new AstrologyApiClient("user", null!));
    }

    [Fact]
    public void Constructor_WithEmptyApiKey_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new AstrologyApiClient("user", ""));
    }

    [Fact]
    public async Task GetWesternHoroscopeAsync_WithValidData_ReturnsResult()
    {
        // Arrange
        var birthData = CreateTestBirthData();
        var expectedFormData = new Dictionary<string, string> { { "day", "10" } };
        var expectedResult = new { data = "test" };

        _mockFormDataConverter
            .Setup(x => x.Convert(It.IsAny<BirthData>()))
            .Returns(expectedFormData);

        _mockHttpClientWrapper
            .Setup(x => x.PostAsync<object>(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _client.GetWesternHoroscopeAsync<object>(birthData);

        // Assert
        Assert.NotNull(result);
        _mockFormDataConverter.Verify(x => x.Convert(birthData), Times.Once);
        _mockHttpClientWrapper.Verify(x => x.PostAsync<object>("western_horoscope", expectedFormData, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetPlanetPredictionAsync_WithValidData_ReturnsResult()
    {
        // Arrange
        var birthData = CreateTestBirthData();
        var planet = "mars";
        var expectedFormData = new Dictionary<string, string> { { "day", "10" } };

        _mockFormDataConverter
            .Setup(x => x.Convert(It.IsAny<BirthData>()))
            .Returns(expectedFormData);

        _mockHttpClientWrapper
            .Setup(x => x.PostAsync<object>(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((object?)new { });

        // Act
        await _client.GetPlanetPredictionAsync<object>(planet, birthData);

        // Assert
        _mockHttpClientWrapper.Verify(x => x.PostAsync<object>($"personalized_planet_prediction/daily/{planet}", expectedFormData, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetPlanetPredictionAsync_WithNullPlanet_ThrowsArgumentException()
    {
        // Arrange
        var birthData = CreateTestBirthData();

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _client.GetPlanetPredictionAsync<object>(null!, birthData));
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
        var expectedFormData = new Dictionary<string, string> { { "day", "10" }, { "prediction_timezone", "0" } };

        _mockFormDataConverter
            .Setup(x => x.Convert(It.IsAny<BirthDataWithPredictionTimezone>()))
            .Returns(expectedFormData);

        _mockHttpClientWrapper
            .Setup(x => x.PostAsync<object>(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((object?)new { });

        // Act
        await _client.GetTropicalTransitsDailyAsync<object>(birthData);

        // Assert
        _mockHttpClientWrapper.Verify(x => x.PostAsync<object>("tropical_transits/daily", expectedFormData, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetSynastryHoroscopeAsync_WithValidData_ReturnsResult()
    {
        // Arrange
        var compositeData = CreateTestCompositeBirthData();
        var expectedFormData = new Dictionary<string, string> { { "p_day", "10" } };

        _mockFormDataConverter
            .Setup(x => x.Convert(It.IsAny<CompositeBirthData>()))
            .Returns(expectedFormData);

        _mockHttpClientWrapper
            .Setup(x => x.PostAsync<object>(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((object?)new { });

        // Act
        await _client.GetSynastryHoroscopeAsync<object>(compositeData);

        // Assert
        _mockHttpClientWrapper.Verify(x => x.PostAsync<object>("synastry_horoscope", expectedFormData, It.IsAny<CancellationToken>()), Times.Once);
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
        var expectedFormData = new Dictionary<string, string>
        {
            { "love", "57" },
            { "career", "32" },
            { "finance", "54" }
        };

        _mockFormDataConverter
            .Setup(x => x.Convert(It.IsAny<TarotPredictionData>()))
            .Returns(expectedFormData);

        _mockHttpClientWrapper
            .Setup(x => x.PostAsync<object>(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((object?)new { });

        // Act
        await _client.GetTarotPredictionsAsync<object>(tarotData);

        // Assert
        _mockHttpClientWrapper.Verify(x => x.PostAsync<object>("tarot_predictions", expectedFormData, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetZodiacCompatibilityAsync_WithNullZodiacName_ThrowsArgumentException()
    {
        // Arrange
        var compositeData = CreateTestCompositeBirthData();

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _client.GetZodiacCompatibilityAsync<object>(null!, "pisces", compositeData));
    }

    [Fact]
    public async Task GetCompatibilityAsync_WithNullParameters_ThrowsArgumentException()
    {
        // Arrange
        var compositeData = CreateTestCompositeBirthData();

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _client.GetCompatibilityAsync<object>(null!, "aries", "cancer", "virgo", compositeData));
        await Assert.ThrowsAsync<ArgumentException>(() => _client.GetCompatibilityAsync<object>("leo", null!, "cancer", "virgo", compositeData));
        await Assert.ThrowsAsync<ArgumentException>(() => _client.GetCompatibilityAsync<object>("leo", "aries", null!, "virgo", compositeData));
        await Assert.ThrowsAsync<ArgumentException>(() => _client.GetCompatibilityAsync<object>("leo", "aries", "cancer", null!, compositeData));
    }

    [Fact]
    public void Dispose_DisposesResources()
    {
        // Arrange
        var client = new AstrologyApiClient("user", "key");

        // Act
        client.Dispose();
        client.Dispose(); // Should not throw on multiple dispose

        // Assert
        // If we get here without exception, dispose worked
        Assert.True(true);
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
            _httpClient?.Dispose();
            _disposed = true;
        }
    }
}

