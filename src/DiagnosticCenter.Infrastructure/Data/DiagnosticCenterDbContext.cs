using DiagnosticCenter.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DiagnosticCenter.Infrastructure.Data;

public class DiagnosticCenterDbContext : DbContext
{
    public DiagnosticCenterDbContext(DbContextOptions<DiagnosticCenterDbContext> options) : base(options)
    {
    }

    public DbSet<TestSetup> TestSetups { get; set; }
    public DbSet<TestType> TestTypes { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DiagnosticCenterDbContext).Assembly);
    }
}
