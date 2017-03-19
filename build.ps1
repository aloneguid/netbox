### Build Script

param(
   [switch]
   $Publish,

   [string]
   $NuGetApiKey
)

$VersionAbsolute = "1.1.2-alpha"
$VersionMajor = 1
$VersionMinor = 0
$SlnPath = "src\netbox.sln"

function Get-VersionByDate
{
   # important - max versio number is 65534
   $date = Get-Date
   "{0}.{1}.{2:D2}{3:D2}.{4:D2}{5:D2}" -f $VersionMajor, $VersionMinor, ($date.Year - 2000), $date.Month, $date.Day, $date.Hour
}

function Set-VstsBuildNumber($BuildNumber)
{
   Write-Verbose -Verbose "##vso[build.updatebuildnumber]$BuildNumber"
}

function Update-ProjectVersion([string]$RelPath, [string]$Version)
{
   $xml = [xml](Get-Content "$PSScriptRoot\$RelPath")

   if($xml.Project.PropertyGroup.Count -eq $null)
   {
      $xml.Project.PropertyGroup.VersionPrefix = $Version
   }
   else
   {
      $xml.Project.PropertyGroup[0].VersionPrefix = $Version
   }

   $xml.Save("$PSScriptRoot\$RelPath")
}

function Exec($Command)
{
   Invoke-Expression $Command
   if($LASTEXITCODE -ne 0)
   {
      Write-Error "command failed (error code: $LASTEXITCODE)"
      exit 1
   }
}

### Start the build

# General validation
if($Publish -and (-not $NuGetApiKey))
{
   Write-Error "Please specify nuget key to publish"
   exit 1
}

# Determine the version
#$ver = Get-Version
$ver = $VersionAbsolute
Write-Host "version is $ver"
Set-VstsBuildNumber $ver

# Set the project version
Update-ProjectVersion "src\NetBox\netbox.csproj" $ver

# Restore packages
Exec "dotnet restore $SlnPath"

# Build solution
Remove-Item ".\src\NetBox\bin\Release\*.nupkg"
Exec "dotnet build $SlnPath -c release"

# Run the tests
Exec "dotnet test src\NetBox.Tests\NetBox.Tests.csproj"

# publish the nugets
if($Publish.IsPresent)
{
   Write-Host "publishing nugets..."

   Exec "nuget push .\src\NetBox\bin\Release\NetBox.$ver.nupkg -Source https://www.nuget.org/api/v2/package -ApiKey $NuGetApiKey"
}

Write-Host "build succeeded."