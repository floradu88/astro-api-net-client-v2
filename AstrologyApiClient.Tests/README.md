# Integration Tests Configuration

## Setting Up Credentials

The integration tests require valid API credentials. You can provide them in one of the following ways:

### Option 1: Environment Variables (Recommended)

Set these environment variables before running tests:

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

### Option 2: appsettings.json

Edit `appsettings.json` in the test project directory:

```json
{
  "AstrologyApi": {
    "UserId": "your_user_id_here",
    "ApiKey": "your_api_key_here"
  }
}
```

**Note:** Make sure to add `appsettings.json` to `.gitignore` to avoid committing credentials!

## Running Integration Tests

```powershell
# Run all tests
dotnet test

# Run only integration tests
dotnet test --filter "FullyQualifiedName~IntegrationTests"

# Run only unit tests (without integration tests)
dotnet test --filter "FullyQualifiedName!~IntegrationTests"
```

## Test Categories

- **Unit Tests**: Use mocks, no API calls (`AstrologyApiClientTests`, `FormDataConverterTests`)
- **Integration Tests**: Make real API calls (`AstrologyApiClientIntegrationTests`)

