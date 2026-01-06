using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Interfaces.Repositories;
using DiagnosticCenter.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DiagnosticCenter.Infrastructure.Repositories;

public class TestEntryRepository : ITestEntryRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<TestEntryRepository> _logger;

    public TestEntryRepository(ApplicationDbContext context, ILogger<TestEntryRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<TestEntry>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting all test entries");
            return await _context.TestEntries
                .AsNoTracking()
                .Where(t => t.IsActive)
                .Include(t => t.Test)
                .OrderByDescending(t => t.CreatedDate)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all test entries");
            throw;
        }
    }

    public async Task<TestEntry?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting test entry by ID: {Id}", id);
            return await _context.TestEntries
                .AsNoTracking()
                .Include(t => t.Test)
                .Include(t => t.Payments)
                .FirstOrDefaultAsync(t => t.Id == id && t.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting test entry by ID: {Id}", id);
            throw;
        }
    }

    public async Task<TestEntry> AddAsync(TestEntry testEntry, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Adding new test entry for Patient: {Name}", testEntry.Name);
            _context.TestEntries.Add(testEntry);
            await _context.SaveChangesAsync(cancellationToken);
            return testEntry;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding test entry for Patient: {Name}", testEntry.Name);
            throw;
        }
    }

    public async Task UpdateAsync(TestEntry testEntry, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating test entry: {Id}", testEntry.Id);
            _context.TestEntries.Update(testEntry);
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating test entry: {Id}", testEntry.Id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Soft deleting test entry: {Id}", id);
            var testEntry = await _context.TestEntries.FindAsync(new object[] { id }, cancellationToken);
            if (testEntry != null)
            {
                testEntry.IsActive = false;
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting test entry: {Id}", id);
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
            _logger.LogError(ex, "Error checking if test entry exists: {Id}", id);
            throw;
        }
    }

    public async Task<TestEntry?> GetByBillNoAsync(string billNo, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting test entry by bill number: {BillNo}", billNo);
            return await _context.TestEntries
                .AsNoTracking()
                .Include(t => t.Test)
                .Include(t => t.Payments)
                .FirstOrDefaultAsync(t => t.IsActive && t.BillNo == billNo, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting test entry by bill number: {BillNo}", billNo);
            throw;
        }
    }

    public async Task<IEnumerable<TestEntry>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Searching test entries: {SearchTerm}", searchTerm);
            return await _context.TestEntries
                .AsNoTracking()
                .Include(t => t.Test)
                .Where(t => t.IsActive && (
                    t.BillNo.Contains(searchTerm) ||
                    t.Name.Contains(searchTerm) ||
                    t.MobileNo.Contains(searchTerm) ||
                    (t.Test != null && t.Test.Name.Contains(searchTerm))))
                .OrderByDescending(t => t.CreatedDate)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching test entries: {SearchTerm}", searchTerm);
            throw;
        }
    }

    public async Task<IEnumerable<TestEntry>> GetUnpaidEntriesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting unpaid test entries");
            return await _context.TestEntries
                .AsNoTracking()
                .Include(t => t.Test)
                .Include(t => t.Payments)
                .Where(t => t.IsActive && t.PaidAmount < t.TotalAmount)
                .OrderByDescending(t => t.CreatedDate)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting unpaid test entries");
            throw;
        }
    }
}