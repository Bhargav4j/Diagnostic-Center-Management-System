namespace DiagnosticCenter.Domain.Entities;

/// <summary>
/// Represents a test type entity
/// </summary>
public class TestType
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public bool IsActive { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string? ModifiedBy { get; set; }

    // Navigation properties
    public ICollection<TestSetup> TestSetups { get; set; } = new List<TestSetup>();
}
