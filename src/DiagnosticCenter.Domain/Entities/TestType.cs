namespace DiagnosticCenter.Domain.Entities;

public class TestType : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public ICollection<TestSetup> TestSetups { get; set; } = new List<TestSetup>();
}
