using Xunit;
using Moq;
using DiagnosticCenter.Application.Services;
using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DiagnosticCenter.Application.Services.Tests;

public class AuthServiceTests
{
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<ILogger<AuthService>> _mockLogger;
    private readonly AuthService _service;

    public AuthServiceTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _mockLogger = new Mock<ILogger<AuthService>>();
        _service = new AuthService(_mockUserRepository.Object, _mockLogger.Object);
    }

    [Fact]
    public void Constructor_WithNullUserRepository_ShouldThrowArgumentNullException()
    {
        // Arrange, Act & Assert
        Assert.Throws<ArgumentNullException>(() => new AuthService(null!, _mockLogger.Object));
    }

    [Fact]
    public void Constructor_WithNullLogger_ShouldThrowArgumentNullException()
    {
        // Arrange, Act & Assert
        Assert.Throws<ArgumentNullException>(() => new AuthService(_mockUserRepository.Object, null!));
    }

    [Fact]
    public async Task AuthenticateAsync_WithValidCredentials_ShouldReturnSuccess()
    {
        // Arrange
        var password = "password123";
        var hashedPassword = await _service.HashPasswordAsync(password);
        var user = new User
        {
            Id = 1,
            Email = "test@example.com",
            PasswordHash = hashedPassword,
            IsActive = true
        };
        _mockUserRepository.Setup(r => r.GetByEmailAsync("test@example.com", It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        // Act
        var result = await _service.AuthenticateAsync("test@example.com", password);

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.User);
        Assert.Equal("Authentication successful", result.Message);
    }

    [Fact]
    public async Task AuthenticateAsync_WithEmptyEmail_ShouldReturnFailure()
    {
        // Act
        var result = await _service.AuthenticateAsync("", "password");

        // Assert
        Assert.False(result.Success);
        Assert.Null(result.User);
        Assert.Equal("Email and password are required", result.Message);
    }

    [Fact]
    public async Task AuthenticateAsync_WithEmptyPassword_ShouldReturnFailure()
    {
        // Act
        var result = await _service.AuthenticateAsync("test@example.com", "");

        // Assert
        Assert.False(result.Success);
        Assert.Null(result.User);
        Assert.Equal("Email and password are required", result.Message);
    }

    [Fact]
    public async Task AuthenticateAsync_WithNonExistentUser_ShouldReturnFailure()
    {
        // Arrange
        _mockUserRepository.Setup(r => r.GetByEmailAsync("nonexistent@example.com", It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        // Act
        var result = await _service.AuthenticateAsync("nonexistent@example.com", "password");

        // Assert
        Assert.False(result.Success);
        Assert.Null(result.User);
        Assert.Equal("Invalid email or password", result.Message);
    }

    [Fact]
    public async Task AuthenticateAsync_WithInactiveUser_ShouldReturnFailure()
    {
        // Arrange
        var user = new User
        {
            Id = 1,
            Email = "inactive@example.com",
            PasswordHash = "hash",
            IsActive = false
        };
        _mockUserRepository.Setup(r => r.GetByEmailAsync("inactive@example.com", It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        // Act
        var result = await _service.AuthenticateAsync("inactive@example.com", "password");

        // Assert
        Assert.False(result.Success);
        Assert.Null(result.User);
        Assert.Equal("Account is inactive", result.Message);
    }

    [Fact]
    public async Task AuthenticateAsync_WithInvalidPassword_ShouldReturnFailure()
    {
        // Arrange
        var hashedPassword = await _service.HashPasswordAsync("correctpassword");
        var user = new User
        {
            Id = 1,
            Email = "test@example.com",
            PasswordHash = hashedPassword,
            IsActive = true
        };
        _mockUserRepository.Setup(r => r.GetByEmailAsync("test@example.com", It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        // Act
        var result = await _service.AuthenticateAsync("test@example.com", "wrongpassword");

        // Assert
        Assert.False(result.Success);
        Assert.Null(result.User);
        Assert.Equal("Invalid email or password", result.Message);
    }

    [Fact]
    public async Task HashPasswordAsync_WithValidPassword_ShouldReturnHashedString()
    {
        // Arrange
        var password = "testpassword";

        // Act
        var hash = await _service.HashPasswordAsync(password);

        // Assert
        Assert.NotNull(hash);
        Assert.NotEmpty(hash);
        Assert.NotEqual(password, hash);
    }

    [Fact]
    public async Task HashPasswordAsync_WithEmptyPassword_ShouldThrowArgumentException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.HashPasswordAsync(""));
    }

    [Fact]
    public async Task HashPasswordAsync_WithNullPassword_ShouldThrowArgumentException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.HashPasswordAsync(null!));
    }

    [Fact]
    public async Task HashPasswordAsync_SamePassword_ShouldGenerateSameHash()
    {
        // Arrange
        var password = "testpassword";

        // Act
        var hash1 = await _service.HashPasswordAsync(password);
        var hash2 = await _service.HashPasswordAsync(password);

        // Assert
        Assert.Equal(hash1, hash2);
    }

    [Fact]
    public async Task VerifyPasswordAsync_WithMatchingPassword_ShouldReturnTrue()
    {
        // Arrange
        var password = "testpassword";
        var hash = await _service.HashPasswordAsync(password);

        // Act
        var result = await _service.VerifyPasswordAsync(password, hash);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task VerifyPasswordAsync_WithNonMatchingPassword_ShouldReturnFalse()
    {
        // Arrange
        var password = "testpassword";
        var wrongPassword = "wrongpassword";
        var hash = await _service.HashPasswordAsync(password);

        // Act
        var result = await _service.VerifyPasswordAsync(wrongPassword, hash);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task VerifyPasswordAsync_WithEmptyPassword_ShouldReturnFalse()
    {
        // Arrange
        var hash = await _service.HashPasswordAsync("testpassword");

        // Act
        var result = await _service.VerifyPasswordAsync("", hash);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task VerifyPasswordAsync_WithEmptyHash_ShouldReturnFalse()
    {
        // Act
        var result = await _service.VerifyPasswordAsync("password", "");

        // Assert
        Assert.False(result);
    }
}
