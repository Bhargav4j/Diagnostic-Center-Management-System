using Xunit;
using DiagnosticCenter.Domain.Entities;

namespace DiagnosticCenterTests.Domain.Entities;

public class PaymentTests
{
    [Fact]
    public void Payment_Constructor_SetsDefaultValues()
    {
        var payment = new Payment();

        Assert.Equal(0, payment.Id);
        Assert.Equal(string.Empty, payment.BillNumber);
        Assert.Equal(0m, payment.Amount);
        Assert.True(payment.IsActive);
    }

    [Fact]
    public void Payment_PaymentDate_IsSetToUtcNow()
    {
        var beforeCreation = DateTime.UtcNow;
        var payment = new Payment();
        var afterCreation = DateTime.UtcNow;

        Assert.True(payment.PaymentDate >= beforeCreation);
        Assert.True(payment.PaymentDate <= afterCreation);
    }

    [Fact]
    public void Payment_SetAmount_UpdatesAmountProperty()
    {
        var payment = new Payment();
        payment.Amount = 500.75m;

        Assert.Equal(500.75m, payment.Amount);
    }
}
