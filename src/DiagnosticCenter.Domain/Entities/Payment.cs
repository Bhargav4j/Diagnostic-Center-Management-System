namespace DiagnosticCenter.Domain.Entities;

/// <summary>
/// Represents a payment transaction for a test entry
/// </summary>
public class Payment
{
    public int Id { get; set; }
    public string BillNo { get; set; } = string.Empty;
    public int TestEntryId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public string? TransactionReference { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public bool IsActive { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string? ModifiedBy { get; set; }

    public TestEntry? TestEntry { get; set; }
}
