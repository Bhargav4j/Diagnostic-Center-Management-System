using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Interfaces.Repositories;
using DiagnosticCenter.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DiagnosticCenter.Infrastructure.Repositories;

public class TestSetupRepository : ITestSetupRepository
{
    private readonly DiagnosticCenterDbContext _context;
    private readonly ILogger<TestSetupRepository> _logger;

    public TestSetupRepository(
        DiagnosticCenterDbContext context,
        ILogger<TestSetupRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<TestSetup>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.TestSetups
            .Include(t => t.TestType)
            .Where(t => t.IsActive)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<TestSetup?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.TestSetups
            .Include(t => t.TestType)
            .Where(t => t.IsActive && t.Id == id)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<TestSetup>> GetByTestTypeIdAsync(int testTypeId, CancellationToken cancellationToken = default)
    {
        return await _context.TestSetups
            .Include(t => t.TestType)
            .Where(t => t.IsActive && t.TestTypeId == testTypeId)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<TestSetup> AddAsync(TestSetup testSetup, CancellationToken cancellationToken = default)
    {
        await _context.TestSetups.AddAsync(testSetup, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return testSetup;
    }

    public async Task UpdateAsync(TestSetup testSetup, CancellationToken cancellationToken = default)
    {
        _context.TestSetups.Update(testSetup);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var testSetup = await _context.TestSetups.FindAsync(new object[] { id }, cancellationToken);
        if (testSetup != null)
        {
            testSetup.IsActive = false;
            testSetup.ModifiedDate = DateTime.UtcNow;
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.TestSetups
            .AnyAsync(t => t.IsActive && t.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<TestSetup>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        return await _context.TestSetups
            .Include(t => t.TestType)
            .Where(t => t.IsActive && t.Name.Contains(searchTerm))
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}
