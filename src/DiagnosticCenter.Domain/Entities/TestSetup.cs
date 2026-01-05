namespace DiagnosticCenter.Domain.Entities;

/// <summary>
/// Represents a test setup entity in the diagnostic center system
/// </summary>
public class TestSetup
{
    public int Id { get; set; }
    public string TestName { get; set; } = string.Empty;
    public int TestTypeId { get; set; }
    public decimal Fee { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public bool IsActive { get; set; } = true;
    public string CreatedBy { get; set; } = string.Empty;
    public string? ModifiedBy { get; set; }

    public virtual TestType? TestType { get; set; }
    public virtual ICollection<TestEntry> TestEntries { get; set; } = new List<TestEntry>();
}
