using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Interfaces.Repositories;
using DiagnosticCenter.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DiagnosticCenter.Infrastructure.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<PaymentRepository> _logger;

    public PaymentRepository(ApplicationDbContext context, ILogger<PaymentRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<Payment>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Payments
                .Include(p => p.TestEntry)
                .Where(p => p.IsActive)
                .OrderByDescending(p => p.PaymentDate)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all payments from database");
            throw;
        }
    }

    public async Task<Payment?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Payments
                .Include(p => p.TestEntry)
                .FirstOrDefaultAsync(p => p.Id == id && p.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving payment with ID {Id} from database", id);
            throw;
        }
    }

    public async Task<Payment> AddAsync(Payment entity, CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.Payments.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return entity;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding payment to database");
            throw;
        }
    }

    public async Task UpdateAsync(Payment entity, CancellationToken cancellationToken = default)
    {
        try
        {
            _context.Payments.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating payment in database");
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = await _context.Payments.FindAsync(new object[] { id }, cancellationToken);
            if (entity != null)
            {
                entity.IsActive = false;
                entity.ModifiedDate = DateTime.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting payment from database");
            throw;
        }
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Payments.AnyAsync(p => p.Id == id && p.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if payment exists in database");
            throw;
        }
    }

    public async Task<IEnumerable<Payment>> GetByTestEntryIdAsync(int testEntryId, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Payments
                .Include(p => p.TestEntry)
                .Where(p => p.TestEntryId == testEntryId && p.IsActive)
                .OrderByDescending(p => p.PaymentDate)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving payments by test entry ID from database");
            throw;
        }
    }

    public async Task<IEnumerable<Payment>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            var query = _context.Payments
                .Include(p => p.TestEntry)
                .Where(p => p.IsActive);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(p =>
                    p.BillNumber.Contains(searchTerm) ||
                    (p.TransactionReference != null && p.TransactionReference.Contains(searchTerm)));
            }

            return await query
                .OrderByDescending(p => p.PaymentDate)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching payments in database");
            throw;
        }
    }
}
