using AstrologyApiClient;
using AstrologyApiClient.Models;
using AstrologyApiClient.Services;
using System.Net;
using System.Net.Http;
using System.Text;

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
        results.Add(await TestEndpoint("Western Horoscope", null, () => _client.GetWesternHoroscopeAsync<object>(birthData)));
        results.Add(await TestEndpoint("Western Chart Data", null, () => _client.GetWesternChartDataAsync<object>(birthData)));
        results.Add(await TestEndpoint("Western Chart Image", null, () => _client.GetWesternChartImageAsync<object>(birthData)));
        
        results.Add(await TestEndpoint("Planet Prediction (Mars)", null, () => _client.GetPlanetPredictionAsync<object>("mars", birthData)));
        results.Add(await TestEndpoint("Planet Prediction (Moon)", null, () => _client.GetPlanetPredictionAsync<object>("moon", birthData)));
        
        results.Add(await TestEndpoint("Tropical Transits Daily", null, () => _client.GetTropicalTransitsDailyAsync<object>(transitsData)));
        results.Add(await TestEndpoint("Tropical Transits Weekly", null, () => _client.GetTropicalTransitsWeeklyAsync<object>(transitsData)));
        results.Add(await TestEndpoint("Tropical Transits Monthly", null, () => _client.GetTropicalTransitsMonthlyAsync<object>(transitsData)));
        
        results.Add(await TestEndpoint("Solar Return Details", null, () => _client.GetSolarReturnDetailsAsync<object>(birthData)));
        results.Add(await TestEndpoint("Solar Return Planets", null, () => _client.GetSolarReturnPlanetsAsync<object>(birthData)));
        results.Add(await TestEndpoint("Solar Return House Cusps", null, () => _client.GetSolarReturnHouseCuspsAsync<object>(birthData)));
        results.Add(await TestEndpoint("Solar Return Planet Aspects", null, () => _client.GetSolarReturnPlanetAspectsAsync<object>(birthData)));
        
        results.Add(await TestEndpoint("Lunar Metrics", null, () => _client.GetLunarMetricsAsync<object>(birthData)));
        
        results.Add(await TestEndpoint("Synastry Horoscope", null, () => _client.GetSynastryHoroscopeAsync<object>(compositeData)));
        
        results.Add(await TestEndpoint("Personality Report", null, () => _client.GetPersonalityReportAsync<object>(birthData)));
        results.Add(await TestEndpoint("Romantic Personality Report", null, () => _client.GetRomanticPersonalityReportAsync<object>(birthData)));
        results.Add(await TestEndpoint("Life Forecast Report", null, () => _client.GetLifeForecastReportAsync<object>(birthData)));
        results.Add(await TestEndpoint("Romantic Forecast Report", null, () => _client.GetRomanticForecastReportAsync<object>(birthData)));
        
        results.Add(await TestEndpoint("Friendship Report", null, () => _client.GetFriendshipReportAsync<object>(compositeData)));
        results.Add(await TestEndpoint("Karma Destiny Report", null, () => _client.GetKarmaDestinyReportAsync<object>(compositeData)));
        results.Add(await TestEndpoint("Love Compatibility Report", null, () => _client.GetLoveCompatibilityReportAsync<object>(compositeData)));
        results.Add(await TestEndpoint("Romantic Forecast Couple Report", null, () => _client.GetRomanticForecastCoupleReportAsync<object>(compositeData)));
        
        // Test general report endpoints with new overload (two BirthData objects)
        var primaryBirthData = new BirthData
        {
            Day = compositeData.Primary.PDay,
            Month = compositeData.Primary.PMonth,
            Year = compositeData.Primary.PYear,
            Hour = compositeData.Primary.PHour,
            Min = compositeData.Primary.PMin,
            Lat = compositeData.Primary.PLat,
            Lon = compositeData.Primary.PLon,
            Tzone = compositeData.Primary.PTzone
        };
        var secondaryBirthData = new BirthData
        {
            Day = compositeData.Secondary.SDay,
            Month = compositeData.Secondary.SMonth,
            Year = compositeData.Secondary.SYear,
            Hour = compositeData.Secondary.SHour,
            Min = compositeData.Secondary.SMin,
            Lat = compositeData.Secondary.SLat,
            Lon = compositeData.Secondary.SLon,
            Tzone = compositeData.Secondary.STzone
        };
        
        results.Add(await TestEndpoint("General Ascendant Report", "general_ascendant_report/tropical", () => _client.GetGeneralAscendantReportAsync<object>(primaryBirthData, secondaryBirthData, compositeData.Orb)));
        results.Add(await TestEndpoint("General Sign Report (Sun)", "general_sign_report/tropical/sun", () => _client.GetGeneralSignReportAsync<object>("sun", primaryBirthData, secondaryBirthData, compositeData.Orb)));
        results.Add(await TestEndpoint("General House Report", "general_house_report/tropical", () => _client.GetGeneralHouseReportAsync<object>(primaryBirthData, secondaryBirthData, compositeData.Orb)));
        
        results.Add(await TestEndpoint("Zodiac Compatibility", null, () => _client.GetZodiacCompatibilityAsync<object>("virgo", "pisces", compositeData)));
        results.Add(await TestEndpoint("Compatibility", null, () => _client.GetCompatibilityAsync<object>("leo", "aries", "cancer", "virgo", compositeData)));
        
        results.Add(await TestEndpoint("Tarot Predictions", null, () => _client.GetTarotPredictionsAsync<object>(tarotData)));

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
        
        var reportContent = GenerateMarkdownReport(accessible, notAccessible);
        
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
            if (!string.IsNullOrEmpty(result.FullUrl))
            {
                Console.WriteLine($"    Full URL: {result.FullUrl}");
            }
            if (!string.IsNullOrEmpty(result.FullResponse))
            {
                Console.WriteLine($"    Full Response: {result.FullResponse}");
            }
        }

        Console.WriteLine("\n=========================================\n");

        // Assert - This test always passes, it's just for reporting
        Assert.True(true, "API availability check completed. See console output for report locations.");
    }

    private async Task<ApiEndpointResult> TestEndpoint(string endpointName, string? endpointPath, Func<Task<object?>> apiCall)
    {
        try
        {
            var result = await apiCall();
            return new ApiEndpointResult
            {
                EndpointName = endpointName,
                IsAccessible = result != null,
                StatusCode = HttpStatusCode.OK,
                ErrorMessage = null,
                FullUrl = null,
                FullResponse = null
            };
        }
        catch (HttpRequestException ex)
        {
            // Check if it's a 405 (Method Not Allowed) or other HTTP error
            var statusCode = HttpStatusCode.InternalServerError;
            var message = ex.Message;
            string? fullUrl = null;
            string? fullResponse = null;

            // Always construct the full URL if we have the endpoint path
            if (!string.IsNullOrEmpty(endpointPath))
            {
                fullUrl = $"https://json.astrologyapi.com/v1/{endpointPath}";
            }

            if (ex.Message.Contains("405") || ex.Message.Contains("Method Not Allowed"))
            {
                statusCode = HttpStatusCode.MethodNotAllowed;
                message = "405 Method Not Allowed - License not available for this endpoint";
                
                // Make a direct call to get full response
                if (!string.IsNullOrEmpty(endpointPath))
                {
                    fullResponse = await GetFullResponseFor405(fullUrl!, endpointPath);
                }
            }
            else if (ex.Message.Contains("401") || ex.Message.Contains("Unauthorized"))
            {
                statusCode = HttpStatusCode.Unauthorized;
                message = "401 Unauthorized - Invalid credentials";
            }
            else if (ex.Message.Contains("403") || ex.Message.Contains("Forbidden"))
            {
                statusCode = HttpStatusCode.Forbidden;
                message = "403 Forbidden - Access denied";
            }
            else if (ex.Message.Contains("404") || ex.Message.Contains("Not Found"))
            {
                statusCode = HttpStatusCode.NotFound;
                message = "404 Not Found - Endpoint not found";
                
                // Make a direct call to get full response
                if (!string.IsNullOrEmpty(endpointPath))
                {
                    fullResponse = await GetFullResponseFor405(fullUrl!, endpointPath);
                }
            }

            return new ApiEndpointResult
            {
                EndpointName = endpointName,
                IsAccessible = false,
                StatusCode = statusCode,
                ErrorMessage = message,
                FullUrl = fullUrl,
                FullResponse = fullResponse
            };
        }
        catch (Exception ex)
        {
            return new ApiEndpointResult
            {
                EndpointName = endpointName,
                IsAccessible = false,
                StatusCode = HttpStatusCode.InternalServerError,
                ErrorMessage = $"Error: {ex.Message}",
                FullUrl = null,
                FullResponse = null
            };
        }
    }

    private async Task<string?> GetFullResponseFor405(string fullUrl, string endpointPath)
    {
        try
        {
            var userId = TestConfiguration.UserId;
            var apiKey = TestConfiguration.ApiKey;
            
            using var httpClient = new HttpClient();
            var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{userId}:{apiKey}"));
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            // Get the form data for the endpoint using FormDataConverter
            var formDataConverter = new FormDataConverter();
            Dictionary<string, string> formData;
            
            // Check if this is a general report endpoint that needs hybrid format
            if (endpointPath.Contains("general_ascendant_report") || 
                endpointPath.Contains("general_sign_report") || 
                (endpointPath.Contains("general_house_report") && endpointPath.Contains("tropical")))
            {
                // Use hybrid format for general report endpoints
                var compositeData = CreateTestCompositeBirthData();
                var primaryBirthData = new BirthData
                {
                    Day = compositeData.Primary.PDay,
                    Month = compositeData.Primary.PMonth,
                    Year = compositeData.Primary.PYear,
                    Hour = compositeData.Primary.PHour,
                    Min = compositeData.Primary.PMin,
                    Lat = compositeData.Primary.PLat,
                    Lon = compositeData.Primary.PLon,
                    Tzone = compositeData.Primary.PTzone
                };
                var secondaryBirthData = new BirthData
                {
                    Day = compositeData.Secondary.SDay,
                    Month = compositeData.Secondary.SMonth,
                    Year = compositeData.Secondary.SYear,
                    Hour = compositeData.Secondary.SHour,
                    Min = compositeData.Secondary.SMin,
                    Lat = compositeData.Secondary.SLat,
                    Lon = compositeData.Secondary.SLon,
                    Tzone = compositeData.Secondary.STzone
                };
                formData = formDataConverter.ConvertToHybridFormat(primaryBirthData, secondaryBirthData, compositeData.Orb);
            }
            else
            {
                // Use standard format for other endpoints
                var compositeData = CreateTestCompositeBirthData();
                formData = formDataConverter.Convert(compositeData);
            }

            var content = new FormUrlEncodedContent(formData);
            var response = await httpClient.PostAsync(fullUrl, content);
            
            var responseBody = await response.Content.ReadAsStringAsync();
            var statusCode = (int)response.StatusCode;
            
            var headers = string.Join("\n", response.Headers.Select(h => $"  {h.Key}: {string.Join(", ", h.Value)}"));
            var contentHeaders = string.Join("\n", response.Content.Headers.Select(h => $"  {h.Key}: {string.Join(", ", h.Value)}"));
            
            return $"Status Code: {statusCode}\nResponse Body:\n{responseBody}\n\nResponse Headers:\n{headers}\n\nContent Headers:\n{contentHeaders}";
        }
        catch (Exception ex)
        {
            return $"Error getting full response: {ex.Message}\nStack Trace: {ex.StackTrace}";
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

    private static string GenerateMarkdownReport(List<ApiEndpointResult> accessible, List<ApiEndpointResult> notAccessible)
    {
        var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        var sb = new System.Text.StringBuilder();

        sb.AppendLine("# API Availability Report");
        sb.AppendLine();
        sb.AppendLine($"**Generated:** {timestamp}");
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
            sb.AppendLine("| # | API Endpoint | Status Code | Error Message | Full URL |");
            sb.AppendLine("|---|--------------|-------------|---------------|----------|");
            var index = 1;
            foreach (var result in notAccessible.OrderBy(r => r.EndpointName))
            {
                var statusCode = result.StatusCode == HttpStatusCode.MethodNotAllowed ? "405" : result.StatusCode.ToString();
                var errorMsg = result.ErrorMessage ?? "Unknown error";
                var fullUrl = result.FullUrl ?? "N/A";
                sb.AppendLine($"| {index} | {result.EndpointName} | {statusCode} | {errorMsg} | {fullUrl} |");
                index++;
            }
            
            // Add detailed section for 405 and 404 errors with full responses
            var error405s = notAccessible.Where(r => r.StatusCode == HttpStatusCode.MethodNotAllowed && !string.IsNullOrEmpty(r.FullResponse)).ToList();
            var error404s = notAccessible.Where(r => r.StatusCode == HttpStatusCode.NotFound && !string.IsNullOrEmpty(r.FullResponse)).ToList();
            
            if (error405s.Any())
            {
                sb.AppendLine();
                sb.AppendLine("### Detailed 405 Error Responses");
                sb.AppendLine();
                foreach (var result in error405s.OrderBy(r => r.EndpointName))
                {
                    sb.AppendLine($"#### {result.EndpointName}");
                    sb.AppendLine();
                    sb.AppendLine($"**Full URL:** `{result.FullUrl}`");
                    sb.AppendLine();
                    sb.AppendLine("**Full Response:**");
                    sb.AppendLine("```");
                    sb.AppendLine(result.FullResponse);
                    sb.AppendLine("```");
                    sb.AppendLine();
                }
            }
            
            if (error404s.Any())
            {
                sb.AppendLine();
                sb.AppendLine("### Detailed 404 Error Responses");
                sb.AppendLine();
                foreach (var result in error404s.OrderBy(r => r.EndpointName))
                {
                    sb.AppendLine($"#### {result.EndpointName}");
                    sb.AppendLine();
                    sb.AppendLine($"**Full URL:** `{result.FullUrl}`");
                    sb.AppendLine();
                    sb.AppendLine("**Full Response:**");
                    sb.AppendLine("```");
                    sb.AppendLine(result.FullResponse);
                    sb.AppendLine("```");
                    sb.AppendLine();
                }
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
        public string? FullUrl { get; set; }
        public string? FullResponse { get; set; }
    }
}

