using Microsoft.EntityFrameworkCore;
using DiagnosticCenter.Domain.Entities;

namespace DiagnosticCenter.Infrastructure.Data;

public class DiagnosticCenterDbContext : DbContext
{
    public DiagnosticCenterDbContext(DbContextOptions<DiagnosticCenterDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<TestType> TestTypes { get; set; } = null!;
    public DbSet<TestSetup> TestSetups { get; set; } = null!;
    public DbSet<TestEntry> TestEntries { get; set; } = null!;
    public DbSet<Payment> Payments { get; set; } = null!;
    public DbSet<Report> Reports { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DiagnosticCenterDbContext).Assembly);
    }
}
