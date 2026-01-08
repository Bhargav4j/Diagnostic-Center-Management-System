namespace DiagnosticCenter.Application.DTOs;

public class PaymentDto
{
    public int Id { get; set; }
    public string BillNo { get; set; } = string.Empty;
    public string MobileNo { get; set; } = string.Empty;
    public string TestName { get; set; } = string.Empty;
    public decimal TestFee { get; set; }
    public DateTime DueDate { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal DueAmount { get; set; }
    public DateTime CreatedDate { get; set; }
    public bool IsActive { get; set; }
}

public class PaymentCreateDto
{
    public string BillNo { get; set; } = string.Empty;
    public string MobileNo { get; set; } = string.Empty;
    public string TestName { get; set; } = string.Empty;
    public decimal TestFee { get; set; }
    public DateTime DueDate { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
}

public class PaymentUpdateDto
{
    public decimal PaidAmount { get; set; }
    public string ModifiedBy { get; set; } = string.Empty;
}
