using AutoMapper;
using DiagnosticCenter.Application.DTOs;
using DiagnosticCenter.Application.Interfaces;
using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace DiagnosticCenter.Application.Services;

/// <summary>
/// Service implementation for test entry operations.
/// Provides business logic for managing test entries with validation, error handling, and logging.
/// </summary>
public class TestEntryService : ITestEntryService
{
    private readonly ITestEntryRepository _repository;
    private readonly ITestSetupRepository _testSetupRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<TestEntryService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="TestEntryService"/> class.
    /// </summary>
    /// <param name="repository">The test entry repository.</param>
    /// <param name="testSetupRepository">The test setup repository.</param>
    /// <param name="mapper">The AutoMapper instance.</param>
    /// <param name="logger">The logger instance.</param>
    /// <exception cref="ArgumentNullException">Thrown when any parameter is null.</exception>
    public TestEntryService(
        ITestEntryRepository repository,
        ITestSetupRepository testSetupRepository,
        IMapper mapper,
        ILogger<TestEntryService> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _testSetupRepository = testSetupRepository ?? throw new ArgumentNullException(nameof(testSetupRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<IEnumerable<TestEntryDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving all test entries");

            var testEntries = await _repository.GetAllAsync(cancellationToken);
            var testEntryDtos = _mapper.Map<IEnumerable<TestEntryDto>>(testEntries);

            _logger.LogInformation("Successfully retrieved {Count} test entries", testEntryDtos.Count());

            return testEntryDtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving all test entries");
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<TestEntryDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving test entry with ID: {Id}", id);

            if (id <= 0)
            {
                _logger.LogWarning("Invalid test entry ID provided: {Id}", id);
                throw new ArgumentException("Test entry ID must be greater than zero.", nameof(id));
            }

            var testEntry = await _repository.GetByIdAsync(id, cancellationToken);

            if (testEntry == null)
            {
                _logger.LogWarning("Test entry with ID {Id} not found", id);
                return null;
            }

            var testEntryDto = _mapper.Map<TestEntryDto>(testEntry);

            _logger.LogInformation("Successfully retrieved test entry with ID: {Id}", id);

            return testEntryDto;
        }
        catch (ArgumentException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving test entry with ID: {Id}", id);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<TestEntryDto> CreateAsync(TestEntryCreateDto createDto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating new test entry for patient: {PatientName}", createDto.PatientName);

            if (createDto == null)
            {
                _logger.LogWarning("Null test entry create DTO provided");
                throw new ArgumentNullException(nameof(createDto), "Test entry create data cannot be null.");
            }

            // Validate required fields
            if (string.IsNullOrWhiteSpace(createDto.PatientName))
            {
                _logger.LogWarning("Empty patient name provided");
                throw new ArgumentException("Patient name is required.", nameof(createDto));
            }

            if (string.IsNullOrWhiteSpace(createDto.MobileNo))
            {
                _logger.LogWarning("Empty mobile number provided");
                throw new ArgumentException("Mobile number is required.", nameof(createDto));
            }

            if (string.IsNullOrWhiteSpace(createDto.BillNo))
            {
                _logger.LogWarning("Empty bill number provided");
                throw new ArgumentException("Bill number is required.", nameof(createDto));
            }

            if (createDto.TotalAmount < 0)
            {
                _logger.LogWarning("Invalid total amount provided: {TotalAmount}", createDto.TotalAmount);
                throw new ArgumentException("Total amount cannot be negative.", nameof(createDto));
            }

            if (createDto.TestId <= 0)
            {
                _logger.LogWarning("Invalid test ID provided: {TestId}", createDto.TestId);
                throw new ArgumentException("Test ID must be greater than zero.", nameof(createDto));
            }

            // Validate that test setup exists
            var testSetupExists = await _testSetupRepository.ExistsAsync(createDto.TestId, cancellationToken);

            if (!testSetupExists)
            {
                _logger.LogWarning("Test setup with ID {TestId} not found", createDto.TestId);
                throw new InvalidOperationException($"Test setup with ID {createDto.TestId} not found.");
            }

            // Map DTO to entity
            var testEntry = _mapper.Map<TestEntry>(createDto);

            // Ensure audit fields are set
            testEntry.CreatedDate = DateTime.UtcNow;
            testEntry.IsActive = createDto.IsActive;
            testEntry.PaidAmount = 0m; // Initialize paid amount to zero

            // Save to repository
            var createdTestEntry = await _repository.AddAsync(testEntry, cancellationToken);

            // Map back to DTO
            var testEntryDto = _mapper.Map<TestEntryDto>(createdTestEntry);

            _logger.LogInformation("Successfully created test entry with ID: {Id} for patient: {PatientName}",
                createdTestEntry.Id, createdTestEntry.PatientName);

            return testEntryDto;
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
            _logger.LogError(ex, "Error occurred while creating test entry for patient: {PatientName}", createDto?.PatientName);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task UpdateAsync(int id, TestEntryUpdateDto updateDto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating test entry with ID: {Id}", id);

            if (id <= 0)
            {
                _logger.LogWarning("Invalid test entry ID provided: {Id}", id);
                throw new ArgumentException("Test entry ID must be greater than zero.", nameof(id));
            }

            if (updateDto == null)
            {
                _logger.LogWarning("Null test entry update DTO provided for ID: {Id}", id);
                throw new ArgumentNullException(nameof(updateDto), "Test entry update data cannot be null.");
            }

            // Validate required fields
            if (string.IsNullOrWhiteSpace(updateDto.PatientName))
            {
                _logger.LogWarning("Empty patient name provided for ID: {Id}", id);
                throw new ArgumentException("Patient name is required.", nameof(updateDto));
            }

            if (string.IsNullOrWhiteSpace(updateDto.MobileNo))
            {
                _logger.LogWarning("Empty mobile number provided for ID: {Id}", id);
                throw new ArgumentException("Mobile number is required.", nameof(updateDto));
            }

            if (string.IsNullOrWhiteSpace(updateDto.BillNo))
            {
                _logger.LogWarning("Empty bill number provided for ID: {Id}", id);
                throw new ArgumentException("Bill number is required.", nameof(updateDto));
            }

            if (updateDto.TotalAmount < 0)
            {
                _logger.LogWarning("Invalid total amount provided for ID {Id}: {TotalAmount}", id, updateDto.TotalAmount);
                throw new ArgumentException("Total amount cannot be negative.", nameof(updateDto));
            }

            if (updateDto.PaidAmount < 0)
            {
                _logger.LogWarning("Invalid paid amount provided for ID {Id}: {PaidAmount}", id, updateDto.PaidAmount);
                throw new ArgumentException("Paid amount cannot be negative.", nameof(updateDto));
            }

            if (updateDto.PaidAmount > updateDto.TotalAmount)
            {
                _logger.LogWarning("Paid amount ({PaidAmount}) exceeds total amount ({TotalAmount}) for ID {Id}",
                    updateDto.PaidAmount, updateDto.TotalAmount, id);
                throw new ArgumentException("Paid amount cannot exceed total amount.", nameof(updateDto));
            }

            if (updateDto.TestId <= 0)
            {
                _logger.LogWarning("Invalid test ID provided for ID {Id}: {TestId}", id, updateDto.TestId);
                throw new ArgumentException("Test ID must be greater than zero.", nameof(updateDto));
            }

            // Check if test entry exists
            var existingTestEntry = await _repository.GetByIdAsync(id, cancellationToken);

            if (existingTestEntry == null)
            {
                _logger.LogWarning("Test entry with ID {Id} not found for update", id);
                throw new InvalidOperationException($"Test entry with ID {id} not found.");
            }

            // Validate that test setup exists
            var testSetupExists = await _testSetupRepository.ExistsAsync(updateDto.TestId, cancellationToken);

            if (!testSetupExists)
            {
                _logger.LogWarning("Test setup with ID {TestId} not found", updateDto.TestId);
                throw new InvalidOperationException($"Test setup with ID {updateDto.TestId} not found.");
            }

            // Map updates to existing entity
            _mapper.Map(updateDto, existingTestEntry);

            // Ensure audit fields are updated
            existingTestEntry.ModifiedDate = DateTime.UtcNow;

            // Update in repository
            await _repository.UpdateAsync(existingTestEntry, cancellationToken);

            _logger.LogInformation("Successfully updated test entry with ID: {Id}", id);
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
            _logger.LogError(ex, "Error occurred while updating test entry with ID: {Id}", id);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting test entry with ID: {Id}", id);

            if (id <= 0)
            {
                _logger.LogWarning("Invalid test entry ID provided: {Id}", id);
                throw new ArgumentException("Test entry ID must be greater than zero.", nameof(id));
            }

            // Check if test entry exists
            var exists = await _repository.ExistsAsync(id, cancellationToken);

            if (!exists)
            {
                _logger.LogWarning("Test entry with ID {Id} not found for deletion", id);
                throw new InvalidOperationException($"Test entry with ID {id} not found.");
            }

            // Delete from repository
            await _repository.DeleteAsync(id, cancellationToken);

            _logger.LogInformation("Successfully deleted test entry with ID: {Id}", id);
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
            _logger.LogError(ex, "Error occurred while deleting test entry with ID: {Id}", id);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<IEnumerable<TestEntryDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Searching test entries with term: {SearchTerm}", searchTerm);

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                _logger.LogWarning("Empty search term provided, returning all test entries");
                return await GetAllAsync(cancellationToken);
            }

            var testEntries = await _repository.SearchAsync(searchTerm, cancellationToken);
            var testEntryDtos = _mapper.Map<IEnumerable<TestEntryDto>>(testEntries);

            _logger.LogInformation("Successfully found {Count} test entries matching search term: {SearchTerm}",
                testEntryDtos.Count(), searchTerm);

            return testEntryDtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while searching test entries with term: {SearchTerm}", searchTerm);
            throw;
        }
    }
}
