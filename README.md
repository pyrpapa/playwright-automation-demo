# Playwright Automation Demo

A multi-layered test automation framework demonstrating UI testing, 
API testing, and database validation using Playwright, Dapper, and SQLite.

## Tech Stack
- **C# / .NET 10**
- **Playwright** — UI and API testing
- **NUnit** — test framework
- **FluentAssertions** — readable assertions
- **Newtonsoft.Json** — JSON deserialization
- **Dapper** — database queries
- **SQLite** — lightweight test database
- **GitHub Actions** — CI/CD pipeline

## Project Structure
playwright-automation-demo/
├── Config/          # Test configuration (URLs, credentials)
├── Helpers/         # Database helper
├── Models/          # API response models (Post, Comment)
├── Pages/           # Page Object Models for UI tests
├── Tests/
│   ├── API/         # Happy path and negative API tests
│   ├── UI/          # UI tests (login, checkbox, file upload, auth)
│   └── DatabaseTests.cs
└── README.md

## Setup
1. Clone the repo
2. Install dependencies
```powershell
dotnet restore
```
3. Install Playwright browsers
```powershell
pwsh bin/Debug/net10.0/.playwright/package/bin/playwright.ps1 install
```

## Running Tests
```powershell
# All tests
dotnet test

# API tests only
dotnet test --filter "Namespace~API"

# UI tests only
dotnet test --filter "Namespace~UI"

# Database tests only
dotnet test --filter "ClassName=DatabaseTests"

# Specific test
dotnet test --filter "Name=CRUD_Post"

# Clean and rebuild
dotnet clean && dotnet build
```
## Using the Run Script
A PowerShell helper script is included to simplify common commands.

```powershell
# Run all tests
.\run.ps1 test

# Run API tests only
.\run.ps1 test-api

# Run UI tests only
.\run.ps1 test-ui

# Run database tests only
.\run.ps1 test-db

# Clean build artifacts
.\run.ps1 clean

# Build project
.\run.ps1 build

# Clean and rebuild
.\run.ps1 rebuild
```

> **Note:** If you get a script execution error, run this first:
> ```powershell
> Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser
> ```

## Test Coverage
### API Tests
- CRUD operations against JSONPlaceholder
- Schema validation using typed models
- Negative testing (404, 500 scenarios)

### UI Tests
- Login (success and failure)
- Checkbox interactions
- File upload (success and failure)
- Basic authentication

### Database Tests
- SQLite database via Dapper
- Create, read, and delete validation
- Negative scenarios (non-existent records)