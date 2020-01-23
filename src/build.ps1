$Major = 0
$Minor = 4
$Patch = 1

if($BuildNo -eq $null)
{
   $BuildNo = "1"
}

$Copyright = "Copyright (c) 2018-2020 by Ivan Gavryliuk"
$PackageIconUrl = "http://i.isolineltd.com/nuget/netbox.png"
$PackageProjectUrl = "https://github.com/aloneguid/support"
$RepositoryUrl = "https://github.com/aloneguid/support"
$Authors = "Ivan Gavryliuk (@aloneguid)"
$PackageLicenseUrl = "https://github.com/aloneguid/support/blob/master/LICENSE"
$RepositoryType = "GitHub"

$SlnPath = "src\netbox.sln"

function Update-ProjectVersion($File)
{
   Write-Host "updating $File ..."

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

   
   [string] $fv = "{0}.{1}.{2}.{3}" -f $Major, $Minor, $Patch, $BuildNo
   [string] $av = "{0}.0.0.0" -f $Major
   [string] $pv = "{0}.{1}.{2}{3}" -f $Major, $Minor, $Patch, $suffix

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

function Get-DisplayVersion()
{
   $v = "$Major.$Minor.$Patch"
   
   if($IsPrerelease)
   {
      $v = "$v-ci-$BuildNo"
   }
   else
   {
      $v = "$v.$BuildNo"
   }

   $v
}

$VDisplay = Get-DisplayVersion
Invoke-Expression "appveyor UpdateBuild -Version $VDisplay"

# Update versioning information
Get-ChildItem *.csproj -Recurse | Where-Object {-not(($_.Name -like "*test*") -or ($_.Name -like "*Core*") -or ($_.Name -like "*input*")) } | % {
   Update-ProjectVersion $_
}

# Restore packages
Exec "dotnet restore $SlnPath"