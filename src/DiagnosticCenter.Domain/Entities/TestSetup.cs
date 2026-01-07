namespace DiagnosticCenter.Domain.Entities;

/// <summary>
/// Represents a test setup entity
/// </summary>
public class TestSetup
{
    public int Id { get; set; }
    public int TestTypeId { get; set; }
    public string TestName { get; set; } = string.Empty;
    public decimal TestFee { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public bool IsActive { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string? ModifiedBy { get; set; }

    // Navigation properties
    public TestType TestType { get; set; } = null!;
    public ICollection<TestEntry> TestEntries { get; set; } = new List<TestEntry>();
}
