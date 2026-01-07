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
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<TestEntry>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.TestEntries
                .Include(t => t.TestEntryItems)
                    .ThenInclude(i => i.TestSetup)
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
                .Include(t => t.TestEntryItems)
                    .ThenInclude(i => i.TestSetup)
                        .ThenInclude(s => s!.TestType)
                .Include(t => t.Payments)
                .FirstOrDefaultAsync(t => t.Id == id && t.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving test entry with ID {Id} from database", id);
            throw;
        }
    }

    public async Task<TestEntry?> GetByBillNumberAsync(string billNumber, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.TestEntries
                .Include(t => t.TestEntryItems)
                    .ThenInclude(i => i.TestSetup)
                .Include(t => t.Payments)
                .FirstOrDefaultAsync(t => t.BillNumber == billNumber && t.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving test entry with bill number {BillNumber} from database", billNumber);
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
            return await _context.TestEntries.AnyAsync(t => t.Id == id && t.IsActive, cancellationToken);
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
            var query = _context.TestEntries
                .Include(t => t.TestEntryItems)
                .Include(t => t.Payments)
                .Where(t => t.IsActive);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(t =>
                    t.PatientName.Contains(searchTerm) ||
                    t.MobileNumber.Contains(searchTerm) ||
                    t.BillNumber.Contains(searchTerm));
            }

            return await query
                .OrderByDescending(t => t.CreatedDate)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching test entries in database");
            throw;
        }
    }

    public async Task<IEnumerable<TestEntry>> GetUnpaidTestsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.TestEntries
                .Include(t => t.TestEntryItems)
                .Include(t => t.Payments)
                .Where(t => t.IsActive && t.TotalAmount > t.PaidAmount)
                .OrderByDescending(t => t.DueDate)
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
