namespace DiagnosticCenter.Domain.Entities;

/// <summary>
/// Represents a test assigned to a patient
/// </summary>
public class PatientTest
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public int TestSetupId { get; set; }
    public string BillNo { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public bool IsActive { get; set; } = true;
    public string CreatedBy { get; set; } = string.Empty;
    public string? ModifiedBy { get; set; }

    public virtual Patient? Patient { get; set; }
    public virtual TestSetup? TestSetup { get; set; }
    public virtual Payment? Payment { get; set; }
}
