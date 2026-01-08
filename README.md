# DiagnosticCenter - .NET 8 Application

## Overview

DiagnosticCenter is a modern medical diagnostic center management system built on .NET 8 using Clean Architecture principles. The application has been successfully migrated from ASP.NET Web Forms 4.5.2 to .NET 8 with ASP.NET Core Razor Pages.

## Architecture

The solution follows Clean Architecture with the following layers:

### Domain Layer (`DiagnosticCenter.Domain`)
- **Entities**: Core business entities (TestType, TestSetup, TestEntry, Payment, User)
- **Interfaces**: Repository and service contracts
- **No external dependencies**

### Application Layer (`DiagnosticCenter.Application`)
- **Services**: Business logic implementation
- **DTOs**: Data transfer objects for communication between layers
- **Mappings**: AutoMapper profiles for entity-DTO conversions
- **Validation**: FluentValidation rules

### Infrastructure Layer (`DiagnosticCenter.Infrastructure`)
- **Data Access**: Entity Framework Core 8 DbContext
- **Repositories**: Concrete repository implementations
- **Configurations**: Entity type configurations
- **Database Migrations**: EF Core migrations

### Web Layer (`DiagnosticCenter.Web`)
- **Razor Pages**: Modern page-based UI
- **Authentication**: Cookie-based authentication
- **Session Management**: Distributed session state
- **Logging**: Serilog integration

### Test Projects
- **DiagnosticCenter.UnitTests**: Unit tests using xUnit, Moq, and FluentAssertions
- **DiagnosticCenter.IntegrationTests**: Integration tests with in-memory database

## Prerequisites

- .NET 8 SDK
- SQL Server (LocalDB, Express, or full version)
- Visual Studio 2022 or VS Code (optional)

## Getting Started

### 1. Update Database Connection String

Edit `src/DiagnosticCenter.Web/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=DiagnosticCenterDb;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=true"
  }
}
```

### 2. Apply Database Migrations

```bash
cd src/DiagnosticCenter.Web
dotnet ef database update
```

Or run the application - it will auto-apply migrations on startup.

### 3. Run the Application

```bash
cd src/DiagnosticCenter.Web
dotnet run
```

The application will be available at `https://localhost:5001` or `http://localhost:5000`.

### 4. Create Initial User

Since this is a fresh migration, you'll need to create a user in the database manually:

```sql
INSERT INTO Users (Email, PasswordHash, AccountType, IsActive, CreatedDate, CreatedBy)
VALUES
('admin@diagnostics.com', '$2a$12$LQv3c1yqBWVHxkd0LHAkCOYz6TtxMQJqhN8/LewY5zyMdFd7z4QLW', 'Admin', 1, GETDATE(), 'System'),
('receptionist@diagnostics.com', '$2a$12$LQv3c1yqBWVHxkd0LHAkCOYz6TtxMQJqhN8/LewY5zyMdFd7z4QLW', 'Receptionist', 1, GETDATE(), 'System'),
('accountant@diagnostics.com', '$2a$12$LQv3c1yqBWVHxkd0LHAkCOYz6TtxMQJqhN8/LewY5zyMdFd7z4QLW', 'Accountant', 1, GETDATE(), 'System');
```

Default password for all users: `Admin@123`

## Features

### TestType Management
- Create, Read, Update, Delete test types
- Search functionality
- Active/Inactive status management

### TestSetup Management
- Manage diagnostic tests with fees
- Link tests to test types
- Fee management

### TestEntry Management
- Patient registration
- Test request management
- Payment tracking
- Due date management

### Payment Management
- Record payments against test entries
- Automatic balance calculation
- Payment history

### Authentication & Authorization
- Cookie-based authentication
- Session management
- Role-based access (Admin, Receptionist, Accountant)

## Technology Stack

- **Framework**: .NET 8
- **Web**: ASP.NET Core Razor Pages
- **ORM**: Entity Framework Core 8
- **Database**: SQL Server
- **Logging**: Serilog
- **Mapping**: AutoMapper
- **Validation**: FluentValidation, Data Annotations
- **Testing**: xUnit, Moq, FluentAssertions
- **UI**: Bootstrap 5
- **Password Hashing**: BCrypt.Net-Next

## Project Structure

```
DiagnosticCenter/
├── src/
│   ├── DiagnosticCenter.Domain/
│   │   ├── Entities/
│   │   └── Interfaces/
│   ├── DiagnosticCenter.Application/
│   │   ├── DTOs/
│   │   ├── Services/
│   │   ├── Mappings/
│   │   └── Extensions/
│   ├── DiagnosticCenter.Infrastructure/
│   │   ├── Data/
│   │   ├── Repositories/
│   │   └── Extensions/
│   └── DiagnosticCenter.Web/
│       ├── Pages/
│       ├── wwwroot/
│       └── Program.cs
├── tests/
│   ├── DiagnosticCenter.UnitTests/
│   └── DiagnosticCenter.IntegrationTests/
└── docs/
    └── MIGRATION_NOTES.md
```

## Migration Notes

This application was migrated from ASP.NET Web Forms 4.5.2 to .NET 8. Key changes include:

### Replaced Technologies
- ❌ System.Web → ✅ ASP.NET Core
- ❌ ADO.NET → ✅ Entity Framework Core 8
- ❌ WebConfigurationManager → ✅ IConfiguration
- ❌ Session State → ✅ Distributed Session
- ❌ Forms Authentication → ✅ Cookie Authentication
- ❌ ViewState → ✅ Modern state management (TempData, Session)
- ❌ Server Controls → ✅ HTML Helpers, Tag Helpers, Bootstrap

### Security Improvements
- ✅ Password hashing with BCrypt (WorkFactor: 12)
- ✅ Parameterized queries (EF Core)
- ✅ SQL injection protection
- ✅ CSRF protection (built-in)
- ✅ Input validation (client and server)

### Performance Improvements
- ✅ Async/await throughout
- ✅ Connection pooling
- ✅ AsNoTracking for read queries
- ✅ Eager loading for navigation properties
- ✅ Index optimization

## Build and Test

### Build the Solution

```bash
dotnet build
```

### Run Unit Tests

```bash
dotnet test tests/DiagnosticCenter.UnitTests
```

### Run Integration Tests

```bash
dotnet test tests/DiagnosticCenter.IntegrationTests
```

### Run All Tests

```bash
dotnet test
```

## Configuration

### appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=...;Database=DiagnosticCenterDb;..."
  },
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "Microsoft.Hosting.Lifetime": "Information"
      }
    }
  },
  "Session": {
    "IdleTimeout": 30,
    "CookieName": ".DiagnosticCenter.Session",
    "CookieHttpOnly": true,
    "CookieIsEssential": true
  },
  "Authentication": {
    "LoginPath": "/Index",
    "AccessDeniedPath": "/AccessDenied",
    "CookieName": ".DiagnosticCenter.Auth",
    "ExpireTimeSpan": 60
  }
}
```

## Troubleshooting

### Database Connection Issues

Ensure SQL Server is running and the connection string is correct. Test with:

```bash
dotnet ef database update --project src/DiagnosticCenter.Infrastructure --startup-project src/DiagnosticCenter.Web
```

### Port Already in Use

Change the port in `src/DiagnosticCenter.Web/Properties/launchSettings.json`.

## License

This project is for educational and internal use.

## Support

For issues or questions, please contact the development team.

---

**Migration Completed**: January 8, 2026
**Target Framework**: .NET 8
**Build Status**: ✅ Success (0 errors, 1 warning)
