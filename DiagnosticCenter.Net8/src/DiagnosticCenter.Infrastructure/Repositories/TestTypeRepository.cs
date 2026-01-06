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
    public class TestTypeRepository : ITestTypeRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TestTypeRepository> _logger;

        public TestTypeRepository(ApplicationDbContext context, ILogger<TestTypeRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<TestType> AddAsync(TestType entity, CancellationToken cancellationToken = default)
        {
            try
            {
                await _context.TestTypes.AddAsync(entity, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                _logger.LogInformation($"TestType created with ID: {entity.Id}");
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error creating TestType: {ex.Message}");
                throw;
            }
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                var entity = await _context.TestTypes.FindAsync(new object[] { id }, cancellationToken);
                if (entity == null)
                {
                    _logger.LogWarning($"TestType with ID {id} not found");
                    return;
                }

                entity.IsActive = false;
                _context.TestTypes.Update(entity);
                await _context.SaveChangesAsync(cancellationToken);
                _logger.LogInformation($"TestType with ID {id} marked as inactive");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting TestType with ID {id}: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<TestType>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.TestTypes
                    .Where(t => t.IsActive)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving all active TestTypes: {ex.Message}");
                throw;
            }
        }

        public async Task<TestType?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.TestTypes
                    .Where(t => t.Id == id && t.IsActive)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving TestType with ID {id}: {ex.Message}");
                throw;
            }
        }

        public async Task UpdateAsync(TestType entity, CancellationToken cancellationToken = default)
        {
            try
            {
                _context.TestTypes.Update(entity);
                await _context.SaveChangesAsync(cancellationToken);
                _logger.LogInformation($"TestType updated with ID: {entity.Id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating TestType with ID {entity.Id}: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.TestTypes
                    .AnyAsync(t => t.Id == id && t.IsActive, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error checking if TestType with ID {id} exists: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> NameExistsAsync(string name, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.TestTypes
                    .AnyAsync(t => t.Name == name && t.IsActive, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error checking if TestType name '{name}' exists: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<TestType>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.TestTypes
                    .Where(t => t.IsActive && t.Name.Contains(searchTerm))
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error searching TestTypes with term '{searchTerm}': {ex.Message}");
                throw;
            }
        }
    }
}