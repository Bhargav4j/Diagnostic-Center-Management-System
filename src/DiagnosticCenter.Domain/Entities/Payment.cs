namespace DiagnosticCenter.Domain.Entities;

/// <summary>
/// Represents a payment made for a test
/// </summary>
public class Payment
{
    public int Id { get; set; }
    public string BillNumber { get; set; } = string.Empty;
    public int TestEntryId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? ModifiedDate { get; set; }
    public bool IsActive { get; set; } = true;
    public string CreatedBy { get; set; } = string.Empty;
    public string? ModifiedBy { get; set; }

    // Navigation properties
    public TestEntry TestEntry { get; set; } = null!;
}
