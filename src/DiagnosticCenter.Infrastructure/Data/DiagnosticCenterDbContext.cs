using DiagnosticCenter.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DiagnosticCenter.Infrastructure.Data;

public class DiagnosticCenterDbContext : IdentityDbContext<User, IdentityRole<int>, int>
{
    public DiagnosticCenterDbContext(DbContextOptions<DiagnosticCenterDbContext> options)
        : base(options)
    {
    }

    public DbSet<TestType> TestTypes => Set<TestType>();
    public DbSet<TestSetup> TestSetups => Set<TestSetup>();
    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<PatientTest> PatientTests => Set<PatientTest>();
    public DbSet<Payment> Payments => Set<Payment>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Set default schema to public for PostgreSQL
        builder.HasDefaultSchema("public");

        // Apply all entity configurations
        builder.ApplyConfigurationsFromAssembly(typeof(DiagnosticCenterDbContext).Assembly);

        // Configure Identity tables for PostgreSQL
        builder.Entity<User>(entity =>
        {
            entity.ToTable("users", "public");
        });

        builder.Entity<IdentityRole<int>>(entity =>
        {
            entity.ToTable("roles", "public");
        });

        builder.Entity<IdentityUserRole<int>>(entity =>
        {
            entity.ToTable("user_roles", "public");
        });

        builder.Entity<IdentityUserClaim<int>>(entity =>
        {
            entity.ToTable("user_claims", "public");
        });

        builder.Entity<IdentityUserLogin<int>>(entity =>
        {
            entity.ToTable("user_logins", "public");
        });

        builder.Entity<IdentityRoleClaim<int>>(entity =>
        {
            entity.ToTable("role_claims", "public");
        });

        builder.Entity<IdentityUserToken<int>>(entity =>
        {
            entity.ToTable("user_tokens", "public");
        });
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        // Enable legacy timestamp behavior for PostgreSQL
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }
}
