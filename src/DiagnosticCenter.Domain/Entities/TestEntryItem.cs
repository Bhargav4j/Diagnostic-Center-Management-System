namespace DiagnosticCenter.Domain.Entities;

public class TestEntryItem : BaseEntity
{
    public int TestEntryId { get; set; }
    public int TestSetupId { get; set; }
    public decimal Fee { get; set; }

    public TestEntry? TestEntry { get; set; }
    public TestSetup? TestSetup { get; set; }
}
