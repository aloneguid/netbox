$BuildNo = $env:APPVEYOR_BUILD_NUMBER
$Major = 2
$Minor = 0
$Patch = 1
$IsPrerelease = $true

# latest released version: 2.0.1

if($BuildNo -eq $null)
{
   $BuildNo = "1"
}

Invoke-Expression "appveyor UpdateBuild -Version $Major.$Minor.$Patch-$BuildNo"


$Copyright = "Copyright (c) 2015-2018 by Ivan Gavryliuk"
$PackageIconUrl = "http://i.isolineltd.com/nuget/netbox.png"
$PackageProjectUrl = "https://github.com/aloneguid/support"
$RepositoryUrl = "https://github.com/aloneguid/support"
$Authors = "Ivan Gavryliuk (@aloneguid)"
$PackageLicenseUrl = "https://github.com/aloneguid/support/blob/master/LICENSE"
$RepositoryType = "GitHub"
$SlnPath = "src/netbox.sln"

function Update-ProjectVersion($File)
{
   Write-Host "updating $File ..."

   $over = $vt.($File.Name)
   if($over -eq $null) {
      $thisMajor = $Major
      $thisMinor = $Minor
      $thisPatch = $Patch
   } else {
      $thisMajor = $over[0]
      $thisMinor = $over[1]
      $thisPatch = $over[2]
   }

   $xml = [xml](Get-Content $File.FullName)

   if($xml.Project.PropertyGroup.Count -eq $null)
   {
      $pg = $xml.Project.PropertyGroup
   }
   else
   {
      $pg = $xml.Project.PropertyGroup[0]
   }

   if($IsPrerelease) {
      $suffix = "-ci-" + $BuildNo.PadLeft(5, '0')
   } else {
      $suffix = ""
   }

   
   [string] $fv = "{0}.{1}.{2}.{3}" -f $thisMajor, $thisMinor, $thisPatch, $BuildNo
   [string] $av = "{0}.0.0.0" -f $thisMajor
   [string] $pv = "{0}.{1}.{2}{3}" -f $thisMajor, $thisMinor, $thisPatch, $suffix

   $pg.Version = $pv
   $pg.FileVersion = $fv
   $pg.AssemblyVersion = $av

   Write-Host "$($File.Name) => fv: $fv, av: $av, pkg: $pv"

   $pg.Copyright = $Copyright
   $pg.PackageIconUrl = $PackageIconUrl
   $pg.PackageProjectUrl = $PackageProjectUrl
   $pg.RepositoryUrl = $RepositoryUrl
   $pg.Authors = $Authors
   $pg.PackageLicenseUrl = $PackageLicenseUrl
   $pg.RepositoryType = $RepositoryType

   $xml.Save($File.FullName)
}

function Exec($Command, [switch]$ContinueOnError)
{
   Invoke-Expression $Command
   if($LASTEXITCODE -ne 0)
   {
      Write-Error "command failed (error code: $LASTEXITCODE)"

      if(-not $ContinueOnError.IsPresent)
      {
         exit 1
      }
   }
}

# Update versioning information
Get-ChildItem *.csproj -Recurse | Where-Object {-not(($_.Name -like "*test*") -or ($_.Name -like "*console*")) } | % {
   Update-ProjectVersion $_
}

# Restore packages
Exec "dotnet restore $SlnPath"