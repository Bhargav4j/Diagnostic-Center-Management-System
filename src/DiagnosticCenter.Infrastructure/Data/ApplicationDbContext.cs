using Microsoft.EntityFrameworkCore;
using DiagnosticCenter.Domain.Entities;

namespace DiagnosticCenter.Infrastructure.Data;

/// <summary>
/// Application database context for the diagnostic center system
/// </summary>
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<TestType> TestTypes { get; set; }
    public DbSet<TestSetup> TestSetups { get; set; }
    public DbSet<TestEntry> TestEntries { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
