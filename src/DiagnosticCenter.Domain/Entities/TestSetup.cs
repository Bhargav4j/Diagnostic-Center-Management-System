namespace DiagnosticCenter.Domain.Entities;

/// <summary>
/// Represents a test setup entity in the diagnostic center system
/// </summary>
public class TestSetup
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public decimal Fee { get; set; }

    public int TypeId { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public bool IsActive { get; set; }

    public string CreatedBy { get; set; } = string.Empty;

    public string? ModifiedBy { get; set; }

    public virtual TestType? TestType { get; set; }
}
