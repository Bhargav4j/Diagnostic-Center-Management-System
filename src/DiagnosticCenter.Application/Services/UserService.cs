using AutoMapper;
using BCrypt.Net;
using DiagnosticCenter.Application.DTOs;
using DiagnosticCenter.Application.Interfaces;
using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace DiagnosticCenter.Application.Services;

/// <summary>
/// Service implementation for user operations.
/// Provides business logic for managing users with password hashing, validation, error handling, and logging.
/// </summary>
public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<UserService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserService"/> class.
    /// </summary>
    /// <param name="repository">The user repository.</param>
    /// <param name="mapper">The AutoMapper instance.</param>
    /// <param name="logger">The logger instance.</param>
    /// <exception cref="ArgumentNullException">Thrown when any parameter is null.</exception>
    public UserService(
        IUserRepository repository,
        IMapper mapper,
        ILogger<UserService> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<IEnumerable<UserDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving all users");

            var users = await _repository.GetAllAsync(cancellationToken);
            var userDtos = _mapper.Map<IEnumerable<UserDto>>(users);

            _logger.LogInformation("Successfully retrieved {Count} users", userDtos.Count());

            return userDtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving all users");
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<UserDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving user with ID: {Id}", id);

            if (id <= 0)
            {
                _logger.LogWarning("Invalid user ID provided: {Id}", id);
                throw new ArgumentException("User ID must be greater than zero.", nameof(id));
            }

            var user = await _repository.GetByIdAsync(id, cancellationToken);

            if (user == null)
            {
                _logger.LogWarning("User with ID {Id} not found", id);
                return null;
            }

            var userDto = _mapper.Map<UserDto>(user);

            _logger.LogInformation("Successfully retrieved user with ID: {Id}", id);

            return userDto;
        }
        catch (ArgumentException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving user with ID: {Id}", id);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<UserDto> CreateAsync(UserCreateDto createDto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating new user with email: {Email}", createDto.Email);

            if (createDto == null)
            {
                _logger.LogWarning("Null user create DTO provided");
                throw new ArgumentNullException(nameof(createDto), "User create data cannot be null.");
            }

            // Validate required fields
            if (string.IsNullOrWhiteSpace(createDto.Email))
            {
                _logger.LogWarning("Empty email provided");
                throw new ArgumentException("Email is required.", nameof(createDto));
            }

            if (string.IsNullOrWhiteSpace(createDto.Password))
            {
                _logger.LogWarning("Empty password provided");
                throw new ArgumentException("Password is required.", nameof(createDto));
            }

            if (createDto.Password != createDto.ConfirmPassword)
            {
                _logger.LogWarning("Password and confirmation password do not match");
                throw new ArgumentException("Password and confirmation password do not match.", nameof(createDto));
            }

            if (string.IsNullOrWhiteSpace(createDto.AccountType))
            {
                _logger.LogWarning("Empty account type provided");
                throw new ArgumentException("Account type is required.", nameof(createDto));
            }

            // Check if user with same email already exists
            var existingUsers = await _repository.SearchAsync(createDto.Email, cancellationToken);
            if (existingUsers.Any(u => u.Email.Equals(createDto.Email, StringComparison.OrdinalIgnoreCase)))
            {
                _logger.LogWarning("User with email {Email} already exists", createDto.Email);
                throw new InvalidOperationException($"User with email {createDto.Email} already exists.");
            }

            // Map DTO to entity
            var user = _mapper.Map<User>(createDto);

            // Hash the password using BCrypt
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(createDto.Password, BCrypt.Net.BCrypt.GenerateSalt(12));

            // Ensure audit fields are set
            user.CreatedDate = DateTime.UtcNow;
            user.IsActive = createDto.IsActive;

            // Save to repository
            var createdUser = await _repository.AddAsync(user, cancellationToken);

            // Map back to DTO
            var userDto = _mapper.Map<UserDto>(createdUser);

            _logger.LogInformation("Successfully created user with ID: {Id} and email: {Email}",
                createdUser.Id, createdUser.Email);

            return userDto;
        }
        catch (ArgumentNullException)
        {
            throw;
        }
        catch (ArgumentException)
        {
            throw;
        }
        catch (InvalidOperationException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating user with email: {Email}", createDto?.Email);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task UpdateAsync(int id, UserUpdateDto updateDto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating user with ID: {Id}", id);

            if (id <= 0)
            {
                _logger.LogWarning("Invalid user ID provided: {Id}", id);
                throw new ArgumentException("User ID must be greater than zero.", nameof(id));
            }

            if (updateDto == null)
            {
                _logger.LogWarning("Null user update DTO provided for ID: {Id}", id);
                throw new ArgumentNullException(nameof(updateDto), "User update data cannot be null.");
            }

            // Validate required fields
            if (string.IsNullOrWhiteSpace(updateDto.Email))
            {
                _logger.LogWarning("Empty email provided for ID: {Id}", id);
                throw new ArgumentException("Email is required.", nameof(updateDto));
            }

            if (string.IsNullOrWhiteSpace(updateDto.AccountType))
            {
                _logger.LogWarning("Empty account type provided for ID: {Id}", id);
                throw new ArgumentException("Account type is required.", nameof(updateDto));
            }

            // Validate password if provided
            if (!string.IsNullOrWhiteSpace(updateDto.Password))
            {
                if (updateDto.Password != updateDto.ConfirmPassword)
                {
                    _logger.LogWarning("Password and confirmation password do not match for ID: {Id}", id);
                    throw new ArgumentException("Password and confirmation password do not match.", nameof(updateDto));
                }
            }

            // Check if user exists
            var existingUser = await _repository.GetByIdAsync(id, cancellationToken);

            if (existingUser == null)
            {
                _logger.LogWarning("User with ID {Id} not found for update", id);
                throw new InvalidOperationException($"User with ID {id} not found.");
            }

            // Check if email is being changed to an existing email
            if (!existingUser.Email.Equals(updateDto.Email, StringComparison.OrdinalIgnoreCase))
            {
                var existingUsers = await _repository.SearchAsync(updateDto.Email, cancellationToken);
                if (existingUsers.Any(u => u.Email.Equals(updateDto.Email, StringComparison.OrdinalIgnoreCase) && u.Id != id))
                {
                    _logger.LogWarning("User with email {Email} already exists", updateDto.Email);
                    throw new InvalidOperationException($"User with email {updateDto.Email} already exists.");
                }
            }

            // Map updates to existing entity
            _mapper.Map(updateDto, existingUser);

            // Hash the password if provided
            if (!string.IsNullOrWhiteSpace(updateDto.Password))
            {
                existingUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(updateDto.Password, BCrypt.Net.BCrypt.GenerateSalt(12));
                _logger.LogInformation("Password updated for user ID: {Id}", id);
            }

            // Ensure audit fields are updated
            existingUser.ModifiedDate = DateTime.UtcNow;

            // Update in repository
            await _repository.UpdateAsync(existingUser, cancellationToken);

            _logger.LogInformation("Successfully updated user with ID: {Id}", id);
        }
        catch (ArgumentNullException)
        {
            throw;
        }
        catch (ArgumentException)
        {
            throw;
        }
        catch (InvalidOperationException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating user with ID: {Id}", id);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting user with ID: {Id}", id);

            if (id <= 0)
            {
                _logger.LogWarning("Invalid user ID provided: {Id}", id);
                throw new ArgumentException("User ID must be greater than zero.", nameof(id));
            }

            // Check if user exists
            var exists = await _repository.ExistsAsync(id, cancellationToken);

            if (!exists)
            {
                _logger.LogWarning("User with ID {Id} not found for deletion", id);
                throw new InvalidOperationException($"User with ID {id} not found.");
            }

            // Delete from repository
            await _repository.DeleteAsync(id, cancellationToken);

            _logger.LogInformation("Successfully deleted user with ID: {Id}", id);
        }
        catch (ArgumentException)
        {
            throw;
        }
        catch (InvalidOperationException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting user with ID: {Id}", id);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<IEnumerable<UserDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Searching users with term: {SearchTerm}", searchTerm);

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                _logger.LogWarning("Empty search term provided, returning all users");
                return await GetAllAsync(cancellationToken);
            }

            var users = await _repository.SearchAsync(searchTerm, cancellationToken);
            var userDtos = _mapper.Map<IEnumerable<UserDto>>(users);

            _logger.LogInformation("Successfully found {Count} users matching search term: {SearchTerm}",
                userDtos.Count(), searchTerm);

            return userDtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while searching users with term: {SearchTerm}", searchTerm);
            throw;
        }
    }
}
