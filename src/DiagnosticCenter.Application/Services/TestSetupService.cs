using AutoMapper;
using DiagnosticCenter.Application.DTOs;
using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Interfaces.Repositories;
using DiagnosticCenter.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

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
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<TestSetupDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving all test setups");
            var testSetups = await _repository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<TestSetupDto>>(testSetups);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all test setups");
            throw;
        }
    }

    public async Task<TestSetupDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving test setup with ID: {Id}", id);
            var testSetup = await _repository.GetByIdAsync(id, cancellationToken);
            return testSetup != null ? _mapper.Map<TestSetupDto>(testSetup) : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving test setup with ID: {Id}", id);
            throw;
        }
    }

    public async Task<TestSetupDto> CreateAsync(TestSetupCreateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating test setup: {Name}", dto.Name);

            if (await _repository.ExistsByNameAsync(dto.Name, cancellationToken))
            {
                throw new InvalidOperationException($"Test setup with name '{dto.Name}' already exists");
            }

            var testSetup = _mapper.Map<TestSetup>(dto);
            var createdTestSetup = await _repository.AddAsync(testSetup, cancellationToken);

            _logger.LogInformation("Successfully created test setup with ID: {Id}", createdTestSetup.Id);
            return _mapper.Map<TestSetupDto>(createdTestSetup);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating test setup: {Name}", dto.Name);
            throw;
        }
    }

    public async Task UpdateAsync(int id, TestSetupUpdateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating test setup with ID: {Id}", id);

            var existingTestSetup = await _repository.GetByIdAsync(id, cancellationToken);
            if (existingTestSetup == null)
            {
                throw new InvalidOperationException($"Test setup with ID {id} not found");
            }

            _mapper.Map(dto, existingTestSetup);
            await _repository.UpdateAsync(existingTestSetup, cancellationToken);

            _logger.LogInformation("Successfully updated test setup with ID: {Id}", id);
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
            _logger.LogInformation("Successfully deleted test setup with ID: {Id}", id);
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
            return await _repository.ExistsByNameAsync(name, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking existence of test setup with name: {Name}", name);
            throw;
        }
    }
}
