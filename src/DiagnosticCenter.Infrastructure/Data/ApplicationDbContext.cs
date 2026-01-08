using DiagnosticCenter.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace DiagnosticCenter.Infrastructure.Data;

/// <summary>
/// Entity Framework Core database context for the Diagnostic Center application.
/// Provides access to database entities and configures entity relationships.
/// </summary>
public class ApplicationDbContext : DbContext
{
    private readonly ILogger<ApplicationDbContext>? _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
    /// </summary>
    /// <param name="options">The options to be used by the DbContext.</param>
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class with logging support.
    /// </summary>
    /// <param name="options">The options to be used by the DbContext.</param>
    /// <param name="logger">The logger instance for logging database operations.</param>
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        ILogger<ApplicationDbContext> logger)
        : base(options)
    {
        _logger = logger;
    }

    /// <summary>
    /// Gets or sets the DbSet for TestType entities.
    /// </summary>
    public DbSet<TestType> TestTypes { get; set; } = null!;

    /// <summary>
    /// Gets or sets the DbSet for TestSetup entities.
    /// </summary>
    public DbSet<TestSetup> TestSetups { get; set; } = null!;

    /// <summary>
    /// Gets or sets the DbSet for TestEntry entities.
    /// </summary>
    public DbSet<TestEntry> TestEntries { get; set; } = null!;

    /// <summary>
    /// Gets or sets the DbSet for Payment entities.
    /// </summary>
    public DbSet<Payment> Payments { get; set; } = null!;

    /// <summary>
    /// Gets or sets the DbSet for User entities.
    /// </summary>
    public DbSet<User> Users { get; set; } = null!;

    /// <summary>
    /// Configures the model that was discovered by convention from the entity types.
    /// </summary>
    /// <param name="modelBuilder">The builder being used to construct the model for this context.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply all entity configurations from the current assembly
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        _logger?.LogDebug("Entity configurations applied successfully");
    }

    /// <summary>
    /// Configures the database context options with connection resiliency.
    /// </summary>
    /// <param name="optionsBuilder">The options builder for configuring the context.</param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        if (_logger != null && !optionsBuilder.IsConfigured)
        {
            _logger.LogWarning("DbContext is not configured. Ensure that connection string is provided.");
        }
    }

    /// <summary>
    /// Saves all changes made in this context to the database asynchronously.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>The number of state entries written to the database.</returns>
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger?.LogDebug("Saving changes to database");
            var result = await base.SaveChangesAsync(cancellationToken);
            _logger?.LogDebug("Successfully saved {Count} changes to database", result);
            return result;
        }
        catch (DbUpdateException ex)
        {
            _logger?.LogError(ex, "Error occurred while saving changes to database");
            throw;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Unexpected error occurred while saving changes to database");
            throw;
        }
    }

    /// <summary>
    /// Saves all changes made in this context to the database synchronously.
    /// </summary>
    /// <returns>The number of state entries written to the database.</returns>
    public override int SaveChanges()
    {
        try
        {
            _logger?.LogDebug("Saving changes to database (synchronous)");
            var result = base.SaveChanges();
            _logger?.LogDebug("Successfully saved {Count} changes to database", result);
            return result;
        }
        catch (DbUpdateException ex)
        {
            _logger?.LogError(ex, "Error occurred while saving changes to database");
            throw;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Unexpected error occurred while saving changes to database");
            throw;
        }
    }
}
