# PowerShell Script to Run All Tests
# This script runs all unit tests, integration tests, and API availability tests

param(
    [switch]$UnitTestsOnly = $false,
    [switch]$IntegrationTestsOnly = $false,
    [switch]$ApiAvailabilityOnly = $false,
    [switch]$SkipIntegration = $false,
    [string]$Configuration = "Release",
    [string]$Verbosity = "normal"
)

$ErrorActionPreference = "Stop"

Write-Host "=========================================" -ForegroundColor Cyan
Write-Host "AstrologyApiClient Test Runner" -ForegroundColor Cyan
Write-Host "=========================================" -ForegroundColor Cyan
Write-Host ""

# Get the script directory
$scriptPath = Split-Path -Parent $MyInvocation.MyCommand.Path
Set-Location $scriptPath

$testResults = @{
    UnitTests = $null
    IntegrationTests = $null
    ApiAvailability = $null
}

$totalPassed = 0
$totalFailed = 0
$totalSkipped = 0

# Function to run tests and capture results
function Run-TestCategory {
    param(
        [string]$CategoryName,
        [string]$Filter,
        [string]$Description
    )
    
    Write-Host "----------------------------------------" -ForegroundColor Yellow
    Write-Host "Running $Description..." -ForegroundColor Yellow
    Write-Host "----------------------------------------" -ForegroundColor Yellow
    Write-Host ""
    
    try {
        $result = dotnet test AstrologyApiClient.Tests/AstrologyApiClient.Tests.csproj `
            --filter $Filter `
            --configuration $Configuration `
            --verbosity $Verbosity `
            --no-build
        
        if ($LASTEXITCODE -eq 0) {
            Write-Host "✅ $Description completed successfully!" -ForegroundColor Green
            Write-Host ""
            return $true
        } else {
            Write-Host "❌ $Description failed!" -ForegroundColor Red
            Write-Host ""
            return $false
        }
    }
    catch {
        Write-Host "❌ Error running $Description : $_" -ForegroundColor Red
        Write-Host ""
        return $false
    }
}

# Build the solution first
Write-Host "Building solution..." -ForegroundColor Yellow
dotnet build AstrologyApiClient.sln --configuration $Configuration --no-incremental
if ($LASTEXITCODE -ne 0) {
    Write-Host "Build failed! Cannot run tests." -ForegroundColor Red
    exit 1
}
Write-Host "Build succeeded!" -ForegroundColor Green
Write-Host ""

# Determine which tests to run
$runUnitTests = $UnitTestsOnly -or (-not $IntegrationTestsOnly -and -not $ApiAvailabilityOnly -and -not $SkipIntegration)
$runIntegrationTests = $IntegrationTestsOnly -or (-not $UnitTestsOnly -and -not $ApiAvailabilityOnly -and -not $SkipIntegration)
$runApiAvailability = $ApiAvailabilityOnly -or (-not $UnitTestsOnly -and -not $IntegrationTestsOnly -and -not $SkipIntegration)

# Run Unit Tests
if ($runUnitTests) {
    $testResults.UnitTests = Run-TestCategory `
        -CategoryName "UnitTests" `
        -Filter "FullyQualifiedName!~IntegrationTests&FullyQualifiedName!~ApiAvailabilityTests" `
        -Description "Unit Tests"
}

# Run Integration Tests
if ($runIntegrationTests) {
    Write-Host "⚠️  Note: Integration tests require valid API credentials." -ForegroundColor Yellow
    Write-Host "   Set credentials via environment variables or appsettings.json" -ForegroundColor Yellow
    Write-Host ""
    
    $testResults.IntegrationTests = Run-TestCategory `
        -CategoryName "IntegrationTests" `
        -Filter "FullyQualifiedName~IntegrationTests" `
        -Description "Integration Tests"
}

# Run API Availability Tests
if ($runApiAvailability) {
    Write-Host "⚠️  Note: API Availability test requires valid API credentials." -ForegroundColor Yellow
    Write-Host "   This test will generate API_Availability_Report.md" -ForegroundColor Yellow
    Write-Host ""
    
    $testResults.ApiAvailability = Run-TestCategory `
        -CategoryName "ApiAvailability" `
        -Filter "FullyQualifiedName~ApiAvailabilityTests" `
        -Description "API Availability Tests"
    
    # Check if report was generated
    if (Test-Path "API_Availability_Report.md") {
        Write-Host "📄 API Availability Report generated: API_Availability_Report.md" -ForegroundColor Cyan
        Write-Host ""
    }
}

# Summary
Write-Host "=========================================" -ForegroundColor Cyan
Write-Host "Test Summary" -ForegroundColor Cyan
Write-Host "=========================================" -ForegroundColor Cyan
Write-Host ""

if ($testResults.UnitTests -ne $null) {
    $status = if ($testResults.UnitTests) { "✅ Passed" } else { "❌ Failed" }
    Write-Host "Unit Tests: $status" -ForegroundColor $(if ($testResults.UnitTests) { "Green" } else { "Red" })
}

if ($testResults.IntegrationTests -ne $null) {
    $status = if ($testResults.IntegrationTests) { "✅ Passed" } else { "❌ Failed" }
    Write-Host "Integration Tests: $status" -ForegroundColor $(if ($testResults.IntegrationTests) { "Green" } else { "Red" })
}

if ($testResults.ApiAvailability -ne $null) {
    $status = if ($testResults.ApiAvailability) { "✅ Passed" } else { "❌ Failed" }
    Write-Host "API Availability Tests: $status" -ForegroundColor $(if ($testResults.ApiAvailability) { "Green" } else { "Red" })
}

Write-Host ""
Write-Host "=========================================" -ForegroundColor Cyan

# Exit with appropriate code
$allPassed = ($testResults.UnitTests -ne $false) -and 
             ($testResults.IntegrationTests -ne $false) -and 
             ($testResults.ApiAvailability -ne $false)

if (-not $allPassed) {
    exit 1
}

exit 0

