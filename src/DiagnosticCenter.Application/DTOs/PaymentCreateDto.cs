using System.ComponentModel.DataAnnotations;

namespace DiagnosticCenter.Application.DTOs;

/// <summary>
/// Data transfer object for creating a new payment.
/// </summary>
public class PaymentCreateDto
{
    /// <summary>
    /// Gets or sets the bill number associated with this payment.
    /// </summary>
    [Required(ErrorMessage = "Bill number is required.")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "Bill number must be between 1 and 50 characters.")]
    public string BillNo { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the payment amount.
    /// </summary>
    [Required(ErrorMessage = "Payment amount is required.")]
    [Range(0.01, 999999.99, ErrorMessage = "Payment amount must be between 0.01 and 999,999.99.")]
    public decimal Amount { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the payment was made.
    /// </summary>
    [Required(ErrorMessage = "Payment date is required.")]
    public DateTime PaymentDate { get; set; }

    /// <summary>
    /// Gets or sets the foreign key to the test entry.
    /// </summary>
    [Required(ErrorMessage = "Test entry is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Invalid test entry ID.")]
    public int TestEntryId { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the payment record is active.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Gets or sets the identifier of the user who is creating the payment record.
    /// </summary>
    public int? CreatedBy { get; set; }
}
