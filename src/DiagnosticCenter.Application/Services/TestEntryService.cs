using AutoMapper;
using Microsoft.Extensions.Logging;
using DiagnosticCenter.Application.DTOs;
using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Interfaces.Repositories;
using DiagnosticCenter.Application.Interfaces;

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
            _logger.LogInformation("Getting all test entries");
            var testEntries = await _repository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<TestEntryDto>>(testEntries);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all test entries");
            throw;
        }
    }

    public async Task<TestEntryDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting test entry with ID: {Id}", id);
            var testEntry = await _repository.GetByIdAsync(id, cancellationToken);
            return testEntry == null ? null : _mapper.Map<TestEntryDto>(testEntry);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting test entry with ID: {Id}", id);
            throw;
        }
    }

    public async Task<TestEntryDto?> GetByBillNoAsync(string billNo, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting test entry with Bill No: {BillNo}", billNo);
            var testEntry = await _repository.GetByBillNoAsync(billNo, cancellationToken);
            return testEntry == null ? null : _mapper.Map<TestEntryDto>(testEntry);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting test entry with Bill No: {BillNo}", billNo);
            throw;
        }
    }

    public async Task<TestEntryDto> CreateAsync(TestEntryCreateDto createDto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating new test entry for patient: {PatientName}", createDto.PatientName);
            var testEntry = _mapper.Map<TestEntry>(createDto);
            var created = await _repository.AddAsync(testEntry, cancellationToken);
            return _mapper.Map<TestEntryDto>(created);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating test entry for patient: {PatientName}", createDto.PatientName);
            throw;
        }
    }

    public async Task UpdateAsync(int id, TestEntryUpdateDto updateDto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating test entry with ID: {Id}", id);
            var existing = await _repository.GetByIdAsync(id, cancellationToken);
            if (existing == null)
            {
                throw new InvalidOperationException($"Test entry with ID {id} not found");
            }

            _mapper.Map(updateDto, existing);
            existing.ModifiedDate = DateTime.UtcNow;
            await _repository.UpdateAsync(existing, cancellationToken);
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
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting test entry with ID: {Id}", id);
            throw;
        }
    }

    public async Task<IEnumerable<TestEntryDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Searching test entries with term: {SearchTerm}", searchTerm);
            var testEntries = await _repository.SearchAsync(searchTerm, cancellationToken);
            return _mapper.Map<IEnumerable<TestEntryDto>>(testEntries);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching test entries with term: {SearchTerm}", searchTerm);
            throw;
        }
    }

    public async Task<IEnumerable<TestEntryDto>> GetUnpaidEntriesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting unpaid test entries");
            var testEntries = await _repository.GetUnpaidEntriesAsync(cancellationToken);
            return _mapper.Map<IEnumerable<TestEntryDto>>(testEntries);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting unpaid test entries");
            throw;
        }
    }
}
