namespace DiagnosticCenter.Domain.Entities;

/// <summary>
/// Represents a diagnostic test configuration
/// </summary>
public class TestSetup
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Fee { get; set; }
    public int TypeId { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? ModifiedDate { get; set; }
    public string CreatedBy { get; set; } = "System";
    public string? ModifiedBy { get; set; }

    public virtual TestType TestType { get; set; } = null!;
    public virtual ICollection<TestEntry> TestEntries { get; set; } = new List<TestEntry>();
}
