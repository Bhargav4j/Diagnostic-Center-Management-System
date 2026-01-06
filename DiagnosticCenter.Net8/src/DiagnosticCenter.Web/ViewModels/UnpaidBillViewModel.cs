using System.ComponentModel.DataAnnotations;

namespace DiagnosticCenter.Web.ViewModels;

/// <summary>
/// View model for unpaid bills (read-only display)
/// </summary>
public class UnpaidBillViewModel
{
    /// <summary>
    /// Gets or sets the unique identifier
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the bill number
    /// </summary>
    [Display(Name = "Bill Number")]
    public string BillNo { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the patient name
    /// </summary>
    [Display(Name = "Patient Name")]
    public string PatientName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the mobile number
    /// </summary>
    [Display(Name = "Mobile Number")]
    public string MobileNo { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the total amount
    /// </summary>
    [Display(Name = "Total Amount")]
    [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Gets or sets the paid amount
    /// </summary>
    [Display(Name = "Paid Amount")]
    [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
    public decimal PaidAmount { get; set; }

    /// <summary>
    /// Gets or sets the due amount
    /// </summary>
    [Display(Name = "Due Amount")]
    [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
    public decimal DueAmount { get; set; }

    /// <summary>
    /// Gets or sets the due date
    /// </summary>
    [Display(Name = "Due Date")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = false)]
    public DateTime DueDate { get; set; }

    /// <summary>
    /// Gets or sets the test name
    /// </summary>
    [Display(Name = "Test Name")]
    public string TestName { get; set; } = string.Empty;
}
