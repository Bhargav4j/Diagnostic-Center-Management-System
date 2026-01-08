namespace DiagnosticCenter.Domain.Entities;

/// <summary>
/// Represents a test setup with fee information
/// </summary>
public class TestSetup
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Fee { get; set; }
    public int TestTypeId { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? ModifiedDate { get; set; }
    public bool IsActive { get; set; } = true;
    public string CreatedBy { get; set; } = string.Empty;
    public string? ModifiedBy { get; set; }

    // Navigation properties
    public TestType TestType { get; set; } = null!;
    public ICollection<TestEntry> TestEntries { get; set; } = new List<TestEntry>();
}
