using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Interfaces.Repositories;
using DiagnosticCenter.Infrastructure.Data;

namespace DiagnosticCenter.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for TestSetup entity
/// </summary>
public class TestSetupRepository : ITestSetupRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<TestSetupRepository> _logger;

    public TestSetupRepository(ApplicationDbContext context, ILogger<TestSetupRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<TestSetup>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Fetching all test setups");
            return await _context.TestSetups
                .AsNoTracking()
                .Include(t => t.TestType)
                .Where(t => t.IsActive)
                .OrderBy(t => t.Name)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching all test setups");
            throw;
        }
    }

    public async Task<TestSetup?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Fetching test setup with ID: {Id}", id);
            return await _context.TestSetups
                .AsNoTracking()
                .Include(t => t.TestType)
                .FirstOrDefaultAsync(t => t.Id == id && t.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching test setup with ID: {Id}", id);
            throw;
        }
    }

    public async Task<IEnumerable<TestSetup>> GetByTypeIdAsync(int typeId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Fetching test setups for type ID: {TypeId}", typeId);
            return await _context.TestSetups
                .AsNoTracking()
                .Include(t => t.TestType)
                .Where(t => t.TypeId == typeId && t.IsActive)
                .OrderBy(t => t.Name)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching test setups for type ID: {TypeId}", typeId);
            throw;
        }
    }

    public async Task<TestSetup> AddAsync(TestSetup entity, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Adding new test setup: {Name}", entity.Name);
            _context.TestSetups.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Successfully added test setup with ID: {Id}", entity.Id);
            return entity;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding test setup: {Name}", entity.Name);
            throw;
        }
    }

    public async Task UpdateAsync(TestSetup entity, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating test setup with ID: {Id}", entity.Id);
            _context.TestSetups.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Successfully updated test setup with ID: {Id}", entity.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating test setup with ID: {Id}", entity.Id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting test setup with ID: {Id}", id);
            var entity = await _context.TestSetups.FindAsync(new object[] { id }, cancellationToken);
            if (entity != null)
            {
                entity.IsActive = false;
                await _context.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("Successfully deleted test setup with ID: {Id}", id);
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
            return await _context.TestSetups
                .AnyAsync(t => t.Id == id && t.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if test setup exists with ID: {Id}", id);
            throw;
        }
    }

    public async Task<IEnumerable<TestSetup>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Searching test setups with term: {SearchTerm}", searchTerm);
            return await _context.TestSetups
                .AsNoTracking()
                .Include(t => t.TestType)
                .Where(t => t.IsActive && (t.Name.Contains(searchTerm) || (t.Description != null && t.Description.Contains(searchTerm))))
                .OrderBy(t => t.Name)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching test setups with term: {SearchTerm}", searchTerm);
            throw;
        }
    }
}
