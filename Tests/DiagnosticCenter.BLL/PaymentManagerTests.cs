using System;
using System.Collections.Generic;
using Xunit;
using DiagnosticCenter.BLL;
using DiagnosticCenter.Models;

namespace DiagnosticCenter.BLL.Tests
{
    public class PaymentManagerTests
    {
        [Fact]
        public void Constructor_ShouldCreateInstance()
        {
            // Arrange & Act
            var paymentManager = new PaymentManager();

            // Assert
            Assert.NotNull(paymentManager);
        }

        [Fact]
        public void GetBillInfo_WithValidBillNo_ShouldReturnPaymentList()
        {
            // Arrange
            var paymentManager = new PaymentManager();
            string billNo = "BILL001";

            // Act
            var result = paymentManager.GetBillInfo(billNo);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Payment>>(result);
        }

        [Fact]
        public void GetBillInfo_WithNullBillNo_ShouldHandleGracefully()
        {
            // Arrange
            var paymentManager = new PaymentManager();
            string billNo = null;

            // Act
            var result = paymentManager.GetBillInfo(billNo);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void GetBillInfo_WithEmptyBillNo_ShouldReturnList()
        {
            // Arrange
            var paymentManager = new PaymentManager();
            string billNo = "";

            // Act
            var result = paymentManager.GetBillInfo(billNo);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Payment>>(result);
        }

        [Fact]
        public void UpdatePayment_WithValidParameters_ShouldReturnBoolean()
        {
            // Arrange
            var paymentManager = new PaymentManager();
            string payAmount = "1000";
            string billNo = "BILL001";

            // Act
            var result = paymentManager.UpdatePayment(payAmount, billNo);

            // Assert
            Assert.IsType<bool>(result);
        }

        [Fact]
        public void UpdatePayment_WithNullPayAmount_ShouldHandleGracefully()
        {
            // Arrange
            var paymentManager = new PaymentManager();
            string payAmount = null;
            string billNo = "BILL001";

            // Act
            var result = paymentManager.UpdatePayment(payAmount, billNo);

            // Assert
            Assert.IsType<bool>(result);
        }

        [Fact]
        public void UpdatePayment_WithNullBillNo_ShouldHandleGracefully()
        {
            // Arrange
            var paymentManager = new PaymentManager();
            string payAmount = "1000";
            string billNo = null;

            // Act
            var result = paymentManager.UpdatePayment(payAmount, billNo);

            // Assert
            Assert.IsType<bool>(result);
        }

        [Fact]
        public void UpdatePayment_WithEmptyStrings_ShouldReturnBoolean()
        {
            // Arrange
            var paymentManager = new PaymentManager();
            string payAmount = "";
            string billNo = "";

            // Act
            var result = paymentManager.UpdatePayment(payAmount, billNo);

            // Assert
            Assert.IsType<bool>(result);
        }

        [Fact]
        public void UpdatePayment_WithNegativePayAmount_ShouldHandleGracefully()
        {
            // Arrange
            var paymentManager = new PaymentManager();
            string payAmount = "-500";
            string billNo = "BILL001";

            // Act
            var result = paymentManager.UpdatePayment(payAmount, billNo);

            // Assert
            Assert.IsType<bool>(result);
        }

        [Fact]
        public void UpdatePayment_WithZeroPayAmount_ShouldReturnBoolean()
        {
            // Arrange
            var paymentManager = new PaymentManager();
            string payAmount = "0";
            string billNo = "BILL001";

            // Act
            var result = paymentManager.UpdatePayment(payAmount, billNo);

            // Assert
            Assert.IsType<bool>(result);
        }
    }
}
