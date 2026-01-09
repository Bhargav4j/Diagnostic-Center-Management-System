using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Exceptions;
using DiagnosticCenter.Domain.Interfaces.Repositories;
using DiagnosticCenter.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace DiagnosticCenter.Application.Services;

/// <summary>
/// Service implementation for TestType business operations
/// </summary>
public class TestTypeService : ITestTypeService
{
    private readonly ITestTypeRepository _repository;
    private readonly ILogger<TestTypeService> _logger;

    public TestTypeService(
        ITestTypeRepository repository,
        ILogger<TestTypeService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<IEnumerable<TestType>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving all test types");
            return await _repository.GetAllAsync(cancellationToken);
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
            _logger.LogInformation("Retrieving test type with ID: {Id}", id);
            return await _repository.GetByIdAsync(id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving test type with ID: {Id}", id);
            throw;
        }
    }

    public async Task<TestType> CreateAsync(TestType testType, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating new test type: {Name}", testType.Name);

            if (await _repository.ExistsByNameAsync(testType.Name, cancellationToken))
            {
                throw new ValidationException(new Dictionary<string, string[]>
                {
                    { nameof(testType.Name), new[] { $"Test type with name '{testType.Name}' already exists." } }
                });
            }

            testType.CreatedDate = DateTime.UtcNow;
            testType.IsActive = true;

            var result = await _repository.AddAsync(testType, cancellationToken);
            _logger.LogInformation("Test type created successfully with ID: {Id}", result.Id);
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
            _logger.LogInformation("Updating test type with ID: {Id}", id);

            var existing = await _repository.GetByIdAsync(id, cancellationToken);
            if (existing == null)
            {
                throw new NotFoundException(nameof(TestType), id);
            }

            existing.Name = testType.Name;
            existing.Description = testType.Description;
            existing.ModifiedDate = DateTime.UtcNow;
            existing.ModifiedBy = testType.ModifiedBy;

            await _repository.UpdateAsync(existing, cancellationToken);
            _logger.LogInformation("Test type updated successfully with ID: {Id}", id);
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

            if (!await _repository.ExistsAsync(id, cancellationToken))
            {
                throw new NotFoundException(nameof(TestType), id);
            }

            await _repository.DeleteAsync(id, cancellationToken);
            _logger.LogInformation("Test type deleted successfully with ID: {Id}", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting test type with ID: {Id}", id);
            throw;
        }
    }

    public async Task<IEnumerable<TestType>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Searching test types with term: {SearchTerm}", searchTerm);
            return await _repository.SearchAsync(searchTerm, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching test types with term: {SearchTerm}", searchTerm);
            throw;
        }
    }
}
