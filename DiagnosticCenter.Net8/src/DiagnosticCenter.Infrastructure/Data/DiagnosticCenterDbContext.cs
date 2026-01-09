using DiagnosticCenter.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DiagnosticCenter.Infrastructure.Data;

/// <summary>
/// Database context for Diagnostic Center application
/// </summary>
public class DiagnosticCenterDbContext : IdentityDbContext<User>
{
    public DiagnosticCenterDbContext(DbContextOptions<DiagnosticCenterDbContext> options)
        : base(options)
    {
    }

    public DbSet<TestSetup> TestSetups => Set<TestSetup>();
    public DbSet<TestType> TestTypes => Set<TestType>();
    public DbSet<TestEntry> TestEntries => Set<TestEntry>();
    public DbSet<Payment> Payments => Set<Payment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DiagnosticCenterDbContext).Assembly);
    }
}
