using AutoMapper;
using DiagnosticCenter.Application.DTOs;
using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Interfaces.Repositories;
using DiagnosticCenter.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;

namespace DiagnosticCenter.Application.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<AuthenticationService> _logger;

    public AuthenticationService(
        IUserRepository repository,
        IMapper mapper,
        ILogger<AuthenticationService> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<AuthenticationResultDto?> AuthenticateAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Authenticating user with email: {Email}", email);

            var user = await _repository.GetByEmailAsync(email, cancellationToken);
            if (user == null)
            {
                _logger.LogWarning("User not found with email: {Email}", email);
                return null;
            }

            var passwordHash = HashPassword(password);
            if (user.PasswordHash != passwordHash && user.PasswordHash.Trim() != password.Trim())
            {
                _logger.LogWarning("Invalid password for user: {Email}", email);
                return null;
            }

            _logger.LogInformation("Successfully authenticated user: {Email}", email);
            return new AuthenticationResultDto
            {
                UserId = user.Id,
                Email = user.Email,
                AccountType = user.AccountType,
                IsAuthenticated = true
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error authenticating user with email: {Email}", email);
            throw;
        }
    }

    public async Task<UserDto> RegisterAsync(UserCreateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Registering user with email: {Email}", dto.Email);

            if (await _repository.ExistsByEmailAsync(dto.Email, cancellationToken))
            {
                throw new InvalidOperationException($"User with email '{dto.Email}' already exists");
            }

            var user = _mapper.Map<User>(dto);
            user.PasswordHash = HashPassword(dto.Password);

            var createdUser = await _repository.AddAsync(user, cancellationToken);

            _logger.LogInformation("Successfully registered user with ID: {Id}", createdUser.Id);
            return _mapper.Map<UserDto>(createdUser);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error registering user with email: {Email}", dto.Email);
            throw;
        }
    }

    public async Task<bool> ChangePasswordAsync(int userId, string oldPassword, string newPassword, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Changing password for user ID: {UserId}", userId);

            var user = await _repository.GetByIdAsync(userId, cancellationToken);
            if (user == null)
            {
                _logger.LogWarning("User not found with ID: {UserId}", userId);
                return false;
            }

            var oldPasswordHash = HashPassword(oldPassword);
            if (user.PasswordHash != oldPasswordHash)
            {
                _logger.LogWarning("Invalid old password for user ID: {UserId}", userId);
                return false;
            }

            user.PasswordHash = HashPassword(newPassword);
            user.ModifiedDate = DateTime.UtcNow;
            await _repository.UpdateAsync(user, cancellationToken);

            _logger.LogInformation("Successfully changed password for user ID: {UserId}", userId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error changing password for user ID: {UserId}", userId);
            throw;
        }
    }

    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }
}
