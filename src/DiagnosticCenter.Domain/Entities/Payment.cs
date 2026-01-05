namespace DiagnosticCenter.Domain.Entities;

/// <summary>
/// Represents a payment entity in the diagnostic center system
/// </summary>
public class Payment
{
    public int Id { get; set; }
    public int TestEntryId { get; set; }
    public decimal AmountPaid { get; set; }
    public DateTime PaymentDate { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public string? ReferenceNumber { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public bool IsActive { get; set; } = true;
    public string CreatedBy { get; set; } = string.Empty;
    public string? ModifiedBy { get; set; }

    public virtual TestEntry? TestEntry { get; set; }
}
