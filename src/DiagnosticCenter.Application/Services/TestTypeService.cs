using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Interfaces.Repositories;
using DiagnosticCenter.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace DiagnosticCenter.Application.Services;

/// <summary>
/// Service implementation for TestType operations
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
            _logger.LogInformation("Getting all test types");
            var testTypes = await _repository.GetAllAsync(cancellationToken);
            _logger.LogInformation("Retrieved {Count} test types", testTypes.Count());
            return testTypes;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all test types");
            throw;
        }
    }

    public async Task<TestType?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting test type with id {Id}", id);
            var testType = await _repository.GetByIdAsync(id, cancellationToken);
            if (testType == null)
            {
                _logger.LogWarning("Test type with id {Id} not found", id);
            }
            return testType;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting test type with id {Id}", id);
            throw;
        }
    }

    public async Task<TestType> CreateAsync(TestType entity, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating test type: {Name}", entity.Name);

            if (await _repository.ExistsByNameAsync(entity.Name, cancellationToken))
            {
                throw new InvalidOperationException($"Test type with name '{entity.Name}' already exists");
            }

            entity.CreatedDate = DateTime.UtcNow;
            entity.IsActive = true;

            var created = await _repository.AddAsync(entity, cancellationToken);
            _logger.LogInformation("Test type created successfully with id {Id}", created.Id);
            return created;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating test type: {Name}", entity.Name);
            throw;
        }
    }

    public async Task UpdateAsync(int id, TestType entity, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating test type with id {Id}", id);

            var existing = await _repository.GetByIdAsync(id, cancellationToken);
            if (existing == null)
            {
                throw new InvalidOperationException($"Test type with id {id} not found");
            }

            existing.Name = entity.Name;
            existing.Description = entity.Description;
            existing.ModifiedDate = DateTime.UtcNow;
            existing.ModifiedBy = entity.ModifiedBy;

            await _repository.UpdateAsync(existing, cancellationToken);
            _logger.LogInformation("Test type with id {Id} updated successfully", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating test type with id {Id}", id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting test type with id {Id}", id);
            await _repository.DeleteAsync(id, cancellationToken);
            _logger.LogInformation("Test type with id {Id} deleted successfully", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting test type with id {Id}", id);
            throw;
        }
    }

    public async Task<IEnumerable<TestType>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Searching test types with term: {SearchTerm}", searchTerm);
            var results = await _repository.SearchAsync(searchTerm, cancellationToken);
            _logger.LogInformation("Found {Count} test types matching search term", results.Count());
            return results;
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
            _logger.LogError(ex, "Error checking if test type exists with name: {Name}", name);
            throw;
        }
    }
}
