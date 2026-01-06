namespace DiagnosticCenter.Domain.Entities;

/// <summary>
/// Represents a type of diagnostic test
/// </summary>
public class TestType
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? ModifiedDate { get; set; }
    public bool IsActive { get; set; } = true;
    public string CreatedBy { get; set; } = string.Empty;
    public string? ModifiedBy { get; set; }

    public ICollection<TestSetup> TestSetups { get; set; } = new List<TestSetup>();
}
