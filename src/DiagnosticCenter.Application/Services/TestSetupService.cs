using AutoMapper;
using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Interfaces.Repositories;
using DiagnosticCenter.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace DiagnosticCenter.Application.Services;

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
            _logger.LogInformation("Getting all test setups from service");
            var testSetups = await _repository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<TestSetupDto>>(testSetups);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all test setups in service");
            throw;
        }
    }

    public async Task<TestSetupDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting test setup by ID from service: {Id}", id);
            var testSetup = await _repository.GetByIdAsync(id, cancellationToken);
            return testSetup == null ? null : _mapper.Map<TestSetupDto>(testSetup);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting test setup by ID in service: {Id}", id);
            throw;
        }
    }

    public async Task<TestSetupDto> CreateAsync(TestSetupCreateDto createDto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating test setup: {Name}", createDto.Name);

            var testSetup = _mapper.Map<TestSetup>(createDto);
            testSetup.CreatedDate = DateTime.UtcNow;
            testSetup.IsActive = true;

            var created = await _repository.AddAsync(testSetup, cancellationToken);
            return _mapper.Map<TestSetupDto>(created);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating test setup: {Name}", createDto.Name);
            throw;
        }
    }

    public async Task UpdateAsync(int id, TestSetupUpdateDto updateDto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating test setup: {Id}", id);

            var testSetup = await _repository.GetByIdAsync(id, cancellationToken);
            if (testSetup == null)
            {
                throw new InvalidOperationException($"Test setup with ID {id} not found");
            }

            testSetup.Name = updateDto.Name;
            testSetup.Description = updateDto.Description;
            testSetup.TypeId = updateDto.TypeId;
            testSetup.Fee = updateDto.Fee;
            testSetup.IsActive = updateDto.IsActive;
            testSetup.ModifiedDate = DateTime.UtcNow;
            testSetup.ModifiedBy = updateDto.ModifiedBy;

            await _repository.UpdateAsync(testSetup, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating test setup: {Id}", id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting test setup: {Id}", id);

            var exists = await _repository.ExistsAsync(id, cancellationToken);
            if (!exists)
            {
                throw new InvalidOperationException($"Test setup with ID {id} not found");
            }

            await _repository.DeleteAsync(id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting test setup: {Id}", id);
            throw;
        }
    }

    public async Task<IEnumerable<TestSetupDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Searching test setups: {SearchTerm}", searchTerm);
            var testSetups = await _repository.SearchAsync(searchTerm, cancellationToken);
            return _mapper.Map<IEnumerable<TestSetupDto>>(testSetups);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching test setups: {SearchTerm}", searchTerm);
            throw;
        }
    }

    public async Task<IEnumerable<TestSetupDto>> GetByTypeIdAsync(int testTypeId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting test setups by type ID: {TestTypeId}", testTypeId);
            var testSetups = await _repository.GetByTypeIdAsync(testTypeId, cancellationToken);
            return _mapper.Map<IEnumerable<TestSetupDto>>(testSetups);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting test setups by type ID: {TestTypeId}", testTypeId);
            throw;
        }
    }
}