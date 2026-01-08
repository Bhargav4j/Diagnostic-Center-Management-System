using AutoMapper;
using DiagnosticCenter.Application.DTOs;
using DiagnosticCenter.Application.Interfaces;
using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace DiagnosticCenter.Application.Services;

/// <summary>
/// Service implementation for test type operations.
/// Provides business logic for managing test types with validation, error handling, and logging.
/// </summary>
public class TestTypeService : ITestTypeService
{
    private readonly ITestTypeRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<TestTypeService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="TestTypeService"/> class.
    /// </summary>
    /// <param name="repository">The test type repository.</param>
    /// <param name="mapper">The AutoMapper instance.</param>
    /// <param name="logger">The logger instance.</param>
    /// <exception cref="ArgumentNullException">Thrown when any parameter is null.</exception>
    public TestTypeService(
        ITestTypeRepository repository,
        IMapper mapper,
        ILogger<TestTypeService> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<IEnumerable<TestTypeDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving all test types");

            var testTypes = await _repository.GetAllAsync(cancellationToken);
            var testTypeDtos = _mapper.Map<IEnumerable<TestTypeDto>>(testTypes);

            _logger.LogInformation("Successfully retrieved {Count} test types", testTypeDtos.Count());

            return testTypeDtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving all test types");
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<TestTypeDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving test type with ID: {Id}", id);

            if (id <= 0)
            {
                _logger.LogWarning("Invalid test type ID provided: {Id}", id);
                throw new ArgumentException("Test type ID must be greater than zero.", nameof(id));
            }

            var testType = await _repository.GetByIdAsync(id, cancellationToken);

            if (testType == null)
            {
                _logger.LogWarning("Test type with ID {Id} not found", id);
                return null;
            }

            var testTypeDto = _mapper.Map<TestTypeDto>(testType);

            _logger.LogInformation("Successfully retrieved test type with ID: {Id}", id);

            return testTypeDto;
        }
        catch (ArgumentException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving test type with ID: {Id}", id);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<TestTypeDto> CreateAsync(TestTypeCreateDto createDto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating new test type with name: {Name}", createDto.Name);

            if (createDto == null)
            {
                _logger.LogWarning("Null test type create DTO provided");
                throw new ArgumentNullException(nameof(createDto), "Test type create data cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(createDto.Name))
            {
                _logger.LogWarning("Empty test type name provided");
                throw new ArgumentException("Test type name is required.", nameof(createDto));
            }

            // Map DTO to entity
            var testType = _mapper.Map<TestType>(createDto);

            // Ensure audit fields are set
            testType.CreatedDate = DateTime.UtcNow;
            testType.IsActive = createDto.IsActive;

            // Save to repository
            var createdTestType = await _repository.AddAsync(testType, cancellationToken);

            // Map back to DTO
            var testTypeDto = _mapper.Map<TestTypeDto>(createdTestType);

            _logger.LogInformation("Successfully created test type with ID: {Id} and name: {Name}",
                createdTestType.Id, createdTestType.Name);

            return testTypeDto;
        }
        catch (ArgumentNullException)
        {
            throw;
        }
        catch (ArgumentException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating test type with name: {Name}", createDto?.Name);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task UpdateAsync(int id, TestTypeUpdateDto updateDto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating test type with ID: {Id}", id);

            if (id <= 0)
            {
                _logger.LogWarning("Invalid test type ID provided: {Id}", id);
                throw new ArgumentException("Test type ID must be greater than zero.", nameof(id));
            }

            if (updateDto == null)
            {
                _logger.LogWarning("Null test type update DTO provided for ID: {Id}", id);
                throw new ArgumentNullException(nameof(updateDto), "Test type update data cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(updateDto.Name))
            {
                _logger.LogWarning("Empty test type name provided for ID: {Id}", id);
                throw new ArgumentException("Test type name is required.", nameof(updateDto));
            }

            // Check if test type exists
            var existingTestType = await _repository.GetByIdAsync(id, cancellationToken);

            if (existingTestType == null)
            {
                _logger.LogWarning("Test type with ID {Id} not found for update", id);
                throw new InvalidOperationException($"Test type with ID {id} not found.");
            }

            // Map updates to existing entity
            _mapper.Map(updateDto, existingTestType);

            // Ensure audit fields are updated
            existingTestType.ModifiedDate = DateTime.UtcNow;

            // Update in repository
            await _repository.UpdateAsync(existingTestType, cancellationToken);

            _logger.LogInformation("Successfully updated test type with ID: {Id}", id);
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
            _logger.LogError(ex, "Error occurred while updating test type with ID: {Id}", id);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting test type with ID: {Id}", id);

            if (id <= 0)
            {
                _logger.LogWarning("Invalid test type ID provided: {Id}", id);
                throw new ArgumentException("Test type ID must be greater than zero.", nameof(id));
            }

            // Check if test type exists
            var exists = await _repository.ExistsAsync(id, cancellationToken);

            if (!exists)
            {
                _logger.LogWarning("Test type with ID {Id} not found for deletion", id);
                throw new InvalidOperationException($"Test type with ID {id} not found.");
            }

            // Delete from repository
            await _repository.DeleteAsync(id, cancellationToken);

            _logger.LogInformation("Successfully deleted test type with ID: {Id}", id);
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
            _logger.LogError(ex, "Error occurred while deleting test type with ID: {Id}", id);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<IEnumerable<TestTypeDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Searching test types with term: {SearchTerm}", searchTerm);

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                _logger.LogWarning("Empty search term provided, returning all test types");
                return await GetAllAsync(cancellationToken);
            }

            var testTypes = await _repository.SearchAsync(searchTerm, cancellationToken);
            var testTypeDtos = _mapper.Map<IEnumerable<TestTypeDto>>(testTypes);

            _logger.LogInformation("Successfully found {Count} test types matching search term: {SearchTerm}",
                testTypeDtos.Count(), searchTerm);

            return testTypeDtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while searching test types with term: {SearchTerm}", searchTerm);
            throw;
        }
    }
}
