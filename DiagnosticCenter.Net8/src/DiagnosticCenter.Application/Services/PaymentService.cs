using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Exceptions;
using DiagnosticCenter.Domain.Interfaces.Repositories;
using DiagnosticCenter.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace DiagnosticCenter.Application.Services;

/// <summary>
/// Service implementation for Payment business operations
/// </summary>
public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _repository;
    private readonly ITestEntryRepository _testEntryRepository;
    private readonly ILogger<PaymentService> _logger;

    public PaymentService(
        IPaymentRepository repository,
        ITestEntryRepository testEntryRepository,
        ILogger<PaymentService> logger)
    {
        _repository = repository;
        _testEntryRepository = testEntryRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<Payment>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving all payments");
            return await _repository.GetAllAsync(cancellationToken);
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
            _logger.LogInformation("Retrieving payment with ID: {Id}", id);
            return await _repository.GetByIdAsync(id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving payment with ID: {Id}", id);
            throw;
        }
    }

    public async Task<Payment> CreateAsync(Payment payment, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating new payment for Bill No: {BillNo}", payment.BillNo);

            payment.CreatedDate = DateTime.UtcNow;
            payment.IsActive = true;
            payment.PaymentDate = DateTime.UtcNow;

            var result = await _repository.AddAsync(payment, cancellationToken);

            var testEntry = await _testEntryRepository.GetByIdAsync(payment.TestEntryId, cancellationToken);
            if (testEntry != null)
            {
                testEntry.PaidAmount += payment.Amount;
                testEntry.ModifiedDate = DateTime.UtcNow;
                await _testEntryRepository.UpdateAsync(testEntry, cancellationToken);
            }

            _logger.LogInformation("Payment created successfully with ID: {Id}", result.Id);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating payment for Bill No: {BillNo}", payment.BillNo);
            throw;
        }
    }

    public async Task UpdateAsync(int id, Payment payment, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating payment with ID: {Id}", id);

            var existing = await _repository.GetByIdAsync(id, cancellationToken);
            if (existing == null)
            {
                throw new NotFoundException(nameof(Payment), id);
            }

            existing.Amount = payment.Amount;
            existing.PaymentMethod = payment.PaymentMethod;
            existing.Remarks = payment.Remarks;
            existing.ModifiedDate = DateTime.UtcNow;
            existing.ModifiedBy = payment.ModifiedBy;

            await _repository.UpdateAsync(existing, cancellationToken);
            _logger.LogInformation("Payment updated successfully with ID: {Id}", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating payment with ID: {Id}", id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting payment with ID: {Id}", id);

            if (!await _repository.ExistsAsync(id, cancellationToken))
            {
                throw new NotFoundException(nameof(Payment), id);
            }

            await _repository.DeleteAsync(id, cancellationToken);
            _logger.LogInformation("Payment deleted successfully with ID: {Id}", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting payment with ID: {Id}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Payment>> GetByBillNoAsync(string billNo, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving payments for Bill No: {BillNo}", billNo);
            return await _repository.GetByBillNoAsync(billNo, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving payments for Bill No: {BillNo}", billNo);
            throw;
        }
    }

    public async Task<IEnumerable<Payment>> GetByTestEntryIdAsync(int testEntryId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving payments for Test Entry ID: {TestEntryId}", testEntryId);
            return await _repository.GetByTestEntryIdAsync(testEntryId, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving payments for Test Entry ID: {TestEntryId}", testEntryId);
            throw;
        }
    }

    public async Task<IEnumerable<Payment>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Searching payments with term: {SearchTerm}", searchTerm);
            return await _repository.SearchAsync(searchTerm, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching payments with term: {SearchTerm}", searchTerm);
            throw;
        }
    }
}
