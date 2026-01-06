using AutoMapper;
using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Interfaces.Repositories;
using DiagnosticCenter.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace DiagnosticCenter.Application.Services;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<PaymentService> _logger;

    public PaymentService(
        IPaymentRepository repository,
        IMapper mapper,
        ILogger<PaymentService> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<PaymentDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting all payments from service");
            var payments = await _repository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<PaymentDto>>(payments);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all payments in service");
            throw;
        }
    }

    public async Task<PaymentDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting payment by ID from service: {Id}", id);
            var payment = await _repository.GetByIdAsync(id, cancellationToken);
            return payment == null ? null : _mapper.Map<PaymentDto>(payment);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting payment by ID in service: {Id}", id);
            throw;
        }
    }

    public async Task<PaymentDto> CreateAsync(PaymentCreateDto createDto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating payment for Test Entry ID: {TestEntryId}", createDto.TestEntryId);

            var payment = _mapper.Map<Payment>(createDto);
            payment.PaymentDate = DateTime.UtcNow;
            payment.IsActive = true;

            var created = await _repository.AddAsync(payment, cancellationToken);
            return _mapper.Map<PaymentDto>(created);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating payment for Test Entry ID: {TestEntryId}", createDto.TestEntryId);
            throw;
        }
    }

    public async Task UpdateAsync(int id, PaymentUpdateDto updateDto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating payment: {Id}", id);

            var payment = await _repository.GetByIdAsync(id, cancellationToken);
            if (payment == null)
            {
                throw new InvalidOperationException($"Payment with ID {id} not found");
            }

            payment.TotalAmount = updateDto.TotalAmount;
            payment.PaidAmount = updateDto.PaidAmount;
            payment.PaymentDate = updateDto.PaymentDate;
            payment.IsActive = updateDto.IsActive;
            payment.ModifiedDate = DateTime.UtcNow;
            payment.ModifiedBy = updateDto.ModifiedBy;

            await _repository.UpdateAsync(payment, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating payment: {Id}", id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting payment: {Id}", id);

            var exists = await _repository.ExistsAsync(id, cancellationToken);
            if (!exists)
            {
                throw new InvalidOperationException($"Payment with ID {id} not found");
            }

            await _repository.DeleteAsync(id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting payment: {Id}", id);
            throw;
        }
    }

    public async Task<IEnumerable<PaymentDto>> GetByBillNoAsync(string billNo, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting payments by Bill No: {BillNo}", billNo);
            var payments = await _repository.GetByBillNoAsync(billNo, cancellationToken);
            return _mapper.Map<IEnumerable<PaymentDto>>(payments);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting payments by Bill No: {BillNo}", billNo);
            throw;
        }
    }

    public async Task<IEnumerable<PaymentDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Searching payments: {SearchTerm}", searchTerm);
            var payments = await _repository.SearchAsync(searchTerm, cancellationToken);
            return _mapper.Map<IEnumerable<PaymentDto>>(payments);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching payments: {SearchTerm}", searchTerm);
            throw;
        }
    }
}