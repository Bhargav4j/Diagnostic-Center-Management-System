namespace DiagnosticCenter.Domain.Entities;

/// <summary>
/// Represents a test entry for a patient
/// </summary>
public class TestEntry
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime DOB { get; set; }
    public string MobileNo { get; set; } = string.Empty;
    public string BillNo { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public DateTime DueDate { get; set; }
    public decimal PaidAmount { get; set; }
    public int TestId { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? ModifiedDate { get; set; }
    public bool IsActive { get; set; } = true;
    public string CreatedBy { get; set; } = string.Empty;
    public string? ModifiedBy { get; set; }

    public TestSetup? Test { get; set; }
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
