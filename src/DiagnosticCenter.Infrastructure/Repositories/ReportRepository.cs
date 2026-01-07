using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Interfaces.Repositories;
using DiagnosticCenter.Infrastructure.Data;

namespace DiagnosticCenter.Infrastructure.Repositories;

public class ReportRepository : IReportRepository
{
    private readonly DiagnosticCenterDbContext _context;
    private readonly ILogger<ReportRepository> _logger;

    public ReportRepository(DiagnosticCenterDbContext context, ILogger<ReportRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<Report>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Reports
                .Include(r => r.TestEntry)
                .Where(r => r.IsActive)
                .AsNoTracking()
                .OrderByDescending(r => r.ReportDate)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all reports");
            throw;
        }
    }

    public async Task<Report?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Reports
                .Include(r => r.TestEntry)
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id && r.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving report with ID: {Id}", id);
            throw;
        }
    }

    public async Task<Report> AddAsync(Report report, CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.Reports.AddAsync(report, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return report;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding report");
            throw;
        }
    }

    public async Task UpdateAsync(Report report, CancellationToken cancellationToken = default)
    {
        try
        {
            _context.Reports.Update(report);
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating report with ID: {Id}", report.Id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var report = await _context.Reports.FindAsync(new object[] { id }, cancellationToken);
            if (report != null)
            {
                report.IsActive = false;
                report.ModifiedDate = DateTime.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting report with ID: {Id}", id);
            throw;
        }
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Reports.AnyAsync(r => r.Id == id && r.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if report exists with ID: {Id}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Report>> GetByTestEntryIdAsync(int testEntryId, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Reports
                .Include(r => r.TestEntry)
                .Where(r => r.TestEntryId == testEntryId && r.IsActive)
                .AsNoTracking()
                .OrderByDescending(r => r.ReportDate)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving reports for test entry: {TestEntryId}", testEntryId);
            throw;
        }
    }
}
