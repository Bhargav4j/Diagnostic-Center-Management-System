namespace DiagnosticCenter.Domain.Entities;

public class TestSetup : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Fee { get; set; }
    public int TestTypeId { get; set; }

    public TestType? TestType { get; set; }
    public ICollection<TestEntryItem> TestEntryItems { get; set; } = new List<TestEntryItem>();
}
