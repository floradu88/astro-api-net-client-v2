using AstrologyApiClient;
using AstrologyApiClient.Models;
using System.Net;

namespace AstrologyApiClient.Tests;

/// <summary>
/// Tests to check API availability and list which APIs are accessible with current license
/// </summary>
public class ApiAvailabilityTests : IDisposable
{
    private readonly AstrologyApiClient _client;
    private bool _disposed = false;

    public ApiAvailabilityTests()
    {
        var userId = TestConfiguration.UserId;
        var apiKey = TestConfiguration.ApiKey;
        
        _client = new AstrologyApiClient(userId, apiKey);
    }

    [Fact]
    public async Task ListAllApiEndpoints_CheckAvailability()
    {
        // Arrange
        var results = new List<ApiEndpointResult>();
        var birthData = CreateTestBirthData();
        var compositeData = CreateTestCompositeBirthData();
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
        var tarotData = new TarotPredictionData
        {
            Love = 57,
            Career = 32,
            Finance = 54
        };

        // Test all endpoints
        results.Add(await TestEndpoint("Western Horoscope", () => _client.GetWesternHoroscopeAsync<object>(birthData)));
        results.Add(await TestEndpoint("Western Chart Data", () => _client.GetWesternChartDataAsync<object>(birthData)));
        results.Add(await TestEndpoint("Western Chart Image", () => _client.GetWesternChartImageAsync<object>(birthData)));
        
        results.Add(await TestEndpoint("Planet Prediction (Mars)", () => _client.GetPlanetPredictionAsync<object>("mars", birthData)));
        results.Add(await TestEndpoint("Planet Prediction (Moon)", () => _client.GetPlanetPredictionAsync<object>("moon", birthData)));
        
        results.Add(await TestEndpoint("Tropical Transits Daily", () => _client.GetTropicalTransitsDailyAsync<object>(transitsData)));
        results.Add(await TestEndpoint("Tropical Transits Weekly", () => _client.GetTropicalTransitsWeeklyAsync<object>(transitsData)));
        results.Add(await TestEndpoint("Tropical Transits Monthly", () => _client.GetTropicalTransitsMonthlyAsync<object>(transitsData)));
        
        results.Add(await TestEndpoint("Solar Return Details", () => _client.GetSolarReturnDetailsAsync<object>(birthData)));
        results.Add(await TestEndpoint("Solar Return Planets", () => _client.GetSolarReturnPlanetsAsync<object>(birthData)));
        results.Add(await TestEndpoint("Solar Return House Cusps", () => _client.GetSolarReturnHouseCuspsAsync<object>(birthData)));
        results.Add(await TestEndpoint("Solar Return Planet Aspects", () => _client.GetSolarReturnPlanetAspectsAsync<object>(birthData)));
        
        results.Add(await TestEndpoint("Lunar Metrics", () => _client.GetLunarMetricsAsync<object>(birthData)));
        
        results.Add(await TestEndpoint("Synastry Horoscope", () => _client.GetSynastryHoroscopeAsync<object>(compositeData)));
        
        results.Add(await TestEndpoint("Personality Report", () => _client.GetPersonalityReportAsync<object>(birthData)));
        results.Add(await TestEndpoint("Romantic Personality Report", () => _client.GetRomanticPersonalityReportAsync<object>(birthData)));
        results.Add(await TestEndpoint("Life Forecast Report", () => _client.GetLifeForecastReportAsync<object>(birthData)));
        results.Add(await TestEndpoint("Romantic Forecast Report", () => _client.GetRomanticForecastReportAsync<object>(birthData)));
        
        results.Add(await TestEndpoint("Friendship Report", () => _client.GetFriendshipReportAsync<object>(compositeData)));
        results.Add(await TestEndpoint("Karma Destiny Report", () => _client.GetKarmaDestinyReportAsync<object>(compositeData)));
        results.Add(await TestEndpoint("Love Compatibility Report", () => _client.GetLoveCompatibilityReportAsync<object>(compositeData)));
        results.Add(await TestEndpoint("Romantic Forecast Couple Report", () => _client.GetRomanticForecastCoupleReportAsync<object>(compositeData)));
        
        results.Add(await TestEndpoint("General Ascendant Report", () => _client.GetGeneralAscendantReportAsync<object>(compositeData)));
        results.Add(await TestEndpoint("General Sign Report (Sun)", () => _client.GetGeneralSignReportAsync<object>("sun", compositeData)));
        results.Add(await TestEndpoint("General House Report", () => _client.GetGeneralHouseReportAsync<object>(compositeData)));
        
        results.Add(await TestEndpoint("Zodiac Compatibility", () => _client.GetZodiacCompatibilityAsync<object>("virgo", "pisces", compositeData)));
        results.Add(await TestEndpoint("Compatibility", () => _client.GetCompatibilityAsync<object>("leo", "aries", "cancer", "virgo", compositeData)));
        
        results.Add(await TestEndpoint("Tarot Predictions", () => _client.GetTarotPredictionsAsync<object>(tarotData)));

        // Output results
        var accessible = results.Where(r => r.IsAccessible).ToList();
        var notAccessible = results.Where(r => !r.IsAccessible).ToList();

        // Generate markdown report - save to both test output and project root
        var testOutputPath = Path.Combine(AppContext.BaseDirectory, "API_Availability_Report.md");
        
        // Calculate project root (go up from bin/Release/net8.0 to project root)
        var currentDir = new DirectoryInfo(AppContext.BaseDirectory);
        var projectRoot = currentDir;
        // Navigate up: bin/Release/net8.0 -> bin/Release -> bin -> AstrologyApiClient.Tests -> project root
        for (int i = 0; i < 4 && projectRoot.Parent != null; i++)
        {
            projectRoot = projectRoot.Parent;
        }
        var projectRootPath = Path.Combine(projectRoot.FullName, "API_Availability_Report.md");
        
        var reportContent = GenerateMarkdownReport(accessible, notAccessible, TestConfiguration.UserId);
        
        // Save to test output directory
        File.WriteAllText(testOutputPath, reportContent);
        Console.WriteLine($"📄 Report saved to test output: {testOutputPath}");
        
        // Also save to project root for easy access
        try
        {
            File.WriteAllText(projectRootPath, reportContent);
            Console.WriteLine($"📄 Report saved to project root: {projectRootPath}");
        }
        catch (Exception ex)
        {
            // If we can't write to project root, that's okay
            Console.WriteLine($"⚠️  Could not save to project root: {ex.Message}");
        }

        // Also output to console
        Console.WriteLine("\n=========================================");
        Console.WriteLine("API AVAILABILITY REPORT");
        Console.WriteLine("=========================================");
        Console.WriteLine($"\n✅ ACCESSIBLE APIs ({accessible.Count}):");
        foreach (var result in accessible.OrderBy(r => r.EndpointName))
        {
            Console.WriteLine($"  ✓ {result.EndpointName}");
        }

        Console.WriteLine($"\n❌ NOT ACCESSIBLE APIs ({notAccessible.Count}):");
        foreach (var result in notAccessible.OrderBy(r => r.EndpointName))
        {
            Console.WriteLine($"  ✗ {result.EndpointName} - {result.ErrorMessage}");
        }

        Console.WriteLine("\n=========================================\n");

        // Assert - This test always passes, it's just for reporting
        Assert.True(true, "API availability check completed. See console output for report locations.");
    }

    private async Task<ApiEndpointResult> TestEndpoint(string endpointName, Func<Task<object?>> apiCall)
    {
        try
        {
            var result = await apiCall();
            return new ApiEndpointResult
            {
                EndpointName = endpointName,
                IsAccessible = result != null,
                StatusCode = HttpStatusCode.OK,
                ErrorMessage = null
            };
        }
        catch (HttpRequestException ex)
        {
            // Check if it's a 405 (Method Not Allowed) or other HTTP error
            var statusCode = HttpStatusCode.InternalServerError;
            var message = ex.Message;

            if (ex.Message.Contains("405"))
            {
                statusCode = HttpStatusCode.MethodNotAllowed;
                message = "405 Method Not Allowed - License not available for this endpoint";
            }
            else if (ex.Message.Contains("401"))
            {
                statusCode = HttpStatusCode.Unauthorized;
                message = "401 Unauthorized - Invalid credentials";
            }
            else if (ex.Message.Contains("403"))
            {
                statusCode = HttpStatusCode.Forbidden;
                message = "403 Forbidden - Access denied";
            }
            else if (ex.Message.Contains("404"))
            {
                statusCode = HttpStatusCode.NotFound;
                message = "404 Not Found - Endpoint not found";
            }

            return new ApiEndpointResult
            {
                EndpointName = endpointName,
                IsAccessible = false,
                StatusCode = statusCode,
                ErrorMessage = message
            };
        }
        catch (Exception ex)
        {
            return new ApiEndpointResult
            {
                EndpointName = endpointName,
                IsAccessible = false,
                StatusCode = HttpStatusCode.InternalServerError,
                ErrorMessage = $"Error: {ex.Message}"
            };
        }
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

    private static string GenerateMarkdownReport(List<ApiEndpointResult> accessible, List<ApiEndpointResult> notAccessible, string userId)
    {
        var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        var sb = new System.Text.StringBuilder();

        sb.AppendLine("# API Availability Report");
        sb.AppendLine();
        sb.AppendLine($"**Generated:** {timestamp}");
        sb.AppendLine($"**User ID:** {userId}");
        sb.AppendLine();
        sb.AppendLine("---");
        sb.AppendLine();
        sb.AppendLine("## Summary");
        sb.AppendLine();
        sb.AppendLine($"- **Total APIs Tested:** {accessible.Count + notAccessible.Count}");
        sb.AppendLine($"- **✅ Accessible:** {accessible.Count}");
        sb.AppendLine($"- **❌ Not Accessible:** {notAccessible.Count}");
        sb.AppendLine();
        sb.AppendLine("---");
        sb.AppendLine();
        sb.AppendLine("## ✅ Accessible APIs");
        sb.AppendLine();
        
        if (accessible.Any())
        {
            sb.AppendLine("| # | API Endpoint | Status |");
            sb.AppendLine("|---|--------------|--------|");
            var index = 1;
            foreach (var result in accessible.OrderBy(r => r.EndpointName))
            {
                sb.AppendLine($"| {index} | {result.EndpointName} | ✅ Available |");
                index++;
            }
        }
        else
        {
            sb.AppendLine("*No accessible APIs found.*");
        }

        sb.AppendLine();
        sb.AppendLine("---");
        sb.AppendLine();
        sb.AppendLine("## ❌ Not Accessible APIs");
        sb.AppendLine();
        sb.AppendLine("> **Note:** These APIs return 405 (Method Not Allowed), indicating they are not included in your current license.");
        sb.AppendLine();

        if (notAccessible.Any())
        {
            sb.AppendLine("| # | API Endpoint | Status Code | Error Message |");
            sb.AppendLine("|---|--------------|-------------|---------------|");
            var index = 1;
            foreach (var result in notAccessible.OrderBy(r => r.EndpointName))
            {
                var statusCode = result.StatusCode == HttpStatusCode.MethodNotAllowed ? "405" : result.StatusCode.ToString();
                var errorMsg = result.ErrorMessage ?? "Unknown error";
                sb.AppendLine($"| {index} | {result.EndpointName} | {statusCode} | {errorMsg} |");
                index++;
            }
        }
        else
        {
            sb.AppendLine("*All APIs are accessible.*");
        }

        sb.AppendLine();
        sb.AppendLine("---");
        sb.AppendLine();
        sb.AppendLine("## License Information");
        sb.AppendLine();
        sb.AppendLine("If you need access to APIs that are currently not available, please contact your API provider to upgrade your license.");
        sb.AppendLine();
        sb.AppendLine("---");
        sb.AppendLine();
        sb.AppendLine("*This report was automatically generated by the API availability test.*");

        return sb.ToString();
    }

    private class ApiEndpointResult
    {
        public string EndpointName { get; set; } = string.Empty;
        public bool IsAccessible { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string? ErrorMessage { get; set; }
    }
}

