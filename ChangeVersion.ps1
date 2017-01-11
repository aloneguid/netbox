param(
   [String]
   $Version
)

Write-Host "version set to $Version"

Write-Host "changing core version..."
$path = "$PSScriptRoot\src\NetBox\project.json"
$json = Get-Content $path | ConvertFrom-Json
$json.version = $Version
$json | ConvertTo-Json -Depth 100 | Set-Content -Path $path

Write-Host "changing dependencies versions..."
$path = "$PSScriptRoot\src\NetBox.Tests\project.json"
$json = Get-Content $path | ConvertFrom-Json
$json.dependencies.NetBox = $Version
$json | ConvertTo-Json -Depth 100 | Set-Content -Path $path

