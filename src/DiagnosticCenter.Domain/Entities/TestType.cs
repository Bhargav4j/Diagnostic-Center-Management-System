namespace DiagnosticCenter.Domain.Entities;

/// <summary>
/// Represents a diagnostic test type
/// </summary>
public class TestType
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public bool IsActive { get; set; } = true;
    public string CreatedBy { get; set; } = string.Empty;
    public string? ModifiedBy { get; set; }

    public virtual ICollection<TestSetup> TestSetups { get; set; } = new List<TestSetup>();
}
