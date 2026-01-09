namespace DiagnosticCenter.Domain.Entities;

/// <summary>
/// Represents a patient in the diagnostic center
/// </summary>
public class Patient
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string MobileNo { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public bool IsActive { get; set; } = true;
    public string CreatedBy { get; set; } = string.Empty;
    public string? ModifiedBy { get; set; }

    public virtual ICollection<PatientTest> PatientTests { get; set; } = new List<PatientTest>();
}
