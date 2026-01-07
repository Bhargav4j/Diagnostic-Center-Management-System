using AutoMapper;
using Microsoft.Extensions.Logging;
using DiagnosticCenter.Application.DTOs;
using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Interfaces.Repositories;
using DiagnosticCenter.Application.Interfaces;

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
            _logger.LogInformation("Getting all test types");
            var testTypes = await _repository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<TestTypeDto>>(testTypes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all test types");
            throw;
        }
    }

    public async Task<TestTypeDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting test type with ID: {Id}", id);
            var testType = await _repository.GetByIdAsync(id, cancellationToken);
            return testType == null ? null : _mapper.Map<TestTypeDto>(testType);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting test type with ID: {Id}", id);
            throw;
        }
    }

    public async Task<TestTypeDto> CreateAsync(TestTypeCreateDto createDto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating new test type: {Name}", createDto.Name);
            var testType = _mapper.Map<TestType>(createDto);
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
            _logger.LogInformation("Updating test type with ID: {Id}", id);
            var existing = await _repository.GetByIdAsync(id, cancellationToken);
            if (existing == null)
            {
                throw new InvalidOperationException($"Test type with ID {id} not found");
            }

            _mapper.Map(updateDto, existing);
            existing.ModifiedDate = DateTime.UtcNow;
            await _repository.UpdateAsync(existing, cancellationToken);
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
