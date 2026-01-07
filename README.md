# DiagnosticCenter - .NET 8 Application

## Overview
DiagnosticCenter is a diagnostic center management application migrated from ASP.NET Web Forms 4.5.2 to .NET 8 using clean architecture principles.

## Architecture
This application follows clean architecture with four main layers:

1. **Domain Layer** - Core business entities and interfaces
2. **Application Layer** - Business logic and services
3. **Infrastructure Layer** - Data access with Entity Framework Core 8
4. **Web Layer** - Razor Pages UI

## Technology Stack
- .NET 8.0
- ASP.NET Core Razor Pages
- Entity Framework Core 8.0.0
- SQL Server
- Serilog for logging
- Bootstrap 5 for UI

## Prerequisites
- .NET 8 SDK
- SQL Server (LocalDB or full instance)

## Getting Started

### 1. Update Connection String
Edit `src/DiagnosticCenter.Web/appsettings.json` and update the connection string:
```json
"ConnectionStrings": {
  "DiagnosticCenterDb": "Server=localhost;Database=diagnostic_center_project;Integrated Security=true;TrustServerCertificate=true"
}
```

### 2. Apply Database Migrations
```bash
cd src/DiagnosticCenter.Web
dotnet ef database update --project ../DiagnosticCenter.Infrastructure
```

### 3. Run the Application
```bash
cd src/DiagnosticCenter.Web
dotnet run
```

The application will be available at `https://localhost:5001` or `http://localhost:5000`.

## Project Structure
```
DiagnosticCenter/
├── src/
│   ├── DiagnosticCenter.Domain/         # Domain entities and interfaces
│   ├── DiagnosticCenter.Application/    # Business logic services
│   ├── DiagnosticCenter.Infrastructure/ # EF Core, repositories
│   └── DiagnosticCenter.Web/            # Razor Pages UI
└── DiagnosticCenter.sln
```

## Features Implemented
- User authentication with role-based access (Admin, Accountant, Receptionist)
- Test Type management (CRUD operations)
- Clean architecture with dependency injection
- Async/await throughout
- Structured logging with Serilog
- Secure password hashing
- Session management

## Migration Notes
This application was migrated from ASP.NET Web Forms 4.5.2 to .NET 8:
- Replaced System.Web with ASP.NET Core
- Migrated Web Forms pages to Razor Pages
- Replaced ADO.NET with Entity Framework Core
- Converted ViewState to modern state management
- Implemented authentication with ASP.NET Core Identity patterns
- Added proper error handling and logging

## Security Improvements
- Password hashing instead of plain text storage
- Parameterized queries via EF Core (prevents SQL injection)
- HTTPS enforcement
- Role-based authorization
- Secure cookie authentication

## Default Users
Check your existing database for user credentials, or create new users through SQL:
```sql
-- Password should be hashed before insertion
INSERT INTO users (email, password, acctype, IsActive, created_at, CreatedBy)
VALUES ('admin@example.com', '<hashed_password>', 'Admin', 1, GETUTCDATE(), 'System')
```

## Build and Test
```bash
# Restore packages
dotnet restore

# Build solution
dotnet build

# Run tests (when implemented)
dotnet test
```

## License
Copyright © 2026 DiagnosticCenter
