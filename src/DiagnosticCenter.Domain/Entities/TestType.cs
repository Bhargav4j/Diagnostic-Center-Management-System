namespace DiagnosticCenter.Domain.Entities;

/// <summary>
/// Represents a test type category
/// </summary>
public class TestType
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? ModifiedDate { get; set; }
    public string CreatedBy { get; set; } = "System";
    public string? ModifiedBy { get; set; }

    public virtual ICollection<TestSetup> TestSetups { get; set; } = new List<TestSetup>();
}
