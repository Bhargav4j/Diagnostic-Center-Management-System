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
    public class TestSetupRepository : ITestSetupRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TestSetupRepository> _logger;

        public TestSetupRepository(ApplicationDbContext context, ILogger<TestSetupRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<TestSetup> AddAsync(TestSetup entity, CancellationToken cancellationToken = default)
        {
            try
            {
                await _context.TestSetups.AddAsync(entity, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                _logger.LogInformation($"TestSetup created with ID: {entity.Id}");
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error creating TestSetup: {ex.Message}");
                throw;
            }
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                var entity = await _context.TestSetups.FindAsync(new object[] { id }, cancellationToken);
                if (entity == null)
                {
                    _logger.LogWarning($"TestSetup with ID {id} not found");
                    return;
                }

                entity.IsActive = false;
                _context.TestSetups.Update(entity);
                await _context.SaveChangesAsync(cancellationToken);
                _logger.LogInformation($"TestSetup with ID {id} marked as inactive");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting TestSetup with ID {id}: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<TestSetup>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.TestSetups
                    .Where(t => t.IsActive)
                    .Include(t => t.TestType)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving all active TestSetups: {ex.Message}");
                throw;
            }
        }

        public async Task<TestSetup?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.TestSetups
                    .Where(t => t.Id == id && t.IsActive)
                    .Include(t => t.TestType)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving TestSetup with ID {id}: {ex.Message}");
                throw;
            }
        }

        public async Task UpdateAsync(TestSetup entity, CancellationToken cancellationToken = default)
        {
            try
            {
                _context.TestSetups.Update(entity);
                await _context.SaveChangesAsync(cancellationToken);
                _logger.LogInformation($"TestSetup updated with ID: {entity.Id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating TestSetup with ID {entity.Id}: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<TestSetup>> GetByTypeIdAsync(int typeId, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.TestSetups
                    .Where(t => t.TypeId == typeId && t.IsActive)
                    .Include(t => t.TestType)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving TestSetups for TypeId {typeId}: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.TestSetups
                    .AnyAsync(t => t.Id == id && t.IsActive, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error checking if TestSetup with ID {id} exists: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<TestSetup>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.TestSetups
                    .Where(t => t.IsActive && t.Name.Contains(searchTerm))
                    .Include(t => t.TestType)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error searching TestSetups with term '{searchTerm}': {ex.Message}");
                throw;
            }
        }
    }
}