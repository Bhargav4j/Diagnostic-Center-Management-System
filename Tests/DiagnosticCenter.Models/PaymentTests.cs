using System;
using Xunit;
using DiagnosticCenter.Models;

namespace Tests.DiagnosticCenter.Models
{
    public class PaymentTests
    {
        [Fact]
        public void Constructor_ShouldCreateInstance()
        {
            // Arrange & Act
            var payment = new Payment();

            // Assert
            Assert.NotNull(payment);
        }

        [Fact]
        public void BillNo_SetAndGet_ShouldWork()
        {
            // Arrange
            var payment = new Payment();
            string expectedBillNo = "BILL001";

            // Act
            payment.BillNo = expectedBillNo;

            // Assert
            Assert.Equal(expectedBillNo, payment.BillNo);
        }

        [Fact]
        public void MobileNo_SetAndGet_ShouldWork()
        {
            // Arrange
            var payment = new Payment();
            string expectedMobileNo = "1234567890";

            // Act
            payment.MobileNo = expectedMobileNo;

            // Assert
            Assert.Equal(expectedMobileNo, payment.MobileNo);
        }

        [Fact]
        public void TestName_SetAndGet_ShouldWork()
        {
            // Arrange
            var payment = new Payment();
            string expectedTestName = "Blood Test";

            // Act
            payment.TestName = expectedTestName;

            // Assert
            Assert.Equal(expectedTestName, payment.TestName);
        }

        [Fact]
        public void TestFee_SetAndGet_ShouldWork()
        {
            // Arrange
            var payment = new Payment();
            string expectedTestFee = "500";

            // Act
            payment.TestFee = expectedTestFee;

            // Assert
            Assert.Equal(expectedTestFee, payment.TestFee);
        }

        [Fact]
        public void DueDate_SetAndGet_ShouldWork()
        {
            // Arrange
            var payment = new Payment();
            DateTime expectedDueDate = new DateTime(2024, 12, 31);

            // Act
            payment.DueDate = expectedDueDate;

            // Assert
            Assert.Equal(expectedDueDate, payment.DueDate);
        }

        [Fact]
        public void TotalAmount_SetAndGet_ShouldWork()
        {
            // Arrange
            var payment = new Payment();
            decimal expectedTotalAmount = 1000m;

            // Act
            payment.TotalAmount = expectedTotalAmount;

            // Assert
            Assert.Equal(expectedTotalAmount, payment.TotalAmount);
        }

        [Fact]
        public void PaidAmount_SetAndGet_ShouldWork()
        {
            // Arrange
            var payment = new Payment();
            decimal expectedPaidAmount = 500m;

            // Act
            payment.PaidAmount = expectedPaidAmount;

            // Assert
            Assert.Equal(expectedPaidAmount, payment.PaidAmount);
        }

        [Fact]
        public void GetDueAmount_WithValidAmounts_ShouldReturnCorrectDifference()
        {
            // Arrange
            var payment = new Payment();
            decimal totalAmount = 1000m;
            decimal paidAmount = 300m;
            decimal expectedDueAmount = 700m;

            // Act
            var result = payment.GetDueAmount(totalAmount, paidAmount);

            // Assert
            Assert.Equal(expectedDueAmount, result);
        }

        [Fact]
        public void GetDueAmount_WithZeroTotalAmount_ShouldReturnNegativePaidAmount()
        {
            // Arrange
            var payment = new Payment();
            decimal totalAmount = 0m;
            decimal paidAmount = 100m;
            decimal expectedDueAmount = -100m;

            // Act
            var result = payment.GetDueAmount(totalAmount, paidAmount);

            // Assert
            Assert.Equal(expectedDueAmount, result);
        }

        [Fact]
        public void GetDueAmount_WithZeroPaidAmount_ShouldReturnTotalAmount()
        {
            // Arrange
            var payment = new Payment();
            decimal totalAmount = 1000m;
            decimal paidAmount = 0m;
            decimal expectedDueAmount = 1000m;

            // Act
            var result = payment.GetDueAmount(totalAmount, paidAmount);

            // Assert
            Assert.Equal(expectedDueAmount, result);
        }

        [Fact]
        public void GetDueAmount_WithEqualAmounts_ShouldReturnZero()
        {
            // Arrange
            var payment = new Payment();
            decimal totalAmount = 1000m;
            decimal paidAmount = 1000m;
            decimal expectedDueAmount = 0m;

            // Act
            var result = payment.GetDueAmount(totalAmount, paidAmount);

            // Assert
            Assert.Equal(expectedDueAmount, result);
        }

        [Fact]
        public void GetDueAmount_WithPaidMoreThanTotal_ShouldReturnNegativeValue()
        {
            // Arrange
            var payment = new Payment();
            decimal totalAmount = 1000m;
            decimal paidAmount = 1500m;
            decimal expectedDueAmount = -500m;

            // Act
            var result = payment.GetDueAmount(totalAmount, paidAmount);

            // Assert
            Assert.Equal(expectedDueAmount, result);
        }

        [Fact]
        public void GetDueAmount_WithNegativeTotalAmount_ShouldCalculateCorrectly()
        {
            // Arrange
            var payment = new Payment();
            decimal totalAmount = -100m;
            decimal paidAmount = 50m;
            decimal expectedDueAmount = -150m;

            // Act
            var result = payment.GetDueAmount(totalAmount, paidAmount);

            // Assert
            Assert.Equal(expectedDueAmount, result);
        }

        [Fact]
        public void GetDueAmount_WithLargeAmounts_ShouldCalculateCorrectly()
        {
            // Arrange
            var payment = new Payment();
            decimal totalAmount = 999999.99m;
            decimal paidAmount = 500000.00m;
            decimal expectedDueAmount = 499999.99m;

            // Act
            var result = payment.GetDueAmount(totalAmount, paidAmount);

            // Assert
            Assert.Equal(expectedDueAmount, result);
        }
    }
}
