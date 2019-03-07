$versionFileName = "Version.txt"

# Get last version from file and bump
$currentVersion = Get-Content -Path $versionFileName -Raw
$versionElements = $currentVersion.Split('.')
$versionElements[2] = [int]$versionElements[2] + 1
$newVersion = $versionElements -join "."

# Get list of .csproj file paths that contain a version tag
$files = Get-ChildItem -Recurse "*.csproj" | Where-Object { (Select-String -InputObject $_ -Pattern '<Version>' -Quiet) -eq $true } | Select -expand FullName

# Replace version tag with new version
foreach ($file in $files)
{
	(Get-Content -Path $file) -replace '<Version>.*<\/Version>', "<Version>$newVersion</Version>" | Set-Content $file 
}

# Update version file
Set-Content -Path $versionFileName -Value $newVersion