# Security: Removing Credentials from Git History

This guide explains how to remove exposed credentials from git history.

## ⚠️ Important Security Notice

**Your credentials have been exposed in git history:**
- User ID: `647584` (found in `API_Availability_Report.md`)
- API Key: Check if it was committed (likely not, but verify)

**You should:**
1. **Rotate your API credentials immediately** - They may have been exposed
2. Remove credentials from git history (see below)
3. Force push the cleaned history to GitHub

## Quick Start

### Option 1: Using the PowerShell Script (Recommended for Windows)

```powershell
# First, commit or stash your current changes
git add .
git commit -m "Remove User ID from reports and update gitignore"

# Test what will be removed (dry run)
.\remove-credentials-from-history.ps1 -DryRun

# Actually remove credentials from history
.\remove-credentials-from-history.ps1
```

### Option 2: Using BFG Repo-Cleaner (Recommended - Faster & Safer)

BFG Repo-Cleaner is the recommended tool for cleaning git history. It's faster and safer than git filter-branch.

#### Installation

1. Download BFG from: https://rtyley.github.io/bfg-repo-cleaner/
2. Or use Chocolatey: `choco install bfg`

#### Usage

```powershell
# 1. Clone a fresh copy of your repo
cd ..
git clone --mirror https://github.com/yourusername/astro-api-client-v2.git repo-backup.git

# 2. Create a file with credentials to remove
@"
647584
bde5153706977d8c6a77aeac71205482ac9e3475
"@ | Out-File -Encoding utf8 passwords.txt

# 3. Remove credentials
java -jar bfg.jar --replace-text passwords.txt repo-backup.git

# 4. Clean up
cd repo-backup.git
git reflog expire --expire=now --all
git gc --prune=now --aggressive

# 5. Push the cleaned history
git push --force
```

### Option 3: Manual git filter-branch

```powershell
# Remove User ID from API_Availability_Report.md
git filter-branch --force --tree-filter `
    "if (Test-Path 'API_Availability_Report.md') { (Get-Content 'API_Availability_Report.md') -replace '647584', '[REMOVED]' | Set-Content 'API_Availability_Report.md' }" `
    --prune-empty --tag-name-filter cat -- --all

# Remove appsettings.json if it was committed
git filter-branch --force --index-filter `
    "git rm --cached --ignore-unmatch '**/appsettings.json'" `
    --prune-empty --tag-name-filter cat -- --all

# Clean up
git reflog expire --expire=now --all
git gc --prune=now --aggressive
```

## After Cleaning History

### 1. Verify Credentials Are Removed

```powershell
# Check if User ID still exists in history
git log --all -S "647584"

# Check if API Key exists in history
git log --all -S "bde5153706977d8c6a77aeac71205482ac9e3475"

# Both should return no results
```

### 2. Force Push to GitHub

**⚠️ WARNING: This overwrites remote history!**

```powershell
# Push all branches
git push --force --all

# Push all tags
git push --force --tags
```

### 3. Notify Team Members

All team members must:
- **Re-clone the repository**, OR
- Run these commands:
  ```powershell
  git fetch origin
  git reset --hard origin/master
  ```

### 4. Rotate API Credentials

**This is critical!** Even after removing from git history:
- The credentials may have been exposed if the repo was public
- Anyone who cloned before the cleanup has the credentials
- **Rotate your API credentials immediately** in your API provider's dashboard

## Prevention

To prevent this in the future:

1. ✅ **Never commit credentials** - Use environment variables or local config files
2. ✅ **Use `.gitignore`** - Already configured for `appsettings.json`
3. ✅ **Use pre-commit hooks** - Scan for secrets before committing
4. ✅ **Use secret scanning tools** - GitHub has built-in secret scanning

## Current Status

- ✅ `appsettings.json` is in `.gitignore` (will not be committed)
- ✅ `appsettings.example.json` is tracked (template file, no credentials)
- ✅ Report generation no longer includes User ID
- ⚠️ User ID still exists in git history (needs cleanup)
- ✅ API Key not found in git history (good!)

## Need Help?

If you encounter issues:
1. Check git documentation: https://git-scm.com/docs/git-filter-branch
2. BFG documentation: https://rtyley.github.io/bfg-repo-cleaner/
3. GitHub guide: https://docs.github.com/en/authentication/keeping-your-account-and-data-secure/removing-sensitive-data-from-a-repository

