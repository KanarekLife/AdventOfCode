<#
    name: KanarekLife's Advent of Code project generator
    author: Stanisław Nieradko <stanislaw@nieradko.com>
#>

param(
    [int] $year = 2022,
    [Parameter(Mandatory=$true)]
    [int] $day
)

$dir = "$PSScriptRoot/../$year/Day$day"

# Create c# project
dotnet.exe new console --use-program-main --no-restore -n AdventOfCode -o "$dir" | Out-Null

# Create input files
New-Item "$dir/example_input.txt" | Out-Null
New-Item "$dir/input.txt" | Out-Null

# Modify .csproj file to copy input files during build
[xml]$xml = Get-Content "$dir/AdventOfCode.csproj"
$itemGroup = $xml.CreateElement("ItemGroup")
$itemGroup.InnerXml = @"
<None Update="example_input.txt">
      <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    </None>
    <None Update="input.txt">
      <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    </None>
  
"@
$xml.Project.AppendChild($itemGroup) | Out-Null
$xml.Save("$dir/AdventOfCode.csproj")

# Change current location to directory
Set-Location "$dir"