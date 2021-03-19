param (
    [switch]$Build,

    [switch]$UnitTests,

    [switch]$CoverReport,
    [switch]$GenerateDocs,

    [switch]$Pack,
    [switch]$Publish
)

# Load Toolkit
. ".build\BuildToolkit.ps1"

# Initialize Toolkit
Invoke-Initialize -Version (Get-Content "VERSION");

if ($Build) {
    Invoke-Build ".\MoryxFactory.sln"
}

if ($UnitTests) {
    Invoke-CoverTests -SearchFilter "*.Tests.csproj"
}

if ($CoverReport) {
    Invoke-CoverReport
}

if ($GenerateDocs) {
    Invoke-DocFx
}

if ($Pack) {
    Invoke-PackAll
}

if ($Publish) {
    Invoke-Publish
}

Write-Host "Success!"