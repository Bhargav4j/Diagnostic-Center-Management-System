using Xunit;
using Microsoft.EntityFrameworkCore;
using DiagnosticCenter.Infrastructure.Data;
using DiagnosticCenter.Domain.Entities;

namespace Tests.DiagnosticCenter.Infrastructure.Data;

public class ApplicationDbContextTests
{
    private ApplicationDbContext CreateInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new ApplicationDbContext(options);
    }

    [Fact]
    public void Constructor_ShouldInitializeContext()
    {
        // Arrange & Act
        using var context = CreateInMemoryContext();

        // Assert
        Assert.NotNull(context);
        Assert.NotNull(context.TestTypes);
        Assert.NotNull(context.TestSetups);
        Assert.NotNull(context.TestEntries);
        Assert.NotNull(context.TestEntryItems);
        Assert.NotNull(context.Payments);
    }

    [Fact]
    public void TestTypes_ShouldBeAccessible()
    {
        // Arrange
        using var context = CreateInMemoryContext();

        // Act
        var testTypes = context.TestTypes;

        // Assert
        Assert.NotNull(testTypes);
        Assert.IsAssignableFrom<DbSet<TestType>>(testTypes);
    }

    [Fact]
    public void TestSetups_ShouldBeAccessible()
    {
        // Arrange
        using var context = CreateInMemoryContext();

        // Act
        var testSetups = context.TestSetups;

        // Assert
        Assert.NotNull(testSetups);
        Assert.IsAssignableFrom<DbSet<TestSetup>>(testSetups);
    }

    [Fact]
    public void TestEntries_ShouldBeAccessible()
    {
        // Arrange
        using var context = CreateInMemoryContext();

        // Act
        var testEntries = context.TestEntries;

        // Assert
        Assert.NotNull(testEntries);
        Assert.IsAssignableFrom<DbSet<TestEntry>>(testEntries);
    }

    [Fact]
    public void TestEntryItems_ShouldBeAccessible()
    {
        // Arrange
        using var context = CreateInMemoryContext();

        // Act
        var testEntryItems = context.TestEntryItems;

        // Assert
        Assert.NotNull(testEntryItems);
        Assert.IsAssignableFrom<DbSet<TestEntryItem>>(testEntryItems);
    }

    [Fact]
    public void Payments_ShouldBeAccessible()
    {
        // Arrange
        using var context = CreateInMemoryContext();

        // Act
        var payments = context.Payments;

        // Assert
        Assert.NotNull(payments);
        Assert.IsAssignableFrom<DbSet<Payment>>(payments);
    }

    [Fact]
    public async Task AddTestType_ShouldSaveToDatabase()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var testType = new TestType { Name = "Blood Test", IsActive = true };

        // Act
        context.TestTypes.Add(testType);
        await context.SaveChangesAsync();

        // Assert
        var saved = await context.TestTypes.FirstOrDefaultAsync(t => t.Name == "Blood Test");
        Assert.NotNull(saved);
        Assert.Equal("Blood Test", saved.Name);
    }

    [Fact]
    public async Task AddTestSetup_ShouldSaveToDatabase()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var testSetup = new TestSetup { Name = "CBC Test", Fee = 50m, IsActive = true };

        // Act
        context.TestSetups.Add(testSetup);
        await context.SaveChangesAsync();

        // Assert
        var saved = await context.TestSetups.FirstOrDefaultAsync(t => t.Name == "CBC Test");
        Assert.NotNull(saved);
        Assert.Equal(50m, saved.Fee);
    }

    [Fact]
    public async Task AddTestEntry_ShouldSaveToDatabase()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var testEntry = new TestEntry
        {
            PatientName = "John Doe",
            BillNumber = "BILL-001",
            TotalAmount = 500m,
            IsActive = true
        };

        // Act
        context.TestEntries.Add(testEntry);
        await context.SaveChangesAsync();

        // Assert
        var saved = await context.TestEntries.FirstOrDefaultAsync(t => t.PatientName == "John Doe");
        Assert.NotNull(saved);
        Assert.Equal("BILL-001", saved.BillNumber);
    }

    [Fact]
    public async Task AddPayment_ShouldSaveToDatabase()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var payment = new Payment
        {
            Amount = 100m,
            BillNumber = "BILL-001",
            IsActive = true
        };

        // Act
        context.Payments.Add(payment);
        await context.SaveChangesAsync();

        // Assert
        var saved = await context.Payments.FirstOrDefaultAsync(p => p.BillNumber == "BILL-001");
        Assert.NotNull(saved);
        Assert.Equal(100m, saved.Amount);
    }
}
