namespace DiagnosticCenter.Domain.Entities;

public class TestEntry : BaseEntity
{
    public string PatientName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string MobileNumber { get; set; } = string.Empty;
    public string BillNumber { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public DateTime DueDate { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal DueAmount => TotalAmount - PaidAmount;

    public ICollection<TestEntryItem> TestEntryItems { get; set; } = new List<TestEntryItem>();
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
