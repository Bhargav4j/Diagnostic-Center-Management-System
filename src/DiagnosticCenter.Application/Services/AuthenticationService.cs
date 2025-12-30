using DiagnosticCenter.Domain.Interfaces.Repositories;
using DiagnosticCenter.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;

namespace DiagnosticCenter.Application.Services;

/// <summary>
/// Service for handling authentication operations
/// </summary>
public class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<AuthenticationService> _logger;

    public AuthenticationService(
        IUserRepository userRepository,
        ILogger<AuthenticationService> logger)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<(bool Success, string? Role, string? Message)> AuthenticateAsync(
        string email,
        string password,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Authentication attempt for email: {Email}", email);

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
                _logger.LogWarning("Inactive user attempted login: {Email}", email);
                return (false, null, "Account is inactive");
            }

            if (!VerifyPassword(password, user.PasswordHash))
            {
                _logger.LogWarning("Invalid password for user: {Email}", email);
                return (false, null, "Invalid email or password");
            }

            _logger.LogInformation("Successful authentication for user: {Email}, Role: {Role}", email, user.AccountType);
            return (true, user.AccountType, "Authentication successful");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during authentication for email: {Email}", email);
            throw;
        }
    }

    public string HashPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException("Password cannot be empty", nameof(password));
        }

        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }

    public bool VerifyPassword(string password, string hashedPassword)
    {
        if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(hashedPassword))
        {
            return false;
        }

        var hashOfInput = HashPassword(password);
        return hashOfInput == hashedPassword;
    }
}
