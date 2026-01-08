namespace DiagnosticCenter.Domain.Interfaces.Services;

/// <summary>
/// Service interface for TestSetup operations
/// </summary>
public interface ITestSetupService
{
    Task<IEnumerable<TestSetupDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<TestSetupDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<TestSetupDto> CreateAsync(TestSetupCreateDto dto, CancellationToken cancellationToken = default);
    Task UpdateAsync(int id, TestSetupUpdateDto dto, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<TestSetupDto>> GetByTestTypeIdAsync(int testTypeId, CancellationToken cancellationToken = default);
    Task<IEnumerable<TestSetupDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}

public class TestSetupDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Fee { get; set; }
    public int TestTypeId { get; set; }
    public string TestTypeName { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public bool IsActive { get; set; }
}

public class TestSetupCreateDto
{
    public string Name { get; set; } = string.Empty;
    public decimal Fee { get; set; }
    public int TestTypeId { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
}

public class TestSetupUpdateDto
{
    public string Name { get; set; } = string.Empty;
    public decimal Fee { get; set; }
    public int TestTypeId { get; set; }
    public string ModifiedBy { get; set; } = string.Empty;
}
