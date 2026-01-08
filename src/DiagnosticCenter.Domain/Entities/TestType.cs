namespace DiagnosticCenter.Domain.Entities;

/// <summary>
/// Represents a test type in the diagnostic center
/// </summary>
public class TestType
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? ModifiedDate { get; set; }
    public bool IsActive { get; set; } = true;
    public string CreatedBy { get; set; } = string.Empty;
    public string? ModifiedBy { get; set; }

    // Navigation property
    public ICollection<TestSetup> TestSetups { get; set; } = new List<TestSetup>();
}
