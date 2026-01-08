using Xunit;
using DiagnosticCenter.Domain.Entities;

namespace DiagnosticCenterTests.Domain.Entities;

public class TestSetupTests
{
    [Fact]
    public void TestSetup_Constructor_SetsDefaultValues()
    {
        var testSetup = new TestSetup();

        Assert.Equal(0, testSetup.Id);
        Assert.Equal(string.Empty, testSetup.Name);
        Assert.Equal(0m, testSetup.Fee);
        Assert.True(testSetup.IsActive);
    }

    [Fact]
    public void TestSetup_SetFee_UpdatesFeeProperty()
    {
        var testSetup = new TestSetup();
        testSetup.Fee = 150.50m;

        Assert.Equal(150.50m, testSetup.Fee);
    }

    [Fact]
    public void TestSetup_TestEntries_CanAddItems()
    {
        var testSetup = new TestSetup();
        var testEntry = new TestEntry { Id = 1, TestSetupId = 1 };

        testSetup.TestEntries.Add(testEntry);

        Assert.Single(testSetup.TestEntries);
    }
}
