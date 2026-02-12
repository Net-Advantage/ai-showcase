# NZ PAYE Calculator

A web-based calculator for New Zealand PAYE (Pay As You Earn) tax, KiwiSaver contributions, and ACC levy calculations. Built with .NET 10 and .NET Aspire.

## Features

- **PAYE Tax Calculation**: Progressive tax calculation using 2024/2025 NZ tax rates (5 tax brackets)
- **KiwiSaver**: 3% employee contribution calculation
- **ACC Levy**: 1.53% ACC Earners' Levy (capped at $139,384)
- **Monthly Breakdown**: Shows monthly amounts for all deductions and take-home pay
- **Annual Breakdown**: Expandable section showing annual totals
- **Modern UI**: Clean, developer-focused design inspired by aspire.dev
- **Responsive**: Works on desktop, tablet, and mobile devices
- **Accessible**: WCAG AA compliant with keyboard navigation and screen reader support

## Tech Stack

- **.NET 10.0**
- **ASP.NET Core Minimal APIs** (Backend)
- **Blazor Server** (Frontend)
- **.NET Aspire** (Orchestration & Service Discovery)
- **C# 13**

## Quick Start

### Prerequisites

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download) or later
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (for .NET Aspire dashboard)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/) with C# Dev Kit

### Running the Application

1. **Run with .NET Aspire**
   ```bash
   cd src/NzPayeCalc
   dotnet run --project NzPayeCalc.AppHost
   ```

2. **Access the application**
   - The .NET Aspire dashboard will open automatically
   - Click on the **nzpayecalc-web** endpoint to open the calculator

### Running Tests

```bash
cd src/NzPayeCalc
dotnet test
```

## Project Structure

```
src/NzPayeCalc/
├── NzPayeCalc.AppHost/            # .NET Aspire orchestration
├── NzPayeCalc.ServiceDefaults/    # Shared service configuration
├── NzPayeCalc.ApiService/         # Backend API
│   ├── Models/                    # Request/response DTOs
│   ├── Services/                  # Calculation business logic
│   └── Program.cs                 # API endpoints
├── NzPayeCalc.Web/                # Frontend Blazor app
│   ├── Components/Pages/          # Blazor pages
│   ├── Models/                    # Frontend models
│   ├── Services/                  # API client
│   └── wwwroot/app.css            # Design system styles
└── NzPayeCalc.Tests/              # Unit and integration tests
```

## How It Works

### Calculation Logic

#### PAYE Tax (Progressive Tax Brackets - 2024/2025)
- $0 - $14,000: 10.5%
- $14,001 - $48,000: 17.5%
- $48,001 - $70,000: 30%
- $70,001 - $180,000: 33%
- Over $180,000: 39%

#### KiwiSaver
- Fixed 3% of gross salary

#### ACC Earners' Levy
- 1.53% of gross salary
- Capped at maximum earnings threshold of $139,384

## Example Calculations

### Example: $60,000 Annual Salary
- **Monthly Gross**: $5,000.00
- **Monthly PAYE**: $858.33
- **Monthly KiwiSaver**: $150.00
- **Monthly ACC**: $76.50
- **Monthly Take-Home**: $3,915.17

## Limitations

This calculator uses 2024/2025 NZ tax rates and does NOT include:
- Student loan repayments
- Additional voluntary KiwiSaver contributions
- Employer contributions
- Other deductions

**This is an estimate only** and should not be used for official tax purposes.

## License

This project is licensed under the MIT License.

---

**Note**: Tax rates and ACC levies are subject to change each tax year. Always verify with [Inland Revenue](https://www.ird.govt.nz/) for official rates.