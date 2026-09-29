param(
    [Parameter(Position = 0)][string]$Command,
    [Parameter(Position = 1)][string]$Target
)

$dbPath     = Join-Path $PSScriptRoot "TestData\testdb.sqlite"
$reportJs   = Join-Path $PSScriptRoot "Scripts\generate-report.js"
$dashboard  = Join-Path $PSScriptRoot "dashboard\index.html"

function Invoke-Test([string]$Filter = "") {
    $dotnetArgs = @('test', '--logger', 'junit;LogFileName=TestResults.xml')
    if ($Filter) { $dotnetArgs += @('--filter', $Filter) }

    Write-Host "> dotnet $($dotnetArgs -join ' ')" -ForegroundColor Cyan
    dotnet @dotnetArgs
    $exitCode = $LASTEXITCODE

    node Scripts/generate-report.js
    exit $exitCode
}

function Require-Target([string]$usage) {
    if (-not $Target) {
        Write-Host "Missing argument. Usage: $usage" -ForegroundColor Yellow
        exit 1
    }
}

switch ($Command) {
    "test"        { Invoke-Test }
    "test-api"    { Invoke-Test "FullyQualifiedName~.API." }
    "test-ui"     { Invoke-Test "FullyQualifiedName~.UI." }
    "test-db"     { Invoke-Test "FullyQualifiedName~.Database." }
    "test-one"    { Require-Target ".\run.ps1 test-one SuccessfulLogin";  Invoke-Test "Name=$Target" }
    "test-class"  { Require-Target ".\run.ps1 test-class LoginTests";     Invoke-Test "FullyQualifiedName~.$Target." }
    "test-filter" { Require-Target '.\run.ps1 test-filter "Name=A|Name=B"'; Invoke-Test $Target }
    "db"          {
                    if (Test-Path $dbPath) {
                        $file = Get-Item $dbPath
                        Write-Host "DB found: $($file.FullName)" -ForegroundColor Green
                        Write-Host "  Size: $([math]::Round($file.Length / 1KB, 1)) KB   Last modified: $($file.LastWriteTime)"
                        code $dbPath
                    } 
                    else {
                        Write-Host "No DB file yet at $dbPath" -ForegroundColor Yellow
                        Write-Host "Run a DB test first, e.g. .\run.ps1 test-db"
                    }
                  }
    
    "list"        { dotnet test --list-tests }
    "report"      { node $reportJs }
    "clean"       { dotnet clean }
    "build"       { dotnet build }
    "open"        { Invoke-Item $dashboard }
    "rebuild"     { dotnet clean; dotnet build }
    
    default {
        Write-Host "Usage: .\run.ps1 <command> [target]`n"
        Write-Host "Run tests:"
        Write-Host "  test                  Run all tests + report"
        Write-Host "  test-api              Run API tests + report"
        Write-Host "  test-ui               Run UI tests + report"
        Write-Host "  test-db               Run DB tests + report"
        Write-Host "  test-one <TestName>   Run a single test method"
        Write-Host "  test-class <Class>    Run every test in one class"
        Write-Host "  test-filter <filter>  Run with a raw dotnet filter"
        Write-Host "  list                  List all test names"
        Write-Host "`nResults:"
        Write-Host "  report                Rebuild report from last run"
        Write-Host "  open                  Open the dashboard in your browser"
        Write-Host "  db                    Check for the test DB and open it"
        Write-Host "`nBuild:"
        Write-Host "  clean | build | rebuild"
    }
}