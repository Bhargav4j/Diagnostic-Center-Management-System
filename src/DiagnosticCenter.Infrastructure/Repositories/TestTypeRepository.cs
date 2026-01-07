using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Interfaces.Repositories;
using DiagnosticCenter.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DiagnosticCenter.Infrastructure.Repositories;

public class TestTypeRepository : ITestTypeRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<TestTypeRepository> _logger;

    public TestTypeRepository(ApplicationDbContext context, ILogger<TestTypeRepository> logger)
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
                .OrderBy(t => t.Name)
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
                .Include(t => t.TestSetups)
                .FirstOrDefaultAsync(t => t.Id == id && t.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving test type with ID {Id} from database", id);
            throw;
        }
    }

    public async Task<TestType> AddAsync(TestType entity, CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.TestTypes.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return entity;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding test type to database");
            throw;
        }
    }

    public async Task UpdateAsync(TestType entity, CancellationToken cancellationToken = default)
    {
        try
        {
            _context.TestTypes.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating test type in database");
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = await _context.TestTypes.FindAsync(new object[] { id }, cancellationToken);
            if (entity != null)
            {
                entity.IsActive = false;
                entity.ModifiedDate = DateTime.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting test type from database");
            throw;
        }
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.TestTypes.AnyAsync(t => t.Id == id && t.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if test type exists in database");
            throw;
        }
    }

    public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.TestTypes.AnyAsync(
                t => t.Name.ToLower() == name.ToLower() && t.IsActive,
                cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if test type name exists in database");
            throw;
        }
    }

    public async Task<IEnumerable<TestType>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            var query = _context.TestTypes.Where(t => t.IsActive);

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
            _logger.LogError(ex, "Error searching test types in database");
            throw;
        }
    }
}
