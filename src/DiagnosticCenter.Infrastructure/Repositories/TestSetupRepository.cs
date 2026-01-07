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
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<TestSetup>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.TestSetups
                .Include(t => t.TestType)
                .Where(t => t.IsActive)
                .OrderBy(t => t.Name)
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
                .Include(t => t.TestType)
                .FirstOrDefaultAsync(t => t.Id == id && t.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving test setup with ID {Id} from database", id);
            throw;
        }
    }

    public async Task<TestSetup> AddAsync(TestSetup entity, CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.TestSetups.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return entity;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding test setup to database");
            throw;
        }
    }

    public async Task UpdateAsync(TestSetup entity, CancellationToken cancellationToken = default)
    {
        try
        {
            _context.TestSetups.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating test setup in database");
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = await _context.TestSetups.FindAsync(new object[] { id }, cancellationToken);
            if (entity != null)
            {
                entity.IsActive = false;
                entity.ModifiedDate = DateTime.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting test setup from database");
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
            _logger.LogError(ex, "Error checking if test setup exists in database");
            throw;
        }
    }

    public async Task<IEnumerable<TestSetup>> GetByTestTypeIdAsync(int testTypeId, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.TestSetups
                .Include(t => t.TestType)
                .Where(t => t.TestTypeId == testTypeId && t.IsActive)
                .OrderBy(t => t.Name)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving test setups by test type ID from database");
            throw;
        }
    }

    public async Task<IEnumerable<TestSetup>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            var query = _context.TestSetups
                .Include(t => t.TestType)
                .Where(t => t.IsActive);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(t =>
                    t.Name.Contains(searchTerm) ||
                    (t.Description != null && t.Description.Contains(searchTerm)));
            }

            return await query
                .OrderBy(t => t.Name)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching test setups in database");
            throw;
        }
    }
}
