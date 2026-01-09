using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Exceptions;
using DiagnosticCenter.Domain.Interfaces.Repositories;
using DiagnosticCenter.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace DiagnosticCenter.Application.Services;

/// <summary>
/// Service implementation for TestEntry business operations
/// </summary>
public class TestEntryService : ITestEntryService
{
    private readonly ITestEntryRepository _repository;
    private readonly ILogger<TestEntryService> _logger;

    public TestEntryService(
        ITestEntryRepository repository,
        ILogger<TestEntryService> logger)
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
            _logger.LogInformation("Retrieving test entry with ID: {Id}", id);
            return await _repository.GetByIdAsync(id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving test entry with ID: {Id}", id);
            throw;
        }
    }

    public async Task<TestEntry?> GetByBillNoAsync(string billNo, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving test entry with Bill No: {BillNo}", billNo);
            return await _repository.GetByBillNoAsync(billNo, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving test entry with Bill No: {BillNo}", billNo);
            throw;
        }
    }

    public async Task<TestEntry> CreateAsync(TestEntry testEntry, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating new test entry for patient: {PatientName}", testEntry.PatientName);

            if (await _repository.ExistsByBillNoAsync(testEntry.BillNo, cancellationToken))
            {
                throw new ValidationException(new Dictionary<string, string[]>
                {
                    { nameof(testEntry.BillNo), new[] { $"Bill number '{testEntry.BillNo}' already exists." } }
                });
            }

            testEntry.CreatedDate = DateTime.UtcNow;
            testEntry.IsActive = true;

            var result = await _repository.AddAsync(testEntry, cancellationToken);
            _logger.LogInformation("Test entry created successfully with ID: {Id}", result.Id);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating test entry for patient: {PatientName}", testEntry.PatientName);
            throw;
        }
    }

    public async Task UpdateAsync(int id, TestEntry testEntry, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating test entry with ID: {Id}", id);

            var existing = await _repository.GetByIdAsync(id, cancellationToken);
            if (existing == null)
            {
                throw new NotFoundException(nameof(TestEntry), id);
            }

            existing.PatientName = testEntry.PatientName;
            existing.DateOfBirth = testEntry.DateOfBirth;
            existing.MobileNo = testEntry.MobileNo;
            existing.TotalAmount = testEntry.TotalAmount;
            existing.DueDate = testEntry.DueDate;
            existing.PaidAmount = testEntry.PaidAmount;
            existing.TestId = testEntry.TestId;
            existing.ModifiedDate = DateTime.UtcNow;
            existing.ModifiedBy = testEntry.ModifiedBy;

            await _repository.UpdateAsync(existing, cancellationToken);
            _logger.LogInformation("Test entry updated successfully with ID: {Id}", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating test entry with ID: {Id}", id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting test entry with ID: {Id}", id);

            if (!await _repository.ExistsAsync(id, cancellationToken))
            {
                throw new NotFoundException(nameof(TestEntry), id);
            }

            await _repository.DeleteAsync(id, cancellationToken);
            _logger.LogInformation("Test entry deleted successfully with ID: {Id}", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting test entry with ID: {Id}", id);
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

    public async Task<IEnumerable<TestEntry>> GetUnpaidEntriesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving unpaid test entries");
            return await _repository.GetUnpaidEntriesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving unpaid test entries");
            throw;
        }
    }
}
