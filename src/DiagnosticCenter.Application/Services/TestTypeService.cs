using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Exceptions;
using DiagnosticCenter.Domain.Interfaces.Repositories;
using DiagnosticCenter.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace DiagnosticCenter.Application.Services;

public class TestTypeService : ITestTypeService
{
    private readonly ITestTypeRepository _repository;
    private readonly ILogger<TestTypeService> _logger;

    public TestTypeService(ITestTypeRepository repository, ILogger<TestTypeService> logger)
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
            _logger.LogInformation("Retrieving test type with ID {Id}", id);
            return await _repository.GetByIdAsync(id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving test type with ID {Id}", id);
            throw;
        }
    }

    public async Task<TestType> CreateAsync(TestType entity, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating new test type: {Name}", entity.Name);

            if (await _repository.ExistsByNameAsync(entity.Name, cancellationToken))
            {
                throw new DuplicateEntityException(nameof(TestType), nameof(entity.Name), entity.Name);
            }

            entity.CreatedDate = DateTime.UtcNow;
            entity.IsActive = true;

            var result = await _repository.AddAsync(entity, cancellationToken);
            _logger.LogInformation("Successfully created test type with ID {Id}", result.Id);
            return result;
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
            _logger.LogInformation("Updating test type with ID {Id}", id);

            var existing = await _repository.GetByIdAsync(id, cancellationToken);
            if (existing == null)
            {
                throw new EntityNotFoundException(nameof(TestType), id);
            }

            existing.Name = entity.Name;
            existing.Description = entity.Description;
            existing.ModifiedDate = DateTime.UtcNow;
            existing.ModifiedBy = entity.ModifiedBy;

            await _repository.UpdateAsync(existing, cancellationToken);
            _logger.LogInformation("Successfully updated test type with ID {Id}", id);
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
            _logger.LogInformation("Deleting test type with ID {Id}", id);

            if (!await _repository.ExistsAsync(id, cancellationToken))
            {
                throw new EntityNotFoundException(nameof(TestType), id);
            }

            await _repository.DeleteAsync(id, cancellationToken);
            _logger.LogInformation("Successfully deleted test type with ID {Id}", id);
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
            return await _repository.SearchAsync(searchTerm, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching test types with term: {SearchTerm}", searchTerm);
            throw;
        }
    }
}
