using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Interfaces.Repositories;
using DiagnosticCenter.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DiagnosticCenter.Infrastructure.Repositories;

public class TestSetupRepository : ITestSetupRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<TestSetupRepository> _logger;

    public TestSetupRepository(ApplicationDbContext context, ILogger<TestSetupRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<TestSetup>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting all test setups");
            return await _context.TestSetups
                .AsNoTracking()
                .Where(t => t.IsActive)
                .Include(t => t.TestType)
                .OrderBy(t => t.Name)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all test setups");
            throw;
        }
    }

    public async Task<TestSetup?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting test setup by ID: {Id}", id);
            return await _context.TestSetups
                .AsNoTracking()
                .Include(t => t.TestType)
                .Include(t => t.TestEntries)
                .FirstOrDefaultAsync(t => t.Id == id && t.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting test setup by ID: {Id}", id);
            throw;
        }
    }

    public async Task<TestSetup> AddAsync(TestSetup testSetup, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Adding new test setup: {Name}", testSetup.Name);
            _context.TestSetups.Add(testSetup);
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
            _logger.LogInformation("Updating test setup: {Id}", testSetup.Id);
            _context.TestSetups.Update(testSetup);
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating test setup: {Id}", testSetup.Id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Soft deleting test setup: {Id}", id);
            var testSetup = await _context.TestSetups.FindAsync(new object[] { id }, cancellationToken);
            if (testSetup != null)
            {
                testSetup.IsActive = false;
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting test setup: {Id}", id);
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
            _logger.LogError(ex, "Error checking if test setup exists: {Id}", id);
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
            _logger.LogError(ex, "Error checking if test setup exists by name: {Name}", name);
            throw;
        }
    }

    public async Task<IEnumerable<TestSetup>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Searching test setups: {SearchTerm}", searchTerm);
            return await _context.TestSetups
                .AsNoTracking()
                .Include(t => t.TestType)
                .Where(t => t.IsActive && (
                    t.Name.Contains(searchTerm) ||
                    (t.Description != null && t.Description.Contains(searchTerm)) ||
                    (t.TestType != null && t.TestType.Name.Contains(searchTerm))))
                .OrderBy(t => t.Name)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching test setups: {SearchTerm}", searchTerm);
            throw;
        }
    }

    public async Task<IEnumerable<TestSetup>> GetByTypeIdAsync(int typeId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting test setups by test type ID: {TypeId}", typeId);
            return await _context.TestSetups
                .AsNoTracking()
                .Include(t => t.TestType)
                .Where(t => t.IsActive && t.TypeId == typeId)
                .OrderBy(t => t.Name)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting test setups by test type ID: {TypeId}", typeId);
            throw;
        }
    }
}