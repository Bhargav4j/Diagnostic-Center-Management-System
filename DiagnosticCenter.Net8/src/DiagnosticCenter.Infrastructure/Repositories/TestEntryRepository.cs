using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Interfaces.Repositories;
using DiagnosticCenter.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DiagnosticCenter.Infrastructure.Repositories
{
    public class TestEntryRepository : ITestEntryRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TestEntryRepository> _logger;

        public TestEntryRepository(ApplicationDbContext context, ILogger<TestEntryRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<TestEntry> AddAsync(TestEntry entity, CancellationToken cancellationToken = default)
        {
            try
            {
                await _context.TestEntries.AddAsync(entity, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                _logger.LogInformation($"TestEntry created with ID: {entity.Id}");
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error creating TestEntry: {ex.Message}");
                throw;
            }
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                var entity = await _context.TestEntries.FindAsync(new object[] { id }, cancellationToken);
                if (entity == null)
                {
                    _logger.LogWarning($"TestEntry with ID {id} not found");
                    return;
                }

                entity.IsActive = false;
                _context.TestEntries.Update(entity);
                await _context.SaveChangesAsync(cancellationToken);
                _logger.LogInformation($"TestEntry with ID {id} marked as inactive");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting TestEntry with ID {id}: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<TestEntry>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.TestEntries
                    .Where(t => t.IsActive)
                    .Include(t => t.TestSetup)
                    .Include(t => t.TestSetup.TestType)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving all active TestEntries: {ex.Message}");
                throw;
            }
        }

        public async Task<TestEntry?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.TestEntries
                    .Where(t => t.Id == id && t.IsActive)
                    .Include(t => t.TestSetup)
                    .Include(t => t.TestSetup.TestType)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving TestEntry with ID {id}: {ex.Message}");
                throw;
            }
        }

        public async Task UpdateAsync(TestEntry entity, CancellationToken cancellationToken = default)
        {
            try
            {
                _context.TestEntries.Update(entity);
                await _context.SaveChangesAsync(cancellationToken);
                _logger.LogInformation($"TestEntry updated with ID: {entity.Id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating TestEntry with ID {entity.Id}: {ex.Message}");
                throw;
            }
        }

        public async Task<TestEntry?> GetByBillNoAsync(string billNo, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.TestEntries
                    .Where(t => t.BillNo == billNo && t.IsActive)
                    .Include(t => t.TestSetup)
                    .Include(t => t.TestSetup.TestType)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving TestEntry with BillNo '{billNo}': {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<TestEntry>> GetUnpaidAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.TestEntries
                    .Where(t => t.IsActive && t.PaidAmount < t.TotalAmount)
                    .Include(t => t.TestSetup)
                    .Include(t => t.TestSetup.TestType)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving unpaid TestEntries: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.TestEntries
                    .AnyAsync(t => t.Id == id && t.IsActive, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error checking if TestEntry with ID {id} exists: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<TestEntry>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.TestEntries
                    .Where(t => t.IsActive && (t.Name.Contains(searchTerm) || t.BillNo.Contains(searchTerm)))
                    .Include(t => t.TestSetup)
                    .Include(t => t.TestSetup.TestType)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error searching TestEntries with term '{searchTerm}': {ex.Message}");
                throw;
            }
        }
    }
}