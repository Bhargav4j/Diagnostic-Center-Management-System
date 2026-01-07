using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Infrastructure.Data;
using DiagnosticCenter.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DiagnosticCenter.Infrastructure.Data.Configurations.Tests;

public class PaymentConfigurationTests
{
    [Fact]
    public void Configure_ShouldSetTableName()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new ApplicationDbContext(options);
        var entityType = context.Model.FindEntityType(typeof(Payment));

        // Assert
        Assert.NotNull(entityType);
        Assert.Equal("Payments", entityType.GetTableName());
    }

    [Fact]
    public void Configure_ShouldSetBillNumberMaxLength()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new ApplicationDbContext(options);
        var entityType = context.Model.FindEntityType(typeof(Payment));
        var billNumberProperty = entityType?.FindProperty(nameof(Payment.BillNumber));

        // Assert
        Assert.NotNull(billNumberProperty);
        Assert.Equal(50, billNumberProperty.GetMaxLength());
        Assert.False(billNumberProperty.IsNullable);
    }

    [Fact]
    public void Configure_ShouldSetAmountAsRequired()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new ApplicationDbContext(options);
        var entityType = context.Model.FindEntityType(typeof(Payment));
        var amountProperty = entityType?.FindProperty(nameof(Payment.Amount));

        // Assert
        Assert.NotNull(amountProperty);
        Assert.False(amountProperty.IsNullable);
    }

    [Fact]
    public void Configure_ShouldSetPaymentMethodMaxLength()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new ApplicationDbContext(options);
        var entityType = context.Model.FindEntityType(typeof(Payment));
        var paymentMethodProperty = entityType?.FindProperty(nameof(Payment.PaymentMethod));

        // Assert
        Assert.NotNull(paymentMethodProperty);
        Assert.Equal(50, paymentMethodProperty.GetMaxLength());
    }

    [Fact]
    public void Configure_ShouldSetTransactionReferenceMaxLength()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new ApplicationDbContext(options);
        var entityType = context.Model.FindEntityType(typeof(Payment));
        var transactionRefProperty = entityType?.FindProperty(nameof(Payment.TransactionReference));

        // Assert
        Assert.NotNull(transactionRefProperty);
        Assert.Equal(100, transactionRefProperty.GetMaxLength());
    }

    [Fact]
    public void Configure_ShouldSetNotesMaxLength()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new ApplicationDbContext(options);
        var entityType = context.Model.FindEntityType(typeof(Payment));
        var notesProperty = entityType?.FindProperty(nameof(Payment.Notes));

        // Assert
        Assert.NotNull(notesProperty);
        Assert.Equal(500, notesProperty.GetMaxLength());
    }

    [Fact]
    public void Configure_ShouldSetIsActiveDefaultValue()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new ApplicationDbContext(options);
        var entityType = context.Model.FindEntityType(typeof(Payment));
        var isActiveProperty = entityType?.FindProperty(nameof(Payment.IsActive));

        // Assert
        Assert.NotNull(isActiveProperty);
        Assert.False(isActiveProperty.IsNullable);
    }

    [Fact]
    public void Configure_ShouldCreateIndexOnBillNumber()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new ApplicationDbContext(options);
        var entityType = context.Model.FindEntityType(typeof(Payment));
        var indexes = entityType?.GetIndexes();

        // Assert
        Assert.NotNull(indexes);
        Assert.Contains(indexes, i => i.Properties.Any(p => p.Name == nameof(Payment.BillNumber)));
    }

    [Fact]
    public void Configure_ShouldCreateIndexOnTestEntryId()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new ApplicationDbContext(options);
        var entityType = context.Model.FindEntityType(typeof(Payment));
        var indexes = entityType?.GetIndexes();

        // Assert
        Assert.NotNull(indexes);
        Assert.Contains(indexes, i => i.Properties.Any(p => p.Name == nameof(Payment.TestEntryId)));
    }

    [Fact]
    public void Configure_ShouldCreateIndexOnPaymentDate()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new ApplicationDbContext(options);
        var entityType = context.Model.FindEntityType(typeof(Payment));
        var indexes = entityType?.GetIndexes();

        // Assert
        Assert.NotNull(indexes);
        Assert.Contains(indexes, i => i.Properties.Any(p => p.Name == nameof(Payment.PaymentDate)));
    }
}
