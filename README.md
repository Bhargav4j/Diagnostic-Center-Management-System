# Diagnostic Center Management System - .NET 8

This is a modernized version of the Diagnostic Center application, migrated from ASP.NET Web Forms to .NET 8 using clean architecture principles.

## Project Structure

- **src/DiagnosticCenter.Domain** - Domain entities, interfaces, and business rules
- **src/DiagnosticCenter.Application** - Business logic and services
- **src/DiagnosticCenter.Infrastructure** - Data access, EF Core, and repositories
- **src/DiagnosticCenter.Web** - ASP.NET Core Razor Pages UI
- **tests/DiagnosticCenter.UnitTests** - Unit tests
- **tests/DiagnosticCenter.IntegrationTests** - Integration tests

## Features

- Test Type Management
- Test Setup Management
- Test Entry (Patient Registration)
- Payment Processing
- Report Generation
- User Authentication with ASP.NET Core Identity

## Prerequisites

- .NET 8 SDK
- SQL Server (LocalDB, Express, or Full)
- Visual Studio 2022 or VS Code

## Setup Instructions

### 1. Database Configuration

Update the connection string in `src/DiagnosticCenter.Web/appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=diagnostic_center_project;Integrated Security=true;TrustServerCertificate=true"
}
```

### 2. Create Database

Run EF Core migrations:

```bash
cd src/DiagnosticCenter.Web
dotnet ef database update --project ../DiagnosticCenter.Infrastructure
```

### 3. Build the Solution

```bash
dotnet build
```

### 4. Run the Application

```bash
cd src/DiagnosticCenter.Web
dotnet run
```

The application will be available at `https://localhost:5001` or `http://localhost:5000`.

## Architecture

This application follows Clean Architecture principles:

- **Domain Layer**: Core business entities and interfaces
- **Application Layer**: Business logic implementation
- **Infrastructure Layer**: Data access with EF Core
- **Web Layer**: Razor Pages UI

## Key Technologies

- **.NET 8**
- **ASP.NET Core Razor Pages**
- **Entity Framework Core 8.0**
- **ASP.NET Core Identity** for authentication
- **Serilog** for logging
- **xUnit** for testing
- **SQL Server**

## Migration Notes

This application was migrated from ASP.NET Web Forms 4.5.2 to .NET 8. Key changes include:

- Replaced Web Forms pages with Razor Pages
- Migrated ADO.NET to Entity Framework Core
- Replaced Forms Authentication with ASP.NET Core Identity
- Converted Web.config to appsettings.json
- Implemented async/await throughout
- Added proper error handling and logging
- Implemented repository and service patterns

## License

This project is for educational purposes.
