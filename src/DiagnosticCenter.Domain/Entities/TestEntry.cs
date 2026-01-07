namespace DiagnosticCenter.Domain.Entities;

/// <summary>
/// Represents a test request entry
/// </summary>
public class TestEntry
{
    public int Id { get; set; }
    public string BillNo { get; set; } = string.Empty;
    public string PatientName { get; set; } = string.Empty;
    public int PatientAge { get; set; }
    public string PatientGender { get; set; } = string.Empty;
    public string? ContactNumber { get; set; }
    public int TestSetupId { get; set; }
    public DateTime TestDate { get; set; }
    public decimal TotalFee { get; set; }
    public bool IsPaid { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? ModifiedDate { get; set; }
    public string CreatedBy { get; set; } = "System";
    public string? ModifiedBy { get; set; }

    public virtual TestSetup TestSetup { get; set; } = null!;
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
