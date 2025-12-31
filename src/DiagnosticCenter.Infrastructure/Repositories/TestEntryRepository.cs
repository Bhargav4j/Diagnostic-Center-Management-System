using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Interfaces.Repositories;
using DiagnosticCenter.Infrastructure.Data;

namespace DiagnosticCenter.Infrastructure.Repositories;

public class TestEntryRepository : ITestEntryRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<TestEntryRepository> _logger;

    public TestEntryRepository(ApplicationDbContext context, ILogger<TestEntryRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<TestEntry>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.TestEntries
                .AsNoTracking()
                .Include(t => t.TestSetup)
                .Where(t => t.IsActive)
                .OrderByDescending(t => t.CreatedDate)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching all test entries");
            throw;
        }
    }

    public async Task<TestEntry?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.TestEntries
                .AsNoTracking()
                .Include(t => t.TestSetup)
                .Include(t => t.Payment)
                .FirstOrDefaultAsync(t => t.Id == id && t.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching test entry with ID: {Id}", id);
            throw;
        }
    }

    public async Task<TestEntry?> GetByBillNoAsync(string billNo, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.TestEntries
                .AsNoTracking()
                .Include(t => t.TestSetup)
                .Include(t => t.Payment)
                .FirstOrDefaultAsync(t => t.BillNo == billNo && t.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching test entry with bill no: {BillNo}", billNo);
            throw;
        }
    }

    public async Task<IEnumerable<TestEntry>> GetByMobileNoAsync(string mobileNo, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.TestEntries
                .AsNoTracking()
                .Include(t => t.TestSetup)
                .Where(t => t.MobileNo == mobileNo && t.IsActive)
                .OrderByDescending(t => t.CreatedDate)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching test entries with mobile no: {MobileNo}", mobileNo);
            throw;
        }
    }

    public async Task<TestEntry> AddAsync(TestEntry entity, CancellationToken cancellationToken = default)
    {
        try
        {
            _context.TestEntries.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return entity;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding test entry");
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
            _logger.LogError(ex, "Error updating test entry with ID: {Id}", entity.Id);
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
            return await _context.TestEntries.AnyAsync(t => t.Id == id && t.IsActive, cancellationToken);
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
                .AsNoTracking()
                .Include(t => t.TestSetup)
                .Where(t => t.IsActive && (t.PatientName.Contains(searchTerm) || t.BillNo.Contains(searchTerm) || t.MobileNo.Contains(searchTerm)))
                .OrderByDescending(t => t.CreatedDate)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching test entries");
            throw;
        }
    }

    public async Task<IEnumerable<TestEntry>> GetUnpaidEntriesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.TestEntries
                .AsNoTracking()
                .Include(t => t.TestSetup)
                .Where(t => t.IsActive && t.PaidAmount < t.TotalAmount)
                .OrderBy(t => t.DueDate)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching unpaid test entries");
            throw;
        }
    }
}
