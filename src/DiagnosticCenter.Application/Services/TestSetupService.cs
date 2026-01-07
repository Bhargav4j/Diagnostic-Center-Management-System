using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Exceptions;
using DiagnosticCenter.Domain.Interfaces.Repositories;
using DiagnosticCenter.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace DiagnosticCenter.Application.Services;

public class TestSetupService : ITestSetupService
{
    private readonly ITestSetupRepository _repository;
    private readonly ILogger<TestSetupService> _logger;

    public TestSetupService(ITestSetupRepository repository, ILogger<TestSetupService> logger)
    {
        _repository = repository;
        _logger = logger;
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
            _logger.LogError(ex, "Error retrieving all test setups");
            throw;
        }
    }

    public async Task<TestSetup?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving test setup with ID {Id}", id);
            return await _repository.GetByIdAsync(id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving test setup with ID {Id}", id);
            throw;
        }
    }

    public async Task<TestSetup> CreateAsync(TestSetup entity, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating new test setup: {Name}", entity.Name);

            entity.CreatedDate = DateTime.UtcNow;
            entity.IsActive = true;

            var result = await _repository.AddAsync(entity, cancellationToken);
            _logger.LogInformation("Successfully created test setup with ID {Id}", result.Id);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating test setup: {Name}", entity.Name);
            throw;
        }
    }

    public async Task UpdateAsync(int id, TestSetup entity, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating test setup with ID {Id}", id);

            var existing = await _repository.GetByIdAsync(id, cancellationToken);
            if (existing == null)
            {
                throw new EntityNotFoundException(nameof(TestSetup), id);
            }

            existing.Name = entity.Name;
            existing.Description = entity.Description;
            existing.Fee = entity.Fee;
            existing.TestTypeId = entity.TestTypeId;
            existing.ModifiedDate = DateTime.UtcNow;
            existing.ModifiedBy = entity.ModifiedBy;

            await _repository.UpdateAsync(existing, cancellationToken);
            _logger.LogInformation("Successfully updated test setup with ID {Id}", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating test setup with ID {Id}", id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting test setup with ID {Id}", id);

            if (!await _repository.ExistsAsync(id, cancellationToken))
            {
                throw new EntityNotFoundException(nameof(TestSetup), id);
            }

            await _repository.DeleteAsync(id, cancellationToken);
            _logger.LogInformation("Successfully deleted test setup with ID {Id}", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting test setup with ID {Id}", id);
            throw;
        }
    }

    public async Task<IEnumerable<TestSetup>> GetByTestTypeIdAsync(int testTypeId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving test setups for test type ID {TestTypeId}", testTypeId);
            return await _repository.GetByTestTypeIdAsync(testTypeId, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving test setups for test type ID {TestTypeId}", testTypeId);
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
