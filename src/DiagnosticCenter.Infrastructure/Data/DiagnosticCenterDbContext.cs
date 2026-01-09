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

        builder.ApplyConfigurationsFromAssembly(typeof(DiagnosticCenterDbContext).Assembly);

        builder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
        });

        builder.Entity<IdentityRole<int>>(entity =>
        {
            entity.ToTable("Roles");
        });

        builder.Entity<IdentityUserRole<int>>(entity =>
        {
            entity.ToTable("UserRoles");
        });

        builder.Entity<IdentityUserClaim<int>>(entity =>
        {
            entity.ToTable("UserClaims");
        });

        builder.Entity<IdentityUserLogin<int>>(entity =>
        {
            entity.ToTable("UserLogins");
        });

        builder.Entity<IdentityRoleClaim<int>>(entity =>
        {
            entity.ToTable("RoleClaims");
        });

        builder.Entity<IdentityUserToken<int>>(entity =>
        {
            entity.ToTable("UserTokens");
        });
    }
}
