using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Interfaces.Repositories;
using DiagnosticCenter.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace DiagnosticCenter.Application.Services;

/// <summary>
/// Service implementation for TestSetup business logic
/// </summary>
public class TestSetupService : ITestSetupService
{
    private readonly ITestSetupRepository _repository;
    private readonly ITestTypeRepository _testTypeRepository;
    private readonly ILogger<TestSetupService> _logger;

    public TestSetupService(
        ITestSetupRepository repository,
        ITestTypeRepository testTypeRepository,
        ILogger<TestSetupService> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _testTypeRepository = testTypeRepository ?? throw new ArgumentNullException(nameof(testTypeRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<TestSetup>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving all test setups");
            var result = await _repository.GetAllAsync(cancellationToken);
            _logger.LogInformation("Retrieved {Count} test setups", result.Count());
            return result;
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
            var result = await _repository.GetByIdAsync(id, cancellationToken);

            if (result == null)
            {
                _logger.LogWarning("Test setup with ID {Id} not found", id);
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving test setup with ID {Id}", id);
            throw;
        }
    }

    public async Task<TestSetup> CreateAsync(TestSetup testSetup, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(testSetup.TestName))
            {
                throw new ArgumentException("Test name cannot be empty", nameof(testSetup));
            }

            var testTypeExists = await _testTypeRepository.ExistsAsync(testSetup.TestTypeId, cancellationToken);
            if (!testTypeExists)
            {
                throw new InvalidOperationException($"Test type with ID {testSetup.TestTypeId} not found");
            }

            if (testSetup.Fee <= 0)
            {
                throw new ArgumentException("Fee must be greater than zero", nameof(testSetup));
            }

            _logger.LogInformation("Creating new test setup: {TestName}", testSetup.TestName);
            testSetup.CreatedDate = DateTime.UtcNow;
            testSetup.IsActive = true;

            var result = await _repository.AddAsync(testSetup, cancellationToken);
            _logger.LogInformation("Created test setup with ID {Id}", result.Id);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating test setup: {TestName}", testSetup.TestName);
            throw;
        }
    }

    public async Task UpdateAsync(int id, TestSetup testSetup, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(testSetup.TestName))
            {
                throw new ArgumentException("Test name cannot be empty", nameof(testSetup));
            }

            var existing = await _repository.GetByIdAsync(id, cancellationToken);
            if (existing == null)
            {
                throw new InvalidOperationException($"Test setup with ID {id} not found");
            }

            var testTypeExists = await _testTypeRepository.ExistsAsync(testSetup.TestTypeId, cancellationToken);
            if (!testTypeExists)
            {
                throw new InvalidOperationException($"Test type with ID {testSetup.TestTypeId} not found");
            }

            if (testSetup.Fee <= 0)
            {
                throw new ArgumentException("Fee must be greater than zero", nameof(testSetup));
            }

            _logger.LogInformation("Updating test setup with ID {Id}", id);
            testSetup.Id = id;
            testSetup.ModifiedDate = DateTime.UtcNow;
            testSetup.CreatedDate = existing.CreatedDate;
            testSetup.CreatedBy = existing.CreatedBy;

            await _repository.UpdateAsync(testSetup, cancellationToken);
            _logger.LogInformation("Updated test setup with ID {Id}", id);
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
            var exists = await _repository.ExistsAsync(id, cancellationToken);
            if (!exists)
            {
                throw new InvalidOperationException($"Test setup with ID {id} not found");
            }

            _logger.LogInformation("Deleting test setup with ID {Id}", id);
            await _repository.DeleteAsync(id, cancellationToken);
            _logger.LogInformation("Deleted test setup with ID {Id}", id);
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
            var result = await _repository.GetByTestTypeIdAsync(testTypeId, cancellationToken);
            _logger.LogInformation("Found {Count} test setups for test type ID {TestTypeId}", result.Count(), testTypeId);
            return result;
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
            var result = await _repository.SearchAsync(searchTerm, cancellationToken);
            _logger.LogInformation("Found {Count} test setups matching search term", result.Count());
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching test setups with term: {SearchTerm}", searchTerm);
            throw;
        }
    }
}
