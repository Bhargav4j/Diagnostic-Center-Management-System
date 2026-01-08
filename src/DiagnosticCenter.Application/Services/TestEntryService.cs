using AutoMapper;
using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Interfaces.Repositories;
using DiagnosticCenter.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace DiagnosticCenter.Application.Services;

/// <summary>
/// Service implementation for TestEntry operations
/// </summary>
public class TestEntryService : ITestEntryService
{
    private readonly ITestEntryRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<TestEntryService> _logger;

    public TestEntryService(
        ITestEntryRepository repository,
        IMapper mapper,
        ILogger<TestEntryService> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<TestEntryDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving all test entries");
            var testEntries = await _repository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<TestEntryDto>>(testEntries);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving test entries");
            throw;
        }
    }

    public async Task<TestEntryDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving test entry with ID: {Id}", id);
            var testEntry = await _repository.GetByIdAsync(id, cancellationToken);
            return testEntry == null ? null : _mapper.Map<TestEntryDto>(testEntry);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving test entry with ID: {Id}", id);
            throw;
        }
    }

    public async Task<TestEntryDto> CreateAsync(TestEntryCreateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating test entry for patient: {PatientName}", dto.PatientName);

            var testEntry = _mapper.Map<TestEntry>(dto);
            testEntry.CreatedDate = DateTime.UtcNow;
            testEntry.IsActive = true;

            var created = await _repository.AddAsync(testEntry, cancellationToken);
            _logger.LogInformation("Test entry created with ID: {Id}", created.Id);

            return _mapper.Map<TestEntryDto>(created);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating test entry for patient: {PatientName}", dto.PatientName);
            throw;
        }
    }

    public async Task UpdateAsync(int id, TestEntryUpdateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating test entry with ID: {Id}", id);

            var testEntry = await _repository.GetByIdAsync(id, cancellationToken);
            if (testEntry == null)
            {
                throw new InvalidOperationException($"Test entry with ID {id} not found");
            }

            testEntry.PatientName = dto.PatientName;
            testEntry.DateOfBirth = dto.DateOfBirth;
            testEntry.MobileNumber = dto.MobileNumber;
            testEntry.TotalAmount = dto.TotalAmount;
            testEntry.DueDate = dto.DueDate;
            testEntry.PaidAmount = dto.PaidAmount;
            testEntry.TestSetupId = dto.TestSetupId;
            testEntry.ModifiedBy = dto.ModifiedBy;
            testEntry.ModifiedDate = DateTime.UtcNow;

            await _repository.UpdateAsync(testEntry, cancellationToken);
            _logger.LogInformation("Test entry updated with ID: {Id}", id);
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
            await _repository.DeleteAsync(id, cancellationToken);
            _logger.LogInformation("Test entry deleted with ID: {Id}", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting test entry with ID: {Id}", id);
            throw;
        }
    }

    public async Task<TestEntryDto?> GetByBillNumberAsync(string billNumber, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving test entry with bill number: {BillNumber}", billNumber);
            var testEntry = await _repository.GetByBillNumberAsync(billNumber, cancellationToken);
            return testEntry == null ? null : _mapper.Map<TestEntryDto>(testEntry);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving test entry with bill number: {BillNumber}", billNumber);
            throw;
        }
    }

    public async Task<IEnumerable<TestEntryDto>> GetByMobileNumberAsync(string mobileNumber, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving test entries for mobile number: {MobileNumber}", mobileNumber);
            var testEntries = await _repository.GetByMobileNumberAsync(mobileNumber, cancellationToken);
            return _mapper.Map<IEnumerable<TestEntryDto>>(testEntries);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving test entries for mobile number: {MobileNumber}", mobileNumber);
            throw;
        }
    }

    public async Task<IEnumerable<TestEntryDto>> GetUnpaidAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving unpaid test entries");
            var testEntries = await _repository.GetUnpaidAsync(cancellationToken);
            return _mapper.Map<IEnumerable<TestEntryDto>>(testEntries);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving unpaid test entries");
            throw;
        }
    }
}
