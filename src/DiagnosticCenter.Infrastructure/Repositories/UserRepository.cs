using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Interfaces.Repositories;
using DiagnosticCenter.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DiagnosticCenter.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for User entity operations.
/// Provides data access methods with error handling and logging.
/// </summary>
public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<UserRepository> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserRepository"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    /// <param name="logger">The logger instance.</param>
    public UserRepository(ApplicationDbContext context, ILogger<UserRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Retrieving all users");

            var users = await _context.Users
                .AsNoTracking()
                .OrderBy(u => u.Email)
                .ToListAsync(cancellationToken);

            _logger.LogInformation("Retrieved {Count} users", users.Count);
            return users;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving all users");
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Retrieving user with ID: {Id}", id);

            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

            if (user == null)
            {
                _logger.LogWarning("User with ID {Id} not found", id);
            }
            else
            {
                _logger.LogDebug("Successfully retrieved user with ID: {Id}", id);
            }

            return user;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving user with ID: {Id}", id);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<User> AddAsync(User entity, CancellationToken cancellationToken = default)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        try
        {
            _logger.LogDebug("Adding new user: {Email}", entity.Email);

            // Check if user with same email already exists
            var existingUser = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == entity.Email, cancellationToken);

            if (existingUser != null)
            {
                _logger.LogWarning("User with email {Email} already exists", entity.Email);
                throw new InvalidOperationException($"User with email {entity.Email} already exists");
            }

            entity.CreatedDate = DateTime.UtcNow;
            entity.IsActive = true;

            await _context.Users.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully added user with ID: {Id}, Email: {Email}", entity.Id, entity.Email);
            return entity;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error occurred while adding user: {Email}", entity.Email);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while adding user: {Email}", entity.Email);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task UpdateAsync(User entity, CancellationToken cancellationToken = default)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        try
        {
            _logger.LogDebug("Updating user with ID: {Id}", entity.Id);

            var existingEntity = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == entity.Id, cancellationToken);

            if (existingEntity == null)
            {
                _logger.LogWarning("User with ID {Id} not found for update", entity.Id);
                throw new InvalidOperationException($"User with ID {entity.Id} not found");
            }

            // Check if email is being changed to an existing email
            if (existingEntity.Email != entity.Email)
            {
                var emailExists = await _context.Users
                    .AsNoTracking()
                    .AnyAsync(u => u.Email == entity.Email && u.Id != entity.Id, cancellationToken);

                if (emailExists)
                {
                    _logger.LogWarning("Email {Email} is already in use by another user", entity.Email);
                    throw new InvalidOperationException($"Email {entity.Email} is already in use");
                }
            }

            // Update properties
            existingEntity.Email = entity.Email;
            existingEntity.PasswordHash = entity.PasswordHash;
            existingEntity.AccountType = entity.AccountType;
            existingEntity.ModifiedDate = DateTime.UtcNow;
            existingEntity.ModifiedBy = entity.ModifiedBy;
            existingEntity.IsActive = entity.IsActive;

            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully updated user with ID: {Id}", entity.Id);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError(ex, "Concurrency error occurred while updating user with ID: {Id}", entity.Id);
            throw;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error occurred while updating user with ID: {Id}", entity.Id);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating user with ID: {Id}", entity.Id);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Deleting user with ID: {Id}", id);

            var entity = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

            if (entity == null)
            {
                _logger.LogWarning("User with ID {Id} not found for deletion", id);
                throw new InvalidOperationException($"User with ID {id} not found");
            }

            // Soft delete
            entity.IsActive = false;
            entity.ModifiedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully deleted (soft) user with ID: {Id}", id);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error occurred while deleting user with ID: {Id}", id);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting user with ID: {Id}", id);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Checking existence of user with ID: {Id}", id);

            var exists = await _context.Users
                .AsNoTracking()
                .AnyAsync(u => u.Id == id, cancellationToken);

            _logger.LogDebug("User with ID {Id} exists: {Exists}", id, exists);
            return exists;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while checking existence of user with ID: {Id}", id);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<IEnumerable<User>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Searching users with term: {SearchTerm}", searchTerm);

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return await GetAllAsync(cancellationToken);
            }

            var normalizedSearchTerm = searchTerm.ToLower().Trim();

            var users = await _context.Users
                .AsNoTracking()
                .Where(u => u.Email.ToLower().Contains(normalizedSearchTerm) ||
                           u.AccountType.ToLower().Contains(normalizedSearchTerm))
                .OrderBy(u => u.Email)
                .ToListAsync(cancellationToken);

            _logger.LogInformation("Found {Count} users matching search term: {SearchTerm}", users.Count, searchTerm);
            return users;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while searching users with term: {SearchTerm}", searchTerm);
            throw;
        }
    }
}
