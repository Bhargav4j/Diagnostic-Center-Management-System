using Xunit;
using DiagnosticCenter.Domain.Entities;

namespace DiagnosticCenterTests.Domain.Entities;

public class TestEntryTests
{
    [Fact]
    public void TestEntry_Constructor_SetsDefaultValues()
    {
        var testEntry = new TestEntry();

        Assert.Equal(0, testEntry.Id);
        Assert.Equal(string.Empty, testEntry.PatientName);
        Assert.True(testEntry.IsActive);
    }

    [Fact]
    public void TestEntry_DueAmount_CalculatesCorrectly()
    {
        var testEntry = new TestEntry
        {
            TotalAmount = 1000m,
            PaidAmount = 400m
        };

        Assert.Equal(600m, testEntry.DueAmount);
    }

    [Fact]
    public void TestEntry_DueAmount_WhenFullyPaid_ReturnsZero()
    {
        var testEntry = new TestEntry
        {
            TotalAmount = 500m,
            PaidAmount = 500m
        };

        Assert.Equal(0m, testEntry.DueAmount);
    }

    [Fact]
    public void TestEntry_Payments_CanAddItems()
    {
        var testEntry = new TestEntry();
        var payment = new Payment { Id = 1, TestEntryId = 1 };

        testEntry.Payments.Add(payment);

        Assert.Single(testEntry.Payments);
    }
}
