namespace DiagnosticCenter.Domain.Interfaces.Services;

public interface ITestEntryService
{
    Task<IEnumerable<TestEntryDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<TestEntryDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<TestEntryDto?> GetByBillNoAsync(string billNo, CancellationToken cancellationToken = default);
    Task<TestEntryDto> CreateAsync(TestEntryCreateDto createDto, CancellationToken cancellationToken = default);
    Task UpdateAsync(int id, TestEntryUpdateDto updateDto, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<TestEntryDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
    Task<IEnumerable<TestEntryDto>> GetUnpaidEntriesAsync(CancellationToken cancellationToken = default);
}

public class TestEntryDto
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
    public string TestName { get; set; } = string.Empty;
    public decimal DueAmount => TotalAmount - PaidAmount;
    public DateTime CreatedDate { get; set; }
    public bool IsActive { get; set; }
}

public class TestEntryCreateDto
{
    public string Name { get; set; } = string.Empty;
    public DateTime DOB { get; set; }
    public string MobileNo { get; set; } = string.Empty;
    public string BillNo { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public DateTime DueDate { get; set; }
    public decimal PaidAmount { get; set; }
    public int TestId { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
}

public class TestEntryUpdateDto
{
    public string Name { get; set; } = string.Empty;
    public DateTime DOB { get; set; }
    public string MobileNo { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public DateTime DueDate { get; set; }
    public decimal PaidAmount { get; set; }
    public bool IsActive { get; set; }
    public string ModifiedBy { get; set; } = string.Empty;
}
