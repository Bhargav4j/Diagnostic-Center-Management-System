using AutoMapper;
using DiagnosticCenter.Application.DTOs;
using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace DiagnosticCenter.Application.Services;

/// <summary>
/// Service for managing test setup operations
/// </summary>
public class TestSetupService : ITestSetupService
{
    private readonly ITestSetupRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<TestSetupService> _logger;

    public TestSetupService(
        ITestSetupRepository repository,
        IMapper mapper,
        ILogger<TestSetupService> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<TestSetupDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving all test setups");
            var entities = await _repository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<TestSetupDto>>(entities);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all test setups");
            throw;
        }
    }

    public async Task<TestSetupDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving test setup with ID: {Id}", id);
            var entity = await _repository.GetByIdAsync(id, cancellationToken);
            return entity == null ? null : _mapper.Map<TestSetupDto>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving test setup with ID: {Id}", id);
            throw;
        }
    }

    public async Task<TestSetupDto> CreateAsync(TestSetupCreateDto dto, string createdBy, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating new test setup: {Name}", dto.Name);
            var entity = _mapper.Map<TestSetup>(dto);
            entity.CreatedDate = DateTime.UtcNow;
            entity.IsActive = true;
            entity.CreatedBy = createdBy;

            var created = await _repository.AddAsync(entity, cancellationToken);
            _logger.LogInformation("Test setup created with ID: {Id}", created.Id);
            return _mapper.Map<TestSetupDto>(created);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating test setup: {Name}", dto.Name);
            throw;
        }
    }

    public async Task<TestSetupDto> UpdateAsync(int id, TestSetupUpdateDto dto, string modifiedBy, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating test setup with ID: {Id}", id);
            var existing = await _repository.GetByIdAsync(id, cancellationToken);
            if (existing == null)
            {
                _logger.LogWarning("Test setup not found with ID: {Id}", id);
                throw new KeyNotFoundException($"Test setup with ID {id} not found");
            }

            existing.Name = dto.Name;
            existing.Fee = dto.Fee;
            existing.TypeId = dto.TypeId;
            existing.ModifiedDate = DateTime.UtcNow;
            existing.ModifiedBy = modifiedBy;

            await _repository.UpdateAsync(existing, cancellationToken);
            _logger.LogInformation("Test setup updated with ID: {Id}", id);
            return _mapper.Map<TestSetupDto>(existing);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating test setup with ID: {Id}", id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting test setup with ID: {Id}", id);
            await _repository.DeleteAsync(id, cancellationToken);
            _logger.LogInformation("Test setup deleted with ID: {Id}", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting test setup with ID: {Id}", id);
            throw;
        }
    }

    public async Task<IEnumerable<TestSetupDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Searching test setups with term: {SearchTerm}", searchTerm);
            var entities = await _repository.SearchAsync(searchTerm, cancellationToken);
            return _mapper.Map<IEnumerable<TestSetupDto>>(entities);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching test setups with term: {SearchTerm}", searchTerm);
            throw;
        }
    }

    public async Task<IEnumerable<TestSetupDto>> GetByTypeIdAsync(int typeId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving test setups by type ID: {TypeId}", typeId);
            var entities = await _repository.GetByTypeIdAsync(typeId, cancellationToken);
            return _mapper.Map<IEnumerable<TestSetupDto>>(entities);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving test setups by type ID: {TypeId}", typeId);
            throw;
        }
    }
}

public interface ITestSetupService
{
    Task<IEnumerable<TestSetupDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<TestSetupDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<TestSetupDto> CreateAsync(TestSetupCreateDto dto, string createdBy, CancellationToken cancellationToken = default);
    Task<TestSetupDto> UpdateAsync(int id, TestSetupUpdateDto dto, string modifiedBy, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<TestSetupDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
    Task<IEnumerable<TestSetupDto>> GetByTypeIdAsync(int typeId, CancellationToken cancellationToken = default);
}
