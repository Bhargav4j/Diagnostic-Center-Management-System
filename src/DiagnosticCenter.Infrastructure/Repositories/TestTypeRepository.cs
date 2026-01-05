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

    public TestTypeRepository(DiagnosticCenterDbContext context, ILogger<TestTypeRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id && t.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving test type with ID {Id}", id);
            throw;
        }
    }

    public async Task<TestType> AddAsync(TestType testType, CancellationToken cancellationToken = default)
    {
        try
        {
            _context.TestTypes.Add(testType);
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
            _logger.LogError(ex, "Error updating test type with ID {Id}", testType.Id);
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
            _logger.LogError(ex, "Error deleting test type with ID {Id}", id);
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
            _logger.LogError(ex, "Error checking if test type exists with ID {Id}", id);
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
            _logger.LogError(ex, "Error checking if test type exists with name {Name}", name);
            throw;
        }
    }

    public async Task<IEnumerable<TestType>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.TestTypes
                .Where(t => t.IsActive && t.Name.Contains(searchTerm))
                .OrderBy(t => t.Name)
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
