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
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<TestType>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting all test types");
            return await _context.TestTypes
                .AsNoTracking()
                .Where(t => t.IsActive)
                .OrderBy(t => t.Name)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all test types");
            throw;
        }
    }

    public async Task<TestType?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting test type by ID: {Id}", id);
            return await _context.TestTypes
                .AsNoTracking()
                .Include(t => t.TestSetups)
                .FirstOrDefaultAsync(t => t.Id == id && t.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting test type by ID: {Id}", id);
            throw;
        }
    }

    public async Task<TestType> AddAsync(TestType testType, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Adding new test type: {Name}", testType.Name);
            _context.TestTypes.Add(testType);
            await _context.SaveChangesAsync(cancellationToken);
            return testType;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding test type: {Name}", testType.Name);
            throw;
        }
    }

    public async Task UpdateAsync(TestType testType, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating test type: {Id}", testType.Id);
            _context.TestTypes.Update(testType);
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating test type: {Id}", testType.Id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Soft deleting test type: {Id}", id);
            var testType = await _context.TestTypes.FindAsync(new object[] { id }, cancellationToken);
            if (testType != null)
            {
                testType.IsActive = false;
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting test type: {Id}", id);
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
            _logger.LogError(ex, "Error checking if test type exists: {Id}", id);
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
            _logger.LogError(ex, "Error checking if test type exists by name: {Name}", name);
            throw;
        }
    }

    public async Task<IEnumerable<TestType>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Searching test types: {SearchTerm}", searchTerm);
            return await _context.TestTypes
                .AsNoTracking()
                .Where(t => t.IsActive && (t.Name.Contains(searchTerm) || (t.Description != null && t.Description.Contains(searchTerm))))
                .OrderBy(t => t.Name)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching test types: {SearchTerm}", searchTerm);
            throw;
        }
    }
}
