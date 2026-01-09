using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Interfaces.Repositories;
using DiagnosticCenter.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DiagnosticCenter.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for TestSetup entity
/// </summary>
public class TestSetupRepository : ITestSetupRepository
{
    private readonly DiagnosticCenterDbContext _context;
    private readonly ILogger<TestSetupRepository> _logger;

    public TestSetupRepository(
        DiagnosticCenterDbContext context,
        ILogger<TestSetupRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<TestSetup>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.TestSetups
                .Include(t => t.Type)
                .Where(t => t.IsActive)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all test setups from database");
            throw;
        }
    }

    public async Task<TestSetup?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.TestSetups
                .Include(t => t.Type)
                .Where(t => t.Id == id && t.IsActive)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving test setup {Id} from database", id);
            throw;
        }
    }

    public async Task<TestSetup> AddAsync(TestSetup testSetup, CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.TestSetups.AddAsync(testSetup, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return testSetup;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding test setup to database");
            throw;
        }
    }

    public async Task UpdateAsync(TestSetup testSetup, CancellationToken cancellationToken = default)
    {
        try
        {
            _context.TestSetups.Update(testSetup);
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating test setup {Id} in database", testSetup.Id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var testSetup = await _context.TestSetups.FindAsync(new object[] { id }, cancellationToken);
            if (testSetup != null)
            {
                testSetup.IsActive = false;
                testSetup.ModifiedDate = DateTime.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting test setup {Id} from database", id);
            throw;
        }
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.TestSetups
                .AnyAsync(t => t.Id == id && t.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking existence of test setup {Id}", id);
            throw;
        }
    }

    public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.TestSetups
                .AnyAsync(t => t.Name == name && t.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking existence of test setup with name {Name}", name);
            throw;
        }
    }

    public async Task<IEnumerable<TestSetup>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.TestSetups
                .Include(t => t.Type)
                .Where(t => t.IsActive &&
                    (t.Name.Contains(searchTerm) || t.Type!.Name.Contains(searchTerm)))
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching test setups with term {SearchTerm}", searchTerm);
            throw;
        }
    }

    public async Task<IEnumerable<TestSetup>> GetByTypeIdAsync(int typeId, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.TestSetups
                .Include(t => t.Type)
                .Where(t => t.TypeId == typeId && t.IsActive)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving test setups for type {TypeId}", typeId);
            throw;
        }
    }
}
