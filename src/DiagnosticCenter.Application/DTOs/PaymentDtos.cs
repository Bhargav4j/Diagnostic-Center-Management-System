namespace DiagnosticCenter.Application.DTOs;

public class PaymentDto
{
    public int Id { get; set; }
    public int TestEntryId { get; set; }
    public string TestEntryBillNo { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public string PaymentMode { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
}

public class PaymentCreateDto
{
    public int TestEntryId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public string PaymentMode { get; set; } = string.Empty;
    public string? Notes { get; set; }
}

public class PaymentUpdateDto
{
    public int TestEntryId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public string PaymentMode { get; set; } = string.Empty;
    public string? Notes { get; set; }
}
