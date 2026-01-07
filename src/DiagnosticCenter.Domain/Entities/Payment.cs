namespace DiagnosticCenter.Domain.Entities;

/// <summary>
/// Represents a payment entity
/// </summary>
public class Payment
{
    public int Id { get; set; }
    public int TestEntryId { get; set; }
    public string BillNo { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public string? PaymentMethod { get; set; }
    public string? TransactionId { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public bool IsActive { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string? ModifiedBy { get; set; }

    // Navigation properties
    public TestEntry TestEntry { get; set; } = null!;
}
