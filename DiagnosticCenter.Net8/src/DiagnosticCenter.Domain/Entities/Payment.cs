namespace DiagnosticCenter.Domain.Entities;

/// <summary>
/// Represents a payment transaction
/// </summary>
public class Payment
{
    /// <summary>
    /// Gets or sets the unique identifier
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the bill number
    /// </summary>
    public string BillNo { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the mobile number
    /// </summary>
    public string MobileNo { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the payment amount
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Gets or sets the payment date
    /// </summary>
    public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the test entry identifier
    /// </summary>
    public int TestEntryId { get; set; }

    /// <summary>
    /// Gets or sets the creation date
    /// </summary>
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the last modified date
    /// </summary>
    public DateTime? ModifiedDate { get; set; }

    /// <summary>
    /// Gets or sets whether this payment is active
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Gets or sets the user who created this record
    /// </summary>
    public string CreatedBy { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user who last modified this record
    /// </summary>
    public string? ModifiedBy { get; set; }

    /// <summary>
    /// Navigation property for test entry
    /// </summary>
    public virtual TestEntry? TestEntry { get; set; }
}
