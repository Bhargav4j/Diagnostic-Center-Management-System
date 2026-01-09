namespace DiagnosticCenter.Domain.Entities;

/// <summary>
/// Represents a payment for a patient test
/// </summary>
public class Payment
{
    public int Id { get; set; }
    public int PatientTestId { get; set; }
    public string BillNo { get; set; } = string.Empty;
    public decimal AmountPaid { get; set; }
    public DateTime PaymentDate { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public bool IsActive { get; set; } = true;
    public string CreatedBy { get; set; } = string.Empty;
    public string? ModifiedBy { get; set; }

    public virtual PatientTest? PatientTest { get; set; }
}
