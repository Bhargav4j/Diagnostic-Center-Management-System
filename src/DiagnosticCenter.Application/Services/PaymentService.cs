using AutoMapper;
using Microsoft.Extensions.Logging;
using DiagnosticCenter.Application.DTOs;
using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Interfaces.Repositories;
using DiagnosticCenter.Application.Interfaces;

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
            _logger.LogInformation("Getting all payments");
            var payments = await _repository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<PaymentDto>>(payments);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all payments");
            throw;
        }
    }

    public async Task<PaymentDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting payment with ID: {Id}", id);
            var payment = await _repository.GetByIdAsync(id, cancellationToken);
            return payment == null ? null : _mapper.Map<PaymentDto>(payment);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting payment with ID: {Id}", id);
            throw;
        }
    }

    public async Task<PaymentDto> CreateAsync(PaymentCreateDto createDto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating new payment for test entry: {TestEntryId}", createDto.TestEntryId);
            var payment = _mapper.Map<Payment>(createDto);
            var created = await _repository.AddAsync(payment, cancellationToken);
            return _mapper.Map<PaymentDto>(created);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating payment for test entry: {TestEntryId}", createDto.TestEntryId);
            throw;
        }
    }

    public async Task UpdateAsync(int id, PaymentUpdateDto updateDto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating payment with ID: {Id}", id);
            var existing = await _repository.GetByIdAsync(id, cancellationToken);
            if (existing == null)
            {
                throw new InvalidOperationException($"Payment with ID {id} not found");
            }

            _mapper.Map(updateDto, existing);
            existing.ModifiedDate = DateTime.UtcNow;
            await _repository.UpdateAsync(existing, cancellationToken);
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
            await _repository.DeleteAsync(id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting payment with ID: {Id}", id);
            throw;
        }
    }

    public async Task<IEnumerable<PaymentDto>> GetByTestEntryIdAsync(int testEntryId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting payments for test entry: {TestEntryId}", testEntryId);
            var payments = await _repository.GetByTestEntryIdAsync(testEntryId, cancellationToken);
            return _mapper.Map<IEnumerable<PaymentDto>>(payments);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting payments for test entry: {TestEntryId}", testEntryId);
            throw;
        }
    }
}
