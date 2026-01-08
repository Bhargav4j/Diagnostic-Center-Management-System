using System;
using Xunit;
using DiagnosticCenter.Models;

namespace DiagnosticCenter.Models.Tests
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
        public void Name_CanBeSetAndRetrieved()
        {
            // Arrange
            var testEntry = new TestEntry();
            var name = "John Doe";

            // Act
            testEntry.Name = name;

            // Assert
            Assert.Equal(name, testEntry.Name);
        }

        [Fact]
        public void DOB_CanBeSetAndRetrieved()
        {
            // Arrange
            var testEntry = new TestEntry();
            var dob = new DateTime(1990, 5, 15);

            // Act
            testEntry.DOB = dob;

            // Assert
            Assert.Equal(dob, testEntry.DOB);
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
            var testId = 1;

            // Act
            testEntry.TestId = testId;

            // Assert
            Assert.Equal(testId, testEntry.TestId);
        }

        [Fact]
        public void TestName_CanBeSetAndRetrieved()
        {
            // Arrange
            var testEntry = new TestEntry();
            var testName = "Blood Test";

            // Act
            testEntry.TestName = testName;

            // Assert
            Assert.Equal(testName, testEntry.TestName);
        }

        [Fact]
        public void TestFee_CanBeSetAndRetrieved()
        {
            // Arrange
            var testEntry = new TestEntry();
            var testFee = "1000";

            // Act
            testEntry.TestFee = testFee;

            // Assert
            Assert.Equal(testFee, testEntry.TestFee);
        }

        [Fact]
        public void AllProperties_CanBeSetTogether()
        {
            // Arrange
            var testEntry = new TestEntry
            {
                Name = "Jane Smith",
                DOB = new DateTime(1985, 3, 20),
                MobileNo = "9876543210",
                BillNo = "BILL002",
                TotalAmount = 2000m,
                DueDate = new DateTime(2024, 11, 30),
                PaidAmount = 1000m,
                TestId = 5,
                TestName = "X-Ray",
                TestFee = "500"
            };

            // Assert
            Assert.Equal("Jane Smith", testEntry.Name);
            Assert.Equal(new DateTime(1985, 3, 20), testEntry.DOB);
            Assert.Equal("9876543210", testEntry.MobileNo);
            Assert.Equal("BILL002", testEntry.BillNo);
            Assert.Equal(2000m, testEntry.TotalAmount);
            Assert.Equal(new DateTime(2024, 11, 30), testEntry.DueDate);
            Assert.Equal(1000m, testEntry.PaidAmount);
            Assert.Equal(5, testEntry.TestId);
            Assert.Equal("X-Ray", testEntry.TestName);
            Assert.Equal("500", testEntry.TestFee);
        }

        [Fact]
        public void TotalAmount_WithZero_CanBeSet()
        {
            // Arrange
            var testEntry = new TestEntry();
            var totalAmount = 0m;

            // Act
            testEntry.TotalAmount = totalAmount;

            // Assert
            Assert.Equal(0m, testEntry.TotalAmount);
        }

        [Fact]
        public void PaidAmount_WithNegative_CanBeSet()
        {
            // Arrange
            var testEntry = new TestEntry();
            var paidAmount = -100m;

            // Act
            testEntry.PaidAmount = paidAmount;

            // Assert
            Assert.Equal(-100m, testEntry.PaidAmount);
        }

        [Fact]
        public void TestId_WithZero_CanBeSet()
        {
            // Arrange
            var testEntry = new TestEntry();
            var testId = 0;

            // Act
            testEntry.TestId = testId;

            // Assert
            Assert.Equal(0, testEntry.TestId);
        }

        [Fact]
        public void DOB_WithMinValue_CanBeSet()
        {
            // Arrange
            var testEntry = new TestEntry();
            var dob = DateTime.MinValue;

            // Act
            testEntry.DOB = dob;

            // Assert
            Assert.Equal(DateTime.MinValue, testEntry.DOB);
        }
    }
}
