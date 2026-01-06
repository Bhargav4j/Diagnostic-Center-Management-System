namespace DiagnosticCenter.Domain.Interfaces.Services;

public interface IPaymentService
{
    Task<IEnumerable<PaymentDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<PaymentDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<PaymentDto>> GetByBillNoAsync(string billNo, CancellationToken cancellationToken = default);
    Task<PaymentDto> CreateAsync(PaymentCreateDto createDto, CancellationToken cancellationToken = default);
    Task UpdateAsync(int id, PaymentUpdateDto updateDto, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<PaymentDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}

public class PaymentDto
{
    public int Id { get; set; }
    public string BillNo { get; set; } = string.Empty;
    public string MobileNo { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public DateTime PaymentDate { get; set; }
    public int TestEntryId { get; set; }
    public decimal DueAmount => TotalAmount - PaidAmount;
    public DateTime CreatedDate { get; set; }
    public bool IsActive { get; set; }
}

public class PaymentCreateDto
{
    public string BillNo { get; set; } = string.Empty;
    public string MobileNo { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public DateTime PaymentDate { get; set; }
    public int TestEntryId { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
}

public class PaymentUpdateDto
{
    public decimal TotalAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public DateTime PaymentDate { get; set; }
    public bool IsActive { get; set; }
    public string ModifiedBy { get; set; } = string.Empty;
}
