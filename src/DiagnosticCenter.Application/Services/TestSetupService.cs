using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Interfaces.Repositories;
using DiagnosticCenter.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace DiagnosticCenter.Application.Services;

public class TestSetupService : ITestSetupService
{
    private readonly ITestSetupRepository _repository;
    private readonly ILogger<TestSetupService> _logger;

    public TestSetupService(
        ITestSetupRepository repository,
        ILogger<TestSetupService> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<TestSetup>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving all test setups");
            return await _repository.GetAllAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving test setups");
            throw;
        }
    }

    public async Task<TestSetup?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving test setup with ID: {Id}", id);
            return await _repository.GetByIdAsync(id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving test setup with ID: {Id}", id);
            throw;
        }
    }

    public async Task<IEnumerable<TestSetup>> GetByTestTypeIdAsync(int testTypeId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving test setups for test type ID: {TestTypeId}", testTypeId);
            return await _repository.GetByTestTypeIdAsync(testTypeId, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving test setups for test type ID: {TestTypeId}", testTypeId);
            throw;
        }
    }

    public async Task<TestSetup> CreateAsync(TestSetup testSetup, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating new test setup: {Name}", testSetup.Name);
            testSetup.CreatedDate = DateTime.UtcNow;
            testSetup.IsActive = true;
            return await _repository.AddAsync(testSetup, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating test setup: {Name}", testSetup.Name);
            throw;
        }
    }

    public async Task UpdateAsync(TestSetup testSetup, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating test setup with ID: {Id}", testSetup.Id);
            testSetup.ModifiedDate = DateTime.UtcNow;
            await _repository.UpdateAsync(testSetup, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating test setup with ID: {Id}", testSetup.Id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting test setup with ID: {Id}", id);
            await _repository.DeleteAsync(id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting test setup with ID: {Id}", id);
            throw;
        }
    }

    public async Task<IEnumerable<TestSetup>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Searching test setups with term: {SearchTerm}", searchTerm);
            return await _repository.SearchAsync(searchTerm, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching test setups with term: {SearchTerm}", searchTerm);
            throw;
        }
    }
}
