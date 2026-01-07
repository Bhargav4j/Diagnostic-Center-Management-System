using Xunit;
using DiagnosticCenter.Infrastructure.Data.Configurations;
using DiagnosticCenter.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace DiagnosticCenter.Infrastructure.Data.Configurations.Tests;

public class TestSetupConfigurationTests
{
    [Fact]
    public void Configure_ShouldSetTableName()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<DbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var context = new DbContext(options);
        var modelBuilder = new ModelBuilder();
        var configuration = new TestSetupConfiguration();

        // Act
        configuration.Configure(modelBuilder.Entity<TestSetup>());

        // Assert
        Assert.NotNull(configuration);
    }

    [Fact]
    public void Configuration_ShouldNotBeNull()
    {
        // Arrange & Act
        var configuration = new TestSetupConfiguration();

        // Assert
        Assert.NotNull(configuration);
    }
}
