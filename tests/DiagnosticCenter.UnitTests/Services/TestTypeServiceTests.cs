using AutoMapper;
using DiagnosticCenter.Application.Services;
using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Interfaces.Repositories;
using DiagnosticCenter.Domain.Interfaces.Services;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace DiagnosticCenter.UnitTests.Services;

public class TestTypeServiceTests
{
    private readonly Mock<ITestTypeRepository> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILogger<TestTypeService>> _mockLogger;
    private readonly TestTypeService _service;

    public TestTypeServiceTests()
    {
        _mockRepository = new Mock<ITestTypeRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<TestTypeService>>();
        _service = new TestTypeService(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllTestTypes()
    {
        var testTypes = new List<TestType>
        {
            new TestType { Id = 1, Name = "Blood Test" },
            new TestType { Id = 2, Name = "X-Ray" }
        };
        var testTypeDtos = new List<TestTypeDto>
        {
            new TestTypeDto { Id = 1, Name = "Blood Test" },
            new TestTypeDto { Id = 2, Name = "X-Ray" }
        };

        _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(testTypes);
        _mockMapper.Setup(m => m.Map<IEnumerable<TestTypeDto>>(testTypes))
            .Returns(testTypeDtos);

        var result = await _service.GetAllAsync();

        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(testTypeDtos);
    }

    [Fact]
    public async Task GetByIdAsync_WithValidId_ReturnsTestType()
    {
        var testType = new TestType { Id = 1, Name = "Blood Test" };
        var testTypeDto = new TestTypeDto { Id = 1, Name = "Blood Test" };

        _mockRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(testType);
        _mockMapper.Setup(m => m.Map<TestTypeDto>(testType))
            .Returns(testTypeDto);

        var result = await _service.GetByIdAsync(1);

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(testTypeDto);
    }

    [Fact]
    public async Task CreateAsync_WithValidData_ReturnsCreatedTestType()
    {
        var createDto = new TestTypeCreateDto { Name = "New Test", CreatedBy = "admin" };
        var testType = new TestType { Id = 1, Name = "New Test" };
        var testTypeDto = new TestTypeDto { Id = 1, Name = "New Test" };

        _mockRepository.Setup(r => r.ExistsByNameAsync("New Test", It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _mockMapper.Setup(m => m.Map<TestType>(createDto))
            .Returns(testType);
        _mockRepository.Setup(r => r.AddAsync(It.IsAny<TestType>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(testType);
        _mockMapper.Setup(m => m.Map<TestTypeDto>(testType))
            .Returns(testTypeDto);

        var result = await _service.CreateAsync(createDto);

        result.Should().NotBeNull();
        result.Name.Should().Be("New Test");
    }

    [Fact]
    public async Task CreateAsync_WithDuplicateName_ThrowsInvalidOperationException()
    {
        var createDto = new TestTypeCreateDto { Name = "Existing Test", CreatedBy = "admin" };

        _mockRepository.Setup(r => r.ExistsByNameAsync("Existing Test", It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        Func<Task> act = async () => await _service.CreateAsync(createDto);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*already exists*");
    }
}
