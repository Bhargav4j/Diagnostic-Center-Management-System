namespace DiagnosticCenter.Domain.Entities;

/// <summary>
/// Represents a test entry (patient test request) in the diagnostic center system
/// </summary>
public class TestEntry
{
    public int Id { get; set; }
    public string PatientName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string MobileNo { get; set; } = string.Empty;
    public string BillNo { get; set; } = string.Empty;
    public int TestId { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public bool IsActive { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string? ModifiedBy { get; set; }

    public TestSetup? TestSetup { get; set; }
    public Payment? Payment { get; set; }

    public decimal GetDueAmount()
    {
        return TotalAmount - PaidAmount;
    }
}
