namespace DiagnosticCenter.Domain.Entities;

public class Payment : BaseEntity
{
    public int TestEntryId { get; set; }
    public string BillNumber { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public string? PaymentMethod { get; set; }
    public string? TransactionReference { get; set; }
    public string? Notes { get; set; }

    public TestEntry? TestEntry { get; set; }
}
