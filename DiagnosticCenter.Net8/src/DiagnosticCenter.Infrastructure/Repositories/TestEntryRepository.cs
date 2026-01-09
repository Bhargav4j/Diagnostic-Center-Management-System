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
                .Where(t => t.Id == id && t.IsActive)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving test entry {Id} from database", id);
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
                .Where(t => t.BillNo == billNo && t.IsActive)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving test entry with bill no {BillNo} from database", billNo);
            throw;
        }
    }

    public async Task<TestEntry> AddAsync(TestEntry testEntry, CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.TestEntries.AddAsync(testEntry, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return testEntry;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding test entry to database");
            throw;
        }
    }

    public async Task UpdateAsync(TestEntry testEntry, CancellationToken cancellationToken = default)
    {
        try
        {
            _context.TestEntries.Update(testEntry);
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating test entry {Id} in database", testEntry.Id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var testEntry = await _context.TestEntries.FindAsync(new object[] { id }, cancellationToken);
            if (testEntry != null)
            {
                testEntry.IsActive = false;
                testEntry.ModifiedDate = DateTime.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting test entry {Id} from database", id);
            throw;
        }
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.TestEntries
                .AnyAsync(t => t.Id == id && t.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking existence of test entry {Id}", id);
            throw;
        }
    }

    public async Task<bool> ExistsByBillNoAsync(string billNo, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.TestEntries
                .AnyAsync(t => t.BillNo == billNo && t.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking existence of test entry with bill no {BillNo}", billNo);
            throw;
        }
    }

    public async Task<IEnumerable<TestEntry>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.TestEntries
                .Include(t => t.Test)
                .Where(t => t.IsActive &&
                    (t.PatientName.Contains(searchTerm) ||
                     t.BillNo.Contains(searchTerm) ||
                     t.MobileNo.Contains(searchTerm)))
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching test entries with term {SearchTerm}", searchTerm);
            throw;
        }
    }

    public async Task<IEnumerable<TestEntry>> GetUnpaidEntriesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.TestEntries
                .Include(t => t.Test)
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
