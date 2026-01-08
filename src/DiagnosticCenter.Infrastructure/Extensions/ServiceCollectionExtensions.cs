using DiagnosticCenter.Domain.Interfaces.Repositories;
using DiagnosticCenter.Infrastructure.Data;
using DiagnosticCenter.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DiagnosticCenter.Infrastructure.Extensions;

/// <summary>
/// Extension methods for configuring Infrastructure layer services in the dependency injection container.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds Infrastructure layer services to the specified <see cref="IServiceCollection"/>.
    /// This includes database context, repositories, and connection resiliency configuration.
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    /// <param name="configuration">The configuration instance containing connection strings and settings.</param>
    /// <returns>The service collection for method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when services or configuration is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown when connection string is not found in configuration.</exception>
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        if (configuration == null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }

        // Get connection string from configuration
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "Database connection string 'DefaultConnection' is not configured. " +
                "Please ensure the connection string is set in appsettings.json or environment variables.");
        }

        // Register DbContext with SQL Server and connection resiliency
        services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
        {
            var logger = serviceProvider.GetService<ILogger<ApplicationDbContext>>();

            options.UseSqlServer(connectionString, sqlServerOptions =>
            {
                // Enable connection resiliency (retry on transient failures)
                sqlServerOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null);

                // Configure command timeout (in seconds)
                sqlServerOptions.CommandTimeout(60);

                // Use the migrations assembly
                sqlServerOptions.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
            });

            // Enable sensitive data logging in development
            if (configuration.GetValue<bool>("Logging:EnableSensitiveDataLogging", false))
            {
                options.EnableSensitiveDataLogging();
                logger?.LogWarning("Sensitive data logging is enabled. This should only be used in development.");
            }

            // Enable detailed errors in development
            if (configuration.GetValue<bool>("Logging:EnableDetailedErrors", false))
            {
                options.EnableDetailedErrors();
            }

            // Configure logging
            options.UseLoggerFactory(
                serviceProvider.GetRequiredService<ILoggerFactory>());
        });

        // Register repositories
        services.AddScoped<ITestTypeRepository, TestTypeRepository>();
        services.AddScoped<ITestSetupRepository, TestSetupRepository>();
        services.AddScoped<ITestEntryRepository, TestEntryRepository>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }

    /// <summary>
    /// Adds Infrastructure layer services with custom DbContext configuration.
    /// Useful for testing or when custom configuration is needed.
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    /// <param name="optionsAction">Action to configure DbContext options.</param>
    /// <returns>The service collection for method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when services or optionsAction is null.</exception>
    public static IServiceCollection AddInfrastructureWithOptions(
        this IServiceCollection services,
        Action<DbContextOptionsBuilder> optionsAction)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        if (optionsAction == null)
        {
            throw new ArgumentNullException(nameof(optionsAction));
        }

        // Register DbContext with custom options
        services.AddDbContext<ApplicationDbContext>(optionsAction);

        // Register repositories
        services.AddScoped<ITestTypeRepository, TestTypeRepository>();
        services.AddScoped<ITestSetupRepository, TestSetupRepository>();
        services.AddScoped<ITestEntryRepository, TestEntryRepository>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }

    /// <summary>
    /// Ensures the database is created and applies any pending migrations.
    /// This method should be called during application startup.
    /// </summary>
    /// <param name="serviceProvider">The service provider to resolve the DbContext.</param>
    /// <param name="applyMigrations">Whether to apply pending migrations. Default is true.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown when serviceProvider is null.</exception>
    public static async Task InitializeDatabaseAsync(
        this IServiceProvider serviceProvider,
        bool applyMigrations = true)
    {
        if (serviceProvider == null)
        {
            throw new ArgumentNullException(nameof(serviceProvider));
        }

        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<ApplicationDbContext>>();

        try
        {
            logger.LogInformation("Initializing database...");

            if (applyMigrations)
            {
                var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
                var pendingMigrationsList = pendingMigrations.ToList();

                if (pendingMigrationsList.Any())
                {
                    logger.LogInformation("Applying {Count} pending migrations", pendingMigrationsList.Count);
                    await context.Database.MigrateAsync();
                    logger.LogInformation("Database migrations applied successfully");
                }
                else
                {
                    logger.LogInformation("No pending migrations to apply");
                }
            }
            else
            {
                // Just ensure the database exists without applying migrations
                var created = await context.Database.EnsureCreatedAsync();
                if (created)
                {
                    logger.LogInformation("Database created successfully");
                }
                else
                {
                    logger.LogInformation("Database already exists");
                }
            }

            logger.LogInformation("Database initialization completed successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while initializing database");
            throw;
        }
    }

    /// <summary>
    /// Validates the database connection.
    /// </summary>
    /// <param name="serviceProvider">The service provider to resolve the DbContext.</param>
    /// <returns>True if the connection is valid; otherwise, false.</returns>
    /// <exception cref="ArgumentNullException">Thrown when serviceProvider is null.</exception>
    public static async Task<bool> ValidateDatabaseConnectionAsync(this IServiceProvider serviceProvider)
    {
        if (serviceProvider == null)
        {
            throw new ArgumentNullException(nameof(serviceProvider));
        }

        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<ApplicationDbContext>>();

        try
        {
            logger.LogInformation("Validating database connection...");
            var canConnect = await context.Database.CanConnectAsync();

            if (canConnect)
            {
                logger.LogInformation("Database connection is valid");
            }
            else
            {
                logger.LogWarning("Cannot connect to database");
            }

            return canConnect;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while validating database connection");
            return false;
        }
    }
}
