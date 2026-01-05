using DiagnosticCenter.Domain.Entities;

namespace DiagnosticCenter.Domain.Interfaces.Services;

/// <summary>
/// Service interface for authentication operations
/// </summary>
public interface IAuthenticationService
{
    Task<User?> AuthenticateAsync(string email, string password, CancellationToken cancellationToken = default);
    Task<User> RegisterAsync(string email, string password, string accountType, string fullName, CancellationToken cancellationToken = default);
    Task<bool> ChangePasswordAsync(int userId, string oldPassword, string newPassword, CancellationToken cancellationToken = default);
    string HashPassword(string password);
    bool VerifyPassword(string password, string passwordHash);
}
