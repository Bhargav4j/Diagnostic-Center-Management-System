using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Interfaces.Repositories;
using DiagnosticCenter.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DiagnosticCenter.Infrastructure.Repositories;

public class PatientRepository : IPatientRepository
{
    private readonly DiagnosticCenterDbContext _context;
    private readonly ILogger<PatientRepository> _logger;

    public PatientRepository(
        DiagnosticCenterDbContext context,
        ILogger<PatientRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<Patient>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Patients
            .Where(p => p.IsActive)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Patient?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Patients
            .Where(p => p.IsActive && p.Id == id)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Patient?> GetByMobileNoAsync(string mobileNo, CancellationToken cancellationToken = default)
    {
        return await _context.Patients
            .Where(p => p.IsActive && p.MobileNo == mobileNo)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Patient> AddAsync(Patient patient, CancellationToken cancellationToken = default)
    {
        await _context.Patients.AddAsync(patient, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return patient;
    }

    public async Task UpdateAsync(Patient patient, CancellationToken cancellationToken = default)
    {
        _context.Patients.Update(patient);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var patient = await _context.Patients.FindAsync(new object[] { id }, cancellationToken);
        if (patient != null)
        {
            patient.IsActive = false;
            patient.ModifiedDate = DateTime.UtcNow;
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Patients
            .AnyAsync(p => p.IsActive && p.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Patient>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        return await _context.Patients
            .Where(p => p.IsActive && (p.Name.Contains(searchTerm) || p.MobileNo.Contains(searchTerm)))
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}
