param(
    [string]$command
)

$logger = '--logger "junit;LogFileName=Test-Results.xml"'

function Invoke-Test([string]$filter = "") {
    if ($filter) {
        Invoke-Expression "dotnet test --filter `"$filter`" $logger"
    } else {
        Invoke-Expression "dotnet test $logger"
    }
    node scripts/generate-report.js
}

switch ($command) {
    "test"        { Invoke-Test }
    "test-api"    { Invoke-Test "FullyQualifiedName~API" }
    "test-ui"     { Invoke-Test "FullyQualifiedName~UI" }
    "test-db"     { Invoke-Test "FullyQualifiedName~Database" }
    "report"      { node scripts/generate-report.js }
    "clean"       { dotnet clean }
    "build"       { dotnet build }
    "rebuild"     { dotnet clean; dotnet build }
    default {
        Write-Host "Available commands:"
        Write-Host "  test          Run all tests + generate report"
        Write-Host "  test-api      Run API tests + generate report"
        Write-Host "  test-ui       Run UI tests + generate report"
        Write-Host "  test-db       Run DB tests + generate report"
        Write-Host "  report        Generate report from last test run"
        Write-Host "  clean         Clean build output"
        Write-Host "  build         Build project"
        Write-Host "  rebuild       Clean then build"
    }
}