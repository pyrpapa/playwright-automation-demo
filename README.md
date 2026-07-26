# Playwright Automation Demo

[![Playwright Tests](https://github.com/pyrpapa/playwright-automation-demo/actions/workflows/tests.yml/badge.svg)](https://github.com/pyrpapa/playwright-automation-demo/actions/workflows/tests.yml)

**Live dashboard:** [pyrpapa.github.io/playwright-automation-demo](https://pyrpapa.github.io/playwright-automation-demo/) — rebuilt automatically on every push to `main`.

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

```
playwright-automation-demo/
├── .github/workflows/
│   └── tests.yml    # CI pipeline
├── Config/          # Test configuration (URLs, credentials)
├── Helpers/         # Database helper
├── Models/          # API response models (Post, Comment)
├── Pages/           # Page Object Models for UI tests
├── Scripts/
│   └── generate-report.js  # builds the HTML dashboard from JUnit results
├── dashboard/       # generated dashboard (history.json is committed to gh-pages by CI)
├── Tests/
│   ├── API/         # Happy path and negative API tests
│   ├── UI/          # UI tests (login, checkbox, file upload, auth)
│   └── DatabaseTests.cs
└── README.md
```

## Setup
1. Clone the repo
2. Install dependencies
```powershell
dotnet restore
```
3. Install Playwright browsers (run `dotnet build` first so `playwright.ps1` exists)
```powershell
pwsh bin/Debug/net10.0/playwright.ps1 install
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

## CI/CD Pipeline

The GitHub Actions workflow (`.github/workflows/tests.yml`) runs automatically on every push to `main` and on every pull request targeting `main` (plus manual triggers via `workflow_dispatch`):

1. Restores dependencies, builds, and installs Playwright browsers
2. Pulls the previous run's `history.json` down from `gh-pages` (if any), so the dashboard's pass/fail trend chart spans runs instead of resetting every time
3. Runs the full test suite (`dotnet test`) with the JUnit logger
4. Builds the consolidated HTML dashboard via `Scripts/generate-report.js`
5. Uploads the dashboard as a workflow artifact (retained 30 days)
6. On pushes to `main`, publishes the dashboard to GitHub Pages at [pyrpapa.github.io/playwright-automation-demo](https://pyrpapa.github.io/playwright-automation-demo/), so that link always reflects the most recent run
7. Fails the workflow (and badge) if any test failed, even though the dashboard step still runs so failures are visible on the live page

**One-time setup required:** GitHub Pages must be enabled for this repo before the live link works — `Settings → Pages → Source: Deploy from a branch → gh-pages → / (root)`. The `gh-pages` branch is created automatically the first time the workflow runs on `main`.
