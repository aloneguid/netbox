### Build Script

$gv = $env:APPVEYOR_BUILD_VERSION
if($gv -eq $null)
{
   $gv = "1.4.6"
}


$Copyright = "Copyright (c) 2015-2017 by Ivan Gavryliuk"
$PackageIconUrl = "http://i.isolineltd.com/nuget/netbox.png"
$PackageProjectUrl = "https://github.com/aloneguid/support"
$RepositoryUrl = "https://github.com/aloneguid/support"
$Authors = "Ivan Gavryliuk (@aloneguid)"
$PackageLicenseUrl = "https://github.com/aloneguid/support/blob/master/LICENSE"
$RepositoryType = "GitHub"
$SlnPath = "src/netbox.sln"

function Update-ProjectVersion($File)
{
   $v = $vt.($File.Name)
   if($v -eq $null) { $v = $gv }

   $xml = [xml](Get-Content $File.FullName)

   if($xml.Project.PropertyGroup.Count -eq $null)
   {
      $pg = $xml.Project.PropertyGroup
   }
   else
   {
      $pg = $xml.Project.PropertyGroup[0]
   }

   $parts = $v -split "\."
   $bv = $parts[2]
   if($bv.Contains("-")) { $bv = $bv.Substring(0, $bv.IndexOf("-"))}
   $fv = "{0}.{1}.{2}.0" -f $parts[0], $parts[1], $bv
   $av = "{0}.0.0.0" -f $parts[0]
   $pv = $v

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


Get-ChildItem *.csproj -Recurse | Where-Object {-not($_.Name -like "*test*") -and -not($_.Name -like "*console*")} | % {
   Write-Host "setting version on $($_.FullName)"
   Update-ProjectVersion $_
}

# Restore packages
Exec "dotnet restore $SlnPath"