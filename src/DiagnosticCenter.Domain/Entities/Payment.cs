namespace DiagnosticCenter.Domain.Entities;

/// <summary>
/// Represents a payment transaction for a test entry
/// </summary>
public class Payment
{
    public int Id { get; set; }
    public string BillNo { get; set; } = string.Empty;
    public string MobileNo { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
    public int TestEntryId { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? ModifiedDate { get; set; }
    public bool IsActive { get; set; } = true;
    public string CreatedBy { get; set; } = string.Empty;
    public string? ModifiedBy { get; set; }

    public TestEntry? TestEntry { get; set; }

    public decimal GetDueAmount() => TotalAmount - PaidAmount;
}
