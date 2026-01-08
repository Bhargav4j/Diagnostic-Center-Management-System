using System.ComponentModel.DataAnnotations;

namespace DiagnosticCenter.Domain.DTOs;

/// <summary>
/// Data transfer object representing a patient's test entry.
/// </summary>
public class TestEntryDto
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
    /// Gets or sets the remaining balance due.
    /// </summary>
    public decimal DueAmount => TotalAmount - PaidAmount;

    /// <summary>
    /// Gets or sets the foreign key to the test setup.
    /// </summary>
    public int TestId { get; set; }

    /// <summary>
    /// Gets or sets the name of the test.
    /// </summary>
    public string? TestName { get; set; }

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
}
