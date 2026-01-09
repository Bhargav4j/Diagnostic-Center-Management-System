using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Interfaces.Repositories;
using DiagnosticCenter.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DiagnosticCenter.Infrastructure.Repositories;

public class PatientTestRepository : IPatientTestRepository
{
    private readonly DiagnosticCenterDbContext _context;
    private readonly ILogger<PatientTestRepository> _logger;

    public PatientTestRepository(
        DiagnosticCenterDbContext context,
        ILogger<PatientTestRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<PatientTest>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.PatientTests
            .Include(pt => pt.Patient)
            .Include(pt => pt.TestSetup)
            .ThenInclude(ts => ts!.TestType)
            .Where(pt => pt.IsActive)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<PatientTest?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.PatientTests
            .Include(pt => pt.Patient)
            .Include(pt => pt.TestSetup)
            .ThenInclude(ts => ts!.TestType)
            .Where(pt => pt.IsActive && pt.Id == id)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<PatientTest?> GetByBillNoAsync(string billNo, CancellationToken cancellationToken = default)
    {
        return await _context.PatientTests
            .Include(pt => pt.Patient)
            .Include(pt => pt.TestSetup)
            .ThenInclude(ts => ts!.TestType)
            .Where(pt => pt.IsActive && pt.BillNo == billNo)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<PatientTest>> GetByPatientIdAsync(int patientId, CancellationToken cancellationToken = default)
    {
        return await _context.PatientTests
            .Include(pt => pt.Patient)
            .Include(pt => pt.TestSetup)
            .ThenInclude(ts => ts!.TestType)
            .Where(pt => pt.IsActive && pt.PatientId == patientId)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<PatientTest>> GetUnpaidTestsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.PatientTests
            .Include(pt => pt.Patient)
            .Include(pt => pt.TestSetup)
            .ThenInclude(ts => ts!.TestType)
            .Where(pt => pt.IsActive && pt.PaidAmount < pt.TotalAmount)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<PatientTest> AddAsync(PatientTest patientTest, CancellationToken cancellationToken = default)
    {
        await _context.PatientTests.AddAsync(patientTest, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return patientTest;
    }

    public async Task UpdateAsync(PatientTest patientTest, CancellationToken cancellationToken = default)
    {
        _context.PatientTests.Update(patientTest);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var patientTest = await _context.PatientTests.FindAsync(new object[] { id }, cancellationToken);
        if (patientTest != null)
        {
            patientTest.IsActive = false;
            patientTest.ModifiedDate = DateTime.UtcNow;
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.PatientTests
            .AnyAsync(pt => pt.IsActive && pt.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<PatientTest>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        return await _context.PatientTests
            .Include(pt => pt.Patient)
            .Include(pt => pt.TestSetup)
            .ThenInclude(ts => ts!.TestType)
            .Where(pt => pt.IsActive && (pt.BillNo.Contains(searchTerm) || pt.Patient!.Name.Contains(searchTerm)))
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}
