# Diagnostic Center - .NET 8 Application

## Overview
A modern diagnostic center management system built with .NET 8, following clean architecture principles.

## Project Structure
```
DiagnosticCenter/
├── src/
│   ├── DiagnosticCenter.Domain/          # Domain entities and interfaces
│   ├── DiagnosticCenter.Application/     # Business logic and services
│   ├── DiagnosticCenter.Infrastructure/  # Data access and EF Core
│   └── DiagnosticCenter.Web/             # ASP.NET Core Razor Pages
└── tests/
    └── DiagnosticCenter.UnitTests/       # Unit tests
```

## Technologies
- .NET 8
- ASP.NET Core Razor Pages
- Entity Framework Core 8.0
- ASP.NET Core Identity
- Serilog for logging
- SQL Server
- xUnit for testing

## Features
- Test Type Management
- Test Setup Configuration
- Patient Test Entry
- Payment Processing
- Role-based authentication (Admin, Receptionist, Accountant)
- Comprehensive logging
- Clean architecture

## Setup Instructions

### Prerequisites
- .NET 8 SDK
- SQL Server (LocalDB or full installation)

### Database Configuration
Update the connection string in `src/DiagnosticCenter.Web/appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=diagnostic_center_project;Trusted_Connection=true;TrustServerCertificate=true"
}
```

### Run Migrations
```bash
cd src/DiagnosticCenter.Web
dotnet ef migrations add InitialCreate --project ../DiagnosticCenter.Infrastructure
dotnet ef database update
```

### Run Application
```bash
cd src/DiagnosticCenter.Web
dotnet run
```

### Run Tests
```bash
dotnet test
```

## Migration from ASP.NET Web Forms
This application was migrated from ASP.NET Web Forms 4.5.2 to .NET 8. Key changes:
- Replaced Web Forms pages with Razor Pages
- Migrated ADO.NET to Entity Framework Core
- Implemented ASP.NET Core Identity for authentication
- Applied clean architecture principles
- Added comprehensive logging
- Replaced Session state with Identity-based authentication
- Fixed SQL injection vulnerabilities with parameterized queries
