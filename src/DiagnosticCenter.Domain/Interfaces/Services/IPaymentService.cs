namespace DiagnosticCenter.Domain.Interfaces.Services;

/// <summary>
/// Service interface for Payment operations
/// </summary>
public interface IPaymentService
{
    Task<IEnumerable<PaymentDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<PaymentDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<PaymentDto> CreateAsync(PaymentCreateDto dto, CancellationToken cancellationToken = default);
    Task<IEnumerable<PaymentDto>> GetByBillNumberAsync(string billNumber, CancellationToken cancellationToken = default);
    Task<IEnumerable<PaymentDto>> GetByTestEntryIdAsync(int testEntryId, CancellationToken cancellationToken = default);
}

public class PaymentDto
{
    public int Id { get; set; }
    public string BillNumber { get; set; } = string.Empty;
    public int TestEntryId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public DateTime CreatedDate { get; set; }
}

public class PaymentCreateDto
{
    public string BillNumber { get; set; } = string.Empty;
    public int TestEntryId { get; set; }
    public decimal Amount { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
}
