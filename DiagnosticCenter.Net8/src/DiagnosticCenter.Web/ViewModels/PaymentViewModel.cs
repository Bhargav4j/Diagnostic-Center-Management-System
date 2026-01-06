using System.ComponentModel.DataAnnotations;

namespace DiagnosticCenter.Web.ViewModels;

/// <summary>
/// View model for Payment entity
/// </summary>
public class PaymentViewModel
{
    /// <summary>
    /// Gets or sets the unique identifier
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the bill number
    /// </summary>
    [Required(ErrorMessage = "Bill number is required")]
    [Display(Name = "Bill Number")]
    [StringLength(50, ErrorMessage = "Bill number cannot be longer than 50 characters")]
    public string BillNo { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the patient name
    /// </summary>
    [Display(Name = "Patient Name")]
    [StringLength(100, ErrorMessage = "Patient name cannot be longer than 100 characters")]
    public string? PatientName { get; set; }

    /// <summary>
    /// Gets or sets the payment amount
    /// </summary>
    [Required(ErrorMessage = "Amount is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero")]
    [Display(Name = "Payment Amount")]
    public decimal Amount { get; set; }

    /// <summary>
    /// Gets or sets the payment date
    /// </summary>
    [Required(ErrorMessage = "Payment date is required")]
    [Display(Name = "Payment Date")]
    [DataType(DataType.Date)]
    public DateTime PaymentDate { get; set; }

    /// <summary>
    /// Gets or sets the test entry identifier
    /// </summary>
    [Required(ErrorMessage = "Test entry is required")]
    public int TestEntryId { get; set; }

    /// <summary>
    /// Gets or sets the creation date
    /// </summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// Gets or sets the last modified date
    /// </summary>
    public DateTime? ModifiedDate { get; set; }
}