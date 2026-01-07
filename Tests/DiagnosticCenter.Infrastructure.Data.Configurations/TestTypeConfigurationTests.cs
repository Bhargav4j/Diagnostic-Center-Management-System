using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DiagnosticCenter.Infrastructure.Data.Configurations.Tests;

public class TestTypeConfigurationTests
{
    [Fact]
    public void Configure_ShouldSetTableName()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new ApplicationDbContext(options);
        var entityType = context.Model.FindEntityType(typeof(TestType));

        // Assert
        Assert.NotNull(entityType);
        Assert.Equal("TestTypes", entityType.GetTableName());
    }

    [Fact]
    public void Configure_ShouldSetNameMaxLength()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new ApplicationDbContext(options);
        var entityType = context.Model.FindEntityType(typeof(TestType));
        var nameProperty = entityType?.FindProperty(nameof(TestType.Name));

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
        var entityType = context.Model.FindEntityType(typeof(TestType));
        var descriptionProperty = entityType?.FindProperty(nameof(TestType.Description));

        // Assert
        Assert.NotNull(descriptionProperty);
        Assert.Equal(1000, descriptionProperty.GetMaxLength());
    }

    [Fact]
    public void Configure_ShouldSetCreatedByMaxLength()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new ApplicationDbContext(options);
        var entityType = context.Model.FindEntityType(typeof(TestType));
        var createdByProperty = entityType?.FindProperty(nameof(TestType.CreatedBy));

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
        var entityType = context.Model.FindEntityType(typeof(TestType));
        var modifiedByProperty = entityType?.FindProperty(nameof(TestType.ModifiedBy));

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
        var entityType = context.Model.FindEntityType(typeof(TestType));
        var isActiveProperty = entityType?.FindProperty(nameof(TestType.IsActive));

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
        var entityType = context.Model.FindEntityType(typeof(TestType));
        var indexes = entityType?.GetIndexes();

        // Assert
        Assert.NotNull(indexes);
        Assert.Contains(indexes, i => i.Properties.Any(p => p.Name == nameof(TestType.Name)));
    }

    [Fact]
    public void Configure_ShouldSetPrimaryKey()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new ApplicationDbContext(options);
        var entityType = context.Model.FindEntityType(typeof(TestType));
        var primaryKey = entityType?.FindPrimaryKey();

        // Assert
        Assert.NotNull(primaryKey);
        Assert.Single(primaryKey.Properties);
        Assert.Equal(nameof(TestType.Id), primaryKey.Properties.First().Name);
    }

    [Fact]
    public void Configure_ShouldConfigureTestSetupsRelationship()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new ApplicationDbContext(options);
        var entityType = context.Model.FindEntityType(typeof(TestType));
        var navigation = entityType?.FindNavigation(nameof(TestType.TestSetups));

        // Assert
        Assert.NotNull(navigation);
        Assert.True(navigation.IsCollection);
    }
}
