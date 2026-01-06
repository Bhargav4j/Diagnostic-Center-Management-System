using AutoMapper;
using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Interfaces.Repositories;
using DiagnosticCenter.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace DiagnosticCenter.Application.Services;

public class TestTypeService : ITestTypeService
{
    private readonly ITestTypeRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<TestTypeService> _logger;

    public TestTypeService(
        ITestTypeRepository repository,
        IMapper mapper,
        ILogger<TestTypeService> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<TestTypeDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting all test types from service");
            var testTypes = await _repository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<TestTypeDto>>(testTypes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all test types in service");
            throw;
        }
    }

    public async Task<TestTypeDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting test type by ID from service: {Id}", id);
            var testType = await _repository.GetByIdAsync(id, cancellationToken);
            return testType == null ? null : _mapper.Map<TestTypeDto>(testType);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting test type by ID in service: {Id}", id);
            throw;
        }
    }

    public async Task<TestTypeDto> CreateAsync(TestTypeCreateDto createDto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating test type: {Name}", createDto.Name);

            var exists = await _repository.ExistsByNameAsync(createDto.Name, cancellationToken);
            if (exists)
            {
                throw new InvalidOperationException($"Test type with name '{createDto.Name}' already exists");
            }

            var testType = _mapper.Map<TestType>(createDto);
            testType.CreatedDate = DateTime.UtcNow;
            testType.IsActive = true;

            var created = await _repository.AddAsync(testType, cancellationToken);
            return _mapper.Map<TestTypeDto>(created);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating test type: {Name}", createDto.Name);
            throw;
        }
    }

    public async Task UpdateAsync(int id, TestTypeUpdateDto updateDto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating test type: {Id}", id);

            var testType = await _repository.GetByIdAsync(id, cancellationToken);
            if (testType == null)
            {
                throw new InvalidOperationException($"Test type with ID {id} not found");
            }

            testType.Name = updateDto.Name;
            testType.Description = updateDto.Description;
            testType.IsActive = updateDto.IsActive;
            testType.ModifiedDate = DateTime.UtcNow;
            testType.ModifiedBy = updateDto.ModifiedBy;

            await _repository.UpdateAsync(testType, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating test type: {Id}", id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting test type: {Id}", id);

            var exists = await _repository.ExistsAsync(id, cancellationToken);
            if (!exists)
            {
                throw new InvalidOperationException($"Test type with ID {id} not found");
            }

            await _repository.DeleteAsync(id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting test type: {Id}", id);
            throw;
        }
    }

    public async Task<IEnumerable<TestTypeDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Searching test types: {SearchTerm}", searchTerm);
            var testTypes = await _repository.SearchAsync(searchTerm, cancellationToken);
            return _mapper.Map<IEnumerable<TestTypeDto>>(testTypes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching test types: {SearchTerm}", searchTerm);
            throw;
        }
    }
}
