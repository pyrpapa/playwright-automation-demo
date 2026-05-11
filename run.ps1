param(
    [string]$command
)

switch ($command) {
    "test"     { dotnet test }
    "test-api" { dotnet test --filter "FullyQualifiedName~API" }
    "test-ui"  { dotnet test --filter "FullyQualifiedName~UI" }
    "test-db"  { dotnet test --filter "FullyQualifiedName~Database" }
    "clean"    { dotnet clean }
    "build"    { dotnet build }
    "rebuild"  { dotnet clean; dotnet build }
    default    { Write-Host "Available commands: test, test-api, test-ui, test-db, clean, build, rebuild" }
}

