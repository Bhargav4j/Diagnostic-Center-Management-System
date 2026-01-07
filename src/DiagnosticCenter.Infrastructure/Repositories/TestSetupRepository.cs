using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Interfaces.Repositories;
using DiagnosticCenter.Infrastructure.Data;

namespace DiagnosticCenter.Infrastructure.Repositories;

public class TestSetupRepository : ITestSetupRepository
{
    private readonly DiagnosticCenterDbContext _context;
    private readonly ILogger<TestSetupRepository> _logger;

    public TestSetupRepository(DiagnosticCenterDbContext context, ILogger<TestSetupRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<TestSetup>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.TestSetups
                .Include(t => t.TestType)
                .Where(t => t.IsActive)
                .AsNoTracking()
                .OrderBy(t => t.Name)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all test setups");
            throw;
        }
    }

    public async Task<TestSetup?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.TestSetups
                .Include(t => t.TestType)
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id && t.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving test setup with ID: {Id}", id);
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
            _logger.LogError(ex, "Error adding test setup: {Name}", testSetup.Name);
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
            _logger.LogError(ex, "Error updating test setup with ID: {Id}", testSetup.Id);
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
            _logger.LogError(ex, "Error deleting test setup with ID: {Id}", id);
            throw;
        }
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.TestSetups.AnyAsync(t => t.Id == id && t.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if test setup exists with ID: {Id}", id);
            throw;
        }
    }

    public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.TestSetups
                .AnyAsync(t => t.Name.ToLower() == name.ToLower() && t.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if test setup exists with name: {Name}", name);
            throw;
        }
    }

    public async Task<IEnumerable<TestSetup>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.TestSetups
                .Include(t => t.TestType)
                .Where(t => t.IsActive && (t.Name.Contains(searchTerm) || t.TestType.Name.Contains(searchTerm)))
                .AsNoTracking()
                .OrderBy(t => t.Name)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching test setups with term: {SearchTerm}", searchTerm);
            throw;
        }
    }

    public async Task<IEnumerable<TestSetup>> GetByTypeIdAsync(int typeId, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.TestSetups
                .Include(t => t.TestType)
                .Where(t => t.TypeId == typeId && t.IsActive)
                .AsNoTracking()
                .OrderBy(t => t.Name)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving test setups for type ID: {TypeId}", typeId);
            throw;
        }
    }
}
