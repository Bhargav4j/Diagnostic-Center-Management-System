namespace DiagnosticCenter.Domain.Entities;

/// <summary>
/// Represents a patient's test entry with billing information.
/// </summary>
public class TestEntry
{
    /// <summary>
    /// Gets or sets the unique identifier for the test entry.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the patient's name.
    /// </summary>
    public string PatientName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the patient's date of birth.
    /// </summary>
    public DateTime DateOfBirth { get; set; }

    /// <summary>
    /// Gets or sets the patient's mobile number.
    /// </summary>
    public string MobileNo { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the bill number for the test entry.
    /// </summary>
    public string BillNo { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the total amount for the test.
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Gets or sets the due date for payment.
    /// </summary>
    public DateTime DueDate { get; set; }

    /// <summary>
    /// Gets or sets the amount already paid.
    /// </summary>
    public decimal PaidAmount { get; set; }

    /// <summary>
    /// Gets or sets the foreign key to the test setup.
    /// </summary>
    public int TestId { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the test entry was created.
    /// </summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the test entry was last modified.
    /// </summary>
    public DateTime? ModifiedDate { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the test entry is active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user who created the test entry.
    /// </summary>
    public int? CreatedBy { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user who last modified the test entry.
    /// </summary>
    public int? ModifiedBy { get; set; }

    /// <summary>
    /// Gets or sets the test setup associated with this test entry.
    /// </summary>
    public virtual TestSetup? TestSetup { get; set; }

    /// <summary>
    /// Gets or sets the collection of payments associated with this test entry.
    /// </summary>
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
