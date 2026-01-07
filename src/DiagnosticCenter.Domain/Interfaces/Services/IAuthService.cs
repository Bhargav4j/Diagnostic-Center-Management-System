using DiagnosticCenter.Domain.Entities;

namespace DiagnosticCenter.Domain.Interfaces.Services;

/// <summary>
/// Service interface for authentication operations
/// </summary>
public interface IAuthService
{
    Task<(bool Success, User? User, string Message)> AuthenticateAsync(string email, string password, CancellationToken cancellationToken = default);
    Task<string> HashPasswordAsync(string password);
    Task<bool> VerifyPasswordAsync(string password, string passwordHash);
}
