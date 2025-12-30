using AutoMapper;
using DiagnosticCenter.Application.DTOs;
using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace DiagnosticCenter.Application.Services;

/// <summary>
/// Service for managing test entry operations
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
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<TestEntryDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving all test entries");
            var entities = await _repository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<TestEntryDto>>(entities);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all test entries");
            throw;
        }
    }

    public async Task<TestEntryDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving test entry with ID: {Id}", id);
            var entity = await _repository.GetByIdAsync(id, cancellationToken);
            return entity == null ? null : _mapper.Map<TestEntryDto>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving test entry with ID: {Id}", id);
            throw;
        }
    }

    public async Task<TestEntryDto?> GetByBillNoAsync(string billNo, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving test entry with Bill No: {BillNo}", billNo);
            var entity = await _repository.GetByBillNoAsync(billNo, cancellationToken);
            return entity == null ? null : _mapper.Map<TestEntryDto>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving test entry with Bill No: {BillNo}", billNo);
            throw;
        }
    }

    public async Task<TestEntryDto> CreateAsync(TestEntryCreateDto dto, string createdBy, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating new test entry for patient: {PatientName}", dto.PatientName);
            var entity = _mapper.Map<TestEntry>(dto);
            entity.BillNo = GenerateBillNo();
            entity.CreatedDate = DateTime.UtcNow;
            entity.IsActive = true;
            entity.CreatedBy = createdBy;

            var created = await _repository.AddAsync(entity, cancellationToken);
            _logger.LogInformation("Test entry created with ID: {Id}", created.Id);
            return _mapper.Map<TestEntryDto>(created);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating test entry for patient: {PatientName}", dto.PatientName);
            throw;
        }
    }

    public async Task<TestEntryDto> UpdateAsync(int id, TestEntryUpdateDto dto, string modifiedBy, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating test entry with ID: {Id}", id);
            var existing = await _repository.GetByIdAsync(id, cancellationToken);
            if (existing == null)
            {
                _logger.LogWarning("Test entry not found with ID: {Id}", id);
                throw new KeyNotFoundException($"Test entry with ID {id} not found");
            }

            existing.PatientName = dto.PatientName;
            existing.DateOfBirth = dto.DateOfBirth;
            existing.MobileNo = dto.MobileNo;
            existing.TotalAmount = dto.TotalAmount;
            existing.DueDate = dto.DueDate;
            existing.PaidAmount = dto.PaidAmount;
            existing.TestId = dto.TestId;
            existing.ModifiedDate = DateTime.UtcNow;
            existing.ModifiedBy = modifiedBy;

            await _repository.UpdateAsync(existing, cancellationToken);
            _logger.LogInformation("Test entry updated with ID: {Id}", id);
            return _mapper.Map<TestEntryDto>(existing);
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

    public async Task<IEnumerable<TestEntryDto>> GetUnpaidEntriesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving unpaid test entries");
            var entities = await _repository.GetUnpaidEntriesAsync(cancellationToken);
            return _mapper.Map<IEnumerable<TestEntryDto>>(entities);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving unpaid test entries");
            throw;
        }
    }

    private string GenerateBillNo()
    {
        return $"BILL-{DateTime.UtcNow:yyyyMMddHHmmss}";
    }
}

public interface ITestEntryService
{
    Task<IEnumerable<TestEntryDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<TestEntryDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<TestEntryDto?> GetByBillNoAsync(string billNo, CancellationToken cancellationToken = default);
    Task<TestEntryDto> CreateAsync(TestEntryCreateDto dto, string createdBy, CancellationToken cancellationToken = default);
    Task<TestEntryDto> UpdateAsync(int id, TestEntryUpdateDto dto, string modifiedBy, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<TestEntryDto>> GetUnpaidEntriesAsync(CancellationToken cancellationToken = default);
}
