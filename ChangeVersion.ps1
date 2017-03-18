param(
   [String]
   $Version
)

Write-Host "version is $Version"

function Update-ProjectVersions([string]$RelPath, [string]$Version, [bool]$UpdatePackageVersion)
{
   $xml = [xml](Get-Content "$PSScriptRoot\$RelPath")

   if($UpdatePackageVersion)
   {
      if($xml.Project.PropertyGroup.Count -eq $null)
      {
         $xml.Project.PropertyGroup.VersionPrefix = $Version
      }
      else
      {
         $xml.Project.PropertyGroup[0].VersionPrefix = $Version
      }
   }

   foreach($other in $args)
   {
      $json.dependencies.$other = $Version

      Write-Host "set $other to $Version"
   }

   $xml.Save("$PSScriptRoot\$RelPath")
}

Update-ProjectVersions "src\NetBox\netbox.csproj" $Version $true