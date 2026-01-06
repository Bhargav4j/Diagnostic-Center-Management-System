namespace DiagnosticCenter.Domain.Interfaces.Services;

public interface ITestSetupService
{
    Task<IEnumerable<TestSetupDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<TestSetupDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<TestSetupDto> CreateAsync(TestSetupCreateDto createDto, CancellationToken cancellationToken = default);
    Task UpdateAsync(int id, TestSetupUpdateDto updateDto, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<TestSetupDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
    Task<IEnumerable<TestSetupDto>> GetByTypeIdAsync(int typeId, CancellationToken cancellationToken = default);
}

public class TestSetupDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Fee { get; set; }
    public int TypeId { get; set; }
    public string TypeName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedDate { get; set; }
    public bool IsActive { get; set; }
}

public class TestSetupCreateDto
{
    public string Name { get; set; } = string.Empty;
    public decimal Fee { get; set; }
    public int TypeId { get; set; }
    public string? Description { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
}

public class TestSetupUpdateDto
{
    public string Name { get; set; } = string.Empty;
    public decimal Fee { get; set; }
    public int TypeId { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public string ModifiedBy { get; set; } = string.Empty;
}
