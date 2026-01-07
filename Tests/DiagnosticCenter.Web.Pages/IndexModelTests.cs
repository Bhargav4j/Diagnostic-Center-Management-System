using Xunit;
using Moq;
using DiagnosticCenter.Web.Pages;
using DiagnosticCenter.Domain.Interfaces.Services;
using DiagnosticCenter.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DiagnosticCenter.Web.Pages.Tests;

public class IndexModelTests
{
    private readonly Mock<IAuthService> _mockAuthService;
    private readonly Mock<ILogger<IndexModel>> _mockLogger;
    private readonly IndexModel _pageModel;

    public IndexModelTests()
    {
        _mockAuthService = new Mock<IAuthService>();
        _mockLogger = new Mock<ILogger<IndexModel>>();
        _pageModel = new IndexModel(_mockAuthService.Object, _mockLogger.Object);

        // Mock HttpContext
        var httpContext = new DefaultHttpContext();
        var tempData = new Mock<Microsoft.AspNetCore.Mvc.ViewFeatures.ITempDataDictionary>();
        _pageModel.PageContext = new PageContext
        {
            HttpContext = httpContext
        };
        _pageModel.TempData = tempData.Object;
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
        _pageModel.ModelState.AddModelError("Email", "Email is required");

        // Act
        var result = await _pageModel.OnPostAsync();

        // Assert
        Assert.IsType<PageResult>(result);
    }

    [Fact]
    public async Task OnPostAsync_WithValidCredentials_ShouldRedirectToAdminIndex()
    {
        // Arrange
        var user = new User { Id = 1, Email = "admin@test.com", AccountType = "Admin", IsActive = true };
        _mockAuthService.Setup(s => s.AuthenticateAsync("admin@test.com", "password", It.IsAny<CancellationToken>()))
            .ReturnsAsync((true, user, "Authentication successful"));

        _pageModel.Email = "admin@test.com";
        _pageModel.Password = "password";

        // Mock authentication service
        var authServiceMock = new Mock<IAuthenticationService>();
        authServiceMock
            .Setup(a => a.SignInAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<System.Security.Claims.ClaimsPrincipal>(), It.IsAny<AuthenticationProperties>()))
            .Returns(Task.CompletedTask);

        var serviceProviderMock = new Mock<IServiceProvider>();
        serviceProviderMock
            .Setup(sp => sp.GetService(typeof(IAuthenticationService)))
            .Returns(authServiceMock.Object);

        _pageModel.PageContext.HttpContext.RequestServices = serviceProviderMock.Object;

        // Act
        var result = await _pageModel.OnPostAsync();

        // Assert
        var redirectResult = Assert.IsType<RedirectToPageResult>(result);
        Assert.Equal("/Admin/Index", redirectResult.PageName);
    }

    [Fact]
    public async Task OnPostAsync_WithInvalidCredentials_ShouldReturnPageWithError()
    {
        // Arrange
        _mockAuthService.Setup(s => s.AuthenticateAsync("wrong@test.com", "wrongpassword", It.IsAny<CancellationToken>()))
            .ReturnsAsync((false, null, "Invalid email or password"));

        _pageModel.Email = "wrong@test.com";
        _pageModel.Password = "wrongpassword";

        // Act
        var result = await _pageModel.OnPostAsync();

        // Assert
        Assert.IsType<PageResult>(result);
        Assert.Equal("Invalid email or password", _pageModel.ErrorMessage);
    }

    [Fact]
    public async Task OnPostAsync_WhenExceptionThrown_ShouldReturnPageWithError()
    {
        // Arrange
        _mockAuthService.Setup(s => s.AuthenticateAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database error"));

        _pageModel.Email = "test@test.com";
        _pageModel.Password = "password";

        // Act
        var result = await _pageModel.OnPostAsync();

        // Assert
        Assert.IsType<PageResult>(result);
        Assert.NotNull(_pageModel.ErrorMessage);
    }

    [Fact]
    public void Email_Property_SetAndGet_ShouldWork()
    {
        // Arrange
        var expectedEmail = "test@example.com";

        // Act
        _pageModel.Email = expectedEmail;

        // Assert
        Assert.Equal(expectedEmail, _pageModel.Email);
    }

    [Fact]
    public void Password_Property_SetAndGet_ShouldWork()
    {
        // Arrange
        var expectedPassword = "password123";

        // Act
        _pageModel.Password = expectedPassword;

        // Assert
        Assert.Equal(expectedPassword, _pageModel.Password);
    }

    [Fact]
    public void ErrorMessage_Property_SetAndGet_ShouldWork()
    {
        // Arrange
        var expectedMessage = "Error occurred";

        // Act
        _pageModel.ErrorMessage = expectedMessage;

        // Assert
        Assert.Equal(expectedMessage, _pageModel.ErrorMessage);
    }
}
