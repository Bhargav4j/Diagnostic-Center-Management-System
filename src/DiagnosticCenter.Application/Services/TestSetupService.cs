using AutoMapper;
using Microsoft.Extensions.Logging;
using DiagnosticCenter.Application.DTOs;
using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Interfaces.Repositories;
using DiagnosticCenter.Application.Interfaces;

namespace DiagnosticCenter.Application.Services;

public class TestSetupService : ITestSetupService
{
    private readonly ITestSetupRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<TestSetupService> _logger;

    public TestSetupService(
        ITestSetupRepository repository,
        IMapper mapper,
        ILogger<TestSetupService> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<TestSetupDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting all test setups");
            var testSetups = await _repository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<TestSetupDto>>(testSetups);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all test setups");
            throw;
        }
    }

    public async Task<TestSetupDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting test setup with ID: {Id}", id);
            var testSetup = await _repository.GetByIdAsync(id, cancellationToken);
            return testSetup == null ? null : _mapper.Map<TestSetupDto>(testSetup);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting test setup with ID: {Id}", id);
            throw;
        }
    }

    public async Task<TestSetupDto> CreateAsync(TestSetupCreateDto createDto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating new test setup: {Name}", createDto.Name);

            if (await _repository.ExistsByNameAsync(createDto.Name, cancellationToken))
            {
                throw new InvalidOperationException($"Test setup with name '{createDto.Name}' already exists");
            }

            var testSetup = _mapper.Map<TestSetup>(createDto);
            var created = await _repository.AddAsync(testSetup, cancellationToken);
            return _mapper.Map<TestSetupDto>(created);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating test setup: {Name}", createDto.Name);
            throw;
        }
    }

    public async Task UpdateAsync(int id, TestSetupUpdateDto updateDto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating test setup with ID: {Id}", id);
            var existing = await _repository.GetByIdAsync(id, cancellationToken);
            if (existing == null)
            {
                throw new InvalidOperationException($"Test setup with ID {id} not found");
            }

            _mapper.Map(updateDto, existing);
            existing.ModifiedDate = DateTime.UtcNow;
            await _repository.UpdateAsync(existing, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating test setup with ID: {Id}", id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting test setup with ID: {Id}", id);
            await _repository.DeleteAsync(id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting test setup with ID: {Id}", id);
            throw;
        }
    }

    public async Task<IEnumerable<TestSetupDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Searching test setups with term: {SearchTerm}", searchTerm);
            var testSetups = await _repository.SearchAsync(searchTerm, cancellationToken);
            return _mapper.Map<IEnumerable<TestSetupDto>>(testSetups);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching test setups with term: {SearchTerm}", searchTerm);
            throw;
        }
    }

    public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Checking if test setup exists with name: {Name}", name);
            return await _repository.ExistsByNameAsync(name, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if test setup exists with name: {Name}", name);
            throw;
        }
    }
}
