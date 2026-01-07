using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Interfaces.Repositories;
using DiagnosticCenter.Infrastructure.Data;

namespace DiagnosticCenter.Infrastructure.Repositories;

public class TestEntryRepository : ITestEntryRepository
{
    private readonly DiagnosticCenterDbContext _context;
    private readonly ILogger<TestEntryRepository> _logger;

    public TestEntryRepository(DiagnosticCenterDbContext context, ILogger<TestEntryRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<TestEntry>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.TestEntries
                .Include(t => t.TestSetup)
                .AsNoTracking()
                .OrderByDescending(t => t.CreatedDate)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all test entries");
            throw;
        }
    }

    public async Task<TestEntry?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.TestEntries
                .Include(t => t.TestSetup)
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving test entry with ID: {Id}", id);
            throw;
        }
    }

    public async Task<TestEntry?> GetByBillNoAsync(string billNo, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.TestEntries
                .Include(t => t.TestSetup)
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.BillNo == billNo, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving test entry with Bill No: {BillNo}", billNo);
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
            _logger.LogError(ex, "Error adding test entry for patient: {PatientName}", testEntry.PatientName);
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
            _logger.LogError(ex, "Error updating test entry with ID: {Id}", testEntry.Id);
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
                _context.TestEntries.Remove(testEntry);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting test entry with ID: {Id}", id);
            throw;
        }
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.TestEntries.AnyAsync(t => t.Id == id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if test entry exists with ID: {Id}", id);
            throw;
        }
    }

    public async Task<IEnumerable<TestEntry>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.TestEntries
                .Include(t => t.TestSetup)
                .Where(t => t.PatientName.Contains(searchTerm) || t.BillNo.Contains(searchTerm))
                .AsNoTracking()
                .OrderByDescending(t => t.CreatedDate)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching test entries with term: {SearchTerm}", searchTerm);
            throw;
        }
    }

    public async Task<IEnumerable<TestEntry>> GetUnpaidEntriesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.TestEntries
                .Include(t => t.TestSetup)
                .Where(t => !t.IsPaid)
                .AsNoTracking()
                .OrderBy(t => t.TestDate)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving unpaid test entries");
            throw;
        }
    }
}
