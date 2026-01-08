using AutoMapper;
using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Interfaces.Repositories;
using DiagnosticCenter.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace DiagnosticCenter.Application.Services;

/// <summary>
/// Service implementation for Payment operations
/// </summary>
public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly ITestEntryRepository _testEntryRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<PaymentService> _logger;

    public PaymentService(
        IPaymentRepository paymentRepository,
        ITestEntryRepository testEntryRepository,
        IMapper mapper,
        ILogger<PaymentService> logger)
    {
        _paymentRepository = paymentRepository;
        _testEntryRepository = testEntryRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<PaymentDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving all payments");
            var payments = await _paymentRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<PaymentDto>>(payments);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving payments");
            throw;
        }
    }

    public async Task<PaymentDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving payment with ID: {Id}", id);
            var payment = await _paymentRepository.GetByIdAsync(id, cancellationToken);
            return payment == null ? null : _mapper.Map<PaymentDto>(payment);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving payment with ID: {Id}", id);
            throw;
        }
    }

    public async Task<PaymentDto> CreateAsync(PaymentCreateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating payment for bill: {BillNumber}", dto.BillNumber);

            var testEntry = await _testEntryRepository.GetByIdAsync(dto.TestEntryId, cancellationToken);
            if (testEntry == null)
            {
                throw new InvalidOperationException($"Test entry with ID {dto.TestEntryId} not found");
            }

            var payment = _mapper.Map<Payment>(dto);
            payment.CreatedDate = DateTime.UtcNow;
            payment.PaymentDate = DateTime.UtcNow;
            payment.IsActive = true;

            var created = await _paymentRepository.AddAsync(payment, cancellationToken);

            testEntry.PaidAmount += dto.Amount;
            testEntry.ModifiedDate = DateTime.UtcNow;
            await _testEntryRepository.UpdateAsync(testEntry, cancellationToken);

            _logger.LogInformation("Payment created with ID: {Id}", created.Id);

            return _mapper.Map<PaymentDto>(created);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating payment for bill: {BillNumber}", dto.BillNumber);
            throw;
        }
    }

    public async Task<IEnumerable<PaymentDto>> GetByBillNumberAsync(string billNumber, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving payments for bill: {BillNumber}", billNumber);
            var payments = await _paymentRepository.GetByBillNumberAsync(billNumber, cancellationToken);
            return _mapper.Map<IEnumerable<PaymentDto>>(payments);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving payments for bill: {BillNumber}", billNumber);
            throw;
        }
    }

    public async Task<IEnumerable<PaymentDto>> GetByTestEntryIdAsync(int testEntryId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving payments for test entry: {TestEntryId}", testEntryId);
            var payments = await _paymentRepository.GetByTestEntryIdAsync(testEntryId, cancellationToken);
            return _mapper.Map<IEnumerable<PaymentDto>>(payments);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving payments for test entry: {TestEntryId}", testEntryId);
            throw;
        }
    }
}
