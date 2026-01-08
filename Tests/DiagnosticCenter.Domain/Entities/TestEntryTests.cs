using System;
using System.Collections.Generic;
using Xunit;
using DiagnosticCenter.Domain.Entities;

namespace DiagnosticCenter.Domain.Entities.Tests
{
    public class TestEntryTests
    {
        [Fact]
        public void Constructor_CreatesInstance()
        {
            // Arrange & Act
            var testEntry = new TestEntry();

            // Assert
            Assert.NotNull(testEntry);
        }

        [Fact]
        public void Id_CanBeSetAndRetrieved()
        {
            // Arrange
            var testEntry = new TestEntry();
            var id = 1;

            // Act
            testEntry.Id = id;

            // Assert
            Assert.Equal(id, testEntry.Id);
        }

        [Fact]
        public void PatientName_DefaultsToEmptyString()
        {
            // Arrange & Act
            var testEntry = new TestEntry();

            // Assert
            Assert.Equal(string.Empty, testEntry.PatientName);
        }

        [Fact]
        public void PatientName_CanBeSetAndRetrieved()
        {
            // Arrange
            var testEntry = new TestEntry();
            var patientName = "John Doe";

            // Act
            testEntry.PatientName = patientName;

            // Assert
            Assert.Equal(patientName, testEntry.PatientName);
        }

        [Fact]
        public void DateOfBirth_CanBeSetAndRetrieved()
        {
            // Arrange
            var testEntry = new TestEntry();
            var dob = new DateTime(1990, 5, 15);

            // Act
            testEntry.DateOfBirth = dob;

            // Assert
            Assert.Equal(dob, testEntry.DateOfBirth);
        }

        [Fact]
        public void MobileNo_DefaultsToEmptyString()
        {
            // Arrange & Act
            var testEntry = new TestEntry();

            // Assert
            Assert.Equal(string.Empty, testEntry.MobileNo);
        }

        [Fact]
        public void MobileNo_CanBeSetAndRetrieved()
        {
            // Arrange
            var testEntry = new TestEntry();
            var mobileNo = "1234567890";

            // Act
            testEntry.MobileNo = mobileNo;

            // Assert
            Assert.Equal(mobileNo, testEntry.MobileNo);
        }

        [Fact]
        public void BillNo_DefaultsToEmptyString()
        {
            // Arrange & Act
            var testEntry = new TestEntry();

            // Assert
            Assert.Equal(string.Empty, testEntry.BillNo);
        }

        [Fact]
        public void BillNo_CanBeSetAndRetrieved()
        {
            // Arrange
            var testEntry = new TestEntry();
            var billNo = "BILL001";

            // Act
            testEntry.BillNo = billNo;

            // Assert
            Assert.Equal(billNo, testEntry.BillNo);
        }

        [Fact]
        public void TotalAmount_CanBeSetAndRetrieved()
        {
            // Arrange
            var testEntry = new TestEntry();
            var totalAmount = 1500.50m;

            // Act
            testEntry.TotalAmount = totalAmount;

            // Assert
            Assert.Equal(totalAmount, testEntry.TotalAmount);
        }

        [Fact]
        public void DueDate_CanBeSetAndRetrieved()
        {
            // Arrange
            var testEntry = new TestEntry();
            var dueDate = new DateTime(2024, 12, 31);

            // Act
            testEntry.DueDate = dueDate;

            // Assert
            Assert.Equal(dueDate, testEntry.DueDate);
        }

        [Fact]
        public void PaidAmount_CanBeSetAndRetrieved()
        {
            // Arrange
            var testEntry = new TestEntry();
            var paidAmount = 750.25m;

            // Act
            testEntry.PaidAmount = paidAmount;

            // Assert
            Assert.Equal(paidAmount, testEntry.PaidAmount);
        }

        [Fact]
        public void TestId_CanBeSetAndRetrieved()
        {
            // Arrange
            var testEntry = new TestEntry();
            var testId = 5;

            // Act
            testEntry.TestId = testId;

            // Assert
            Assert.Equal(testId, testEntry.TestId);
        }

        [Fact]
        public void CreatedDate_CanBeSetAndRetrieved()
        {
            // Arrange
            var testEntry = new TestEntry();
            var createdDate = new DateTime(2024, 1, 1);

            // Act
            testEntry.CreatedDate = createdDate;

            // Assert
            Assert.Equal(createdDate, testEntry.CreatedDate);
        }

        [Fact]
        public void ModifiedDate_CanBeNull()
        {
            // Arrange
            var testEntry = new TestEntry();

            // Assert
            Assert.Null(testEntry.ModifiedDate);
        }

        [Fact]
        public void ModifiedDate_CanBeSetAndRetrieved()
        {
            // Arrange
            var testEntry = new TestEntry();
            var modifiedDate = new DateTime(2024, 6, 15);

            // Act
            testEntry.ModifiedDate = modifiedDate;

            // Assert
            Assert.Equal(modifiedDate, testEntry.ModifiedDate);
        }

        [Fact]
        public void IsActive_DefaultsToFalse()
        {
            // Arrange & Act
            var testEntry = new TestEntry();

            // Assert
            Assert.False(testEntry.IsActive);
        }

        [Fact]
        public void IsActive_CanBeSetToTrue()
        {
            // Arrange
            var testEntry = new TestEntry();

            // Act
            testEntry.IsActive = true;

            // Assert
            Assert.True(testEntry.IsActive);
        }

        [Fact]
        public void CreatedBy_CanBeNull()
        {
            // Arrange
            var testEntry = new TestEntry();

            // Assert
            Assert.Null(testEntry.CreatedBy);
        }

        [Fact]
        public void CreatedBy_CanBeSetAndRetrieved()
        {
            // Arrange
            var testEntry = new TestEntry();
            var createdBy = 1;

            // Act
            testEntry.CreatedBy = createdBy;

            // Assert
            Assert.Equal(createdBy, testEntry.CreatedBy);
        }

        [Fact]
        public void ModifiedBy_CanBeNull()
        {
            // Arrange
            var testEntry = new TestEntry();

            // Assert
            Assert.Null(testEntry.ModifiedBy);
        }

        [Fact]
        public void ModifiedBy_CanBeSetAndRetrieved()
        {
            // Arrange
            var testEntry = new TestEntry();
            var modifiedBy = 2;

            // Act
            testEntry.ModifiedBy = modifiedBy;

            // Assert
            Assert.Equal(modifiedBy, testEntry.ModifiedBy);
        }

        [Fact]
        public void TestSetup_CanBeNull()
        {
            // Arrange
            var testEntry = new TestEntry();

            // Assert
            Assert.Null(testEntry.TestSetup);
        }

        [Fact]
        public void TestSetup_CanBeSetAndRetrieved()
        {
            // Arrange
            var testEntry = new TestEntry();
            var testSetup = new TestSetup { Id = 1, Name = "Blood Test" };

            // Act
            testEntry.TestSetup = testSetup;

            // Assert
            Assert.Equal(testSetup, testEntry.TestSetup);
        }

        [Fact]
        public void Payments_InitializesAsEmptyList()
        {
            // Arrange & Act
            var testEntry = new TestEntry();

            // Assert
            Assert.NotNull(testEntry.Payments);
            Assert.Empty(testEntry.Payments);
        }

        [Fact]
        public void Payments_CanAddItems()
        {
            // Arrange
            var testEntry = new TestEntry();
            var payment = new Payment { Id = 1, Amount = 500m };

            // Act
            testEntry.Payments.Add(payment);

            // Assert
            Assert.Single(testEntry.Payments);
            Assert.Contains(payment, testEntry.Payments);
        }

        [Fact]
        public void AllProperties_CanBeSetTogether()
        {
            // Arrange
            var testEntry = new TestEntry
            {
                Id = 10,
                PatientName = "Jane Smith",
                DateOfBirth = new DateTime(1985, 3, 20),
                MobileNo = "9876543210",
                BillNo = "BILL002",
                TotalAmount = 2000m,
                DueDate = new DateTime(2024, 11, 30),
                PaidAmount = 1000m,
                TestId = 5,
                CreatedDate = new DateTime(2024, 1, 1),
                ModifiedDate = new DateTime(2024, 6, 15),
                IsActive = true,
                CreatedBy = 1,
                ModifiedBy = 2
            };

            // Assert
            Assert.Equal(10, testEntry.Id);
            Assert.Equal("Jane Smith", testEntry.PatientName);
            Assert.Equal(new DateTime(1985, 3, 20), testEntry.DateOfBirth);
            Assert.Equal("9876543210", testEntry.MobileNo);
            Assert.Equal("BILL002", testEntry.BillNo);
            Assert.Equal(2000m, testEntry.TotalAmount);
            Assert.Equal(new DateTime(2024, 11, 30), testEntry.DueDate);
            Assert.Equal(1000m, testEntry.PaidAmount);
            Assert.Equal(5, testEntry.TestId);
            Assert.Equal(new DateTime(2024, 1, 1), testEntry.CreatedDate);
            Assert.Equal(new DateTime(2024, 6, 15), testEntry.ModifiedDate);
            Assert.True(testEntry.IsActive);
            Assert.Equal(1, testEntry.CreatedBy);
            Assert.Equal(2, testEntry.ModifiedBy);
        }
    }
}
