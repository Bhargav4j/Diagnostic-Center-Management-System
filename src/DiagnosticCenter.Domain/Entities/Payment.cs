namespace DiagnosticCenter.Domain.Entities;

/// <summary>
/// Represents a payment transaction
/// </summary>
public class Payment
{
    public int Id { get; set; }
    public int TestEntryId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
    public string PaymentMode { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? ModifiedDate { get; set; }
    public string CreatedBy { get; set; } = "System";
    public string? ModifiedBy { get; set; }

    public virtual TestEntry TestEntry { get; set; } = null!;
}
