using AutoMapper;
using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Interfaces.Repositories;
using DiagnosticCenter.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace DiagnosticCenter.Application.Services;

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
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<TestEntryDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting all test entries from service");
            var testEntries = await _repository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<TestEntryDto>>(testEntries);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all test entries in service");
            throw;
        }
    }

    public async Task<TestEntryDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting test entry by ID from service: {Id}", id);
            var testEntry = await _repository.GetByIdAsync(id, cancellationToken);
            return testEntry == null ? null : _mapper.Map<TestEntryDto>(testEntry);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting test entry by ID in service: {Id}", id);
            throw;
        }
    }

    public async Task<TestEntryDto> CreateAsync(TestEntryCreateDto createDto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating test entry: {Name}", createDto.Name);

            var testEntry = _mapper.Map<TestEntry>(createDto);
            testEntry.CreatedDate = DateTime.UtcNow;
            testEntry.IsActive = true;

            var created = await _repository.AddAsync(testEntry, cancellationToken);
            return _mapper.Map<TestEntryDto>(created);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating test entry: {Name}", createDto.Name);
            throw;
        }
    }

    public async Task UpdateAsync(int id, TestEntryUpdateDto updateDto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating test entry: {Id}", id);

            var testEntry = await _repository.GetByIdAsync(id, cancellationToken);
            if (testEntry == null)
            {
                throw new InvalidOperationException($"Test entry with ID {id} not found");
            }

            testEntry.Name = updateDto.Name;
            testEntry.DOB = updateDto.DOB;
            testEntry.MobileNo = updateDto.MobileNo;
            testEntry.TotalAmount = updateDto.TotalAmount;
            testEntry.DueDate = updateDto.DueDate;
            testEntry.PaidAmount = updateDto.PaidAmount;
            testEntry.IsActive = updateDto.IsActive;
            testEntry.ModifiedDate = DateTime.UtcNow;
            testEntry.ModifiedBy = updateDto.ModifiedBy;

            await _repository.UpdateAsync(testEntry, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating test entry: {Id}", id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting test entry: {Id}", id);

            var exists = await _repository.ExistsAsync(id, cancellationToken);
            if (!exists)
            {
                throw new InvalidOperationException($"Test entry with ID {id} not found");
            }

            await _repository.DeleteAsync(id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting test entry: {Id}", id);
            throw;
        }
    }

    public async Task<TestEntryDto?> GetByBillNoAsync(string billNo, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting test entry by Bill No: {BillNo}", billNo);
            var testEntry = await _repository.GetByBillNoAsync(billNo, cancellationToken);
            return testEntry == null ? null : _mapper.Map<TestEntryDto>(testEntry);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting test entry by Bill No: {BillNo}", billNo);
            throw;
        }
    }

    public async Task<IEnumerable<TestEntryDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Searching test entries: {SearchTerm}", searchTerm);
            var testEntries = await _repository.SearchAsync(searchTerm, cancellationToken);
            return _mapper.Map<IEnumerable<TestEntryDto>>(testEntries);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching test entries: {SearchTerm}", searchTerm);
            throw;
        }
    }

    public async Task<IEnumerable<TestEntryDto>> GetUnpaidEntriesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting unpaid test entries");
            var unpaidEntries = await _repository.GetUnpaidEntriesAsync(cancellationToken);
            return _mapper.Map<IEnumerable<TestEntryDto>>(unpaidEntries);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting unpaid test entries");
            throw;
        }
    }
}