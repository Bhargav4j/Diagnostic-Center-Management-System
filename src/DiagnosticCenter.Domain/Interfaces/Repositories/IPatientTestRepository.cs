using DiagnosticCenter.Domain.Entities;

namespace DiagnosticCenter.Domain.Interfaces.Repositories;

public interface IPatientTestRepository
{
    Task<IEnumerable<PatientTest>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<PatientTest?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<PatientTest?> GetByBillNoAsync(string billNo, CancellationToken cancellationToken = default);
    Task<IEnumerable<PatientTest>> GetByPatientIdAsync(int patientId, CancellationToken cancellationToken = default);
    Task<IEnumerable<PatientTest>> GetUnpaidTestsAsync(CancellationToken cancellationToken = default);
    Task<PatientTest> AddAsync(PatientTest patientTest, CancellationToken cancellationToken = default);
    Task UpdateAsync(PatientTest patientTest, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<PatientTest>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}
