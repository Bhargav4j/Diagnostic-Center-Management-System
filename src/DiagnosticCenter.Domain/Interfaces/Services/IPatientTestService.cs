using DiagnosticCenter.Domain.Entities;

namespace DiagnosticCenter.Domain.Interfaces.Services;

public interface IPatientTestService
{
    Task<IEnumerable<PatientTest>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<PatientTest?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<PatientTest?> GetByBillNoAsync(string billNo, CancellationToken cancellationToken = default);
    Task<IEnumerable<PatientTest>> GetByPatientIdAsync(int patientId, CancellationToken cancellationToken = default);
    Task<IEnumerable<PatientTest>> GetUnpaidTestsAsync(CancellationToken cancellationToken = default);
    Task<PatientTest> CreateAsync(PatientTest patientTest, CancellationToken cancellationToken = default);
    Task UpdateAsync(PatientTest patientTest, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
