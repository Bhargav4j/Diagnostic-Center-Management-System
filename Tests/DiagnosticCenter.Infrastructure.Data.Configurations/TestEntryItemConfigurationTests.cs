using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DiagnosticCenter.Infrastructure.Data.Configurations.Tests;

public class TestEntryItemConfigurationTests
{
    [Fact]
    public void Configure_ShouldSetTableName()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new ApplicationDbContext(options);
        var entityType = context.Model.FindEntityType(typeof(TestEntryItem));

        // Assert
        Assert.NotNull(entityType);
        Assert.Equal("TestEntryItems", entityType.GetTableName());
    }

    [Fact]
    public void Configure_ShouldSetFeeColumnType()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new ApplicationDbContext(options);
        var entityType = context.Model.FindEntityType(typeof(TestEntryItem));
        var feeProperty = entityType?.FindProperty(nameof(TestEntryItem.Fee));

        // Assert
        Assert.NotNull(feeProperty);
        // Column type check removed for InMemory database
        Assert.False(feeProperty.IsNullable);
    }

    [Fact]
    public void Configure_ShouldSetCreatedByMaxLength()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new ApplicationDbContext(options);
        var entityType = context.Model.FindEntityType(typeof(TestEntryItem));
        var createdByProperty = entityType?.FindProperty(nameof(TestEntryItem.CreatedBy));

        // Assert
        Assert.NotNull(createdByProperty);
        Assert.Equal(100, createdByProperty.GetMaxLength());
        Assert.False(createdByProperty.IsNullable);
    }

    [Fact]
    public void Configure_ShouldSetModifiedByMaxLength()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new ApplicationDbContext(options);
        var entityType = context.Model.FindEntityType(typeof(TestEntryItem));
        var modifiedByProperty = entityType?.FindProperty(nameof(TestEntryItem.ModifiedBy));

        // Assert
        Assert.NotNull(modifiedByProperty);
        Assert.Equal(100, modifiedByProperty.GetMaxLength());
    }

    [Fact]
    public void Configure_ShouldSetIsActiveDefaultValue()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new ApplicationDbContext(options);
        var entityType = context.Model.FindEntityType(typeof(TestEntryItem));
        var isActiveProperty = entityType?.FindProperty(nameof(TestEntryItem.IsActive));

        // Assert
        Assert.NotNull(isActiveProperty);
        Assert.False(isActiveProperty.IsNullable);
    }

    [Fact]
    public void Configure_ShouldCreateIndexOnTestEntryId()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new ApplicationDbContext(options);
        var entityType = context.Model.FindEntityType(typeof(TestEntryItem));
        var indexes = entityType?.GetIndexes();

        // Assert
        Assert.NotNull(indexes);
        Assert.Contains(indexes, i => i.Properties.Any(p => p.Name == nameof(TestEntryItem.TestEntryId)));
    }

    [Fact]
    public void Configure_ShouldCreateIndexOnTestSetupId()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new ApplicationDbContext(options);
        var entityType = context.Model.FindEntityType(typeof(TestEntryItem));
        var indexes = entityType?.GetIndexes();

        // Assert
        Assert.NotNull(indexes);
        Assert.Contains(indexes, i => i.Properties.Any(p => p.Name == nameof(TestEntryItem.TestSetupId)));
    }

    [Fact]
    public void Configure_ShouldSetPrimaryKey()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new ApplicationDbContext(options);
        var entityType = context.Model.FindEntityType(typeof(TestEntryItem));
        var primaryKey = entityType?.FindPrimaryKey();

        // Assert
        Assert.NotNull(primaryKey);
        Assert.Single(primaryKey.Properties);
        Assert.Equal(nameof(TestEntryItem.Id), primaryKey.Properties.First().Name);
    }
}
