# Implementation Guide: Fixing 405 Errors for General Report APIs

## Problem Identified

The three general report endpoints were returning 405 errors with validation messages indicating they expected a `day` field (not `p_day`):

- `general_ascendant_report/tropical`
- `general_sign_report/tropical/{sign}`
- `general_house_report`

## Root Cause

The API expects a **hybrid format** for these endpoints:
- **Primary person**: Uses simple format (`day`, `month`, `year`) instead of (`p_day`, `p_month`, `p_year`)
- **Secondary person**: Uses `s_` prefix format (`s_day`, `s_month`, `s_year`)

This is different from other composite endpoints (like `synastry_horoscope`) which use `p_day`, `p_month` for the primary person.

## Data Models

### BirthData Model
Used for single-person endpoints:

```csharp
namespace AstrologyApiClient.Models;

/// <summary>
/// Represents birth data for astrology calculations
/// </summary>
public class BirthData
{
    public int Day { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
    public int Hour { get; set; }
    public int Min { get; set; }
    public double Lat { get; set; }
    public double Lon { get; set; }
    public double Tzone { get; set; }
}
```

### PrimaryBirthData Model
Represents the primary person in composite calculations:

```csharp
/// <summary>
/// Represents birth data for composite/synastry calculations (primary person)
/// </summary>
public class PrimaryBirthData
{
    public int PDay { get; set; }
    public int PMonth { get; set; }
    public int PYear { get; set; }
    public int PHour { get; set; }
    public int PMin { get; set; }
    public double PLat { get; set; }
    public double PLon { get; set; }
    public double PTzone { get; set; }
}
```

### SecondaryBirthData Model
Represents the secondary person in composite calculations:

```csharp
/// <summary>
/// Represents birth data for composite/synastry calculations (secondary person)
/// </summary>
public class SecondaryBirthData
{
    public int SDay { get; set; }
    public int SMonth { get; set; }
    public int SYear { get; set; }
    public int SHour { get; set; }
    public int SMin { get; set; }
    public double SLat { get; set; }
    public double SLon { get; set; }
    public double STzone { get; set; }
}
```

### CompositeBirthData Model
Used for composite/synastry endpoints (including general reports):

```csharp
/// <summary>
/// Represents birth data for composite/synastry calculations (both persons)
/// </summary>
public class CompositeBirthData
{
    public PrimaryBirthData Primary { get; set; } = new();
    public SecondaryBirthData Secondary { get; set; } = new();
    public double? Orb { get; set; }  // Optional orb value for aspect calculations
}
```

## Solution Implemented

### 1. Added Hybrid Format Converter

Created a new method `ConvertToHybridFormat()` in `FormDataConverter`:

```csharp
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
        // Primary person in simple format (day, not p_day)
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
```

### 2. Updated API Methods

Updated the three methods to use the hybrid format:

```csharp
public async Task<T?> GetGeneralAscendantReportAsync<T>(CompositeBirthData compositeData, CancellationToken cancellationToken = default)
{
    var formData = _formDataConverter.ConvertToHybridFormat(compositeData);
    return await _httpClientWrapper.PostAsync<T>("general_ascendant_report/tropical", formData, cancellationToken);
}

public async Task<T?> GetGeneralSignReportAsync<T>(string sign, CompositeBirthData compositeData, CancellationToken cancellationToken = default)
{
    var formData = _formDataConverter.ConvertToHybridFormat(compositeData);
    return await _httpClientWrapper.PostAsync<T>($"general_sign_report/tropical/{sign}", formData, cancellationToken);
}

public async Task<T?> GetGeneralHouseReportAsync<T>(CompositeBirthData compositeData, CancellationToken cancellationToken = default)
{
    var formData = _formDataConverter.ConvertToHybridFormat(compositeData);
    return await _httpClientWrapper.PostAsync<T>("general_house_report", formData, cancellationToken);
}
```

## Data Format Comparison

### Standard Composite Format (for most endpoints):
```
p_day=10
p_month=5
p_year=1990
...
s_day=10
s_month=5
s_year=1990
...
```

### Hybrid Format (for general report endpoints):
```
day=10          ← Primary person uses simple format
month=5
year=1990
...
s_day=10        ← Secondary person uses s_ prefix
s_month=5
s_year=1990
...
```

## Usage Examples

### Creating CompositeBirthData

```csharp
using AstrologyApiClient;
using AstrologyApiClient.Models;

// Initialize the client
var client = new AstrologyApiClient(userId, apiKey);

// Create composite birth data
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
    Orb = 1  // Optional: orb value for aspect calculations
};

// Use with general report endpoints
var ascendantReport = await client.GetGeneralAscendantReportAsync<object>(compositeData);
var signReport = await client.GetGeneralSignReportAsync<object>("sun", compositeData);
var houseReport = await client.GetGeneralHouseReportAsync<object>(compositeData);
```

### Example: Using CompositeBirthData

```csharp
using AstrologyApiClient;
using AstrologyApiClient.Models;

public async Task ExampleUsageWithCompositeData()
{
    var userId = "your_user_id";
    var apiKey = "your_api_key";
    
    var client = new AstrologyApiClient(userId, apiKey);
    
    try
    {
        // Prepare birth data for two people using CompositeBirthData
        var compositeData = new CompositeBirthData
        {
            Primary = new PrimaryBirthData
            {
                PDay = 15,
                PMonth = 3,
                PYear = 1985,
                PHour = 14,
                PMin = 30,
                PLat = 40.7128,  // New York
                PLon = -74.0060,
                PTzone = -5.0
            },
            Secondary = new SecondaryBirthData
            {
                SDay = 22,
                SMonth = 7,
                SYear = 1990,
                SHour = 9,
                SMin = 15,
                SLat = 34.0522,  // Los Angeles
                SLon = -118.2437,
                STzone = -8.0
            },
            Orb = 1.0
        };
        
        // Get general ascendant report
        var ascendantReport = await client.GetGeneralAscendantReportAsync<dynamic>(compositeData);
        Console.WriteLine($"Ascendant Report: {ascendantReport}");
        
        // Get general sign report for Sun
        var sunReport = await client.GetGeneralSignReportAsync<dynamic>("sun", compositeData);
        Console.WriteLine($"Sun Sign Report: {sunReport}");
        
        // Get general house report
        var houseReport = await client.GetGeneralHouseReportAsync<dynamic>(compositeData);
        Console.WriteLine($"House Report: {houseReport}");
    }
    catch (HttpRequestException ex)
    {
        if (ex.Message.Contains("405"))
        {
            Console.WriteLine("Endpoint not available - may require license upgrade");
        }
        else
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
    finally
    {
        client.Dispose();
    }
}
```

### Example: Using Two BirthData Objects (New Overload)

```csharp
using AstrologyApiClient;
using AstrologyApiClient.Models;

public async Task ExampleUsageWithTwoObjects()
{
    var userId = "your_user_id";
    var apiKey = "your_api_key";
    
    var client = new AstrologyApiClient(userId, apiKey);
    
    try
    {
        // Prepare birth data for primary person
        var primaryPerson = new BirthData
        {
            Day = 15,
            Month = 3,
            Year = 1985,
            Hour = 14,
            Min = 30,
            Lat = 40.7128,  // New York
            Lon = -74.0060,
            Tzone = -5.0
        };
        
        // Prepare birth data for secondary person
        var secondaryPerson = new BirthData
        {
            Day = 22,
            Month = 7,
            Year = 1990,
            Hour = 9,
            Min = 15,
            Lat = 34.0522,  // Los Angeles
            Lon = -118.2437,
            Tzone = -8.0
        };
        
        // Get general ascendant report using two BirthData objects
        var ascendantReport = await client.GetGeneralAscendantReportAsync<dynamic>(
            primaryPerson, 
            secondaryPerson, 
            orb: 1.0
        );
        Console.WriteLine($"Ascendant Report: {ascendantReport}");
        
        // Get general sign report for Sun using two BirthData objects
        var sunReport = await client.GetGeneralSignReportAsync<dynamic>(
            "sun", 
            primaryPerson, 
            secondaryPerson, 
            orb: 1.0
        );
        Console.WriteLine($"Sun Sign Report: {sunReport}");
        
        // Get general house report using two BirthData objects
        var houseReport = await client.GetGeneralHouseReportAsync<dynamic>(
            primaryPerson, 
            secondaryPerson, 
            orb: 1.0
        );
        Console.WriteLine($"House Report: {houseReport}");
    }
    catch (HttpRequestException ex)
    {
        if (ex.Message.Contains("405"))
        {
            Console.WriteLine("Endpoint not available - may require license upgrade");
        }
        else
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
    finally
    {
        client.Dispose();
    }
}
```

## Testing

To verify the fix works:

1. Run the API availability tests:
   ```powershell
   .\run-api-availability.ps1
   ```

2. Check if the 405 errors are resolved in `API_Availability_Report.md`

3. If still getting errors, check:
   - License restrictions (405 might indicate endpoint not available)
   - API documentation for exact parameter requirements
   - Response body for specific validation errors

4. Test with actual data:
   ```csharp
   // Create a test
   var testData = new CompositeBirthData
   {
       Primary = new PrimaryBirthData { /* ... */ },
       Secondary = new SecondaryBirthData { /* ... */ }
   };
   
   var result = await client.GetGeneralAscendantReportAsync<object>(testData);
   ```

## Files Modified

1. `AstrologyApiClient/Services/FormDataConverter.cs` - Added `ConvertToHybridFormat()` methods (both overloads)
2. `AstrologyApiClient/AstrologyApiClient.cs` - Updated three methods with overloads to support both `CompositeBirthData` and two `BirthData` objects
3. `AstrologyApiClient/Interfaces/IAstrologyApiClient.cs` - Added interface overloads for the three methods

## Next Steps

1. **Test the implementation**: Run the API availability tests to verify the fix
2. **Check license**: If still getting 405, verify these endpoints are available in your license tier
3. **Update documentation**: If successful, update README with correct usage examples

## Model Property Mapping

### Standard Composite Format (p_day format)
Used by: `synastry_horoscope`, `friendship_report`, `karma_destiny_report`, etc.

| Model Property | Form Data Key | Example |
|---------------|---------------|---------|
| `Primary.PDay` | `p_day` | `10` |
| `Primary.PMonth` | `p_month` | `5` |
| `Primary.PYear` | `p_year` | `1990` |
| `Primary.PHour` | `p_hour` | `11` |
| `Primary.PMin` | `p_min` | `55` |
| `Primary.PLat` | `p_lat` | `19.20` |
| `Primary.PLon` | `p_lon` | `25.20` |
| `Primary.PTzone` | `p_tzone` | `5.5` |
| `Secondary.SDay` | `s_day` | `10` |
| `Secondary.SMonth` | `s_month` | `5` |
| `Secondary.SYear` | `s_year` | `1990` |
| `Secondary.SHour` | `s_hour` | `15` |
| `Secondary.SMin` | `s_min` | `22` |
| `Secondary.SLat` | `s_lat` | `19.33` |
| `Secondary.SLon` | `s_lon` | `25.20` |
| `Secondary.STzone` | `s_tzone` | `5.5` |
| `Orb` | `orb` | `1` |

### Hybrid Format (day format for primary)
Used by: `general_ascendant_report`, `general_sign_report`, `general_house_report`

| Model Property | Form Data Key | Example |
|---------------|---------------|---------|
| `Primary.PDay` | `day` ⚠️ | `10` |
| `Primary.PMonth` | `month` ⚠️ | `5` |
| `Primary.PYear` | `year` ⚠️ | `1990` |
| `Primary.PHour` | `hour` ⚠️ | `11` |
| `Primary.PMin` | `min` ⚠️ | `55` |
| `Primary.PLat` | `lat` ⚠️ | `19.20` |
| `Primary.PLon` | `lon` ⚠️ | `25.20` |
| `Primary.PTzone` | `tzone` ⚠️ | `5.5` |
| `Secondary.SDay` | `s_day` | `10` |
| `Secondary.SMonth` | `s_month` | `5` |
| `Secondary.SYear` | `s_year` | `1990` |
| `Secondary.SHour` | `s_hour` | `15` |
| `Secondary.SMin` | `s_min` | `22` |
| `Secondary.SLat` | `s_lat` | `19.33` |
| `Secondary.SLon` | `s_lon` | `25.20` |
| `Secondary.STzone` | `s_tzone` | `5.5` |
| `Orb` | `orb` | `1` |

⚠️ **Key Difference**: Primary person uses simple field names (`day`, `month`) instead of prefixed (`p_day`, `p_month`)

## Notes

- The Postman collection shows `p_day` format, but the actual API validation requires `day` format for these specific endpoints
- This inconsistency suggests the API may have been updated or the Postman collection is outdated
- The error message was the key to identifying the correct format: `"day" must be a number` indicated the API was looking for `day`, not `p_day`
- All three general report endpoints (`general_ascendant_report`, `general_sign_report`, `general_house_report`) use the same hybrid format
- The `CompositeBirthData` model structure remains the same - only the form data conversion differs

