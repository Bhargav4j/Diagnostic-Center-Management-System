using DiagnosticCenter.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DiagnosticCenter.Infrastructure.Data;

/// <summary>
/// Database context for DiagnosticCenter application
/// </summary>
public class DiagnosticCenterDbContext : DbContext
{
    public DiagnosticCenterDbContext(DbContextOptions<DiagnosticCenterDbContext> options)
        : base(options)
    {
    }

    public DbSet<TestType> TestTypes => Set<TestType>();
    public DbSet<TestSetup> TestSetups => Set<TestSetup>();
    public DbSet<TestEntry> TestEntries => Set<TestEntry>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DiagnosticCenterDbContext).Assembly);
    }
}
