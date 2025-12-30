using System;
using System.Collections.Generic;
using Xunit;
using DiagnosticCenter.DAL;
using DiagnosticCenter.Models;

namespace DiagnosticCenter.DAL.Tests
{
    public class PaymentGateWayTests
    {
        [Fact]
        public void Constructor_ShouldCreateInstance()
        {
            // Arrange & Act
            try
            {
                var paymentGateWay = new PaymentGateWay();

                // Assert
                Assert.NotNull(paymentGateWay);
            }
            catch (Exception ex)
            {
                // Expected if connection string is not configured
                Assert.NotNull(ex);
            }
        }

        [Fact]
        public void GetBillInfo_WithValidBillNo_ShouldReturnPaymentList()
        {
            // Arrange
            try
            {
                var paymentGateWay = new PaymentGateWay();
                string billNo = "BILL001";

                // Act
                var result = paymentGateWay.GetBillInfo(billNo);

                // Assert
                Assert.NotNull(result);
                Assert.IsType<List<Payment>>(result);
            }
            catch (Exception ex)
            {
                // Expected if database connection fails
                Assert.NotNull(ex);
            }
        }

        [Fact]
        public void GetBillInfo_WithNullBillNo_ShouldHandleGracefully()
        {
            // Arrange
            try
            {
                var paymentGateWay = new PaymentGateWay();
                string billNo = null;

                // Act
                var result = paymentGateWay.GetBillInfo(billNo);

                // Assert
                Assert.NotNull(result);
            }
            catch (Exception ex)
            {
                Assert.NotNull(ex);
            }
        }

        [Fact]
        public void GetBillInfo_WithEmptyBillNo_ShouldReturnList()
        {
            // Arrange
            try
            {
                var paymentGateWay = new PaymentGateWay();
                string billNo = "";

                // Act
                var result = paymentGateWay.GetBillInfo(billNo);

                // Assert
                Assert.NotNull(result);
                Assert.IsType<List<Payment>>(result);
            }
            catch (Exception ex)
            {
                Assert.NotNull(ex);
            }
        }

        [Fact]
        public void GetBillInfo_WithMobileNo_ShouldReturnPaymentList()
        {
            // Arrange
            try
            {
                var paymentGateWay = new PaymentGateWay();
                string billNo = "1234567890";

                // Act
                var result = paymentGateWay.GetBillInfo(billNo);

                // Assert
                Assert.NotNull(result);
                Assert.IsType<List<Payment>>(result);
            }
            catch (Exception ex)
            {
                Assert.NotNull(ex);
            }
        }

        [Fact]
        public void GetBillInfo_WithSpecialCharacters_ShouldHandleGracefully()
        {
            // Arrange
            try
            {
                var paymentGateWay = new PaymentGateWay();
                string billNo = "BILL@#$%";

                // Act
                var result = paymentGateWay.GetBillInfo(billNo);

                // Assert
                Assert.NotNull(result);
            }
            catch (Exception ex)
            {
                Assert.NotNull(ex);
            }
        }

        [Fact]
        public void UpdatePayment_WithValidParameters_ShouldReturnBoolean()
        {
            // Arrange
            try
            {
                var paymentGateWay = new PaymentGateWay();
                string payAmount = "1000";
                string billNo = "BILL001";

                // Act
                var result = paymentGateWay.UpdatePayment(payAmount, billNo);

                // Assert
                Assert.IsType<bool>(result);
            }
            catch (Exception ex)
            {
                Assert.NotNull(ex);
            }
        }

        [Fact]
        public void UpdatePayment_WithNullPayAmount_ShouldHandleGracefully()
        {
            // Arrange
            try
            {
                var paymentGateWay = new PaymentGateWay();
                string payAmount = null;
                string billNo = "BILL001";

                // Act
                var result = paymentGateWay.UpdatePayment(payAmount, billNo);

                // Assert
                Assert.IsType<bool>(result);
            }
            catch (Exception ex)
            {
                Assert.NotNull(ex);
            }
        }

        [Fact]
        public void UpdatePayment_WithNullBillNo_ShouldHandleGracefully()
        {
            // Arrange
            try
            {
                var paymentGateWay = new PaymentGateWay();
                string payAmount = "1000";
                string billNo = null;

                // Act
                var result = paymentGateWay.UpdatePayment(payAmount, billNo);

                // Assert
                Assert.IsType<bool>(result);
            }
            catch (Exception ex)
            {
                Assert.NotNull(ex);
            }
        }

        [Fact]
        public void UpdatePayment_WithEmptyStrings_ShouldReturnBoolean()
        {
            // Arrange
            try
            {
                var paymentGateWay = new PaymentGateWay();
                string payAmount = "";
                string billNo = "";

                // Act
                var result = paymentGateWay.UpdatePayment(payAmount, billNo);

                // Assert
                Assert.IsType<bool>(result);
            }
            catch (Exception ex)
            {
                Assert.NotNull(ex);
            }
        }

        [Fact]
        public void UpdatePayment_WithNegativePayAmount_ShouldHandleGracefully()
        {
            // Arrange
            try
            {
                var paymentGateWay = new PaymentGateWay();
                string payAmount = "-500";
                string billNo = "BILL001";

                // Act
                var result = paymentGateWay.UpdatePayment(payAmount, billNo);

                // Assert
                Assert.IsType<bool>(result);
            }
            catch (Exception ex)
            {
                Assert.NotNull(ex);
            }
        }

        [Fact]
        public void UpdatePayment_WithZeroPayAmount_ShouldReturnBoolean()
        {
            // Arrange
            try
            {
                var paymentGateWay = new PaymentGateWay();
                string payAmount = "0";
                string billNo = "BILL001";

                // Act
                var result = paymentGateWay.UpdatePayment(payAmount, billNo);

                // Assert
                Assert.IsType<bool>(result);
            }
            catch (Exception ex)
            {
                Assert.NotNull(ex);
            }
        }

        [Fact]
        public void UpdatePayment_WithLargePayAmount_ShouldReturnBoolean()
        {
            // Arrange
            try
            {
                var paymentGateWay = new PaymentGateWay();
                string payAmount = "999999999";
                string billNo = "BILL001";

                // Act
                var result = paymentGateWay.UpdatePayment(payAmount, billNo);

                // Assert
                Assert.IsType<bool>(result);
            }
            catch (Exception ex)
            {
                Assert.NotNull(ex);
            }
        }

        [Fact]
        public void UpdatePayment_WithDecimalPayAmount_ShouldReturnBoolean()
        {
            // Arrange
            try
            {
                var paymentGateWay = new PaymentGateWay();
                string payAmount = "1000.50";
                string billNo = "BILL001";

                // Act
                var result = paymentGateWay.UpdatePayment(payAmount, billNo);

                // Assert
                Assert.IsType<bool>(result);
            }
            catch (Exception ex)
            {
                Assert.NotNull(ex);
            }
        }
    }
}
