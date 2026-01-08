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
        public void Constructor_CreatesInstance()
        {
            // Arrange & Act
            var paymentManager = new PaymentManager();

            // Assert
            Assert.NotNull(paymentManager);
        }

        [Fact]
        public void GetBillInfo_WithValidBillNo_ReturnsPaymentList()
        {
            // Arrange
            var paymentManager = new PaymentManager();
            var billNo = "BILL001";

            // Act
            var result = paymentManager.GetBillInfo(billNo);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Payment>>(result);
        }

        [Fact]
        public void GetBillInfo_WithEmptyBillNo_ReturnsPaymentList()
        {
            // Arrange
            var paymentManager = new PaymentManager();
            var billNo = "";

            // Act
            var result = paymentManager.GetBillInfo(billNo);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void GetBillInfo_WithNullBillNo_HandlesNull()
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
        public void UpdatePayment_WithValidParameters_ReturnsBoolean()
        {
            // Arrange
            var paymentManager = new PaymentManager();
            var payAmount = "1000";
            var billNo = "BILL001";

            // Act
            var result = paymentManager.UpdatePayment(payAmount, billNo);

            // Assert
            Assert.IsType<bool>(result);
        }

        [Fact]
        public void UpdatePayment_WithEmptyPayAmount_ReturnsBoolean()
        {
            // Arrange
            var paymentManager = new PaymentManager();
            var payAmount = "";
            var billNo = "BILL001";

            // Act
            var result = paymentManager.UpdatePayment(payAmount, billNo);

            // Assert
            Assert.IsType<bool>(result);
        }

        [Fact]
        public void UpdatePayment_WithNullPayAmount_HandlesNull()
        {
            // Arrange
            var paymentManager = new PaymentManager();
            string payAmount = null;
            var billNo = "BILL001";

            // Act
            var result = paymentManager.UpdatePayment(payAmount, billNo);

            // Assert
            Assert.IsType<bool>(result);
        }

        [Fact]
        public void UpdatePayment_WithEmptyBillNo_ReturnsBoolean()
        {
            // Arrange
            var paymentManager = new PaymentManager();
            var payAmount = "1000";
            var billNo = "";

            // Act
            var result = paymentManager.UpdatePayment(payAmount, billNo);

            // Assert
            Assert.IsType<bool>(result);
        }

        [Fact]
        public void UpdatePayment_WithNegativePayAmount_ReturnsBoolean()
        {
            // Arrange
            var paymentManager = new PaymentManager();
            var payAmount = "-100";
            var billNo = "BILL001";

            // Act
            var result = paymentManager.UpdatePayment(payAmount, billNo);

            // Assert
            Assert.IsType<bool>(result);
        }

        [Fact]
        public void UpdatePayment_WithZeroPayAmount_ReturnsBoolean()
        {
            // Arrange
            var paymentManager = new PaymentManager();
            var payAmount = "0";
            var billNo = "BILL001";

            // Act
            var result = paymentManager.UpdatePayment(payAmount, billNo);

            // Assert
            Assert.IsType<bool>(result);
        }
    }
}
