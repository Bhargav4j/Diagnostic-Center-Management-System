using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Interfaces.Repositories;
using DiagnosticCenter.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace DiagnosticCenter.Application.Services;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _repository;
    private readonly IPatientTestRepository _patientTestRepository;
    private readonly ILogger<PaymentService> _logger;

    public PaymentService(
        IPaymentRepository repository,
        IPatientTestRepository patientTestRepository,
        ILogger<PaymentService> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _patientTestRepository = patientTestRepository ?? throw new ArgumentNullException(nameof(patientTestRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
            _logger.LogError(ex, "Error retrieving payments");
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

    public async Task<IEnumerable<Payment>> GetByBillNoAsync(string billNo, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving payments for bill number: {BillNo}", billNo);
            return await _repository.GetByBillNoAsync(billNo, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving payments for bill number: {BillNo}", billNo);
            throw;
        }
    }

    public async Task<Payment> CreateAsync(Payment payment, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating new payment for bill number: {BillNo}", payment.BillNo);
            payment.CreatedDate = DateTime.UtcNow;
            payment.PaymentDate = DateTime.UtcNow;
            payment.IsActive = true;

            var result = await _repository.AddAsync(payment, cancellationToken);

            var patientTest = await _patientTestRepository.GetByIdAsync(payment.PatientTestId, cancellationToken);
            if (patientTest != null)
            {
                patientTest.PaidAmount += payment.AmountPaid;
                await _patientTestRepository.UpdateAsync(patientTest, cancellationToken);
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating payment for bill number: {BillNo}", payment.BillNo);
            throw;
        }
    }

    public async Task UpdateAsync(Payment payment, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating payment with ID: {Id}", payment.Id);
            payment.ModifiedDate = DateTime.UtcNow;
            await _repository.UpdateAsync(payment, cancellationToken);
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
            _logger.LogInformation("Deleting payment with ID: {Id}", id);
            await _repository.DeleteAsync(id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting payment with ID: {Id}", id);
            throw;
        }
    }
}
