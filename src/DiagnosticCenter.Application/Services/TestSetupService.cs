using AutoMapper;
using DiagnosticCenter.Application.DTOs;
using DiagnosticCenter.Application.Interfaces;
using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace DiagnosticCenter.Application.Services;

/// <summary>
/// Service implementation for test setup operations.
/// Provides business logic for managing test setups with validation, error handling, and logging.
/// </summary>
public class TestSetupService : ITestSetupService
{
    private readonly ITestSetupRepository _repository;
    private readonly ITestTypeRepository _testTypeRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<TestSetupService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="TestSetupService"/> class.
    /// </summary>
    /// <param name="repository">The test setup repository.</param>
    /// <param name="testTypeRepository">The test type repository.</param>
    /// <param name="mapper">The AutoMapper instance.</param>
    /// <param name="logger">The logger instance.</param>
    /// <exception cref="ArgumentNullException">Thrown when any parameter is null.</exception>
    public TestSetupService(
        ITestSetupRepository repository,
        ITestTypeRepository testTypeRepository,
        IMapper mapper,
        ILogger<TestSetupService> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _testTypeRepository = testTypeRepository ?? throw new ArgumentNullException(nameof(testTypeRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<IEnumerable<TestSetupDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving all test setups");

            var testSetups = await _repository.GetAllAsync(cancellationToken);
            var testSetupDtos = _mapper.Map<IEnumerable<TestSetupDto>>(testSetups);

            _logger.LogInformation("Successfully retrieved {Count} test setups", testSetupDtos.Count());

            return testSetupDtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving all test setups");
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<TestSetupDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving test setup with ID: {Id}", id);

            if (id <= 0)
            {
                _logger.LogWarning("Invalid test setup ID provided: {Id}", id);
                throw new ArgumentException("Test setup ID must be greater than zero.", nameof(id));
            }

            var testSetup = await _repository.GetByIdAsync(id, cancellationToken);

            if (testSetup == null)
            {
                _logger.LogWarning("Test setup with ID {Id} not found", id);
                return null;
            }

            var testSetupDto = _mapper.Map<TestSetupDto>(testSetup);

            _logger.LogInformation("Successfully retrieved test setup with ID: {Id}", id);

            return testSetupDto;
        }
        catch (ArgumentException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving test setup with ID: {Id}", id);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<TestSetupDto> CreateAsync(TestSetupCreateDto createDto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating new test setup with name: {Name}", createDto.Name);

            if (createDto == null)
            {
                _logger.LogWarning("Null test setup create DTO provided");
                throw new ArgumentNullException(nameof(createDto), "Test setup create data cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(createDto.Name))
            {
                _logger.LogWarning("Empty test setup name provided");
                throw new ArgumentException("Test setup name is required.", nameof(createDto));
            }

            if (createDto.Fee < 0)
            {
                _logger.LogWarning("Invalid fee amount provided: {Fee}", createDto.Fee);
                throw new ArgumentException("Test setup fee cannot be negative.", nameof(createDto));
            }

            if (createDto.TypeId <= 0)
            {
                _logger.LogWarning("Invalid test type ID provided: {TypeId}", createDto.TypeId);
                throw new ArgumentException("Test type ID must be greater than zero.", nameof(createDto));
            }

            // Validate that test type exists
            var testTypeExists = await _testTypeRepository.ExistsAsync(createDto.TypeId, cancellationToken);

            if (!testTypeExists)
            {
                _logger.LogWarning("Test type with ID {TypeId} not found", createDto.TypeId);
                throw new InvalidOperationException($"Test type with ID {createDto.TypeId} not found.");
            }

            // Map DTO to entity
            var testSetup = _mapper.Map<TestSetup>(createDto);

            // Ensure audit fields are set
            testSetup.CreatedDate = DateTime.UtcNow;
            testSetup.IsActive = createDto.IsActive;

            // Save to repository
            var createdTestSetup = await _repository.AddAsync(testSetup, cancellationToken);

            // Map back to DTO
            var testSetupDto = _mapper.Map<TestSetupDto>(createdTestSetup);

            _logger.LogInformation("Successfully created test setup with ID: {Id} and name: {Name}",
                createdTestSetup.Id, createdTestSetup.Name);

            return testSetupDto;
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
            _logger.LogError(ex, "Error occurred while creating test setup with name: {Name}", createDto?.Name);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task UpdateAsync(int id, TestSetupUpdateDto updateDto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating test setup with ID: {Id}", id);

            if (id <= 0)
            {
                _logger.LogWarning("Invalid test setup ID provided: {Id}", id);
                throw new ArgumentException("Test setup ID must be greater than zero.", nameof(id));
            }

            if (updateDto == null)
            {
                _logger.LogWarning("Null test setup update DTO provided for ID: {Id}", id);
                throw new ArgumentNullException(nameof(updateDto), "Test setup update data cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(updateDto.Name))
            {
                _logger.LogWarning("Empty test setup name provided for ID: {Id}", id);
                throw new ArgumentException("Test setup name is required.", nameof(updateDto));
            }

            if (updateDto.Fee < 0)
            {
                _logger.LogWarning("Invalid fee amount provided for ID {Id}: {Fee}", id, updateDto.Fee);
                throw new ArgumentException("Test setup fee cannot be negative.", nameof(updateDto));
            }

            if (updateDto.TypeId <= 0)
            {
                _logger.LogWarning("Invalid test type ID provided for ID {Id}: {TypeId}", id, updateDto.TypeId);
                throw new ArgumentException("Test type ID must be greater than zero.", nameof(updateDto));
            }

            // Check if test setup exists
            var existingTestSetup = await _repository.GetByIdAsync(id, cancellationToken);

            if (existingTestSetup == null)
            {
                _logger.LogWarning("Test setup with ID {Id} not found for update", id);
                throw new InvalidOperationException($"Test setup with ID {id} not found.");
            }

            // Validate that test type exists
            var testTypeExists = await _testTypeRepository.ExistsAsync(updateDto.TypeId, cancellationToken);

            if (!testTypeExists)
            {
                _logger.LogWarning("Test type with ID {TypeId} not found", updateDto.TypeId);
                throw new InvalidOperationException($"Test type with ID {updateDto.TypeId} not found.");
            }

            // Map updates to existing entity
            _mapper.Map(updateDto, existingTestSetup);

            // Ensure audit fields are updated
            existingTestSetup.ModifiedDate = DateTime.UtcNow;

            // Update in repository
            await _repository.UpdateAsync(existingTestSetup, cancellationToken);

            _logger.LogInformation("Successfully updated test setup with ID: {Id}", id);
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
            _logger.LogError(ex, "Error occurred while updating test setup with ID: {Id}", id);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting test setup with ID: {Id}", id);

            if (id <= 0)
            {
                _logger.LogWarning("Invalid test setup ID provided: {Id}", id);
                throw new ArgumentException("Test setup ID must be greater than zero.", nameof(id));
            }

            // Check if test setup exists
            var exists = await _repository.ExistsAsync(id, cancellationToken);

            if (!exists)
            {
                _logger.LogWarning("Test setup with ID {Id} not found for deletion", id);
                throw new InvalidOperationException($"Test setup with ID {id} not found.");
            }

            // Delete from repository
            await _repository.DeleteAsync(id, cancellationToken);

            _logger.LogInformation("Successfully deleted test setup with ID: {Id}", id);
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
            _logger.LogError(ex, "Error occurred while deleting test setup with ID: {Id}", id);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<IEnumerable<TestSetupDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Searching test setups with term: {SearchTerm}", searchTerm);

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                _logger.LogWarning("Empty search term provided, returning all test setups");
                return await GetAllAsync(cancellationToken);
            }

            var testSetups = await _repository.SearchAsync(searchTerm, cancellationToken);
            var testSetupDtos = _mapper.Map<IEnumerable<TestSetupDto>>(testSetups);

            _logger.LogInformation("Successfully found {Count} test setups matching search term: {SearchTerm}",
                testSetupDtos.Count(), searchTerm);

            return testSetupDtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while searching test setups with term: {SearchTerm}", searchTerm);
            throw;
        }
    }
}
