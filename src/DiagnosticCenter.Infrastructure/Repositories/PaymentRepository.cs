using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Interfaces.Repositories;
using DiagnosticCenter.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DiagnosticCenter.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for Payment entity operations.
/// Provides data access methods with error handling and logging.
/// </summary>
public class PaymentRepository : IPaymentRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<PaymentRepository> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="PaymentRepository"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    /// <param name="logger">The logger instance.</param>
    public PaymentRepository(ApplicationDbContext context, ILogger<PaymentRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Payment>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Retrieving all payments");

            var payments = await _context.Payments
                .AsNoTracking()
                .Include(p => p.TestEntry)
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync(cancellationToken);

            _logger.LogInformation("Retrieved {Count} payments", payments.Count);
            return payments;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving all payments");
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<Payment?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Retrieving payment with ID: {Id}", id);

            var payment = await _context.Payments
                .AsNoTracking()
                .Include(p => p.TestEntry)
                    .ThenInclude(te => te!.TestSetup)
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

            if (payment == null)
            {
                _logger.LogWarning("Payment with ID {Id} not found", id);
            }
            else
            {
                _logger.LogDebug("Successfully retrieved payment with ID: {Id}", id);
            }

            return payment;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving payment with ID: {Id}", id);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<Payment> AddAsync(Payment entity, CancellationToken cancellationToken = default)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        try
        {
            _logger.LogDebug("Adding new payment for BillNo: {BillNo}", entity.BillNo);

            entity.CreatedDate = DateTime.UtcNow;
            entity.IsActive = true;

            await _context.Payments.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully added payment with ID: {Id}, Amount: {Amount}", entity.Id, entity.Amount);
            return entity;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error occurred while adding payment for BillNo: {BillNo}", entity.BillNo);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while adding payment for BillNo: {BillNo}", entity.BillNo);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task UpdateAsync(Payment entity, CancellationToken cancellationToken = default)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        try
        {
            _logger.LogDebug("Updating payment with ID: {Id}", entity.Id);

            var existingEntity = await _context.Payments
                .FirstOrDefaultAsync(p => p.Id == entity.Id, cancellationToken);

            if (existingEntity == null)
            {
                _logger.LogWarning("Payment with ID {Id} not found for update", entity.Id);
                throw new InvalidOperationException($"Payment with ID {entity.Id} not found");
            }

            // Update properties
            existingEntity.BillNo = entity.BillNo;
            existingEntity.Amount = entity.Amount;
            existingEntity.PaymentDate = entity.PaymentDate;
            existingEntity.TestEntryId = entity.TestEntryId;
            existingEntity.ModifiedDate = DateTime.UtcNow;
            existingEntity.ModifiedBy = entity.ModifiedBy;
            existingEntity.IsActive = entity.IsActive;

            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully updated payment with ID: {Id}", entity.Id);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError(ex, "Concurrency error occurred while updating payment with ID: {Id}", entity.Id);
            throw;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error occurred while updating payment with ID: {Id}", entity.Id);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating payment with ID: {Id}", entity.Id);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Deleting payment with ID: {Id}", id);

            var entity = await _context.Payments
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

            if (entity == null)
            {
                _logger.LogWarning("Payment with ID {Id} not found for deletion", id);
                throw new InvalidOperationException($"Payment with ID {id} not found");
            }

            // Soft delete
            entity.IsActive = false;
            entity.ModifiedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully deleted (soft) payment with ID: {Id}", id);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error occurred while deleting payment with ID: {Id}", id);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting payment with ID: {Id}", id);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Checking existence of payment with ID: {Id}", id);

            var exists = await _context.Payments
                .AsNoTracking()
                .AnyAsync(p => p.Id == id, cancellationToken);

            _logger.LogDebug("Payment with ID {Id} exists: {Exists}", id, exists);
            return exists;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while checking existence of payment with ID: {Id}", id);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Payment>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Searching payments with term: {SearchTerm}", searchTerm);

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return await GetAllAsync(cancellationToken);
            }

            var normalizedSearchTerm = searchTerm.ToLower().Trim();

            var payments = await _context.Payments
                .AsNoTracking()
                .Include(p => p.TestEntry)
                .Where(p => p.BillNo.ToLower().Contains(normalizedSearchTerm) ||
                           (p.TestEntry != null && p.TestEntry.PatientName.ToLower().Contains(normalizedSearchTerm)))
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync(cancellationToken);

            _logger.LogInformation("Found {Count} payments matching search term: {SearchTerm}", payments.Count, searchTerm);
            return payments;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while searching payments with term: {SearchTerm}", searchTerm);
            throw;
        }
    }
}
