using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Interfaces.Repositories;
using DiagnosticCenter.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DiagnosticCenter.Infrastructure.Repositories;

public class TestTypeRepository : ITestTypeRepository
{
    private readonly DiagnosticCenterDbContext _context;
    private readonly ILogger<TestTypeRepository> _logger;

    public TestTypeRepository(
        DiagnosticCenterDbContext context,
        ILogger<TestTypeRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<TestType>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.TestTypes
            .Where(t => t.IsActive)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<TestType?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.TestTypes
            .Where(t => t.IsActive && t.Id == id)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<TestType> AddAsync(TestType testType, CancellationToken cancellationToken = default)
    {
        await _context.TestTypes.AddAsync(testType, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return testType;
    }

    public async Task UpdateAsync(TestType testType, CancellationToken cancellationToken = default)
    {
        _context.TestTypes.Update(testType);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var testType = await _context.TestTypes.FindAsync(new object[] { id }, cancellationToken);
        if (testType != null)
        {
            testType.IsActive = false;
            testType.ModifiedDate = DateTime.UtcNow;
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.TestTypes
            .AnyAsync(t => t.IsActive && t.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<TestType>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        return await _context.TestTypes
            .Where(t => t.IsActive && t.Name.Contains(searchTerm))
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}
