using System.ComponentModel.DataAnnotations;

namespace DiagnosticCenter.Application.DTOs;

/// <summary>
/// Data transfer object for creating a new test entry.
/// </summary>
public class TestEntryCreateDto
{
    /// <summary>
    /// Gets or sets the patient's name.
    /// </summary>
    [Required(ErrorMessage = "Patient name is required.")]
    [StringLength(200, MinimumLength = 2, ErrorMessage = "Patient name must be between 2 and 200 characters.")]
    public string PatientName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the patient's date of birth.
    /// </summary>
    [Required(ErrorMessage = "Date of birth is required.")]
    public DateTime DateOfBirth { get; set; }

    /// <summary>
    /// Gets or sets the patient's mobile number.
    /// </summary>
    [Required(ErrorMessage = "Mobile number is required.")]
    [StringLength(20, MinimumLength = 10, ErrorMessage = "Mobile number must be between 10 and 20 characters.")]
    [RegularExpression(@"^[0-9+\-() ]+$", ErrorMessage = "Mobile number can only contain digits, +, -, (), and spaces.")]
    public string MobileNo { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the bill number for the test entry.
    /// </summary>
    [Required(ErrorMessage = "Bill number is required.")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "Bill number must be between 1 and 50 characters.")]
    public string BillNo { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the total amount for the test.
    /// </summary>
    [Required(ErrorMessage = "Total amount is required.")]
    [Range(0.01, 999999.99, ErrorMessage = "Total amount must be between 0.01 and 999,999.99.")]
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Gets or sets the due date for payment.
    /// </summary>
    [Required(ErrorMessage = "Due date is required.")]
    public DateTime DueDate { get; set; }

    /// <summary>
    /// Gets or sets the amount already paid.
    /// </summary>
    [Range(0, 999999.99, ErrorMessage = "Paid amount must be between 0 and 999,999.99.")]
    public decimal PaidAmount { get; set; }

    /// <summary>
    /// Gets or sets the foreign key to the test setup.
    /// </summary>
    [Required(ErrorMessage = "Test selection is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Invalid test ID.")]
    public int TestId { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the test entry is active.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Gets or sets the identifier of the user who is creating the test entry.
    /// </summary>
    public int? CreatedBy { get; set; }
}
