using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Interfaces.Repositories;
using DiagnosticCenter.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DiagnosticCenter.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for TestEntry entity operations.
/// Provides data access methods with error handling and logging.
/// </summary>
public class TestEntryRepository : ITestEntryRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<TestEntryRepository> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="TestEntryRepository"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    /// <param name="logger">The logger instance.</param>
    public TestEntryRepository(ApplicationDbContext context, ILogger<TestEntryRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<IEnumerable<TestEntry>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Retrieving all test entries");

            var testEntries = await _context.TestEntries
                .AsNoTracking()
                .Include(te => te.TestSetup)
                    .ThenInclude(ts => ts!.TestType)
                .OrderByDescending(te => te.CreatedDate)
                .ToListAsync(cancellationToken);

            _logger.LogInformation("Retrieved {Count} test entries", testEntries.Count);
            return testEntries;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving all test entries");
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<TestEntry?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Retrieving test entry with ID: {Id}", id);

            var testEntry = await _context.TestEntries
                .AsNoTracking()
                .Include(te => te.TestSetup)
                    .ThenInclude(ts => ts!.TestType)
                .Include(te => te.Payments)
                .FirstOrDefaultAsync(te => te.Id == id, cancellationToken);

            if (testEntry == null)
            {
                _logger.LogWarning("Test entry with ID {Id} not found", id);
            }
            else
            {
                _logger.LogDebug("Successfully retrieved test entry with ID: {Id}", id);
            }

            return testEntry;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving test entry with ID: {Id}", id);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<TestEntry> AddAsync(TestEntry entity, CancellationToken cancellationToken = default)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        try
        {
            _logger.LogDebug("Adding new test entry for patient: {PatientName}", entity.PatientName);

            entity.CreatedDate = DateTime.UtcNow;
            entity.IsActive = true;

            await _context.TestEntries.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully added test entry with ID: {Id}, BillNo: {BillNo}", entity.Id, entity.BillNo);
            return entity;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error occurred while adding test entry for patient: {PatientName}", entity.PatientName);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while adding test entry for patient: {PatientName}", entity.PatientName);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task UpdateAsync(TestEntry entity, CancellationToken cancellationToken = default)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        try
        {
            _logger.LogDebug("Updating test entry with ID: {Id}", entity.Id);

            var existingEntity = await _context.TestEntries
                .FirstOrDefaultAsync(te => te.Id == entity.Id, cancellationToken);

            if (existingEntity == null)
            {
                _logger.LogWarning("Test entry with ID {Id} not found for update", entity.Id);
                throw new InvalidOperationException($"Test entry with ID {entity.Id} not found");
            }

            // Update properties
            existingEntity.PatientName = entity.PatientName;
            existingEntity.DateOfBirth = entity.DateOfBirth;
            existingEntity.MobileNo = entity.MobileNo;
            existingEntity.BillNo = entity.BillNo;
            existingEntity.TotalAmount = entity.TotalAmount;
            existingEntity.DueDate = entity.DueDate;
            existingEntity.PaidAmount = entity.PaidAmount;
            existingEntity.TestId = entity.TestId;
            existingEntity.ModifiedDate = DateTime.UtcNow;
            existingEntity.ModifiedBy = entity.ModifiedBy;
            existingEntity.IsActive = entity.IsActive;

            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully updated test entry with ID: {Id}", entity.Id);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError(ex, "Concurrency error occurred while updating test entry with ID: {Id}", entity.Id);
            throw;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error occurred while updating test entry with ID: {Id}", entity.Id);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating test entry with ID: {Id}", entity.Id);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Deleting test entry with ID: {Id}", id);

            var entity = await _context.TestEntries
                .FirstOrDefaultAsync(te => te.Id == id, cancellationToken);

            if (entity == null)
            {
                _logger.LogWarning("Test entry with ID {Id} not found for deletion", id);
                throw new InvalidOperationException($"Test entry with ID {id} not found");
            }

            // Soft delete
            entity.IsActive = false;
            entity.ModifiedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully deleted (soft) test entry with ID: {Id}", id);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error occurred while deleting test entry with ID: {Id}", id);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting test entry with ID: {Id}", id);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Checking existence of test entry with ID: {Id}", id);

            var exists = await _context.TestEntries
                .AsNoTracking()
                .AnyAsync(te => te.Id == id, cancellationToken);

            _logger.LogDebug("Test entry with ID {Id} exists: {Exists}", id, exists);
            return exists;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while checking existence of test entry with ID: {Id}", id);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<IEnumerable<TestEntry>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Searching test entries with term: {SearchTerm}", searchTerm);

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return await GetAllAsync(cancellationToken);
            }

            var normalizedSearchTerm = searchTerm.ToLower().Trim();

            var testEntries = await _context.TestEntries
                .AsNoTracking()
                .Include(te => te.TestSetup)
                    .ThenInclude(ts => ts!.TestType)
                .Where(te => te.PatientName.ToLower().Contains(normalizedSearchTerm) ||
                            te.MobileNo.Contains(normalizedSearchTerm) ||
                            te.BillNo.ToLower().Contains(normalizedSearchTerm))
                .OrderByDescending(te => te.CreatedDate)
                .ToListAsync(cancellationToken);

            _logger.LogInformation("Found {Count} test entries matching search term: {SearchTerm}", testEntries.Count, searchTerm);
            return testEntries;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while searching test entries with term: {SearchTerm}", searchTerm);
            throw;
        }
    }
}
