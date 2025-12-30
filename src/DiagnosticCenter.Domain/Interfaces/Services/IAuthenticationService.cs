namespace DiagnosticCenter.Domain.Interfaces.Services;

/// <summary>
/// Authentication service interface
/// </summary>
public interface IAuthenticationService
{
    Task<(bool Success, string? Role, string? Message)> AuthenticateAsync(string email, string password, CancellationToken cancellationToken = default);
    string HashPassword(string password);
    bool VerifyPassword(string password, string hashedPassword);
}
