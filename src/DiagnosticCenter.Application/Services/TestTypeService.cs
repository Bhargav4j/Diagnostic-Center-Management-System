using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Interfaces.Repositories;
using DiagnosticCenter.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace DiagnosticCenter.Application.Services;

public class TestTypeService : ITestTypeService
{
    private readonly ITestTypeRepository _repository;
    private readonly ILogger<TestTypeService> _logger;

    public TestTypeService(
        ITestTypeRepository repository,
        ILogger<TestTypeService> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
            _logger.LogError(ex, "Error retrieving test types");
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
            testType.CreatedDate = DateTime.UtcNow;
            testType.IsActive = true;
            return await _repository.AddAsync(testType, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating test type: {Name}", testType.Name);
            throw;
        }
    }

    public async Task UpdateAsync(TestType testType, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating test type with ID: {Id}", testType.Id);
            testType.ModifiedDate = DateTime.UtcNow;
            await _repository.UpdateAsync(testType, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating test type with ID: {Id}", testType.Id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting test type with ID: {Id}", id);
            await _repository.DeleteAsync(id, cancellationToken);
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
