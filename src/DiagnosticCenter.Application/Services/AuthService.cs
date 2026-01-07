using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Interfaces.Repositories;
using DiagnosticCenter.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;

namespace DiagnosticCenter.Application.Services;

/// <summary>
/// Service implementation for authentication operations
/// </summary>
public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<AuthService> _logger;

    public AuthService(IUserRepository userRepository, ILogger<AuthService> logger)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<(bool Success, User? User, string Message)> AuthenticateAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Authenticating user with email: {Email}", email);

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                return (false, null, "Email and password are required");
            }

            var user = await _userRepository.GetByEmailAsync(email, cancellationToken);
            if (user == null)
            {
                _logger.LogWarning("User not found with email: {Email}", email);
                return (false, null, "Invalid email or password");
            }

            if (!user.IsActive)
            {
                _logger.LogWarning("User account is inactive: {Email}", email);
                return (false, null, "Account is inactive");
            }

            var isPasswordValid = await VerifyPasswordAsync(password, user.PasswordHash);
            if (!isPasswordValid)
            {
                _logger.LogWarning("Invalid password for user: {Email}", email);
                return (false, null, "Invalid email or password");
            }

            _logger.LogInformation("User authenticated successfully: {Email}", email);
            return (true, user, "Authentication successful");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error authenticating user with email: {Email}", email);
            throw;
        }
    }

    public Task<string> HashPasswordAsync(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException("Password cannot be null or empty", nameof(password));
        }

        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Task.FromResult(Convert.ToBase64String(hashedBytes));
    }

    public Task<bool> VerifyPasswordAsync(string password, string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(passwordHash))
        {
            return Task.FromResult(false);
        }

        var hashedPassword = HashPasswordAsync(password).Result;
        return Task.FromResult(hashedPassword == passwordHash);
    }
}
