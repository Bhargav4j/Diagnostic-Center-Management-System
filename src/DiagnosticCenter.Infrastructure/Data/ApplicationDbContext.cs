using DiagnosticCenter.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DiagnosticCenter.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<TestType> TestTypes => Set<TestType>();
    public DbSet<TestSetup> TestSetups => Set<TestSetup>();
    public DbSet<TestEntry> TestEntries => Set<TestEntry>();
    public DbSet<TestEntryItem> TestEntryItems => Set<TestEntryItem>();
    public DbSet<Payment> Payments => Set<Payment>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
