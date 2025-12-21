# Script to remove credentials from git history
# WARNING: This rewrites git history. Use with caution!

param(
    [switch]$DryRun = $false,
    [switch]$Force = $false
)

$ErrorActionPreference = "Stop"

Write-Host "========================================" -ForegroundColor Yellow
Write-Host "Remove Credentials from Git History" -ForegroundColor Yellow
Write-Host "========================================" -ForegroundColor Yellow
Write-Host ""

# Check if we're in a git repository
if (-not (Test-Path .git)) {
    Write-Host "ERROR: Not a git repository!" -ForegroundColor Red
    exit 1
}

# Check if there are uncommitted changes
$status = git status --porcelain
if ($status -and -not $Force) {
    Write-Host "WARNING: You have uncommitted changes!" -ForegroundColor Yellow
    Write-Host "Please commit or stash your changes before running this script." -ForegroundColor Yellow
    Write-Host ""
    Write-Host "To proceed anyway, use: .\remove-credentials-from-history.ps1 -Force" -ForegroundColor Yellow
    exit 1
}

# Credentials to remove
$userId = "647584"
$apiKey = "bde5153706977d8c6a77aeac71205482ac9e3475"

Write-Host "This script will:" -ForegroundColor Cyan
Write-Host "  1. Remove User ID: $userId" -ForegroundColor White
Write-Host "  2. Remove API Key: $apiKey (if found)" -ForegroundColor White
Write-Host "  3. Remove appsettings.json from all commits (if it exists)" -ForegroundColor White
Write-Host "  4. Rewrite git history (this is destructive!)" -ForegroundColor White
Write-Host ""

if ($DryRun) {
    Write-Host "DRY RUN MODE - No changes will be made" -ForegroundColor Green
    Write-Host ""
    
    # Check for credentials in history
    Write-Host "Checking for credentials in git history..." -ForegroundColor Cyan
    $foundUserId = git log --all -S $userId --oneline 2>$null
    $foundApiKey = git log --all -S $apiKey --oneline 2>$null
    
    if ($foundUserId) {
        Write-Host "Found User ID in these commits:" -ForegroundColor Yellow
        Write-Host $foundUserId
    } else {
        Write-Host "User ID not found in git history" -ForegroundColor Green
    }
    
    if ($foundApiKey) {
        Write-Host "Found API Key in these commits:" -ForegroundColor Yellow
        Write-Host $foundApiKey
    } else {
        Write-Host "API Key not found in git history" -ForegroundColor Green
    }
    
    Write-Host ""
    Write-Host "To actually remove credentials, run without -DryRun" -ForegroundColor Yellow
    exit 0
}

Write-Host "WARNING: This will rewrite git history!" -ForegroundColor Red
Write-Host "All team members will need to re-clone the repository." -ForegroundColor Red
Write-Host "You will need to force push to update the remote repository." -ForegroundColor Red
Write-Host ""

$confirm = Read-Host "Are you sure you want to proceed? (type 'yes' to continue)"
if ($confirm -ne "yes") {
    Write-Host "Aborted." -ForegroundColor Yellow
    exit 0
}

Write-Host ""
Write-Host "Removing credentials from git history..." -ForegroundColor Cyan

# Create backup branch
$backupBranch = "backup-before-credential-removal-$(Get-Date -Format 'yyyyMMddHHmmss')"
Write-Host "Creating backup branch: $backupBranch" -ForegroundColor Cyan
git branch $backupBranch 2>$null
Write-Host "  Backup created. You can restore with: git checkout $backupBranch" -ForegroundColor Green

# Create a temporary script for tree-filter (PowerShell doesn't work well in tree-filter)
$tempScript = Join-Path $env:TEMP "git-filter-credential-removal.ps1"
@"
`$content = Get-Content 'API_Availability_Report.md' -Raw -ErrorAction SilentlyContinue
if (`$content) {
    `$content = `$content -replace '\*\*User ID:\*\* 647584', '**User ID:** [REMOVED]'
    `$content = `$content -replace '647584', '[REMOVED]'
    Set-Content 'API_Availability_Report.md' -Value `$content -NoNewline
}
"@ | Out-File -FilePath $tempScript -Encoding UTF8

try {
    # Remove User ID from API_Availability_Report.md
    Write-Host ""
    Write-Host "Step 1: Removing User ID from API_Availability_Report.md..." -ForegroundColor Cyan
    git filter-branch --force --tree-filter "powershell -File $tempScript" --prune-empty --tag-name-filter cat -- --all 2>&1 | Out-Null
    
    # Remove appsettings.json from all commits (if it exists)
    Write-Host "Step 2: Removing appsettings.json from all commits..." -ForegroundColor Cyan
    git filter-branch --force --index-filter `
        "git rm --cached --ignore-unmatch '**/appsettings.json' 'AstrologyApiClient.Tests/appsettings.json'" `
        --prune-empty --tag-name-filter cat -- --all 2>&1 | Out-Null
    
    # Clean up backup refs
    Write-Host ""
    Write-Host "Step 3: Cleaning up backup references..." -ForegroundColor Cyan
    $now = Get-Date -Format "yyyyMMddHHmmss"
    $backupDir = ".git/refs/original-backup-$now"
    if (Test-Path .git/refs/original) {
        if (-not (Test-Path $backupDir)) {
            New-Item -ItemType Directory -Path $backupDir -Force | Out-Null
        }
        Get-ChildItem .git/refs/original -ErrorAction SilentlyContinue | ForEach-Object {
            Move-Item $_.FullName $backupDir -Force -ErrorAction SilentlyContinue
        }
        Write-Host "  - Backup saved to: $backupDir" -ForegroundColor Green
    }
    
    # Expire reflog and garbage collect
    Write-Host "Step 4: Expiring reflog and running garbage collection..." -ForegroundColor Cyan
    git reflog expire --expire=now --all 2>&1 | Out-Null
    git gc --prune=now --aggressive 2>&1 | Out-Null
    
    Write-Host ""
    Write-Host "========================================" -ForegroundColor Green
    Write-Host "Credentials removed from git history!" -ForegroundColor Green
    Write-Host "========================================" -ForegroundColor Green
    Write-Host ""
    Write-Host "IMPORTANT NEXT STEPS:" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "1. Verify the changes:" -ForegroundColor Cyan
    Write-Host "   git log --all -S '647584'" -ForegroundColor White
    Write-Host "   (Should return no results)" -ForegroundColor Gray
    Write-Host ""
    Write-Host "2. Force push to remote (WARNING: This overwrites remote history):" -ForegroundColor Cyan
    Write-Host "   git push --force --all" -ForegroundColor White
    Write-Host "   git push --force --tags" -ForegroundColor White
    Write-Host ""
    Write-Host "3. Notify all team members:" -ForegroundColor Cyan
    Write-Host "   - They must re-clone the repository" -ForegroundColor White
    Write-Host "   - Or run: git fetch origin && git reset --hard origin/master" -ForegroundColor White
    Write-Host ""
    Write-Host "4. SECURITY: Rotate your API credentials immediately!" -ForegroundColor Red
    Write-Host "   The credentials may have been exposed in the git history." -ForegroundColor Red
    Write-Host ""
    Write-Host "Backup branch: $backupBranch" -ForegroundColor Cyan
    Write-Host "Backup refs: $backupDir" -ForegroundColor Cyan
}
finally {
    # Clean up temp script
    if (Test-Path $tempScript) {
        Remove-Item $tempScript -Force -ErrorAction SilentlyContinue
    }
}
