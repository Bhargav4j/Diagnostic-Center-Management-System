using AutoMapper;
using DiagnosticCenter.Application.DTOs;
using DiagnosticCenter.Application.Interfaces;
using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace DiagnosticCenter.Application.Services;

/// <summary>
/// Service implementation for payment operations.
/// Provides business logic for managing payments with validation, error handling, and logging.
/// </summary>
public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _repository;
    private readonly ITestEntryRepository _testEntryRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<PaymentService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="PaymentService"/> class.
    /// </summary>
    /// <param name="repository">The payment repository.</param>
    /// <param name="testEntryRepository">The test entry repository.</param>
    /// <param name="mapper">The AutoMapper instance.</param>
    /// <param name="logger">The logger instance.</param>
    /// <exception cref="ArgumentNullException">Thrown when any parameter is null.</exception>
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

    /// <inheritdoc />
    public async Task<IEnumerable<PaymentDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving all payments");

            var payments = await _repository.GetAllAsync(cancellationToken);
            var paymentDtos = _mapper.Map<IEnumerable<PaymentDto>>(payments);

            _logger.LogInformation("Successfully retrieved {Count} payments", paymentDtos.Count());

            return paymentDtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving all payments");
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<PaymentDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving payment with ID: {Id}", id);

            if (id <= 0)
            {
                _logger.LogWarning("Invalid payment ID provided: {Id}", id);
                throw new ArgumentException("Payment ID must be greater than zero.", nameof(id));
            }

            var payment = await _repository.GetByIdAsync(id, cancellationToken);

            if (payment == null)
            {
                _logger.LogWarning("Payment with ID {Id} not found", id);
                return null;
            }

            var paymentDto = _mapper.Map<PaymentDto>(payment);

            _logger.LogInformation("Successfully retrieved payment with ID: {Id}", id);

            return paymentDto;
        }
        catch (ArgumentException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving payment with ID: {Id}", id);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<PaymentDto> CreateAsync(PaymentCreateDto createDto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating new payment for bill: {BillNo}", createDto.BillNo);

            if (createDto == null)
            {
                _logger.LogWarning("Null payment create DTO provided");
                throw new ArgumentNullException(nameof(createDto), "Payment create data cannot be null.");
            }

            // Validate required fields
            if (string.IsNullOrWhiteSpace(createDto.BillNo))
            {
                _logger.LogWarning("Empty bill number provided");
                throw new ArgumentException("Bill number is required.", nameof(createDto));
            }

            if (createDto.Amount <= 0)
            {
                _logger.LogWarning("Invalid payment amount provided: {Amount}", createDto.Amount);
                throw new ArgumentException("Payment amount must be greater than zero.", nameof(createDto));
            }

            if (createDto.TestEntryId <= 0)
            {
                _logger.LogWarning("Invalid test entry ID provided: {TestEntryId}", createDto.TestEntryId);
                throw new ArgumentException("Test entry ID must be greater than zero.", nameof(createDto));
            }

            // Validate that test entry exists
            var testEntry = await _testEntryRepository.GetByIdAsync(createDto.TestEntryId, cancellationToken);

            if (testEntry == null)
            {
                _logger.LogWarning("Test entry with ID {TestEntryId} not found", createDto.TestEntryId);
                throw new InvalidOperationException($"Test entry with ID {createDto.TestEntryId} not found.");
            }

            // Validate that payment doesn't exceed remaining balance
            var remainingBalance = testEntry.TotalAmount - testEntry.PaidAmount;

            if (createDto.Amount > remainingBalance)
            {
                _logger.LogWarning("Payment amount ({Amount}) exceeds remaining balance ({RemainingBalance}) for test entry {TestEntryId}",
                    createDto.Amount, remainingBalance, createDto.TestEntryId);
                throw new InvalidOperationException($"Payment amount cannot exceed remaining balance of {remainingBalance:C}.");
            }

            // Map DTO to entity
            var payment = _mapper.Map<Payment>(createDto);

            // Ensure audit fields are set
            payment.CreatedDate = DateTime.UtcNow;
            payment.IsActive = createDto.IsActive;

            // Set payment date if not provided
            if (payment.PaymentDate == default)
            {
                payment.PaymentDate = DateTime.UtcNow;
            }

            // Save payment to repository
            var createdPayment = await _repository.AddAsync(payment, cancellationToken);

            // Update test entry's paid amount
            testEntry.PaidAmount += createDto.Amount;
            testEntry.ModifiedDate = DateTime.UtcNow;
            await _testEntryRepository.UpdateAsync(testEntry, cancellationToken);

            // Map back to DTO
            var paymentDto = _mapper.Map<PaymentDto>(createdPayment);

            _logger.LogInformation("Successfully created payment with ID: {Id} for bill: {BillNo}, amount: {Amount}",
                createdPayment.Id, createdPayment.BillNo, createdPayment.Amount);

            return paymentDto;
        }
        catch (ArgumentNullException)
        {
            throw;
        }
        catch (ArgumentException)
        {
            throw;
        }
        catch (InvalidOperationException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating payment for bill: {BillNo}", createDto?.BillNo);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task UpdateAsync(int id, PaymentUpdateDto updateDto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating payment with ID: {Id}", id);

            if (id <= 0)
            {
                _logger.LogWarning("Invalid payment ID provided: {Id}", id);
                throw new ArgumentException("Payment ID must be greater than zero.", nameof(id));
            }

            if (updateDto == null)
            {
                _logger.LogWarning("Null payment update DTO provided for ID: {Id}", id);
                throw new ArgumentNullException(nameof(updateDto), "Payment update data cannot be null.");
            }

            // Validate required fields
            if (string.IsNullOrWhiteSpace(updateDto.BillNo))
            {
                _logger.LogWarning("Empty bill number provided for ID: {Id}", id);
                throw new ArgumentException("Bill number is required.", nameof(updateDto));
            }

            if (updateDto.Amount <= 0)
            {
                _logger.LogWarning("Invalid payment amount provided for ID {Id}: {Amount}", id, updateDto.Amount);
                throw new ArgumentException("Payment amount must be greater than zero.", nameof(updateDto));
            }

            if (updateDto.TestEntryId <= 0)
            {
                _logger.LogWarning("Invalid test entry ID provided for ID {Id}: {TestEntryId}", id, updateDto.TestEntryId);
                throw new ArgumentException("Test entry ID must be greater than zero.", nameof(updateDto));
            }

            // Check if payment exists
            var existingPayment = await _repository.GetByIdAsync(id, cancellationToken);

            if (existingPayment == null)
            {
                _logger.LogWarning("Payment with ID {Id} not found for update", id);
                throw new InvalidOperationException($"Payment with ID {id} not found.");
            }

            // Validate that test entry exists
            var testEntry = await _testEntryRepository.GetByIdAsync(updateDto.TestEntryId, cancellationToken);

            if (testEntry == null)
            {
                _logger.LogWarning("Test entry with ID {TestEntryId} not found", updateDto.TestEntryId);
                throw new InvalidOperationException($"Test entry with ID {updateDto.TestEntryId} not found.");
            }

            // Calculate the difference in payment amount
            var amountDifference = updateDto.Amount - existingPayment.Amount;

            // Validate that the new amount doesn't exceed remaining balance
            var remainingBalance = testEntry.TotalAmount - testEntry.PaidAmount + existingPayment.Amount;

            if (updateDto.Amount > remainingBalance)
            {
                _logger.LogWarning("Updated payment amount ({Amount}) exceeds remaining balance ({RemainingBalance}) for test entry {TestEntryId}",
                    updateDto.Amount, remainingBalance, updateDto.TestEntryId);
                throw new InvalidOperationException($"Payment amount cannot exceed remaining balance of {remainingBalance:C}.");
            }

            // Map updates to existing entity
            _mapper.Map(updateDto, existingPayment);

            // Ensure audit fields are updated
            existingPayment.ModifiedDate = DateTime.UtcNow;

            // Update payment in repository
            await _repository.UpdateAsync(existingPayment, cancellationToken);

            // Update test entry's paid amount if amount changed
            if (amountDifference != 0)
            {
                testEntry.PaidAmount += amountDifference;
                testEntry.ModifiedDate = DateTime.UtcNow;
                await _testEntryRepository.UpdateAsync(testEntry, cancellationToken);

                _logger.LogInformation("Updated test entry {TestEntryId} paid amount by {AmountDifference}",
                    testEntry.Id, amountDifference);
            }

            _logger.LogInformation("Successfully updated payment with ID: {Id}", id);
        }
        catch (ArgumentNullException)
        {
            throw;
        }
        catch (ArgumentException)
        {
            throw;
        }
        catch (InvalidOperationException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating payment with ID: {Id}", id);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting payment with ID: {Id}", id);

            if (id <= 0)
            {
                _logger.LogWarning("Invalid payment ID provided: {Id}", id);
                throw new ArgumentException("Payment ID must be greater than zero.", nameof(id));
            }

            // Check if payment exists
            var payment = await _repository.GetByIdAsync(id, cancellationToken);

            if (payment == null)
            {
                _logger.LogWarning("Payment with ID {Id} not found for deletion", id);
                throw new InvalidOperationException($"Payment with ID {id} not found.");
            }

            // Get the associated test entry
            var testEntry = await _testEntryRepository.GetByIdAsync(payment.TestEntryId, cancellationToken);

            if (testEntry != null)
            {
                // Reduce the paid amount on the test entry
                testEntry.PaidAmount -= payment.Amount;
                testEntry.ModifiedDate = DateTime.UtcNow;
                await _testEntryRepository.UpdateAsync(testEntry, cancellationToken);

                _logger.LogInformation("Reduced test entry {TestEntryId} paid amount by {Amount}",
                    testEntry.Id, payment.Amount);
            }

            // Delete from repository
            await _repository.DeleteAsync(id, cancellationToken);

            _logger.LogInformation("Successfully deleted payment with ID: {Id}", id);
        }
        catch (ArgumentException)
        {
            throw;
        }
        catch (InvalidOperationException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting payment with ID: {Id}", id);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<IEnumerable<PaymentDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Searching payments with term: {SearchTerm}", searchTerm);

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                _logger.LogWarning("Empty search term provided, returning all payments");
                return await GetAllAsync(cancellationToken);
            }

            var payments = await _repository.SearchAsync(searchTerm, cancellationToken);
            var paymentDtos = _mapper.Map<IEnumerable<PaymentDto>>(payments);

            _logger.LogInformation("Successfully found {Count} payments matching search term: {SearchTerm}",
                paymentDtos.Count(), searchTerm);

            return paymentDtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while searching payments with term: {SearchTerm}", searchTerm);
            throw;
        }
    }
}
