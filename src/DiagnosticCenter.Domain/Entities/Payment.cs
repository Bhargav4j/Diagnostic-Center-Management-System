namespace DiagnosticCenter.Domain.Entities;

/// <summary>
/// Represents a payment entity in the diagnostic center system
/// </summary>
public class Payment
{
    public int Id { get; set; }

    public string BillNo { get; set; } = string.Empty;

    public string MobileNo { get; set; } = string.Empty;

    public string TestName { get; set; } = string.Empty;

    public decimal TestFee { get; set; }

    public DateTime DueDate { get; set; }

    public decimal TotalAmount { get; set; }

    public decimal PaidAmount { get; set; }

    public decimal DueAmount => TotalAmount - PaidAmount;

    public DateTime CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public bool IsActive { get; set; }

    public string CreatedBy { get; set; } = string.Empty;

    public string? ModifiedBy { get; set; }
}
