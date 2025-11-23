# Test Scripts Documentation

This document describes all the PowerShell test scripts created for the AstrologyApiClient project.

## Main Test Runner

### `run-tests.ps1`

The main test runner script with flexible options to run different test categories.

**Usage:**
```powershell
# Run all tests (unit + integration + API availability)
.\run-tests.ps1

# Run only unit tests (fast, no API calls)
.\run-tests.ps1 -UnitTestsOnly

# Run only integration tests
.\run-tests.ps1 -IntegrationTestsOnly

# Run only API availability test (generates markdown report)
.\run-tests.ps1 -ApiAvailabilityOnly

# Run tests without integration tests
.\run-tests.ps1 -SkipIntegration

# Custom configuration and verbosity
.\run-tests.ps1 -Configuration Debug -Verbosity detailed
```

**Parameters:**
- `-UnitTestsOnly` - Run only unit tests (no API calls)
- `-IntegrationTestsOnly` - Run only integration tests
- `-ApiAvailabilityOnly` - Run only API availability test
- `-SkipIntegration` - Skip integration tests
- `-Configuration` - Build configuration (Release/Debug, default: Release)
- `-Verbosity` - Test output verbosity (minimal/normal/detailed, default: normal)

## Quick Test Scripts

### `run-all-tests.ps1`
Runs all tests (unit + integration + API availability)
```powershell
.\run-all-tests.ps1
```

### `run-unit-tests.ps1`
Runs only unit tests (fast, no API calls, no credentials needed)
```powershell
.\run-unit-tests.ps1
```

### `run-integration-tests.ps1`
Runs only integration tests (requires API credentials)
```powershell
.\run-integration-tests.ps1
```

### `run-api-availability.ps1`
Runs API availability test and generates markdown report (requires API credentials)
```powershell
.\run-api-availability.ps1
```

## Test Categories

### 1. Unit Tests
- **Location:** `AstrologyApiClientTests.cs`, `FormDataConverterTests.cs`
- **Description:** Fast tests using mocks, no API calls
- **Count:** 21 tests
- **Credentials:** Not required
- **Filter:** `FullyQualifiedName!~IntegrationTests&FullyQualifiedName!~ApiAvailabilityTests`

### 2. Integration Tests
- **Location:** `AstrologyApiClientIntegrationTests.cs`
- **Description:** Real API calls testing all endpoints
- **Count:** 30+ tests
- **Credentials:** Required (via environment variables or appsettings.json)
- **Filter:** `FullyQualifiedName~IntegrationTests`
- **Note:** Handles 405 errors gracefully (license not available)

### 3. API Availability Tests
- **Location:** `ApiAvailabilityTests.cs`
- **Description:** Tests all APIs and generates markdown report
- **Count:** 1 test (tests all 28 endpoints)
- **Credentials:** Required
- **Filter:** `FullyQualifiedName~ApiAvailabilityTests`
- **Output:** Generates `API_Availability_Report.md` in project root

## Direct dotnet test Commands

You can also use `dotnet test` directly:

```powershell
# Run all tests
dotnet test

# Run only unit tests
dotnet test --filter "FullyQualifiedName!~IntegrationTests&FullyQualifiedName!~ApiAvailabilityTests"

# Run only integration tests
dotnet test --filter "FullyQualifiedName~IntegrationTests"

# Run API availability test
dotnet test --filter "FullyQualifiedName~ApiAvailabilityTests" --verbosity normal
```

## Credentials Setup

Before running integration or API availability tests, set up credentials:

**Option 1: Environment Variables**
```powershell
$env:ASTROLOGY_API_USER_ID = "your_user_id"
$env:ASTROLOGY_API_KEY = "your_api_key"
```

**Option 2: appsettings.json**
Edit `AstrologyApiClient.Tests/appsettings.json`:
```json
{
  "AstrologyApi": {
    "UserId": "your_user_id",
    "ApiKey": "your_api_key"
  }
}
```

## Output Files

- **API_Availability_Report.md** - Generated in project root after running API availability test
  - Lists all accessible and non-accessible APIs
  - Shows license status for each endpoint
  - Includes timestamp and user ID

## Examples

```powershell
# Quick unit test run (fastest)
.\run-unit-tests.ps1

# Full test suite
.\run-all-tests.ps1

# Check API availability and generate report
.\run-api-availability.ps1

# Run with custom settings
.\run-tests.ps1 -Configuration Debug -Verbosity detailed -SkipIntegration
```

