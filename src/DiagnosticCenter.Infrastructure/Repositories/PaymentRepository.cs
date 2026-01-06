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
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<Payment>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting all payments");
            return await _context.Payments
                .AsNoTracking()
                .Where(p => p.IsActive)
                .Include(p => p.TestEntry)
                .OrderByDescending(p => p.CreatedDate)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all payments");
            throw;
        }
    }

    public async Task<Payment?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting payment by ID: {Id}", id);
            return await _context.Payments
                .AsNoTracking()
                .Include(p => p.TestEntry)
                .FirstOrDefaultAsync(p => p.Id == id && p.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting payment by ID: {Id}", id);
            throw;
        }
    }

    public async Task<Payment> AddAsync(Payment payment, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Adding new payment for Test Entry ID: {TestEntryId}", payment.TestEntryId);
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync(cancellationToken);
            return payment;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding payment for Test Entry ID: {TestEntryId}", payment.TestEntryId);
            throw;
        }
    }

    public async Task UpdateAsync(Payment payment, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating payment: {Id}", payment.Id);
            _context.Payments.Update(payment);
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating payment: {Id}", payment.Id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Soft deleting payment: {Id}", id);
            var payment = await _context.Payments.FindAsync(new object[] { id }, cancellationToken);
            if (payment != null)
            {
                payment.IsActive = false;
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting payment: {Id}", id);
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
            _logger.LogError(ex, "Error checking if payment exists: {Id}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Payment>> GetByBillNoAsync(string billNo, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting payments by bill number: {BillNo}", billNo);
            return await _context.Payments
                .AsNoTracking()
                .Include(p => p.TestEntry)
                .Where(p => p.IsActive && p.BillNo == billNo)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting payments by bill number: {BillNo}", billNo);
            throw;
        }
    }

    public async Task<IEnumerable<Payment>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Searching payments: {SearchTerm}", searchTerm);
            return await _context.Payments
                .AsNoTracking()
                .Include(p => p.TestEntry)
                .Where(p => p.IsActive && (
                    p.BillNo.Contains(searchTerm) ||
                    p.MobileNo.Contains(searchTerm) ||
                    (p.TestEntry != null && p.TestEntry.BillNo.Contains(searchTerm))))
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching payments: {SearchTerm}", searchTerm);
            throw;
        }
    }
}