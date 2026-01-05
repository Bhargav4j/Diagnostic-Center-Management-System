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
public class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<AuthenticationService> _logger;

    public AuthenticationService(IUserRepository userRepository, ILogger<AuthenticationService> logger)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<User?> AuthenticateAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                _logger.LogWarning("Authentication failed: Email or password is empty");
                return null;
            }

            _logger.LogInformation("Authenticating user with email: {Email}", email);

            var user = await _userRepository.GetByEmailAsync(email, cancellationToken);

            if (user == null)
            {
                _logger.LogWarning("Authentication failed: User with email {Email} not found", email);
                return null;
            }

            if (!user.IsActive)
            {
                _logger.LogWarning("Authentication failed: User account is inactive");
                return null;
            }

            if (!VerifyPassword(password, user.PasswordHash))
            {
                _logger.LogWarning("Authentication failed: Invalid password for email {Email}", email);
                return null;
            }

            _logger.LogInformation("User {Email} authenticated successfully", email);
            return user;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during authentication for email: {Email}", email);
            throw;
        }
    }

    public async Task<User> RegisterAsync(string email, string password, string accountType, string fullName, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Email cannot be empty", nameof(email));
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Password cannot be empty", nameof(password));
            }

            if (string.IsNullOrWhiteSpace(accountType))
            {
                throw new ArgumentException("Account type cannot be empty", nameof(accountType));
            }

            var exists = await _userRepository.ExistsByEmailAsync(email, cancellationToken);
            if (exists)
            {
                throw new InvalidOperationException($"User with email {email} already exists");
            }

            _logger.LogInformation("Registering new user with email: {Email}", email);

            var user = new User
            {
                Email = email,
                PasswordHash = HashPassword(password),
                AccountType = accountType,
                FullName = fullName,
                CreatedDate = DateTime.UtcNow,
                IsActive = true,
                CreatedBy = "System"
            };

            var result = await _userRepository.AddAsync(user, cancellationToken);
            _logger.LogInformation("User {Email} registered successfully with ID {Id}", email, result.Id);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error registering user with email: {Email}", email);
            throw;
        }
    }

    public async Task<bool> ChangePasswordAsync(int userId, string oldPassword, string newPassword, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(oldPassword) || string.IsNullOrWhiteSpace(newPassword))
            {
                throw new ArgumentException("Old password and new password cannot be empty");
            }

            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
            if (user == null)
            {
                throw new InvalidOperationException($"User with ID {userId} not found");
            }

            if (!VerifyPassword(oldPassword, user.PasswordHash))
            {
                _logger.LogWarning("Change password failed: Invalid old password for user ID {UserId}", userId);
                return false;
            }

            _logger.LogInformation("Changing password for user ID {UserId}", userId);
            user.PasswordHash = HashPassword(newPassword);
            user.ModifiedDate = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user, cancellationToken);
            _logger.LogInformation("Password changed successfully for user ID {UserId}", userId);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error changing password for user ID {UserId}", userId);
            throw;
        }
    }

    public string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }

    public bool VerifyPassword(string password, string passwordHash)
    {
        var hash = HashPassword(password);
        return hash == passwordHash;
    }
}
