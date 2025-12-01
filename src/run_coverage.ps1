$ErrorActionPreference = "Stop"

Write-Host "Cleaning previous results..." -ForegroundColor Yellow
Remove-Item -Path ./TestResults -Recurse -Force -ErrorAction SilentlyContinue
Remove-Item -Path ./CoverageReport -Recurse -Force -ErrorAction SilentlyContinue

Write-Host "Running Tests with Coverage..." -ForegroundColor Cyan
dotnet test ProjInv.Tests/ProjInv.Tests.csproj --collect:"XPlat Code Coverage" --settings coverlet.runsettings --results-directory ./TestResults

$reportGeneratorInstalled = Get-Command reportgenerator -ErrorAction SilentlyContinue
if ($null -eq $reportGeneratorInstalled) {
    Write-Host "Installing ReportGenerator tool..." -ForegroundColor Yellow
    dotnet tool install -g dotnet-reportgenerator-globaltool
}

$latestCoverageFile = Get-ChildItem -Path ./TestResults -Recurse -Filter "coverage.cobertura.xml" | Sort-Object LastWriteTime -Descending | Select-Object -First 1

if ($latestCoverageFile) {
    Write-Host "Generating HTML Report..." -ForegroundColor Cyan
    reportgenerator -reports:$latestCoverageFile.FullName -targetdir:./CoverageReport -reporttypes:Html -assemblyfilters:"-ProjInv.Tests" -classfilters:"-Program;-*Migrations*;-*DTO*;-*Dto*;-*Exception*;-*AppDbContext*;-*TransactionType*" -filefilters:"-*Migrations*;-*Program.cs*;-*Designer.cs*;-*DTOs*"
    
    Write-Host "Opening Report..." -ForegroundColor Green
    Start-Process "./CoverageReport/index.html"
} else {
    Write-Host "No coverage file found!" -ForegroundColor Red
}
