# Investigation: 405 Errors for General Report APIs

## Problem Summary

Three API endpoints are returning 405 errors with validation messages:
- `general_ascendant_report/tropical`
- `general_sign_report/tropical/sun`
- `general_house_report`

## Error Analysis

### Error Response
```json
{
  "status": false,
  "msg": "Request data validation failed. Kindly check your request data again.",
  "error": [{
    "message": "\"day\" must be a number",
    "path": ["day"],
    "type": "number.base",
    "context": {
      "label": "day",
      "value": null,
      "key": "day"
    }
  }]
}
```

**Key Finding**: The API is expecting a `day` field (not `p_day`), but receiving `null`.

## Current Implementation

### What We're Sending (CompositeBirthData format):
```
p_day=10
p_month=5
p_year=1990
p_hour=11
p_min=55
p_lat=19.20
p_lon=25.20
p_tzone=5.5
s_day=10
s_month=5
s_year=1990
s_hour=15
s_min=22
s_lat=19.33
s_lon=25.20
s_tzone=5.5
orb=1
```

### What the API Expects:
Based on the error, the API is looking for a `day` field (singular, not `p_day`).

## Postman Collection Analysis

The Postman collection shows these endpoints using `p_day`, `p_month`, etc. format, which matches our current implementation. However, the API validation error suggests otherwise.

## Possible Solutions

### Option 1: These endpoints need BirthData format (single person)
These endpoints might only need one person's data in `day`, `month`, `year` format:
- Change method signature to accept `BirthData` instead of `CompositeBirthData`
- Use `BirthData` form data conversion

### Option 2: These endpoints need hybrid format
These endpoints might need:
- Primary person in `day`, `month`, `year` format (not `p_day`)
- Secondary person in `s_day`, `s_month`, `s_year` format
- This would require a custom form data converter

### Option 3: These endpoints are actually license-restricted
The 405 status code might indicate these endpoints are not available in the current license tier, and the validation error is a red herring.

## Recommended Next Steps

1. **Test with BirthData format**: Try calling these endpoints with `BirthData` instead of `CompositeBirthData`
2. **Check API documentation**: If available, verify the exact parameter format
3. **Contact API provider**: If these are license-restricted, confirm with the provider
4. **Create custom converter**: If hybrid format is needed, create a specialized converter

## Implementation Options

### Option A: Change to BirthData (if single person only)
```csharp
public async Task<T?> GetGeneralAscendantReportAsync<T>(BirthData birthData, CancellationToken cancellationToken = default)
{
    var formData = _formDataConverter.Convert(birthData);
    return await _httpClientWrapper.PostAsync<T>("general_ascendant_report/tropical", formData, cancellationToken);
}
```

### Option B: Create Hybrid Converter (if both formats needed)
```csharp
private Dictionary<string, string> ConvertToHybridFormat(CompositeBirthData data)
{
    return new Dictionary<string, string>
    {
        // Primary person in simple format
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
}
```

## Testing Strategy

1. Create a test that tries `BirthData` format
2. Create a test that tries hybrid format
3. Compare responses to determine correct format
4. Update implementation based on test results

## Full URLs Tested

- `https://json.astrologyapi.com/v1/general_ascendant_report/tropical`
- `https://json.astrologyapi.com/v1/general_house_report`
- `https://json.astrologyapi.com/v1/general_sign_report/tropical/sun`

## Status

**Current Status**: Investigation in progress
**Next Action**: Test with different data formats to determine correct implementation

