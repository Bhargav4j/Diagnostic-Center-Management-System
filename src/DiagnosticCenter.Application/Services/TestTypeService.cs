using AutoMapper;
using DiagnosticCenter.Application.DTOs;
using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace DiagnosticCenter.Application.Services;

/// <summary>
/// Service for managing test type operations
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
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<TestTypeDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving all test types");
            var entities = await _repository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<TestTypeDto>>(entities);
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
            var entity = await _repository.GetByIdAsync(id, cancellationToken);
            return entity == null ? null : _mapper.Map<TestTypeDto>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving test type with ID: {Id}", id);
            throw;
        }
    }

    public async Task<TestTypeDto> CreateAsync(TestTypeCreateDto dto, string createdBy, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating new test type: {Name}", dto.Name);
            var entity = _mapper.Map<TestType>(dto);
            entity.CreatedDate = DateTime.UtcNow;
            entity.IsActive = true;
            entity.CreatedBy = createdBy;

            var created = await _repository.AddAsync(entity, cancellationToken);
            _logger.LogInformation("Test type created with ID: {Id}", created.Id);
            return _mapper.Map<TestTypeDto>(created);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating test type: {Name}", dto.Name);
            throw;
        }
    }

    public async Task<TestTypeDto> UpdateAsync(int id, TestTypeUpdateDto dto, string modifiedBy, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating test type with ID: {Id}", id);
            var existing = await _repository.GetByIdAsync(id, cancellationToken);
            if (existing == null)
            {
                _logger.LogWarning("Test type not found with ID: {Id}", id);
                throw new KeyNotFoundException($"Test type with ID {id} not found");
            }

            existing.Name = dto.Name;
            existing.Description = dto.Description;
            existing.ModifiedDate = DateTime.UtcNow;
            existing.ModifiedBy = modifiedBy;

            await _repository.UpdateAsync(existing, cancellationToken);
            _logger.LogInformation("Test type updated with ID: {Id}", id);
            return _mapper.Map<TestTypeDto>(existing);
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

    public async Task<IEnumerable<TestTypeDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Searching test types with term: {SearchTerm}", searchTerm);
            var entities = await _repository.SearchAsync(searchTerm, cancellationToken);
            return _mapper.Map<IEnumerable<TestTypeDto>>(entities);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching test types with term: {SearchTerm}", searchTerm);
            throw;
        }
    }
}

public interface ITestTypeService
{
    Task<IEnumerable<TestTypeDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<TestTypeDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<TestTypeDto> CreateAsync(TestTypeCreateDto dto, string createdBy, CancellationToken cancellationToken = default);
    Task<TestTypeDto> UpdateAsync(int id, TestTypeUpdateDto dto, string modifiedBy, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<TestTypeDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}
