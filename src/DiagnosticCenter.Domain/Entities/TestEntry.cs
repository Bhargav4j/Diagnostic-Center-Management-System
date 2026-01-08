namespace DiagnosticCenter.Domain.Entities;

/// <summary>
/// Represents a test entry/request for a patient
/// </summary>
public class TestEntry
{
    public int Id { get; set; }
    public string PatientName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string MobileNumber { get; set; } = string.Empty;
    public string BillNumber { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public DateTime DueDate { get; set; }
    public decimal PaidAmount { get; set; }
    public int TestSetupId { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? ModifiedDate { get; set; }
    public bool IsActive { get; set; } = true;
    public string CreatedBy { get; set; } = string.Empty;
    public string? ModifiedBy { get; set; }

    // Navigation properties
    public TestSetup TestSetup { get; set; } = null!;
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();

    // Calculated property
    public decimal DueAmount => TotalAmount - PaidAmount;
}
