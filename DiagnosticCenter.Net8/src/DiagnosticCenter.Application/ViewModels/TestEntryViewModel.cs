using System.ComponentModel.DataAnnotations;

namespace DiagnosticCenter.Application.ViewModels;

/// <summary>
/// View model for TestEntry entity
/// </summary>
public class TestEntryViewModel
{
    /// <summary>
    /// Gets or sets the unique identifier
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the patient name
    /// </summary>
    [Required(ErrorMessage = "Patient name is required")]
    [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date of birth
    /// </summary>
    [Required(ErrorMessage = "Date of birth is required")]
    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; }

    /// <summary>
    /// Gets or sets the mobile number
    /// </summary>
    [Required(ErrorMessage = "Mobile number is required")]
    [StringLength(15, ErrorMessage = "Mobile number cannot be longer than 15 characters")]
    [Phone(ErrorMessage = "Invalid phone number format")]
    public string MobileNo { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the bill number
    /// </summary>
    [Required(ErrorMessage = "Bill number is required")]
    [StringLength(50, ErrorMessage = "Bill number cannot be longer than 50 characters")]
    public string BillNo { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the test name (for display purposes)
    /// </summary>
    public string? TestName { get; set; }

    /// <summary>
    /// Gets or sets the total amount
    /// </summary>
    [Required(ErrorMessage = "Total amount is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Total amount must be greater than zero")]
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Gets or sets the paid amount
    /// </summary>
    [Range(0, double.MaxValue, ErrorMessage = "Paid amount cannot be negative")]
    public decimal PaidAmount { get; set; }

    /// <summary>
    /// Gets or sets the due amount (calculated)
    /// </summary>
    public decimal DueAmount { get; set; }

    /// <summary>
    /// Gets or sets the due date
    /// </summary>
    [Required(ErrorMessage = "Due date is required")]
    [DataType(DataType.Date)]
    public DateTime DueDate { get; set; }

    /// <summary>
    /// Gets or sets the test identifier
    /// </summary>
    [Required(ErrorMessage = "Test is required")]
    public int TestId { get; set; }

    /// <summary>
    /// Gets or sets the creation date
    /// </summary>
    public DateTime CreatedDate { get; set; }

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
}
