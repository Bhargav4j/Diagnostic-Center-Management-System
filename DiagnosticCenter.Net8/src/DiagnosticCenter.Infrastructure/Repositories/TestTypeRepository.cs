using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Interfaces.Repositories;
using DiagnosticCenter.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DiagnosticCenter.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for TestType entity
/// </summary>
public class TestTypeRepository : ITestTypeRepository
{
    private readonly DiagnosticCenterDbContext _context;
    private readonly ILogger<TestTypeRepository> _logger;

    public TestTypeRepository(
        DiagnosticCenterDbContext context,
        ILogger<TestTypeRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<TestType>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.TestTypes
                .Where(t => t.IsActive)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all test types from database");
            throw;
        }
    }

    public async Task<TestType?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.TestTypes
                .Where(t => t.Id == id && t.IsActive)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving test type {Id} from database", id);
            throw;
        }
    }

    public async Task<TestType> AddAsync(TestType testType, CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.TestTypes.AddAsync(testType, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return testType;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding test type to database");
            throw;
        }
    }

    public async Task UpdateAsync(TestType testType, CancellationToken cancellationToken = default)
    {
        try
        {
            _context.TestTypes.Update(testType);
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating test type {Id} in database", testType.Id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var testType = await _context.TestTypes.FindAsync(new object[] { id }, cancellationToken);
            if (testType != null)
            {
                testType.IsActive = false;
                testType.ModifiedDate = DateTime.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting test type {Id} from database", id);
            throw;
        }
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.TestTypes
                .AnyAsync(t => t.Id == id && t.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking existence of test type {Id}", id);
            throw;
        }
    }

    public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.TestTypes
                .AnyAsync(t => t.Name == name && t.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking existence of test type with name {Name}", name);
            throw;
        }
    }

    public async Task<IEnumerable<TestType>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.TestTypes
                .Where(t => t.IsActive &&
                    (t.Name.Contains(searchTerm) || (t.Description != null && t.Description.Contains(searchTerm))))
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching test types with term {SearchTerm}", searchTerm);
            throw;
        }
    }
}
