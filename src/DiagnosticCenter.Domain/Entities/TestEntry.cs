namespace DiagnosticCenter.Domain.Entities;

/// <summary>
/// Represents a patient's test request entry
/// </summary>
public class TestEntry
{
    public int Id { get; set; }
    public string PatientName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string MobileNo { get; set; } = string.Empty;
    public string BillNo { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public DateTime DueDate { get; set; }
    public decimal PaidAmount { get; set; }
    public int TestId { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public bool IsActive { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string? ModifiedBy { get; set; }

    // Navigation properties
    public TestSetup? Test { get; set; }
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();

    // Calculated properties
    public decimal DueAmount => TotalAmount - PaidAmount;
}
