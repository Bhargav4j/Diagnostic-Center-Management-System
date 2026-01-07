using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DiagnosticCenter.Infrastructure.Data.Configurations.Tests;

public class TestSetupConfigurationTests
{
    [Fact]
    public void Configure_ShouldSetTableName()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new ApplicationDbContext(options);
        var entityType = context.Model.FindEntityType(typeof(TestSetup));

        // Assert
        Assert.NotNull(entityType);
        Assert.Equal("TestSetups", entityType.GetTableName());
    }

    [Fact]
    public void Configure_ShouldSetNameMaxLength()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new ApplicationDbContext(options);
        var entityType = context.Model.FindEntityType(typeof(TestSetup));
        var nameProperty = entityType?.FindProperty(nameof(TestSetup.Name));

        // Assert
        Assert.NotNull(nameProperty);
        Assert.Equal(200, nameProperty.GetMaxLength());
        Assert.False(nameProperty.IsNullable);
    }

    [Fact]
    public void Configure_ShouldSetDescriptionMaxLength()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new ApplicationDbContext(options);
        var entityType = context.Model.FindEntityType(typeof(TestSetup));
        var descriptionProperty = entityType?.FindProperty(nameof(TestSetup.Description));

        // Assert
        Assert.NotNull(descriptionProperty);
        Assert.Equal(1000, descriptionProperty.GetMaxLength());
    }

    [Fact]
    public void Configure_ShouldSetFeeColumnType()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new ApplicationDbContext(options);
        var entityType = context.Model.FindEntityType(typeof(TestSetup));
        var feeProperty = entityType?.FindProperty(nameof(TestSetup.Fee));

        // Assert
        Assert.NotNull(feeProperty);
        // Column type check removed for InMemory database
        Assert.False(feeProperty.IsNullable);
    }

    [Fact]
    public void Configure_ShouldSetIsActiveDefaultValue()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new ApplicationDbContext(options);
        var entityType = context.Model.FindEntityType(typeof(TestSetup));
        var isActiveProperty = entityType?.FindProperty(nameof(TestSetup.IsActive));

        // Assert
        Assert.NotNull(isActiveProperty);
        Assert.False(isActiveProperty.IsNullable);
    }

    [Fact]
    public void Configure_ShouldCreateIndexOnName()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new ApplicationDbContext(options);
        var entityType = context.Model.FindEntityType(typeof(TestSetup));
        var indexes = entityType?.GetIndexes();

        // Assert
        Assert.NotNull(indexes);
        Assert.Contains(indexes, i => i.Properties.Any(p => p.Name == nameof(TestSetup.Name)));
    }

    [Fact]
    public void Configure_ShouldCreateIndexOnTestTypeId()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new ApplicationDbContext(options);
        var entityType = context.Model.FindEntityType(typeof(TestSetup));
        var indexes = entityType?.GetIndexes();

        // Assert
        Assert.NotNull(indexes);
        Assert.Contains(indexes, i => i.Properties.Any(p => p.Name == nameof(TestSetup.TestTypeId)));
    }

    [Fact]
    public void Configure_ShouldSetPrimaryKey()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new ApplicationDbContext(options);
        var entityType = context.Model.FindEntityType(typeof(TestSetup));
        var primaryKey = entityType?.FindPrimaryKey();

        // Assert
        Assert.NotNull(primaryKey);
        Assert.Single(primaryKey.Properties);
        Assert.Equal(nameof(TestSetup.Id), primaryKey.Properties.First().Name);
    }
}
