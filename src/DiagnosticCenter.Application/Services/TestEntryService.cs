using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Exceptions;
using DiagnosticCenter.Domain.Interfaces.Repositories;
using DiagnosticCenter.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace DiagnosticCenter.Application.Services;

public class TestEntryService : ITestEntryService
{
    private readonly ITestEntryRepository _repository;
    private readonly ILogger<TestEntryService> _logger;

    public TestEntryService(ITestEntryRepository repository, ILogger<TestEntryService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<IEnumerable<TestEntry>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving all test entries");
            return await _repository.GetAllAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all test entries");
            throw;
        }
    }

    public async Task<TestEntry?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving test entry with ID {Id}", id);
            return await _repository.GetByIdAsync(id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving test entry with ID {Id}", id);
            throw;
        }
    }

    public async Task<TestEntry?> GetByBillNumberAsync(string billNumber, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving test entry with bill number {BillNumber}", billNumber);
            return await _repository.GetByBillNumberAsync(billNumber, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving test entry with bill number {BillNumber}", billNumber);
            throw;
        }
    }

    public async Task<TestEntry> CreateAsync(TestEntry entity, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating new test entry for patient: {PatientName}", entity.PatientName);

            entity.CreatedDate = DateTime.UtcNow;
            entity.IsActive = true;
            entity.BillNumber = GenerateBillNumber();

            var result = await _repository.AddAsync(entity, cancellationToken);
            _logger.LogInformation("Successfully created test entry with ID {Id}", result.Id);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating test entry for patient: {PatientName}", entity.PatientName);
            throw;
        }
    }

    public async Task UpdateAsync(int id, TestEntry entity, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating test entry with ID {Id}", id);

            var existing = await _repository.GetByIdAsync(id, cancellationToken);
            if (existing == null)
            {
                throw new EntityNotFoundException(nameof(TestEntry), id);
            }

            existing.PatientName = entity.PatientName;
            existing.DateOfBirth = entity.DateOfBirth;
            existing.MobileNumber = entity.MobileNumber;
            existing.TotalAmount = entity.TotalAmount;
            existing.DueDate = entity.DueDate;
            existing.PaidAmount = entity.PaidAmount;
            existing.ModifiedDate = DateTime.UtcNow;
            existing.ModifiedBy = entity.ModifiedBy;

            await _repository.UpdateAsync(existing, cancellationToken);
            _logger.LogInformation("Successfully updated test entry with ID {Id}", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating test entry with ID {Id}", id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting test entry with ID {Id}", id);

            if (!await _repository.ExistsAsync(id, cancellationToken))
            {
                throw new EntityNotFoundException(nameof(TestEntry), id);
            }

            await _repository.DeleteAsync(id, cancellationToken);
            _logger.LogInformation("Successfully deleted test entry with ID {Id}", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting test entry with ID {Id}", id);
            throw;
        }
    }

    public async Task<IEnumerable<TestEntry>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Searching test entries with term: {SearchTerm}", searchTerm);
            return await _repository.SearchAsync(searchTerm, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching test entries with term: {SearchTerm}", searchTerm);
            throw;
        }
    }

    public async Task<IEnumerable<TestEntry>> GetUnpaidTestsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving unpaid test entries");
            return await _repository.GetUnpaidTestsAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving unpaid test entries");
            throw;
        }
    }

    private string GenerateBillNumber()
    {
        return $"BILL-{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper()}";
    }
}
