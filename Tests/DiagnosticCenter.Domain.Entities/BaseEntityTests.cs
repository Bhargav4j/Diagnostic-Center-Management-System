using Xunit;
using DiagnosticCenter.Domain.Entities;

namespace Tests.DiagnosticCenter.Domain.Entities;

public class BaseEntityTests
{
    private class TestEntity : BaseEntity
    {
    }

    [Fact]
    public void Constructor_ShouldInitializeProperties()
    {
        // Arrange & Act
        var entity = new TestEntity();

        // Assert
        Assert.Equal(0, entity.Id);
        Assert.Equal(default(DateTime), entity.CreatedDate);
        Assert.Null(entity.ModifiedDate);
        Assert.False(entity.IsActive);
        Assert.Equal(string.Empty, entity.CreatedBy);
        Assert.Null(entity.ModifiedBy);
    }

    [Fact]
    public void Id_ShouldSetAndGetValue()
    {
        // Arrange
        var entity = new TestEntity();
        var expectedId = 123;

        // Act
        entity.Id = expectedId;

        // Assert
        Assert.Equal(expectedId, entity.Id);
    }

    [Fact]
    public void CreatedDate_ShouldSetAndGetValue()
    {
        // Arrange
        var entity = new TestEntity();
        var expectedDate = DateTime.UtcNow;

        // Act
        entity.CreatedDate = expectedDate;

        // Assert
        Assert.Equal(expectedDate, entity.CreatedDate);
    }

    [Fact]
    public void ModifiedDate_ShouldSetAndGetValue()
    {
        // Arrange
        var entity = new TestEntity();
        var expectedDate = DateTime.UtcNow;

        // Act
        entity.ModifiedDate = expectedDate;

        // Assert
        Assert.Equal(expectedDate, entity.ModifiedDate);
    }

    [Fact]
    public void IsActive_ShouldSetAndGetValue()
    {
        // Arrange
        var entity = new TestEntity();

        // Act
        entity.IsActive = true;

        // Assert
        Assert.True(entity.IsActive);
    }

    [Fact]
    public void CreatedBy_ShouldSetAndGetValue()
    {
        // Arrange
        var entity = new TestEntity();
        var expectedCreatedBy = "TestUser";

        // Act
        entity.CreatedBy = expectedCreatedBy;

        // Assert
        Assert.Equal(expectedCreatedBy, entity.CreatedBy);
    }

    [Fact]
    public void ModifiedBy_ShouldSetAndGetValue()
    {
        // Arrange
        var entity = new TestEntity();
        var expectedModifiedBy = "TestUser";

        // Act
        entity.ModifiedBy = expectedModifiedBy;

        // Assert
        Assert.Equal(expectedModifiedBy, entity.ModifiedBy);
    }
}
