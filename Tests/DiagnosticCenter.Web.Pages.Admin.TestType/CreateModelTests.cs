using Xunit;
using Moq;
using DiagnosticCenter.Web.Pages.Admin.TestType;
using DiagnosticCenter.Domain.Interfaces.Services;
using DiagnosticCenter.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace DiagnosticCenter.Web.Pages.Admin.TestType.Tests;

public class CreateModelTests
{
    private readonly Mock<ITestTypeService> _mockTestTypeService;
    private readonly Mock<ILogger<CreateModel>> _mockLogger;
    private readonly CreateModel _pageModel;

    public CreateModelTests()
    {
        _mockTestTypeService = new Mock<ITestTypeService>();
        _mockLogger = new Mock<ILogger<CreateModel>>();
        _pageModel = new CreateModel(_mockTestTypeService.Object, _mockLogger.Object);

        // Mock HttpContext with User identity
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.Name, "TestAdmin")
        }, "mock"));

        _pageModel.PageContext = new PageContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };
    }

    [Fact]
    public void OnGet_ShouldExecuteWithoutErrors()
    {
        // Act
        _pageModel.OnGet();

        // Assert
        Assert.NotNull(_pageModel);
    }

    [Fact]
    public async Task OnPostAsync_WithInvalidModelState_ShouldReturnPage()
    {
        // Arrange
        _pageModel.ModelState.AddModelError("Name", "Name is required");

        // Act
        var result = await _pageModel.OnPostAsync();

        // Assert
        Assert.IsType<PageResult>(result);
    }

    [Fact]
    public async Task OnPostAsync_WithValidData_ShouldRedirectToIndex()
    {
        // Arrange
        _pageModel.Name = "New Test Type";
        _pageModel.Description = "Test Description";

        _mockTestTypeService.Setup(s => s.CreateAsync(It.IsAny<Domain.Entities.TestType>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Domain.Entities.TestType { Id = 1, Name = "New Test Type" });

        // Act
        var result = await _pageModel.OnPostAsync();

        // Assert
        var redirectResult = Assert.IsType<RedirectToPageResult>(result);
        Assert.Equal("Index", redirectResult.PageName);
    }

    [Fact]
    public async Task OnPostAsync_WhenDuplicateName_ShouldReturnPageWithError()
    {
        // Arrange
        _pageModel.Name = "Duplicate Test";
        _mockTestTypeService.Setup(s => s.CreateAsync(It.IsAny<Domain.Entities.TestType>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Test type with name 'Duplicate Test' already exists"));

        // Act
        var result = await _pageModel.OnPostAsync();

        // Assert
        Assert.IsType<PageResult>(result);
        Assert.NotNull(_pageModel.ErrorMessage);
    }

    [Fact]
    public async Task OnPostAsync_WhenExceptionThrown_ShouldReturnPageWithError()
    {
        // Arrange
        _pageModel.Name = "Test";
        _mockTestTypeService.Setup(s => s.CreateAsync(It.IsAny<Domain.Entities.TestType>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _pageModel.OnPostAsync();

        // Assert
        Assert.IsType<PageResult>(result);
        Assert.NotNull(_pageModel.ErrorMessage);
    }

    [Fact]
    public void Name_Property_SetAndGet_ShouldWork()
    {
        // Arrange
        var name = "Test Name";

        // Act
        _pageModel.Name = name;

        // Assert
        Assert.Equal(name, _pageModel.Name);
    }

    [Fact]
    public void Description_Property_SetAndGet_ShouldWork()
    {
        // Arrange
        var description = "Test Description";

        // Act
        _pageModel.Description = description;

        // Assert
        Assert.Equal(description, _pageModel.Description);
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
