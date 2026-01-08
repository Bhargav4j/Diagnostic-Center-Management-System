namespace DiagnosticCenter.Domain.Interfaces.Services;

/// <summary>
/// Service interface for authentication operations
/// </summary>
public interface IAuthenticationService
{
    Task<object?> AuthenticateAsync(string email, string password, CancellationToken cancellationToken = default);

    Task<object> RegisterAsync(object dto, CancellationToken cancellationToken = default);

    Task<bool> ChangePasswordAsync(int userId, string oldPassword, string newPassword, CancellationToken cancellationToken = default);
}
