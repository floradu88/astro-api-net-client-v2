# AstrologyApiClient

A .NET client library for the Astrology API with credential management, following SOLID, DRY, KISS, and OOP best practices.

## Project Summary

**AstrologyApiClient** is a comprehensive .NET Core class library that provides a type-safe, easy-to-use interface for interacting with the [Astrology API](https://json.astrologyapi.com). This library simplifies access to various astrology services including horoscope calculations, planetary predictions, compatibility reports, and more.

### What is the Astrology API?

The Astrology API is a RESTful web service that provides access to a wide range of astrological calculations and reports. It offers endpoints for:

- **Western Astrology**: Horoscope data, chart calculations, and natal wheel charts
- **Planetary Predictions**: Daily personalized predictions for planets (Mars, Moon, etc.)
- **Tropical Transits**: Daily, weekly, and monthly transit calculations
- **Solar Returns**: Detailed solar return analysis including planets, house cusps, and aspects
- **Lunar Metrics**: Moon phase and lunar cycle information
- **Synastry & Compatibility**: Relationship analysis between two birth charts
- **Personality Reports**: Comprehensive personality analysis based on birth charts
- **Forecast Reports**: Life and romantic forecast predictions
- **Tarot Predictions**: Tarot card readings for love, career, and finance

### How This Library Works

This library acts as a wrapper around the Astrology API, providing:

1. **Simplified Authentication**: Set your credentials once when creating the client, and they're automatically included in all API requests
2. **Type-Safe Methods**: Strongly-typed methods for each API endpoint with proper request/response models
3. **Error Handling**: Graceful handling of API errors including license restrictions (405 errors)
4. **Comprehensive Coverage**: Support for all 28+ API endpoints from the official Postman collection
5. **Best Practices**: Built following SOLID, DRY, KISS, and OOP principles for maintainability and extensibility

### Use Cases

- **Astrology Applications**: Build web or mobile apps that provide horoscope services
- **Compatibility Tools**: Create relationship compatibility analyzers
- **Personalized Reports**: Generate detailed astrological reports for users
- **Forecast Services**: Provide daily, weekly, or monthly astrological forecasts
- **Chart Generation**: Generate and display natal charts and wheel charts
- **Tarot Services**: Integrate tarot card reading functionality

## Features

- ✅ Credentials set once and reused for all requests
- ✅ Full API coverage from Postman collection
- ✅ SOLID principles implementation
- ✅ Comprehensive unit tests
- ✅ NuGet package ready
- ✅ PowerShell build scripts

## Installation

```bash
dotnet add package AstrologyApiClient
```

Or via NuGet Package Manager:
```
Install-Package AstrologyApiClient
```

## Quick Start

```csharp
using AstrologyApiClient;
using AstrologyApiClient.Models;

// Set credentials once when creating the client
var client = new AstrologyApiClient("your_user_id", "your_api_key");

try
{
    // Use the client - credentials are automatically included
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

    var horoscope = await client.GetWesternHoroscopeAsync<YourResponseType>(birthData);
}
finally
{
    client.Dispose();
}
```

## Setting Up Credentials for Development/Testing

**⚠️ Important:** Credentials are stored locally and never committed to the repository.

### For Running Tests

The test project supports multiple ways to provide credentials:

#### Option 1: Environment Variables (Recommended)

**Windows (PowerShell):**
```powershell
$env:ASTROLOGY_API_USER_ID = "your_user_id"
$env:ASTROLOGY_API_KEY = "your_api_key"
```

**Windows (Command Prompt):**
```cmd
set ASTROLOGY_API_USER_ID=your_user_id
set ASTROLOGY_API_KEY=your_api_key
```

**Linux/Mac:**
```bash
export ASTROLOGY_API_USER_ID=your_user_id
export ASTROLOGY_API_KEY=your_api_key
```

#### Option 2: Local appsettings.json

1. Copy the example file:
   ```powershell
   Copy-Item AstrologyApiClient.Tests\appsettings.example.json AstrologyApiClient.Tests\appsettings.json
   ```

2. Edit `AstrologyApiClient.Tests\appsettings.json` and add your credentials:
   ```json
   {
     "AstrologyApi": {
       "UserId": "your_actual_user_id",
       "ApiKey": "your_actual_api_key"
     }
   }
   ```

**Note:** `appsettings.json` is already in `.gitignore` and will not be committed to the repository.

### For Your Application

In your application code, you can:
- Pass credentials directly to the constructor (as shown in Quick Start)
- Load from environment variables
- Load from your application's configuration (appsettings.json, user secrets, etc.)

## Building

### Build and Test
```powershell
.\build.ps1
```

### Build, Test, and Pack
```powershell
.\build.ps1 -Pack
```

### Build with Custom Version
```powershell
.\build.ps1 -Pack -Version "1.0.1"
```

### Skip Tests
```powershell
.\build.ps1 -SkipTests
```

## Running Tests

### Run All Tests
```powershell
.\run-tests.ps1
```

### Run Only Unit Tests (Fast, No API Calls)
```powershell
.\run-tests.ps1 -UnitTestsOnly
```

### Run Only Integration Tests
```powershell
.\run-tests.ps1 -IntegrationTestsOnly
```

### Run Only API Availability Test (Generates Report)
```powershell
.\run-tests.ps1 -ApiAvailabilityOnly
```

### Run Tests Without Integration Tests
```powershell
.\run-tests.ps1 -SkipIntegration
```

### Quick Test Scripts
```powershell
.\run-all-tests.ps1          # Run all tests
.\run-unit-tests.ps1          # Run only unit tests
.\run-integration-tests.ps1   # Run only integration tests
.\run-api-availability.ps1    # Run API availability test
```

### Using dotnet test directly
```powershell
# Run all tests
dotnet test

# Run only unit tests
dotnet test --filter "FullyQualifiedName!~IntegrationTests&FullyQualifiedName!~ApiAvailabilityTests"

# Run only integration tests
dotnet test --filter "FullyQualifiedName~IntegrationTests"

# Run API availability test (generates markdown report)
dotnet test --filter "FullyQualifiedName~ApiAvailabilityTests"
```

## Supported API Endpoints

This library provides methods for all endpoints available in the Astrology API:

### Western Astrology
- `GetWesternHoroscopeAsync` - Get western horoscope data
- `GetWesternChartDataAsync` - Get western chart data
- `GetWesternChartImageAsync` - Get natal wheel chart image (Note: Western wheel chart is the same as natal wheel chart, uses `natal_wheel_chart` endpoint)

### Planetary Predictions
- `GetPlanetPredictionAsync` - Get personalized daily predictions for any planet (Mars, Moon, etc.)

### Tropical Transits
- `GetTropicalTransitsDailyAsync` - Daily transit calculations
- `GetTropicalTransitsWeeklyAsync` - Weekly transit calculations
- `GetTropicalTransitsMonthlyAsync` - Monthly transit calculations

### Solar Returns
- `GetSolarReturnDetailsAsync` - Solar return details
- `GetSolarReturnPlanetsAsync` - Solar return planets
- `GetSolarReturnHouseCuspsAsync` - Solar return house cusps
- `GetSolarReturnPlanetAspectsAsync` - Solar return planet aspects

### Lunar Metrics
- `GetLunarMetricsAsync` - Lunar phase and cycle information

### Synastry & Compatibility
- `GetSynastryHoroscopeAsync` - Synastry analysis between two charts
- `GetZodiacCompatibilityAsync` - Zodiac sign compatibility
- `GetCompatibilityAsync` - Detailed compatibility based on sun and rising signs

### Reports
- `GetPersonalityReportAsync` - Personality analysis report
- `GetRomanticPersonalityReportAsync` - Romantic personality report
- `GetLifeForecastReportAsync` - Life forecast report
- `GetRomanticForecastReportAsync` - Romantic forecast report
- `GetFriendshipReportAsync` - Friendship compatibility report
- `GetKarmaDestinyReportAsync` - Karma and destiny report
- `GetLoveCompatibilityReportAsync` - Love compatibility report
- `GetRomanticForecastCoupleReportAsync` - Couple romantic forecast
- `GetGeneralAscendantReportAsync` - General ascendant report
- `GetGeneralSignReportAsync` - General sign report
- `GetGeneralHouseReportAsync` - General house report

### Tarot
- `GetTarotPredictionsAsync` - Tarot card predictions for love, career, and finance

> **Note**: Some endpoints may require specific license tiers. Use the API availability test to check which endpoints are available with your license.

## Project Structure

```
AstrologyApiClient/
├── Interfaces/          # Interface definitions (IAstrologyApiClient, etc.)
├── Models/              # Data models (BirthData, CompositeBirthData, etc.)
├── Services/            # Service implementations (FormDataConverter, HttpClientWrapper, etc.)
└── AstrologyApiClient.cs

AstrologyApiClient.Tests/
├── AstrologyApiClientTests.cs          # Unit tests with mocks
├── AstrologyApiClientIntegrationTests.cs  # Integration tests (real API calls)
├── ApiAvailabilityTests.cs            # API availability checker
├── FormDataConverterTests.cs           # Form data converter tests
└── TestConfiguration.cs                # Centralized credential management
```

## API Availability & Licensing

The Astrology API uses a tiered licensing model. Some endpoints may return `405 Method Not Allowed` if they're not included in your current license plan.

### Check Your API Availability

Run the API availability test to see which endpoints are accessible with your license:

```powershell
.\run-api-availability.ps1
```

This will generate `API_Availability_Report.md` listing:
- ✅ Accessible APIs (available with your license)
- ❌ Not Accessible APIs (require license upgrade)

The report is saved to the project root for easy reference.

## Architecture

The library follows SOLID principles:

- **Single Responsibility**: Each class has one responsibility
  - `FormDataConverter` - Handles data conversion
  - `HttpClientWrapper` - Manages HTTP operations
  - `AuthenticationService` - Handles authentication
  - `AstrologyApiClient` - Orchestrates API calls

- **Open/Closed**: Extensible through interfaces (`IAstrologyApiClient`, `IFormDataConverter`, `IHttpClientWrapper`)

- **Liskov Substitution**: Interfaces allow substitution of implementations

- **Interface Segregation**: Focused, specific interfaces

- **Dependency Inversion**: Depends on abstractions, not concrete implementations

## Additional Resources

- **Test Scripts**: See `TEST_SCRIPTS.md` for detailed test script documentation
- **API Availability Report**: Generated automatically when running API availability tests
- **Example Usage**: See `AstrologyApiClient/ExampleUsage.cs` for code examples
- **Article**: See `ARTICLE_Working_Smart_Without_Documentation.md` - Learn how we built this library using test-driven exploration when documentation was unclear

## License

MIT

