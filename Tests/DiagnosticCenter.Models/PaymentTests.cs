using System;
using Xunit;
using DiagnosticCenter.Models;

namespace DiagnosticCenter.Models.Tests
{
    public class PaymentTests
    {
        [Fact]
        public void Constructor_CreatesInstance()
        {
            // Arrange & Act
            var payment = new Payment();

            // Assert
            Assert.NotNull(payment);
        }

        [Fact]
        public void BillNo_CanBeSetAndRetrieved()
        {
            // Arrange
            var payment = new Payment();
            var billNo = "BILL001";

            // Act
            payment.BillNo = billNo;

            // Assert
            Assert.Equal(billNo, payment.BillNo);
        }

        [Fact]
        public void MobileNo_CanBeSetAndRetrieved()
        {
            // Arrange
            var payment = new Payment();
            var mobileNo = "1234567890";

            // Act
            payment.MobileNo = mobileNo;

            // Assert
            Assert.Equal(mobileNo, payment.MobileNo);
        }

        [Fact]
        public void TestName_CanBeSetAndRetrieved()
        {
            // Arrange
            var payment = new Payment();
            var testName = "Blood Test";

            // Act
            payment.TestName = testName;

            // Assert
            Assert.Equal(testName, payment.TestName);
        }

        [Fact]
        public void TestFee_CanBeSetAndRetrieved()
        {
            // Arrange
            var payment = new Payment();
            var testFee = "1000";

            // Act
            payment.TestFee = testFee;

            // Assert
            Assert.Equal(testFee, payment.TestFee);
        }

        [Fact]
        public void DueDate_CanBeSetAndRetrieved()
        {
            // Arrange
            var payment = new Payment();
            var dueDate = new DateTime(2024, 12, 31);

            // Act
            payment.DueDate = dueDate;

            // Assert
            Assert.Equal(dueDate, payment.DueDate);
        }

        [Fact]
        public void TotalAmount_CanBeSetAndRetrieved()
        {
            // Arrange
            var payment = new Payment();
            var totalAmount = 1000m;

            // Act
            payment.TotalAmount = totalAmount;

            // Assert
            Assert.Equal(totalAmount, payment.TotalAmount);
        }

        [Fact]
        public void PaidAmount_CanBeSetAndRetrieved()
        {
            // Arrange
            var payment = new Payment();
            var paidAmount = 500m;

            // Act
            payment.PaidAmount = paidAmount;

            // Assert
            Assert.Equal(paidAmount, payment.PaidAmount);
        }

        [Fact]
        public void GetDueAmount_WithValidAmounts_ReturnsCorrectDifference()
        {
            // Arrange
            var payment = new Payment();
            var totalAmount = 1000m;
            var paidAmount = 400m;

            // Act
            var result = payment.GetDueAmount(totalAmount, paidAmount);

            // Assert
            Assert.Equal(600m, result);
        }

        [Fact]
        public void GetDueAmount_WithZeroPaidAmount_ReturnsTotalAmount()
        {
            // Arrange
            var payment = new Payment();
            var totalAmount = 1000m;
            var paidAmount = 0m;

            // Act
            var result = payment.GetDueAmount(totalAmount, paidAmount);

            // Assert
            Assert.Equal(1000m, result);
        }

        [Fact]
        public void GetDueAmount_WithFullyPaidAmount_ReturnsZero()
        {
            // Arrange
            var payment = new Payment();
            var totalAmount = 1000m;
            var paidAmount = 1000m;

            // Act
            var result = payment.GetDueAmount(totalAmount, paidAmount);

            // Assert
            Assert.Equal(0m, result);
        }

        [Fact]
        public void GetDueAmount_WithOverpaidAmount_ReturnsNegative()
        {
            // Arrange
            var payment = new Payment();
            var totalAmount = 1000m;
            var paidAmount = 1500m;

            // Act
            var result = payment.GetDueAmount(totalAmount, paidAmount);

            // Assert
            Assert.Equal(-500m, result);
        }

        [Fact]
        public void GetDueAmount_WithZeroTotalAmount_ReturnsNegativePaidAmount()
        {
            // Arrange
            var payment = new Payment();
            var totalAmount = 0m;
            var paidAmount = 500m;

            // Act
            var result = payment.GetDueAmount(totalAmount, paidAmount);

            // Assert
            Assert.Equal(-500m, result);
        }

        [Fact]
        public void GetDueAmount_WithBothZero_ReturnsZero()
        {
            // Arrange
            var payment = new Payment();
            var totalAmount = 0m;
            var paidAmount = 0m;

            // Act
            var result = payment.GetDueAmount(totalAmount, paidAmount);

            // Assert
            Assert.Equal(0m, result);
        }

        [Fact]
        public void GetDueAmount_WithDecimalAmounts_ReturnsCorrectResult()
        {
            // Arrange
            var payment = new Payment();
            var totalAmount = 1234.56m;
            var paidAmount = 789.12m;

            // Act
            var result = payment.GetDueAmount(totalAmount, paidAmount);

            // Assert
            Assert.Equal(445.44m, result);
        }
    }
}
