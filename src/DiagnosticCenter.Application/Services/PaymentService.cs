using AutoMapper;
using DiagnosticCenter.Application.DTOs;
using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace DiagnosticCenter.Application.Services;

/// <summary>
/// Service for managing payment operations
/// </summary>
public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _repository;
    private readonly ITestEntryRepository _testEntryRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<PaymentService> _logger;

    public PaymentService(
        IPaymentRepository repository,
        ITestEntryRepository testEntryRepository,
        IMapper mapper,
        ILogger<PaymentService> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _testEntryRepository = testEntryRepository ?? throw new ArgumentNullException(nameof(testEntryRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<PaymentDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving all payments");
            var entities = await _repository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<PaymentDto>>(entities);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all payments");
            throw;
        }
    }

    public async Task<PaymentDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving payment with ID: {Id}", id);
            var entity = await _repository.GetByIdAsync(id, cancellationToken);
            return entity == null ? null : _mapper.Map<PaymentDto>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving payment with ID: {Id}", id);
            throw;
        }
    }

    public async Task<IEnumerable<PaymentDto>> GetByBillNoAsync(string billNo, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving payments for Bill No: {BillNo}", billNo);
            var entities = await _repository.GetByBillNoAsync(billNo, cancellationToken);
            return _mapper.Map<IEnumerable<PaymentDto>>(entities);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving payments for Bill No: {BillNo}", billNo);
            throw;
        }
    }

    public async Task<PaymentDto> CreateAsync(PaymentCreateDto dto, string createdBy, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating new payment for Bill No: {BillNo}", dto.BillNo);

            var testEntry = await _testEntryRepository.GetByIdAsync(dto.TestEntryId, cancellationToken);
            if (testEntry == null)
            {
                throw new KeyNotFoundException($"Test entry with ID {dto.TestEntryId} not found");
            }

            var entity = _mapper.Map<Payment>(dto);
            entity.CreatedDate = DateTime.UtcNow;
            entity.IsActive = true;
            entity.CreatedBy = createdBy;

            var created = await _repository.AddAsync(entity, cancellationToken);

            testEntry.PaidAmount += dto.Amount;
            await _testEntryRepository.UpdateAsync(testEntry, cancellationToken);

            _logger.LogInformation("Payment created with ID: {Id}", created.Id);
            return _mapper.Map<PaymentDto>(created);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating payment for Bill No: {BillNo}", dto.BillNo);
            throw;
        }
    }

    public async Task<PaymentDto> UpdateAsync(int id, PaymentUpdateDto dto, string modifiedBy, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating payment with ID: {Id}", id);
            var existing = await _repository.GetByIdAsync(id, cancellationToken);
            if (existing == null)
            {
                _logger.LogWarning("Payment not found with ID: {Id}", id);
                throw new KeyNotFoundException($"Payment with ID {id} not found");
            }

            var oldAmount = existing.Amount;
            existing.Amount = dto.Amount;
            existing.PaymentDate = dto.PaymentDate;
            existing.PaymentMethod = dto.PaymentMethod;
            existing.Notes = dto.Notes;
            existing.ModifiedDate = DateTime.UtcNow;
            existing.ModifiedBy = modifiedBy;

            await _repository.UpdateAsync(existing, cancellationToken);

            var testEntry = await _testEntryRepository.GetByIdAsync(existing.TestEntryId, cancellationToken);
            if (testEntry != null)
            {
                testEntry.PaidAmount = testEntry.PaidAmount - oldAmount + dto.Amount;
                await _testEntryRepository.UpdateAsync(testEntry, cancellationToken);
            }

            _logger.LogInformation("Payment updated with ID: {Id}", id);
            return _mapper.Map<PaymentDto>(existing);
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
            var payment = await _repository.GetByIdAsync(id, cancellationToken);
            if (payment != null)
            {
                var testEntry = await _testEntryRepository.GetByIdAsync(payment.TestEntryId, cancellationToken);
                if (testEntry != null)
                {
                    testEntry.PaidAmount -= payment.Amount;
                    await _testEntryRepository.UpdateAsync(testEntry, cancellationToken);
                }
            }

            await _repository.DeleteAsync(id, cancellationToken);
            _logger.LogInformation("Payment deleted with ID: {Id}", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting payment with ID: {Id}", id);
            throw;
        }
    }
}

public interface IPaymentService
{
    Task<IEnumerable<PaymentDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<PaymentDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<PaymentDto>> GetByBillNoAsync(string billNo, CancellationToken cancellationToken = default);
    Task<PaymentDto> CreateAsync(PaymentCreateDto dto, string createdBy, CancellationToken cancellationToken = default);
    Task<PaymentDto> UpdateAsync(int id, PaymentUpdateDto dto, string modifiedBy, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
