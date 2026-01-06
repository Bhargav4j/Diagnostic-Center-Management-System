namespace DiagnosticCenter.Domain.Entities;

/// <summary>
/// Represents a test entry request from a patient
/// </summary>
public class TestEntry
{
    /// <summary>
    /// Gets or sets the unique identifier
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the patient name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date of birth
    /// </summary>
    public DateTime DateOfBirth { get; set; }

    /// <summary>
    /// Gets or sets the mobile number
    /// </summary>
    public string MobileNo { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the bill number
    /// </summary>
    public string BillNo { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the total amount
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Gets or sets the due date
    /// </summary>
    public DateTime DueDate { get; set; }

    /// <summary>
    /// Gets or sets the paid amount
    /// </summary>
    public decimal PaidAmount { get; set; }

    /// <summary>
    /// Gets or sets the test identifier
    /// </summary>
    public int TestId { get; set; }

    /// <summary>
    /// Gets or sets the creation date
    /// </summary>
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the last modified date
    /// </summary>
    public DateTime? ModifiedDate { get; set; }

    /// <summary>
    /// Gets or sets whether this entry is active
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
    /// Navigation property for test setup
    /// </summary>
    public virtual TestSetup? TestSetup { get; set; }

    /// <summary>
    /// Navigation property for payments
    /// </summary>
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    /// <summary>
    /// Calculates the due amount
    /// </summary>
    public decimal DueAmount => TotalAmount - PaidAmount;
}
