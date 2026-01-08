using AutoMapper;
using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Interfaces.Repositories;
using DiagnosticCenter.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace DiagnosticCenter.Application.Services;

/// <summary>
/// Service implementation for TestType operations
/// </summary>
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
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<TestTypeDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving all test types");
            var testTypes = await _repository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<TestTypeDto>>(testTypes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving test types");
            throw;
        }
    }

    public async Task<TestTypeDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving test type with ID: {Id}", id);
            var testType = await _repository.GetByIdAsync(id, cancellationToken);
            return testType == null ? null : _mapper.Map<TestTypeDto>(testType);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving test type with ID: {Id}", id);
            throw;
        }
    }

    public async Task<TestTypeDto> CreateAsync(TestTypeCreateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating test type: {Name}", dto.Name);

            if (await _repository.ExistsAsync(dto.Name, cancellationToken))
            {
                throw new InvalidOperationException($"Test type with name '{dto.Name}' already exists");
            }

            var testType = _mapper.Map<TestType>(dto);
            testType.CreatedDate = DateTime.UtcNow;
            testType.IsActive = true;

            var created = await _repository.AddAsync(testType, cancellationToken);
            _logger.LogInformation("Test type created with ID: {Id}", created.Id);

            return _mapper.Map<TestTypeDto>(created);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating test type: {Name}", dto.Name);
            throw;
        }
    }

    public async Task UpdateAsync(int id, TestTypeUpdateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating test type with ID: {Id}", id);

            var testType = await _repository.GetByIdAsync(id, cancellationToken);
            if (testType == null)
            {
                throw new InvalidOperationException($"Test type with ID {id} not found");
            }

            testType.Name = dto.Name;
            testType.ModifiedBy = dto.ModifiedBy;
            testType.ModifiedDate = DateTime.UtcNow;

            await _repository.UpdateAsync(testType, cancellationToken);
            _logger.LogInformation("Test type updated with ID: {Id}", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating test type with ID: {Id}", id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting test type with ID: {Id}", id);
            await _repository.DeleteAsync(id, cancellationToken);
            _logger.LogInformation("Test type deleted with ID: {Id}", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting test type with ID: {Id}", id);
            throw;
        }
    }

    public async Task<bool> ExistsAsync(string name, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _repository.ExistsAsync(name, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if test type exists: {Name}", name);
            throw;
        }
    }

    public async Task<IEnumerable<TestTypeDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Searching test types with term: {SearchTerm}", searchTerm);
            var testTypes = await _repository.SearchAsync(searchTerm, cancellationToken);
            return _mapper.Map<IEnumerable<TestTypeDto>>(testTypes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching test types with term: {SearchTerm}", searchTerm);
            throw;
        }
    }
}
