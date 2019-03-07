$versionFileName = "Version.txt"

# Get last version from file and bump
$currentVersion = Get-Content -Path $versionFileName -Raw
$versionElements = $currentVersion.Split('.')
$versionElements[2] = [int]$versionElements[2] + 1
$newVersion = $versionElements -join "."

# Update version file
Set-Content -Path $versionFileName -Value $newVersion