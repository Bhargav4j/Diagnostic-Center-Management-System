namespace DiagnosticCenter.Application.DTOs;

public class TestEntryDto
{
    public int Id { get; set; }
    public string BillNo { get; set; } = string.Empty;
    public string PatientName { get; set; } = string.Empty;
    public int PatientAge { get; set; }
    public string PatientGender { get; set; } = string.Empty;
    public string? ContactNumber { get; set; }
    public int TestSetupId { get; set; }
    public string TestSetupName { get; set; } = string.Empty;
    public DateTime TestDate { get; set; }
    public decimal TotalFee { get; set; }
    public bool IsPaid { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
}

public class TestEntryCreateDto
{
    public string BillNo { get; set; } = string.Empty;
    public string PatientName { get; set; } = string.Empty;
    public int PatientAge { get; set; }
    public string PatientGender { get; set; } = string.Empty;
    public string? ContactNumber { get; set; }
    public int TestSetupId { get; set; }
    public DateTime TestDate { get; set; }
    public decimal TotalFee { get; set; }
    public bool IsPaid { get; set; }
}

public class TestEntryUpdateDto
{
    public string PatientName { get; set; } = string.Empty;
    public int PatientAge { get; set; }
    public string PatientGender { get; set; } = string.Empty;
    public string? ContactNumber { get; set; }
    public int TestSetupId { get; set; }
    public DateTime TestDate { get; set; }
    public decimal TotalFee { get; set; }
    public bool IsPaid { get; set; }
}
