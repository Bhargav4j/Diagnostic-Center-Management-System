namespace DiagnosticCenter.Domain.Entities;

/// <summary>
/// Represents a test entry entity in the diagnostic center system
/// </summary>
public class TestEntry
{
    public int Id { get; set; }
    public string PatientName { get; set; } = string.Empty;
    public int Age { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public int TestSetupId { get; set; }
    public decimal Amount { get; set; }
    public DateTime TestDate { get; set; }
    public string? Status { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public bool IsActive { get; set; } = true;
    public string CreatedBy { get; set; } = string.Empty;
    public string? ModifiedBy { get; set; }

    public virtual TestSetup? TestSetup { get; set; }
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
