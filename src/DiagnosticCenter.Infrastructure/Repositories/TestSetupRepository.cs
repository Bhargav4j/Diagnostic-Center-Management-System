using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Interfaces.Repositories;
using DiagnosticCenter.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DiagnosticCenter.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for TestSetup entity operations.
/// Provides data access methods with error handling and logging.
/// </summary>
public class TestSetupRepository : ITestSetupRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<TestSetupRepository> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="TestSetupRepository"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    /// <param name="logger">The logger instance.</param>
    public TestSetupRepository(ApplicationDbContext context, ILogger<TestSetupRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<IEnumerable<TestSetup>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Retrieving all test setups");

            var testSetups = await _context.TestSetups
                .AsNoTracking()
                .Include(ts => ts.TestType)
                .OrderBy(ts => ts.Name)
                .ToListAsync(cancellationToken);

            _logger.LogInformation("Retrieved {Count} test setups", testSetups.Count);
            return testSetups;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving all test setups");
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<TestSetup?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Retrieving test setup with ID: {Id}", id);

            var testSetup = await _context.TestSetups
                .AsNoTracking()
                .Include(ts => ts.TestType)
                .Include(ts => ts.TestEntries)
                .FirstOrDefaultAsync(ts => ts.Id == id, cancellationToken);

            if (testSetup == null)
            {
                _logger.LogWarning("Test setup with ID {Id} not found", id);
            }
            else
            {
                _logger.LogDebug("Successfully retrieved test setup with ID: {Id}", id);
            }

            return testSetup;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving test setup with ID: {Id}", id);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<TestSetup> AddAsync(TestSetup entity, CancellationToken cancellationToken = default)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        try
        {
            _logger.LogDebug("Adding new test setup: {Name}", entity.Name);

            entity.CreatedDate = DateTime.UtcNow;
            entity.IsActive = true;

            await _context.TestSetups.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully added test setup with ID: {Id}, Name: {Name}", entity.Id, entity.Name);
            return entity;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error occurred while adding test setup: {Name}", entity.Name);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while adding test setup: {Name}", entity.Name);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task UpdateAsync(TestSetup entity, CancellationToken cancellationToken = default)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        try
        {
            _logger.LogDebug("Updating test setup with ID: {Id}", entity.Id);

            var existingEntity = await _context.TestSetups
                .FirstOrDefaultAsync(ts => ts.Id == entity.Id, cancellationToken);

            if (existingEntity == null)
            {
                _logger.LogWarning("Test setup with ID {Id} not found for update", entity.Id);
                throw new InvalidOperationException($"Test setup with ID {entity.Id} not found");
            }

            // Update properties
            existingEntity.Name = entity.Name;
            existingEntity.Fee = entity.Fee;
            existingEntity.TypeId = entity.TypeId;
            existingEntity.ModifiedDate = DateTime.UtcNow;
            existingEntity.ModifiedBy = entity.ModifiedBy;
            existingEntity.IsActive = entity.IsActive;

            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully updated test setup with ID: {Id}", entity.Id);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError(ex, "Concurrency error occurred while updating test setup with ID: {Id}", entity.Id);
            throw;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error occurred while updating test setup with ID: {Id}", entity.Id);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating test setup with ID: {Id}", entity.Id);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Deleting test setup with ID: {Id}", id);

            var entity = await _context.TestSetups
                .FirstOrDefaultAsync(ts => ts.Id == id, cancellationToken);

            if (entity == null)
            {
                _logger.LogWarning("Test setup with ID {Id} not found for deletion", id);
                throw new InvalidOperationException($"Test setup with ID {id} not found");
            }

            // Soft delete
            entity.IsActive = false;
            entity.ModifiedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully deleted (soft) test setup with ID: {Id}", id);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error occurred while deleting test setup with ID: {Id}", id);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting test setup with ID: {Id}", id);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Checking existence of test setup with ID: {Id}", id);

            var exists = await _context.TestSetups
                .AsNoTracking()
                .AnyAsync(ts => ts.Id == id, cancellationToken);

            _logger.LogDebug("Test setup with ID {Id} exists: {Exists}", id, exists);
            return exists;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while checking existence of test setup with ID: {Id}", id);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<IEnumerable<TestSetup>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Searching test setups with term: {SearchTerm}", searchTerm);

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return await GetAllAsync(cancellationToken);
            }

            var normalizedSearchTerm = searchTerm.ToLower().Trim();

            var testSetups = await _context.TestSetups
                .AsNoTracking()
                .Include(ts => ts.TestType)
                .Where(ts => ts.Name.ToLower().Contains(normalizedSearchTerm) ||
                            (ts.TestType != null && ts.TestType.Name.ToLower().Contains(normalizedSearchTerm)))
                .OrderBy(ts => ts.Name)
                .ToListAsync(cancellationToken);

            _logger.LogInformation("Found {Count} test setups matching search term: {SearchTerm}", testSetups.Count, searchTerm);
            return testSetups;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while searching test setups with term: {SearchTerm}", searchTerm);
            throw;
        }
    }
}
