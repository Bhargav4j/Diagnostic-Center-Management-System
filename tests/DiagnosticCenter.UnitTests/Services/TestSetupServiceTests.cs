using Xunit;
using Moq;
using AutoMapper;
using Microsoft.Extensions.Logging;
using FluentAssertions;
using DiagnosticCenter.Application.Services;
using DiagnosticCenter.Application.DTOs;
using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Interfaces.Repositories;

namespace DiagnosticCenter.UnitTests.Services;

public class TestSetupServiceTests
{
    private readonly Mock<ITestSetupRepository> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ILogger<TestSetupService>> _loggerMock;
    private readonly TestSetupService _service;

    public TestSetupServiceTests()
    {
        _repositoryMock = new Mock<ITestSetupRepository>();
        _mapperMock = new Mock<IMapper>();
        _loggerMock = new Mock<ILogger<TestSetupService>>();
        _service = new TestSetupService(_repositoryMock.Object, _mapperMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllTestSetups()
    {
        // Arrange
        var testSetups = new List<TestSetup>
        {
            new() { Id = 1, Name = "Test 1" },
            new() { Id = 2, Name = "Test 2" }
        };
        var testSetupDtos = new List<TestSetupDto>
        {
            new() { Id = 1, Name = "Test 1" },
            new() { Id = 2, Name = "Test 2" }
        };

        _repositoryMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(testSetups);
        _mapperMock.Setup(m => m.Map<IEnumerable<TestSetupDto>>(testSetups))
            .Returns(testSetupDtos);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(testSetupDtos);
    }
}
