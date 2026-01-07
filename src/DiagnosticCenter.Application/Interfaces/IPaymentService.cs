using DiagnosticCenter.Application.DTOs;

namespace DiagnosticCenter.Application.Interfaces;

public interface IPaymentService
{
    Task<IEnumerable<PaymentDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<PaymentDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<PaymentDto> CreateAsync(PaymentCreateDto createDto, CancellationToken cancellationToken = default);
    Task UpdateAsync(int id, PaymentUpdateDto updateDto, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<PaymentDto>> GetByTestEntryIdAsync(int testEntryId, CancellationToken cancellationToken = default);
}
