using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Interfaces.Repositories;
using DiagnosticCenter.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace DiagnosticCenter.Application.Services;

public class PatientTestService : IPatientTestService
{
    private readonly IPatientTestRepository _repository;
    private readonly IPatientRepository _patientRepository;
    private readonly ILogger<PatientTestService> _logger;

    public PatientTestService(
        IPatientTestRepository repository,
        IPatientRepository patientRepository,
        ILogger<PatientTestService> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _patientRepository = patientRepository ?? throw new ArgumentNullException(nameof(patientRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<PatientTest>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving all patient tests");
            return await _repository.GetAllAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving patient tests");
            throw;
        }
    }

    public async Task<PatientTest?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving patient test with ID: {Id}", id);
            return await _repository.GetByIdAsync(id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving patient test with ID: {Id}", id);
            throw;
        }
    }

    public async Task<PatientTest?> GetByBillNoAsync(string billNo, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving patient test with bill number: {BillNo}", billNo);
            return await _repository.GetByBillNoAsync(billNo, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving patient test with bill number: {BillNo}", billNo);
            throw;
        }
    }

    public async Task<IEnumerable<PatientTest>> GetByPatientIdAsync(int patientId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving patient tests for patient ID: {PatientId}", patientId);
            return await _repository.GetByPatientIdAsync(patientId, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving patient tests for patient ID: {PatientId}", patientId);
            throw;
        }
    }

    public async Task<IEnumerable<PatientTest>> GetUnpaidTestsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving unpaid patient tests");
            return await _repository.GetUnpaidTestsAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving unpaid patient tests");
            throw;
        }
    }

    public async Task<PatientTest> CreateAsync(PatientTest patientTest, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating new patient test for patient ID: {PatientId}", patientTest.PatientId);
            patientTest.CreatedDate = DateTime.UtcNow;
            patientTest.IsActive = true;
            patientTest.BillNo = GenerateBillNo();
            return await _repository.AddAsync(patientTest, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating patient test for patient ID: {PatientId}", patientTest.PatientId);
            throw;
        }
    }

    public async Task UpdateAsync(PatientTest patientTest, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating patient test with ID: {Id}", patientTest.Id);
            patientTest.ModifiedDate = DateTime.UtcNow;
            await _repository.UpdateAsync(patientTest, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating patient test with ID: {Id}", patientTest.Id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting patient test with ID: {Id}", id);
            await _repository.DeleteAsync(id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting patient test with ID: {Id}", id);
            throw;
        }
    }

    private string GenerateBillNo()
    {
        return $"BILL{DateTime.UtcNow:yyyyMMddHHmmss}{new Random().Next(1000, 9999)}";
    }
}
