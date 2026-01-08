using AutoMapper;
using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Interfaces.Repositories;
using DiagnosticCenter.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace DiagnosticCenter.Application.Services;

/// <summary>
/// Service implementation for TestSetup operations
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
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<TestSetupDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving all test setups");
            var testSetups = await _repository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<TestSetupDto>>(testSetups);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving test setups");
            throw;
        }
    }

    public async Task<TestSetupDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving test setup with ID: {Id}", id);
            var testSetup = await _repository.GetByIdAsync(id, cancellationToken);
            return testSetup == null ? null : _mapper.Map<TestSetupDto>(testSetup);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving test setup with ID: {Id}", id);
            throw;
        }
    }

    public async Task<TestSetupDto> CreateAsync(TestSetupCreateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating test setup: {Name}", dto.Name);

            if (await _repository.ExistsAsync(dto.Name, cancellationToken))
            {
                throw new InvalidOperationException($"Test setup with name '{dto.Name}' already exists");
            }

            var testSetup = _mapper.Map<TestSetup>(dto);
            testSetup.CreatedDate = DateTime.UtcNow;
            testSetup.IsActive = true;

            var created = await _repository.AddAsync(testSetup, cancellationToken);
            _logger.LogInformation("Test setup created with ID: {Id}", created.Id);

            return _mapper.Map<TestSetupDto>(created);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating test setup: {Name}", dto.Name);
            throw;
        }
    }

    public async Task UpdateAsync(int id, TestSetupUpdateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating test setup with ID: {Id}", id);

            var testSetup = await _repository.GetByIdAsync(id, cancellationToken);
            if (testSetup == null)
            {
                throw new InvalidOperationException($"Test setup with ID {id} not found");
            }

            testSetup.Name = dto.Name;
            testSetup.Fee = dto.Fee;
            testSetup.TestTypeId = dto.TestTypeId;
            testSetup.ModifiedBy = dto.ModifiedBy;
            testSetup.ModifiedDate = DateTime.UtcNow;

            await _repository.UpdateAsync(testSetup, cancellationToken);
            _logger.LogInformation("Test setup updated with ID: {Id}", id);
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

    public async Task<IEnumerable<TestSetupDto>> GetByTestTypeIdAsync(int testTypeId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving test setups for test type ID: {TestTypeId}", testTypeId);
            var testSetups = await _repository.GetByTestTypeIdAsync(testTypeId, cancellationToken);
            return _mapper.Map<IEnumerable<TestSetupDto>>(testSetups);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving test setups for test type ID: {TestTypeId}", testTypeId);
            throw;
        }
    }

    public async Task<IEnumerable<TestSetupDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Searching test setups with term: {SearchTerm}", searchTerm);
            var testSetups = await _repository.SearchAsync(searchTerm, cancellationToken);
            return _mapper.Map<IEnumerable<TestSetupDto>>(testSetups);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching test setups with term: {SearchTerm}", searchTerm);
            throw;
        }
    }
}
