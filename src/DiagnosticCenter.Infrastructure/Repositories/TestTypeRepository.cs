using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Interfaces.Repositories;
using DiagnosticCenter.Infrastructure.Data;

namespace DiagnosticCenter.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for TestType entity
/// </summary>
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
            _logger.LogInformation("Fetching all test types");
            return await _context.TestTypes
                .AsNoTracking()
                .Where(t => t.IsActive)
                .OrderBy(t => t.Name)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching all test types");
            throw;
        }
    }

    public async Task<TestType?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Fetching test type with ID: {Id}", id);
            return await _context.TestTypes
                .AsNoTracking()
                .Include(t => t.TestSetups)
                .FirstOrDefaultAsync(t => t.Id == id && t.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching test type with ID: {Id}", id);
            throw;
        }
    }

    public async Task<TestType?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Fetching test type with name: {Name}", name);
            return await _context.TestTypes
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Name == name && t.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching test type with name: {Name}", name);
            throw;
        }
    }

    public async Task<TestType> AddAsync(TestType entity, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Adding new test type: {Name}", entity.Name);
            _context.TestTypes.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Successfully added test type with ID: {Id}", entity.Id);
            return entity;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding test type: {Name}", entity.Name);
            throw;
        }
    }

    public async Task UpdateAsync(TestType entity, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating test type with ID: {Id}", entity.Id);
            _context.TestTypes.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Successfully updated test type with ID: {Id}", entity.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating test type with ID: {Id}", entity.Id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting test type with ID: {Id}", id);
            var entity = await _context.TestTypes.FindAsync(new object[] { id }, cancellationToken);
            if (entity != null)
            {
                entity.IsActive = false;
                await _context.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("Successfully deleted test type with ID: {Id}", id);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting test type with ID: {Id}", id);
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
            _logger.LogError(ex, "Error checking if test type exists with ID: {Id}", id);
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
            _logger.LogError(ex, "Error checking if test type exists with name: {Name}", name);
            throw;
        }
    }

    public async Task<IEnumerable<TestType>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Searching test types with term: {SearchTerm}", searchTerm);
            return await _context.TestTypes
                .AsNoTracking()
                .Where(t => t.IsActive && (t.Name.Contains(searchTerm) || (t.Description != null && t.Description.Contains(searchTerm))))
                .OrderBy(t => t.Name)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching test types with term: {SearchTerm}", searchTerm);
            throw;
        }
    }
}
