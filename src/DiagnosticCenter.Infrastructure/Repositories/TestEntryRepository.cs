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
                .Include(t => t.TestSetup)
                    .ThenInclude(ts => ts.TestType)
                .Include(t => t.Payments)
                .Where(t => t.IsActive)
                .OrderByDescending(t => t.CreatedDate)
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
                .Include(t => t.TestSetup)
                    .ThenInclude(ts => ts.TestType)
                .Include(t => t.Payments)
                .FirstOrDefaultAsync(t => t.Id == id && t.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving test entry with ID: {Id}", id);
            throw;
        }
    }

    public async Task<TestEntry> AddAsync(TestEntry testEntry, CancellationToken cancellationToken = default)
    {
        try
        {
            _context.TestEntries.Add(testEntry);
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
            var testEntry = await GetByIdAsync(id, cancellationToken);
            if (testEntry != null)
            {
                testEntry.IsActive = false;
                testEntry.ModifiedDate = DateTime.UtcNow;
                await UpdateAsync(testEntry, cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting test entry with ID: {Id}", id);
            throw;
        }
    }

    public async Task<TestEntry?> GetByBillNumberAsync(string billNumber, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.TestEntries
                .Include(t => t.TestSetup)
                    .ThenInclude(ts => ts.TestType)
                .Include(t => t.Payments)
                .FirstOrDefaultAsync(t => t.BillNumber == billNumber && t.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving test entry with bill number: {BillNumber}", billNumber);
            throw;
        }
    }

    public async Task<IEnumerable<TestEntry>> GetByMobileNumberAsync(string mobileNumber, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.TestEntries
                .Include(t => t.TestSetup)
                    .ThenInclude(ts => ts.TestType)
                .Include(t => t.Payments)
                .Where(t => t.MobileNumber == mobileNumber && t.IsActive)
                .OrderByDescending(t => t.CreatedDate)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving test entries for mobile number: {MobileNumber}", mobileNumber);
            throw;
        }
    }

    public async Task<IEnumerable<TestEntry>> GetUnpaidAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.TestEntries
                .Include(t => t.TestSetup)
                    .ThenInclude(ts => ts.TestType)
                .Include(t => t.Payments)
                .Where(t => t.IsActive && t.PaidAmount < t.TotalAmount)
                .OrderByDescending(t => t.DueDate)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving unpaid test entries");
            throw;
        }
    }

    public async Task<IEnumerable<TestEntry>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.TestEntries
                .Include(t => t.TestSetup)
                    .ThenInclude(ts => ts.TestType)
                .Include(t => t.Payments)
                .Where(t => t.IsActive &&
                    (t.PatientName.Contains(searchTerm) ||
                     t.BillNumber.Contains(searchTerm) ||
                     t.MobileNumber.Contains(searchTerm)))
                .OrderByDescending(t => t.CreatedDate)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching test entries with term: {SearchTerm}", searchTerm);
            throw;
        }
    }
}
