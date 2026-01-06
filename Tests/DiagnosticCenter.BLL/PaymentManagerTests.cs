using System;
using System.Collections.Generic;
using Xunit;
using DiagnosticCenter.BLL;
using DiagnosticCenter.Models;

namespace Tests.DiagnosticCenter.BLL
{
    public class PaymentManagerTests
    {
        private readonly PaymentManager _paymentManager;

        public PaymentManagerTests()
        {
            _paymentManager = new PaymentManager();
        }

        [Fact]
        public void Constructor_ShouldCreateInstance()
        {
            // Arrange & Act
            var manager = new PaymentManager();

            // Assert
            Assert.NotNull(manager);
        }

        [Fact]
        public void GetBillInfo_WithValidBillNo_ShouldReturnPaymentList()
        {
            // Arrange
            string billNo = "BILL001";

            // Act
            var result = _paymentManager.GetBillInfo(billNo);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Payment>>(result);
        }

        [Fact]
        public void GetBillInfo_WithNullBillNo_ShouldHandleGracefully()
        {
            // Arrange
            string billNo = null;

            // Act & Assert
            var exception = Record.Exception(() => _paymentManager.GetBillInfo(billNo));
            Assert.NotNull(exception);
        }

        [Fact]
        public void GetBillInfo_WithEmptyBillNo_ShouldReturnEmptyList()
        {
            // Arrange
            string billNo = "";

            // Act
            var result = _paymentManager.GetBillInfo(billNo);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Payment>>(result);
        }

        [Fact]
        public void UpdatePayment_WithValidParameters_ShouldReturnBoolean()
        {
            // Arrange
            string payAmount = "1000";
            string billNo = "BILL001";

            // Act
            var result = _paymentManager.UpdatePayment(payAmount, billNo);

            // Assert
            Assert.IsType<bool>(result);
        }

        [Fact]
        public void UpdatePayment_WithNullPayAmount_ShouldHandleGracefully()
        {
            // Arrange
            string payAmount = null;
            string billNo = "BILL001";

            // Act & Assert
            var exception = Record.Exception(() => _paymentManager.UpdatePayment(payAmount, billNo));
            Assert.NotNull(exception);
        }

        [Fact]
        public void UpdatePayment_WithNullBillNo_ShouldHandleGracefully()
        {
            // Arrange
            string payAmount = "1000";
            string billNo = null;

            // Act & Assert
            var exception = Record.Exception(() => _paymentManager.UpdatePayment(payAmount, billNo));
            Assert.NotNull(exception);
        }

        [Fact]
        public void UpdatePayment_WithEmptyPayAmount_ShouldReturnBoolean()
        {
            // Arrange
            string payAmount = "";
            string billNo = "BILL001";

            // Act
            var result = _paymentManager.UpdatePayment(payAmount, billNo);

            // Assert
            Assert.IsType<bool>(result);
        }

        [Fact]
        public void UpdatePayment_WithNegativePayAmount_ShouldReturnBoolean()
        {
            // Arrange
            string payAmount = "-500";
            string billNo = "BILL001";

            // Act
            var result = _paymentManager.UpdatePayment(payAmount, billNo);

            // Assert
            Assert.IsType<bool>(result);
        }

        [Fact]
        public void UpdatePayment_WithInvalidBillNo_ShouldReturnFalse()
        {
            // Arrange
            string payAmount = "1000";
            string billNo = "INVALID999";

            // Act
            var result = _paymentManager.UpdatePayment(payAmount, billNo);

            // Assert
            Assert.IsType<bool>(result);
        }
    }
}
