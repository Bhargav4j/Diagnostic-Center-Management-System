using Xunit;
using Moq;
using DiagnosticCenter.Web.Pages.Admin.TestType;
using DiagnosticCenter.Domain.Interfaces.Services;
using DiagnosticCenter.Domain.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DiagnosticCenter.Web.Pages.Admin.TestType.Tests;

public class IndexModelTests
{
    private readonly Mock<ITestTypeService> _mockTestTypeService;
    private readonly Mock<ILogger<IndexModel>> _mockLogger;
    private readonly IndexModel _pageModel;

    public IndexModelTests()
    {
        _mockTestTypeService = new Mock<ITestTypeService>();
        _mockLogger = new Mock<ILogger<IndexModel>>();
        _pageModel = new IndexModel(_mockTestTypeService.Object, _mockLogger.Object);

        // Mock HttpContext
        _pageModel.PageContext = new PageContext
        {
            HttpContext = new DefaultHttpContext()
        };
    }

    [Fact]
    public async Task OnGetAsync_ShouldLoadTestTypes()
    {
        // Arrange
        var testTypes = new List<Domain.Entities.TestType>
        {
            new Domain.Entities.TestType { Id = 1, Name = "Blood Test" },
            new Domain.Entities.TestType { Id = 2, Name = "X-Ray" }
        };
        _mockTestTypeService.Setup(s => s.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(testTypes);

        // Act
        await _pageModel.OnGetAsync();

        // Assert
        Assert.NotNull(_pageModel.TestTypes);
        Assert.Equal(2, ((List<Domain.Entities.TestType>)_pageModel.TestTypes).Count);
    }

    [Fact]
    public async Task OnGetAsync_WhenExceptionThrown_ShouldSetErrorMessage()
    {
        // Arrange
        _mockTestTypeService.Setup(s => s.GetAllAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database error"));

        // Act
        await _pageModel.OnGetAsync();

        // Assert
        Assert.NotNull(_pageModel.ErrorMessage);
        Assert.Contains("Error loading test types", _pageModel.ErrorMessage);
    }

    [Fact]
    public void TestTypes_Property_SetAndGet_ShouldWork()
    {
        // Arrange
        var testTypes = new List<Domain.Entities.TestType>
        {
            new Domain.Entities.TestType { Id = 1, Name = "Test" }
        };

        // Act
        _pageModel.TestTypes = testTypes;

        // Assert
        Assert.NotNull(_pageModel.TestTypes);
        Assert.Single(_pageModel.TestTypes);
    }

    [Fact]
    public void SuccessMessage_Property_SetAndGet_ShouldWork()
    {
        // Arrange
        var message = "Success";

        // Act
        _pageModel.SuccessMessage = message;

        // Assert
        Assert.Equal(message, _pageModel.SuccessMessage);
    }

    [Fact]
    public void ErrorMessage_Property_SetAndGet_ShouldWork()
    {
        // Arrange
        var message = "Error";

        // Act
        _pageModel.ErrorMessage = message;

        // Assert
        Assert.Equal(message, _pageModel.ErrorMessage);
    }
}
