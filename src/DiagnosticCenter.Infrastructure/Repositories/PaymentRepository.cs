using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Interfaces.Repositories;
using DiagnosticCenter.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DiagnosticCenter.Infrastructure.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly DiagnosticCenterDbContext _context;
    private readonly ILogger<PaymentRepository> _logger;

    public PaymentRepository(DiagnosticCenterDbContext context, ILogger<PaymentRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<Payment>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Payments
                .Where(p => p.IsActive)
                .AsNoTracking()
                .OrderByDescending(p => p.CreatedDate)
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
                .FirstOrDefaultAsync(p => p.Id == id && p.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving payment with ID: {Id}", id);
            throw;
        }
    }

    public async Task<Payment?> GetByBillNoAsync(string billNo, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Payments
                .FirstOrDefaultAsync(p => p.BillNo == billNo && p.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving payment with Bill No: {BillNo}", billNo);
            throw;
        }
    }

    public async Task<Payment> AddAsync(Payment payment, CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.Payments.AddAsync(payment, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return payment;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding payment: {BillNo}", payment.BillNo);
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
            var payment = await GetByIdAsync(id, cancellationToken);
            if (payment != null)
            {
                payment.IsActive = false;
                await UpdateAsync(payment, cancellationToken);
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
            return await _context.Payments
                .AnyAsync(p => p.Id == id && p.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking existence of payment with ID: {Id}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Payment>> GetUnpaidBillsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Payments
                .Where(p => p.IsActive && p.PaidAmount < p.TotalAmount)
                .AsNoTracking()
                .OrderBy(p => p.DueDate)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving unpaid bills");
            throw;
        }
    }

    public async Task<IEnumerable<Payment>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Payments
                .Where(p => p.IsActive && (p.BillNo.Contains(searchTerm) || p.MobileNo.Contains(searchTerm)))
                .AsNoTracking()
                .OrderByDescending(p => p.CreatedDate)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching payments with term: {SearchTerm}", searchTerm);
            throw;
        }
    }
}
