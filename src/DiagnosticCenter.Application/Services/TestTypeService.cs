using AutoMapper;
using DiagnosticCenter.Application.DTOs;
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
            _logger.LogError(ex, "Error retrieving all test types");
            throw;
        }
    }

    public async Task<TestTypeDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving test type with ID: {Id}", id);
            var testType = await _repository.GetByIdAsync(id, cancellationToken);
            return testType != null ? _mapper.Map<TestTypeDto>(testType) : null;
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

            var testType = _mapper.Map<TestType>(dto);
            var createdTestType = await _repository.AddAsync(testType, cancellationToken);

            _logger.LogInformation("Successfully created test type with ID: {Id}", createdTestType.Id);
            return _mapper.Map<TestTypeDto>(createdTestType);
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

            var existingTestType = await _repository.GetByIdAsync(id, cancellationToken);
            if (existingTestType == null)
            {
                throw new InvalidOperationException($"Test type with ID {id} not found");
            }

            _mapper.Map(dto, existingTestType);
            await _repository.UpdateAsync(existingTestType, cancellationToken);

            _logger.LogInformation("Successfully updated test type with ID: {Id}", id);
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
            _logger.LogInformation("Successfully deleted test type with ID: {Id}", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting test type with ID: {Id}", id);
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
