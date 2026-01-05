using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Interfaces.Repositories;
using DiagnosticCenter.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace DiagnosticCenter.Application.Services;

/// <summary>
/// Service implementation for TestType business logic
/// </summary>
public class TestTypeService : ITestTypeService
{
    private readonly ITestTypeRepository _repository;
    private readonly ILogger<TestTypeService> _logger;

    public TestTypeService(ITestTypeRepository repository, ILogger<TestTypeService> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<TestType>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving all test types");
            var result = await _repository.GetAllAsync(cancellationToken);
            _logger.LogInformation("Retrieved {Count} test types", result.Count());
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all test types");
            throw;
        }
    }

    public async Task<TestType?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving test type with ID {Id}", id);
            var result = await _repository.GetByIdAsync(id, cancellationToken);

            if (result == null)
            {
                _logger.LogWarning("Test type with ID {Id} not found", id);
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving test type with ID {Id}", id);
            throw;
        }
    }

    public async Task<TestType> CreateAsync(TestType testType, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(testType.Name))
            {
                throw new ArgumentException("Test type name cannot be empty", nameof(testType));
            }

            var exists = await _repository.ExistsByNameAsync(testType.Name, cancellationToken);
            if (exists)
            {
                throw new InvalidOperationException($"Test type with name '{testType.Name}' already exists");
            }

            _logger.LogInformation("Creating new test type: {Name}", testType.Name);
            testType.CreatedDate = DateTime.UtcNow;
            testType.IsActive = true;

            var result = await _repository.AddAsync(testType, cancellationToken);
            _logger.LogInformation("Created test type with ID {Id}", result.Id);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating test type: {Name}", testType.Name);
            throw;
        }
    }

    public async Task UpdateAsync(int id, TestType testType, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(testType.Name))
            {
                throw new ArgumentException("Test type name cannot be empty", nameof(testType));
            }

            var existing = await _repository.GetByIdAsync(id, cancellationToken);
            if (existing == null)
            {
                throw new InvalidOperationException($"Test type with ID {id} not found");
            }

            _logger.LogInformation("Updating test type with ID {Id}", id);
            testType.Id = id;
            testType.ModifiedDate = DateTime.UtcNow;
            testType.CreatedDate = existing.CreatedDate;
            testType.CreatedBy = existing.CreatedBy;

            await _repository.UpdateAsync(testType, cancellationToken);
            _logger.LogInformation("Updated test type with ID {Id}", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating test type with ID {Id}", id);
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
                throw new InvalidOperationException($"Test type with ID {id} not found");
            }

            _logger.LogInformation("Deleting test type with ID {Id}", id);
            await _repository.DeleteAsync(id, cancellationToken);
            _logger.LogInformation("Deleted test type with ID {Id}", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting test type with ID {Id}", id);
            throw;
        }
    }

    public async Task<IEnumerable<TestType>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Searching test types with term: {SearchTerm}", searchTerm);
            var result = await _repository.SearchAsync(searchTerm, cancellationToken);
            _logger.LogInformation("Found {Count} test types matching search term", result.Count());
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching test types with term: {SearchTerm}", searchTerm);
            throw;
        }
    }

    public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _repository.ExistsByNameAsync(name, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if test type exists: {Name}", name);
            throw;
        }
    }
}
