namespace DiagnosticCenter.Domain.Interfaces.Services;

/// <summary>
/// Service interface for TestType operations
/// </summary>
public interface ITestTypeService
{
    Task<IEnumerable<TestTypeDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<TestTypeDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<TestTypeDto> CreateAsync(TestTypeCreateDto dto, CancellationToken cancellationToken = default);
    Task UpdateAsync(int id, TestTypeUpdateDto dto, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(string name, CancellationToken cancellationToken = default);
    Task<IEnumerable<TestTypeDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}

public class TestTypeDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public bool IsActive { get; set; }
}

public class TestTypeCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string CreatedBy { get; set; } = string.Empty;
}

public class TestTypeUpdateDto
{
    public string Name { get; set; } = string.Empty;
    public string ModifiedBy { get; set; } = string.Empty;
}
