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
    public class PaymentRepository : IPaymentRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PaymentRepository> _logger;

        public PaymentRepository(ApplicationDbContext context, ILogger<PaymentRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Payment> AddAsync(Payment entity, CancellationToken cancellationToken = default)
        {
            try
            {
                await _context.Payments.AddAsync(entity, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                _logger.LogInformation($"Payment created with ID: {entity.Id}");
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error creating Payment: {ex.Message}");
                throw;
            }
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                var entity = await _context.Payments.FindAsync(new object[] { id }, cancellationToken);
                if (entity == null)
                {
                    _logger.LogWarning($"Payment with ID {id} not found");
                    return;
                }

                entity.IsActive = false;
                _context.Payments.Update(entity);
                await _context.SaveChangesAsync(cancellationToken);
                _logger.LogInformation($"Payment with ID {id} marked as inactive");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting Payment with ID {id}: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<Payment>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.Payments
                    .Where(p => p.IsActive)
                    .Include(p => p.TestEntry)
                    .Include(p => p.TestEntry.TestSetup)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving all active Payments: {ex.Message}");
                throw;
            }
        }

        public async Task<Payment?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.Payments
                    .Where(p => p.Id == id && p.IsActive)
                    .Include(p => p.TestEntry)
                    .Include(p => p.TestEntry.TestSetup)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving Payment with ID {id}: {ex.Message}");
                throw;
            }
        }

        public async Task UpdateAsync(Payment entity, CancellationToken cancellationToken = default)
        {
            try
            {
                _context.Payments.Update(entity);
                await _context.SaveChangesAsync(cancellationToken);
                _logger.LogInformation($"Payment updated with ID: {entity.Id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating Payment with ID {entity.Id}: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<Payment>> GetByBillNoAsync(string billNo, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.Payments
                    .Where(p => p.IsActive && p.TestEntry.BillNo == billNo)
                    .Include(p => p.TestEntry)
                    .Include(p => p.TestEntry.TestSetup)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving Payments with BillNo '{billNo}': {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<Payment>> GetByTestEntryIdAsync(int testEntryId, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.Payments
                    .Where(p => p.TestEntryId == testEntryId && p.IsActive)
                    .Include(p => p.TestEntry)
                    .Include(p => p.TestEntry.TestSetup)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving Payments for TestEntryId {testEntryId}: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.Payments
                    .AnyAsync(p => p.Id == id && p.IsActive, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error checking if Payment with ID {id} exists: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<Payment>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.Payments
                    .Where(p => p.IsActive && (p.TestEntry.BillNo.Contains(searchTerm) || p.TestEntry.Name.Contains(searchTerm)))
                    .Include(p => p.TestEntry)
                    .Include(p => p.TestEntry.TestSetup)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error searching Payments with term '{searchTerm}': {ex.Message}");
                throw;
            }
        }
    }
}