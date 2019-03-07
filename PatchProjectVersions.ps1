$versionFileName = "Version.txt"

# Get version from file
$currentVersion = Get-Content -Path $versionFileName -Raw

# Get list of .csproj file paths that contain a version tag
$files = Get-ChildItem -Recurse "*.csproj" | Where-Object { (Select-String -InputObject $_ -Pattern '<Version>' -Quiet) -eq $true } | Select -expand FullName

# Replace version tag with committed version
foreach ($file in $files)
{
	(Get-Content -Path $file) -replace '<Version>.*<\/Version>', "<Version>$currentVersion</Version>" | Set-Content $file 
}