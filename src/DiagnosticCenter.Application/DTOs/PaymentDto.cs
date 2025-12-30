namespace DiagnosticCenter.Application.DTOs;

public class PaymentDto
{
    public int Id { get; set; }
    public string BillNo { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public int TestEntryId { get; set; }
    public bool IsActive { get; set; }
}

public class PaymentCreateDto
{
    public string BillNo { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public int TestEntryId { get; set; }
}

public class PaymentUpdateDto
{
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public string? Notes { get; set; }
}
