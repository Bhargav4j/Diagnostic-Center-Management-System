using AutoMapper;
using DiagnosticCenter.Application.DTOs;
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
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<PaymentDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving all payments");
            var payments = await _repository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<PaymentDto>>(payments);
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
            var payment = await _repository.GetByIdAsync(id, cancellationToken);
            return payment != null ? _mapper.Map<PaymentDto>(payment) : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving payment with ID: {Id}", id);
            throw;
        }
    }

    public async Task<PaymentDto?> GetByBillNoAsync(string billNo, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving payment with Bill No: {BillNo}", billNo);
            var payment = await _repository.GetByBillNoAsync(billNo, cancellationToken);
            return payment != null ? _mapper.Map<PaymentDto>(payment) : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving payment with Bill No: {BillNo}", billNo);
            throw;
        }
    }

    public async Task<PaymentDto> CreateAsync(PaymentCreateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating payment: {BillNo}", dto.BillNo);

            var payment = _mapper.Map<Payment>(dto);
            var createdPayment = await _repository.AddAsync(payment, cancellationToken);

            _logger.LogInformation("Successfully created payment with ID: {Id}", createdPayment.Id);
            return _mapper.Map<PaymentDto>(createdPayment);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating payment: {BillNo}", dto.BillNo);
            throw;
        }
    }

    public async Task UpdateAsync(int id, PaymentUpdateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating payment with ID: {Id}", id);

            var existingPayment = await _repository.GetByIdAsync(id, cancellationToken);
            if (existingPayment == null)
            {
                throw new InvalidOperationException($"Payment with ID {id} not found");
            }

            _mapper.Map(dto, existingPayment);
            await _repository.UpdateAsync(existingPayment, cancellationToken);

            _logger.LogInformation("Successfully updated payment with ID: {Id}", id);
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
            _logger.LogInformation("Successfully deleted payment with ID: {Id}", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting payment with ID: {Id}", id);
            throw;
        }
    }

    public async Task<IEnumerable<PaymentDto>> GetUnpaidBillsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving unpaid bills");
            var payments = await _repository.GetUnpaidBillsAsync(cancellationToken);
            return _mapper.Map<IEnumerable<PaymentDto>>(payments);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving unpaid bills");
            throw;
        }
    }

    public async Task<IEnumerable<PaymentDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Searching payments with term: {SearchTerm}", searchTerm);
            var payments = await _repository.SearchAsync(searchTerm, cancellationToken);
            return _mapper.Map<IEnumerable<PaymentDto>>(payments);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching payments with term: {SearchTerm}", searchTerm);
            throw;
        }
    }
}
