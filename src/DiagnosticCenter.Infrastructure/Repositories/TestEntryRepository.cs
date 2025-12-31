using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Interfaces.Repositories;
using DiagnosticCenter.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DiagnosticCenter.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for TestEntry entity
/// </summary>
public class TestEntryRepository : ITestEntryRepository
{
    private readonly DiagnosticCenterDbContext _context;
    private readonly ILogger<TestEntryRepository> _logger;

    public TestEntryRepository(
        DiagnosticCenterDbContext context,
        ILogger<TestEntryRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<TestEntry>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.TestEntries
                .Include(t => t.Test)
                .Include(t => t.Payments)
                .Where(t => t.IsActive)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all test entries from database");
            throw;
        }
    }

    public async Task<TestEntry?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.TestEntries
                .Include(t => t.Test)
                .Include(t => t.Payments)
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id && t.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving test entry with ID {Id} from database", id);
            throw;
        }
    }

    public async Task<TestEntry> AddAsync(TestEntry entity, CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.TestEntries.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return entity;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding test entry to database");
            throw;
        }
    }

    public async Task UpdateAsync(TestEntry entity, CancellationToken cancellationToken = default)
    {
        try
        {
            _context.TestEntries.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating test entry in database");
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = await _context.TestEntries.FindAsync(new object[] { id }, cancellationToken);
            if (entity != null)
            {
                entity.IsActive = false;
                entity.ModifiedDate = DateTime.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting test entry from database");
            throw;
        }
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.TestEntries
                .AsNoTracking()
                .AnyAsync(t => t.Id == id && t.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if test entry exists in database");
            throw;
        }
    }

    public async Task<IEnumerable<TestEntry>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.TestEntries
                .Include(t => t.Test)
                .Include(t => t.Payments)
                .Where(t => t.IsActive &&
                    (t.PatientName.Contains(searchTerm) ||
                     t.BillNo.Contains(searchTerm) ||
                     t.MobileNo.Contains(searchTerm)))
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching test entries in database");
            throw;
        }
    }

    public async Task<TestEntry?> GetByBillNoAsync(string billNo, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.TestEntries
                .Include(t => t.Test)
                .Include(t => t.Payments)
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.BillNo == billNo && t.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving test entry by bill number from database");
            throw;
        }
    }

    public async Task<IEnumerable<TestEntry>> GetUnpaidAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.TestEntries
                .Include(t => t.Test)
                .Include(t => t.Payments)
                .Where(t => t.IsActive && t.PaidAmount < t.TotalAmount)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving unpaid test entries from database");
            throw;
        }
    }
}
