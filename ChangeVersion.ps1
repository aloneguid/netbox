param(
   [String]
   $Version
)

Write-Host "version is $Version"

function Get-Json($RelPath)
{
   $path = "$PSScriptRoot\$RelPath"
   Get-Content $path | ConvertFrom-Json
}

function Set-Json($Json, $RelPath)
{
   $path = "$PSScriptRoot\$RelPath"
   $content = $Json | ConvertTo-Json -Depth 100
   Write-Host $content
   $content | Set-Content -Path $path
}

$jsonMain = Get-Json "src\NetBox\project.json"
$jsonTests = Get-Json "src\NetBox.Tests\project.json"

$jsonMain.version = $Version
$jsonTests.dependencies.NetBox = $Version

Set-Json $jsonMain "src\NetBox\project.json"
Set-Json $jsonTests "src\NetBox.Tests\project.json"

