using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Interfaces.Repositories;
using DiagnosticCenter.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DiagnosticCenter.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for TestType entity operations.
/// Provides data access methods with error handling and logging.
/// </summary>
public class TestTypeRepository : ITestTypeRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<TestTypeRepository> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="TestTypeRepository"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    /// <param name="logger">The logger instance.</param>
    public TestTypeRepository(ApplicationDbContext context, ILogger<TestTypeRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<IEnumerable<TestType>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Retrieving all test types");

            var testTypes = await _context.TestTypes
                .AsNoTracking()
                .OrderBy(t => t.Name)
                .ToListAsync(cancellationToken);

            _logger.LogInformation("Retrieved {Count} test types", testTypes.Count);
            return testTypes;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving all test types");
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<TestType?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Retrieving test type with ID: {Id}", id);

            var testType = await _context.TestTypes
                .AsNoTracking()
                .Include(t => t.TestSetups)
                .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

            if (testType == null)
            {
                _logger.LogWarning("Test type with ID {Id} not found", id);
            }
            else
            {
                _logger.LogDebug("Successfully retrieved test type with ID: {Id}", id);
            }

            return testType;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving test type with ID: {Id}", id);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<TestType> AddAsync(TestType entity, CancellationToken cancellationToken = default)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        try
        {
            _logger.LogDebug("Adding new test type: {Name}", entity.Name);

            entity.CreatedDate = DateTime.UtcNow;
            entity.IsActive = true;

            await _context.TestTypes.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully added test type with ID: {Id}, Name: {Name}", entity.Id, entity.Name);
            return entity;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error occurred while adding test type: {Name}", entity.Name);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while adding test type: {Name}", entity.Name);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task UpdateAsync(TestType entity, CancellationToken cancellationToken = default)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        try
        {
            _logger.LogDebug("Updating test type with ID: {Id}", entity.Id);

            var existingEntity = await _context.TestTypes
                .FirstOrDefaultAsync(t => t.Id == entity.Id, cancellationToken);

            if (existingEntity == null)
            {
                _logger.LogWarning("Test type with ID {Id} not found for update", entity.Id);
                throw new InvalidOperationException($"Test type with ID {entity.Id} not found");
            }

            // Update properties
            existingEntity.Name = entity.Name;
            existingEntity.Description = entity.Description;
            existingEntity.ModifiedDate = DateTime.UtcNow;
            existingEntity.ModifiedBy = entity.ModifiedBy;
            existingEntity.IsActive = entity.IsActive;

            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully updated test type with ID: {Id}", entity.Id);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError(ex, "Concurrency error occurred while updating test type with ID: {Id}", entity.Id);
            throw;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error occurred while updating test type with ID: {Id}", entity.Id);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating test type with ID: {Id}", entity.Id);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Deleting test type with ID: {Id}", id);

            var entity = await _context.TestTypes
                .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

            if (entity == null)
            {
                _logger.LogWarning("Test type with ID {Id} not found for deletion", id);
                throw new InvalidOperationException($"Test type with ID {id} not found");
            }

            // Soft delete
            entity.IsActive = false;
            entity.ModifiedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully deleted (soft) test type with ID: {Id}", id);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error occurred while deleting test type with ID: {Id}", id);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting test type with ID: {Id}", id);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Checking existence of test type with ID: {Id}", id);

            var exists = await _context.TestTypes
                .AsNoTracking()
                .AnyAsync(t => t.Id == id, cancellationToken);

            _logger.LogDebug("Test type with ID {Id} exists: {Exists}", id, exists);
            return exists;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while checking existence of test type with ID: {Id}", id);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<IEnumerable<TestType>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Searching test types with term: {SearchTerm}", searchTerm);

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return await GetAllAsync(cancellationToken);
            }

            var normalizedSearchTerm = searchTerm.ToLower().Trim();

            var testTypes = await _context.TestTypes
                .AsNoTracking()
                .Where(t => t.Name.ToLower().Contains(normalizedSearchTerm) ||
                           (t.Description != null && t.Description.ToLower().Contains(normalizedSearchTerm)))
                .OrderBy(t => t.Name)
                .ToListAsync(cancellationToken);

            _logger.LogInformation("Found {Count} test types matching search term: {SearchTerm}", testTypes.Count, searchTerm);
            return testTypes;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while searching test types with term: {SearchTerm}", searchTerm);
            throw;
        }
    }
}
