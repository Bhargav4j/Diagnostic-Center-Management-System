using DiagnosticCenter.Application.DTOs;

namespace DiagnosticCenter.Application.Interfaces;

public interface ITestTypeService
{
    Task<IEnumerable<TestTypeDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<TestTypeDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<TestTypeDto> CreateAsync(TestTypeCreateDto createDto, CancellationToken cancellationToken = default);
    Task UpdateAsync(int id, TestTypeUpdateDto updateDto, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<TestTypeDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}
