namespace DiagnosticCenter.Domain.Entities;

/// <summary>
/// Represents a payment transaction for a test entry.
/// </summary>
public class Payment
{
    /// <summary>
    /// Gets or sets the unique identifier for the payment.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the bill number associated with this payment.
    /// </summary>
    public string BillNo { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the payment amount.
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the payment was made.
    /// </summary>
    public DateTime PaymentDate { get; set; }

    /// <summary>
    /// Gets or sets the foreign key to the test entry.
    /// </summary>
    public int TestEntryId { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the payment record was created.
    /// </summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the payment record was last modified.
    /// </summary>
    public DateTime? ModifiedDate { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the payment record is active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user who created the payment record.
    /// </summary>
    public int? CreatedBy { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user who last modified the payment record.
    /// </summary>
    public int? ModifiedBy { get; set; }

    /// <summary>
    /// Gets or sets the test entry associated with this payment.
    /// </summary>
    public virtual TestEntry? TestEntry { get; set; }
}
