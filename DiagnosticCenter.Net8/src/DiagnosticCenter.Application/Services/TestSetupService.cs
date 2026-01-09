using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Exceptions;
using DiagnosticCenter.Domain.Interfaces.Repositories;
using DiagnosticCenter.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace DiagnosticCenter.Application.Services;

/// <summary>
/// Service implementation for TestSetup business operations
/// </summary>
public class TestSetupService : ITestSetupService
{
    private readonly ITestSetupRepository _repository;
    private readonly ILogger<TestSetupService> _logger;

    public TestSetupService(
        ITestSetupRepository repository,
        ILogger<TestSetupService> logger)
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
            _logger.LogInformation("Retrieving test setup with ID: {Id}", id);
            return await _repository.GetByIdAsync(id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving test setup with ID: {Id}", id);
            throw;
        }
    }

    public async Task<TestSetup> CreateAsync(TestSetup testSetup, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating new test setup: {Name}", testSetup.Name);

            if (await _repository.ExistsByNameAsync(testSetup.Name, cancellationToken))
            {
                throw new ValidationException(new Dictionary<string, string[]>
                {
                    { nameof(testSetup.Name), new[] { $"Test setup with name '{testSetup.Name}' already exists." } }
                });
            }

            testSetup.CreatedDate = DateTime.UtcNow;
            testSetup.IsActive = true;

            var result = await _repository.AddAsync(testSetup, cancellationToken);
            _logger.LogInformation("Test setup created successfully with ID: {Id}", result.Id);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating test setup: {Name}", testSetup.Name);
            throw;
        }
    }

    public async Task UpdateAsync(int id, TestSetup testSetup, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating test setup with ID: {Id}", id);

            var existing = await _repository.GetByIdAsync(id, cancellationToken);
            if (existing == null)
            {
                throw new NotFoundException(nameof(TestSetup), id);
            }

            existing.Name = testSetup.Name;
            existing.Fee = testSetup.Fee;
            existing.TypeId = testSetup.TypeId;
            existing.ModifiedDate = DateTime.UtcNow;
            existing.ModifiedBy = testSetup.ModifiedBy;

            await _repository.UpdateAsync(existing, cancellationToken);
            _logger.LogInformation("Test setup updated successfully with ID: {Id}", id);
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

            if (!await _repository.ExistsAsync(id, cancellationToken))
            {
                throw new NotFoundException(nameof(TestSetup), id);
            }

            await _repository.DeleteAsync(id, cancellationToken);
            _logger.LogInformation("Test setup deleted successfully with ID: {Id}", id);
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

    public async Task<IEnumerable<TestSetup>> GetByTypeIdAsync(int typeId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving test setups for type ID: {TypeId}", typeId);
            return await _repository.GetByTypeIdAsync(typeId, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving test setups for type ID: {TypeId}", typeId);
            throw;
        }
    }
}
