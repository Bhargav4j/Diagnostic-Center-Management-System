namespace DiagnosticCenter.Domain.Entities;

/// <summary>
/// Represents a payment for a test entry in the diagnostic center system
/// </summary>
public class Payment
{
    public int Id { get; set; }
    public int TestEntryId { get; set; }
    public string BillNo { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public DateTime PaymentDate { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public bool IsActive { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string? ModifiedBy { get; set; }

    public TestEntry? TestEntry { get; set; }

    public decimal GetDueAmount()
    {
        return TotalAmount - PaidAmount;
    }
}
