using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Interfaces.Repositories;
using DiagnosticCenter.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DiagnosticCenter.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for Payment entity
/// </summary>
public class PaymentRepository : IPaymentRepository
{
    private readonly DiagnosticCenterDbContext _context;
    private readonly ILogger<PaymentRepository> _logger;

    public PaymentRepository(DiagnosticCenterDbContext context, ILogger<PaymentRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<Payment>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Payments
                .Include(p => p.TestEntry)
                .Where(p => p.IsActive)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all payments");
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
            _logger.LogError(ex, "Error retrieving payment with ID: {Id}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Payment>> GetByBillNoAsync(string billNo, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Payments
                .Include(p => p.TestEntry)
                .Where(p => p.BillNo == billNo && p.IsActive)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving payments for Bill No: {BillNo}", billNo);
            throw;
        }
    }

    public async Task<Payment> AddAsync(Payment payment, CancellationToken cancellationToken = default)
    {
        try
        {
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync(cancellationToken);
            return payment;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding payment");
            throw;
        }
    }

    public async Task UpdateAsync(Payment payment, CancellationToken cancellationToken = default)
    {
        try
        {
            _context.Payments.Update(payment);
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating payment with ID: {Id}", payment.Id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var payment = await _context.Payments.FindAsync(new object[] { id }, cancellationToken);
            if (payment != null)
            {
                payment.IsActive = false;
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting payment with ID: {Id}", id);
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
            _logger.LogError(ex, "Error checking existence of payment with ID: {Id}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Payment>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Payments
                .Include(p => p.TestEntry)
                .Where(p => p.IsActive && (p.BillNo.Contains(searchTerm) || p.PaymentMethod.Contains(searchTerm)))
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching payments with term: {SearchTerm}", searchTerm);
            throw;
        }
    }
}
