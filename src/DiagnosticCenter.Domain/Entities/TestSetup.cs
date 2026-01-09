namespace DiagnosticCenter.Domain.Entities;

/// <summary>
/// Represents a diagnostic test setup/configuration
/// </summary>
public class TestSetup
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Fee { get; set; }
    public int TestTypeId { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public bool IsActive { get; set; } = true;
    public string CreatedBy { get; set; } = string.Empty;
    public string? ModifiedBy { get; set; }

    public virtual TestType? TestType { get; set; }
    public virtual ICollection<PatientTest> PatientTests { get; set; } = new List<PatientTest>();
}
