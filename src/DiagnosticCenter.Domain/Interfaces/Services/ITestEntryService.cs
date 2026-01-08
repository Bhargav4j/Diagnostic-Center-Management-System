namespace DiagnosticCenter.Domain.Interfaces.Services;

/// <summary>
/// Service interface for TestEntry operations
/// </summary>
public interface ITestEntryService
{
    Task<IEnumerable<TestEntryDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<TestEntryDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<TestEntryDto> CreateAsync(TestEntryCreateDto dto, CancellationToken cancellationToken = default);
    Task UpdateAsync(int id, TestEntryUpdateDto dto, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<TestEntryDto?> GetByBillNumberAsync(string billNumber, CancellationToken cancellationToken = default);
    Task<IEnumerable<TestEntryDto>> GetByMobileNumberAsync(string mobileNumber, CancellationToken cancellationToken = default);
    Task<IEnumerable<TestEntryDto>> GetUnpaidAsync(CancellationToken cancellationToken = default);
}

public class TestEntryDto
{
    public int Id { get; set; }
    public string PatientName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string MobileNumber { get; set; } = string.Empty;
    public string BillNumber { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public DateTime DueDate { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal DueAmount { get; set; }
    public int TestSetupId { get; set; }
    public string TestName { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
}

public class TestEntryCreateDto
{
    public string PatientName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string MobileNumber { get; set; } = string.Empty;
    public string BillNumber { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public DateTime DueDate { get; set; }
    public decimal PaidAmount { get; set; }
    public int TestSetupId { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
}

public class TestEntryUpdateDto
{
    public string PatientName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string MobileNumber { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public DateTime DueDate { get; set; }
    public decimal PaidAmount { get; set; }
    public int TestSetupId { get; set; }
    public string ModifiedBy { get; set; } = string.Empty;
}
