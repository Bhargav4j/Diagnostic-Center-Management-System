using System;
using Xunit;
using DiagnosticCenter.Domain.Entities;

namespace DiagnosticCenter.Domain.Entities.Tests
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
        public void Id_CanBeSetAndRetrieved()
        {
            // Arrange
            var payment = new Payment();
            var id = 1;

            // Act
            payment.Id = id;

            // Assert
            Assert.Equal(id, payment.Id);
        }

        [Fact]
        public void BillNo_DefaultsToEmptyString()
        {
            // Arrange & Act
            var payment = new Payment();

            // Assert
            Assert.Equal(string.Empty, payment.BillNo);
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
        public void Amount_CanBeSetAndRetrieved()
        {
            // Arrange
            var payment = new Payment();
            var amount = 1500.50m;

            // Act
            payment.Amount = amount;

            // Assert
            Assert.Equal(amount, payment.Amount);
        }

        [Fact]
        public void Amount_WithZero_CanBeSet()
        {
            // Arrange
            var payment = new Payment();
            var amount = 0m;

            // Act
            payment.Amount = amount;

            // Assert
            Assert.Equal(0m, payment.Amount);
        }

        [Fact]
        public void PaymentDate_CanBeSetAndRetrieved()
        {
            // Arrange
            var payment = new Payment();
            var paymentDate = new DateTime(2024, 6, 15);

            // Act
            payment.PaymentDate = paymentDate;

            // Assert
            Assert.Equal(paymentDate, payment.PaymentDate);
        }

        [Fact]
        public void TestEntryId_CanBeSetAndRetrieved()
        {
            // Arrange
            var payment = new Payment();
            var testEntryId = 5;

            // Act
            payment.TestEntryId = testEntryId;

            // Assert
            Assert.Equal(testEntryId, payment.TestEntryId);
        }

        [Fact]
        public void CreatedDate_CanBeSetAndRetrieved()
        {
            // Arrange
            var payment = new Payment();
            var createdDate = new DateTime(2024, 1, 1);

            // Act
            payment.CreatedDate = createdDate;

            // Assert
            Assert.Equal(createdDate, payment.CreatedDate);
        }

        [Fact]
        public void ModifiedDate_CanBeNull()
        {
            // Arrange
            var payment = new Payment();

            // Assert
            Assert.Null(payment.ModifiedDate);
        }

        [Fact]
        public void ModifiedDate_CanBeSetAndRetrieved()
        {
            // Arrange
            var payment = new Payment();
            var modifiedDate = new DateTime(2024, 6, 15);

            // Act
            payment.ModifiedDate = modifiedDate;

            // Assert
            Assert.Equal(modifiedDate, payment.ModifiedDate);
        }

        [Fact]
        public void IsActive_DefaultsToFalse()
        {
            // Arrange & Act
            var payment = new Payment();

            // Assert
            Assert.False(payment.IsActive);
        }

        [Fact]
        public void IsActive_CanBeSetToTrue()
        {
            // Arrange
            var payment = new Payment();

            // Act
            payment.IsActive = true;

            // Assert
            Assert.True(payment.IsActive);
        }

        [Fact]
        public void CreatedBy_CanBeNull()
        {
            // Arrange
            var payment = new Payment();

            // Assert
            Assert.Null(payment.CreatedBy);
        }

        [Fact]
        public void CreatedBy_CanBeSetAndRetrieved()
        {
            // Arrange
            var payment = new Payment();
            var createdBy = 1;

            // Act
            payment.CreatedBy = createdBy;

            // Assert
            Assert.Equal(createdBy, payment.CreatedBy);
        }

        [Fact]
        public void ModifiedBy_CanBeNull()
        {
            // Arrange
            var payment = new Payment();

            // Assert
            Assert.Null(payment.ModifiedBy);
        }

        [Fact]
        public void ModifiedBy_CanBeSetAndRetrieved()
        {
            // Arrange
            var payment = new Payment();
            var modifiedBy = 2;

            // Act
            payment.ModifiedBy = modifiedBy;

            // Assert
            Assert.Equal(modifiedBy, payment.ModifiedBy);
        }

        [Fact]
        public void TestEntry_CanBeNull()
        {
            // Arrange
            var payment = new Payment();

            // Assert
            Assert.Null(payment.TestEntry);
        }

        [Fact]
        public void TestEntry_CanBeSetAndRetrieved()
        {
            // Arrange
            var payment = new Payment();
            var testEntry = new TestEntry { Id = 1, PatientName = "John Doe" };

            // Act
            payment.TestEntry = testEntry;

            // Assert
            Assert.Equal(testEntry, payment.TestEntry);
        }

        [Fact]
        public void AllProperties_CanBeSetTogether()
        {
            // Arrange
            var payment = new Payment
            {
                Id = 10,
                BillNo = "BILL002",
                Amount = 2500.75m,
                PaymentDate = new DateTime(2024, 6, 15),
                TestEntryId = 5,
                CreatedDate = new DateTime(2024, 1, 1),
                ModifiedDate = new DateTime(2024, 6, 15),
                IsActive = true,
                CreatedBy = 1,
                ModifiedBy = 2
            };

            // Assert
            Assert.Equal(10, payment.Id);
            Assert.Equal("BILL002", payment.BillNo);
            Assert.Equal(2500.75m, payment.Amount);
            Assert.Equal(new DateTime(2024, 6, 15), payment.PaymentDate);
            Assert.Equal(5, payment.TestEntryId);
            Assert.Equal(new DateTime(2024, 1, 1), payment.CreatedDate);
            Assert.Equal(new DateTime(2024, 6, 15), payment.ModifiedDate);
            Assert.True(payment.IsActive);
            Assert.Equal(1, payment.CreatedBy);
            Assert.Equal(2, payment.ModifiedBy);
        }

        [Fact]
        public void Amount_WithNegative_CanBeSet()
        {
            // Arrange
            var payment = new Payment();
            var amount = -100m;

            // Act
            payment.Amount = amount;

            // Assert
            Assert.Equal(-100m, payment.Amount);
        }

        [Fact]
        public void PaymentDate_WithMinValue_CanBeSet()
        {
            // Arrange
            var payment = new Payment();
            var paymentDate = DateTime.MinValue;

            // Act
            payment.PaymentDate = paymentDate;

            // Assert
            Assert.Equal(DateTime.MinValue, payment.PaymentDate);
        }

        [Fact]
        public void PaymentDate_WithMaxValue_CanBeSet()
        {
            // Arrange
            var payment = new Payment();
            var paymentDate = DateTime.MaxValue;

            // Act
            payment.PaymentDate = paymentDate;

            // Assert
            Assert.Equal(DateTime.MaxValue, payment.PaymentDate);
        }
    }
}
