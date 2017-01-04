# Ran manually to regenerate markdown documentation pages
# modules are taken from https://github.com/aloneguid/powershell

Import-Module "C:\dev\powershell\Markdown"

$xmlDocs = "$PSScriptRoot\NetBox\bin\Debug\net45\NetBox.xml"
$out = "$PSScriptRoot\..\doc\reference.md"

$xmlDocs
$out

Convert-XmlDocToMarkdown -XmlDocPath $xmlDocs -OutPath $out