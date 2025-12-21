using AstrologyApiClient.Models;
using AstrologyApiClient.Services;

namespace AstrologyApiClient.Tests;

public class FormDataConverterTests
{
    private readonly FormDataConverter _converter;

    public FormDataConverterTests()
    {
        _converter = new FormDataConverter();
    }

    [Fact]
    public void Convert_BirthData_ReturnsCorrectFormData()
    {
        // Arrange
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

        // Act
        var result = _converter.Convert(birthData);

        // Assert
        Assert.Equal("10", result["day"]);
        Assert.Equal("5", result["month"]);
        Assert.Equal("1990", result["year"]);
        Assert.Equal("19", result["hour"]);
        Assert.Equal("55", result["min"]);
        Assert.Equal("19.2", result["lat"]);
        Assert.Equal("25.2", result["lon"]);
        Assert.Equal("5.5", result["tzone"]);
    }

    [Fact]
    public void Convert_NullBirthData_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _converter.Convert((BirthData)null!));
    }

    [Fact]
    public void Convert_CompositeBirthData_ReturnsCorrectFormData()
    {
        // Arrange
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

        // Act
        var result = _converter.Convert(compositeData);

        // Assert
        Assert.Equal("10", result["p_day"]);
        Assert.Equal("5", result["p_month"]);
        Assert.Equal("1990", result["p_year"]);
        Assert.Equal("10", result["s_day"]);
        Assert.Equal("5", result["s_month"]);
        Assert.Equal("1990", result["s_year"]);
        Assert.Equal("1", result["orb"]);
    }

    [Fact]
    public void Convert_CompositeBirthData_WithoutOrb_DoesNotIncludeOrb()
    {
        // Arrange
        var compositeData = new CompositeBirthData
        {
            Primary = new PrimaryBirthData { PDay = 10 },
            Secondary = new SecondaryBirthData { SDay = 10 },
            Orb = null
        };

        // Act
        var result = _converter.Convert(compositeData);

        // Assert
        Assert.False(result.ContainsKey("orb"));
    }

    [Fact]
    public void Convert_BirthDataWithPredictionTimezone_IncludesPredictionTimezone()
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

        // Act
        var result = _converter.Convert(birthData);

        // Assert
        Assert.True(result.ContainsKey("prediction_timezone"));
        Assert.Equal("0", result["prediction_timezone"]);
    }

    [Fact]
    public void Convert_TarotPredictionData_ReturnsCorrectFormData()
    {
        // Arrange
        var tarotData = new TarotPredictionData
        {
            Love = 57,
            Career = 32,
            Finance = 54
        };

        // Act
        var result = _converter.Convert(tarotData);

        // Assert
        Assert.Equal("57", result["love"]);
        Assert.Equal("32", result["career"]);
        Assert.Equal("54", result["finance"]);
    }

    [Fact]
    public void Convert_NullTarotPredictionData_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _converter.Convert((TarotPredictionData)null!));
    }
}

