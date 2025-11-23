# PowerShell Build Script for AstrologyApiClient
# This script builds, tests, and packages the solution

param(
    [string]$Configuration = "Release",
    [switch]$SkipTests = $false,
    [switch]$Pack = $false,
    [string]$Version = ""
)

$ErrorActionPreference = "Stop"

Write-Host "=========================================" -ForegroundColor Cyan
Write-Host "AstrologyApiClient Build Script" -ForegroundColor Cyan
Write-Host "=========================================" -ForegroundColor Cyan
Write-Host ""

# Get the script directory
$scriptPath = Split-Path -Parent $MyInvocation.MyCommand.Path
Set-Location $scriptPath

# Clean previous builds
Write-Host "Cleaning previous builds..." -ForegroundColor Yellow
dotnet clean AstrologyApiClient.sln --configuration $Configuration
if ($LASTEXITCODE -ne 0) {
    Write-Host "Clean failed!" -ForegroundColor Red
    exit 1
}

# Restore packages
Write-Host "Restoring NuGet packages..." -ForegroundColor Yellow
dotnet restore AstrologyApiClient.sln
if ($LASTEXITCODE -ne 0) {
    Write-Host "Restore failed!" -ForegroundColor Red
    exit 1
}

# Build solution
Write-Host "Building solution..." -ForegroundColor Yellow
if ($Version -ne "") {
    dotnet build AstrologyApiClient.sln --configuration $Configuration /p:Version=$Version
} else {
    dotnet build AstrologyApiClient.sln --configuration $Configuration
}
if ($LASTEXITCODE -ne 0) {
    Write-Host "Build failed!" -ForegroundColor Red
    exit 1
}
Write-Host "Build succeeded!" -ForegroundColor Green
Write-Host ""

# Run tests
if (-not $SkipTests) {
    Write-Host "Running unit tests..." -ForegroundColor Yellow
    dotnet test AstrologyApiClient.sln --configuration $Configuration --no-build --verbosity normal
    if ($LASTEXITCODE -ne 0) {
        Write-Host "Tests failed!" -ForegroundColor Red
        exit 1
    }
    Write-Host "All tests passed!" -ForegroundColor Green
    Write-Host ""
}

# Pack NuGet package
if ($Pack) {
    Write-Host "Creating NuGet package..." -ForegroundColor Yellow
    if ($Version -ne "") {
        dotnet pack AstrologyApiClient/AstrologyApiClient.csproj --configuration $Configuration --no-build --output ./artifacts /p:Version=$Version
    } else {
        dotnet pack AstrologyApiClient/AstrologyApiClient.csproj --configuration $Configuration --no-build --output ./artifacts
    }
    if ($LASTEXITCODE -ne 0) {
        Write-Host "Pack failed!" -ForegroundColor Red
        exit 1
    }
    Write-Host "NuGet package created successfully!" -ForegroundColor Green
    Write-Host "Package location: ./artifacts" -ForegroundColor Cyan
    Write-Host ""
}

Write-Host "=========================================" -ForegroundColor Cyan
Write-Host "Build completed successfully!" -ForegroundColor Green
Write-Host "=========================================" -ForegroundColor Cyan

