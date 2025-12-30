using System;
using Xunit;
using DiagnosticCenter.Models;

namespace DiagnosticCenter.Models.Tests
{
    public class TestEntryTests
    {
        [Fact]
        public void Constructor_ShouldCreateInstance()
        {
            // Arrange & Act
            var testEntry = new TestEntry();

            // Assert
            Assert.NotNull(testEntry);
        }

        [Fact]
        public void Name_ShouldSetAndGetValue()
        {
            // Arrange
            var testEntry = new TestEntry();
            string expectedName = "John Doe";

            // Act
            testEntry.Name = expectedName;

            // Assert
            Assert.Equal(expectedName, testEntry.Name);
        }

        [Fact]
        public void DOB_ShouldSetAndGetValue()
        {
            // Arrange
            var testEntry = new TestEntry();
            DateTime expectedDOB = new DateTime(1990, 5, 15);

            // Act
            testEntry.DOB = expectedDOB;

            // Assert
            Assert.Equal(expectedDOB, testEntry.DOB);
        }

        [Fact]
        public void MobileNo_ShouldSetAndGetValue()
        {
            // Arrange
            var testEntry = new TestEntry();
            string expectedMobileNo = "1234567890";

            // Act
            testEntry.MobileNo = expectedMobileNo;

            // Assert
            Assert.Equal(expectedMobileNo, testEntry.MobileNo);
        }

        [Fact]
        public void BillNo_ShouldSetAndGetValue()
        {
            // Arrange
            var testEntry = new TestEntry();
            string expectedBillNo = "BILL001";

            // Act
            testEntry.BillNo = expectedBillNo;

            // Assert
            Assert.Equal(expectedBillNo, testEntry.BillNo);
        }

        [Fact]
        public void TotalAmount_ShouldSetAndGetValue()
        {
            // Arrange
            var testEntry = new TestEntry();
            decimal expectedTotalAmount = 1000.50m;

            // Act
            testEntry.TotalAmount = expectedTotalAmount;

            // Assert
            Assert.Equal(expectedTotalAmount, testEntry.TotalAmount);
        }

        [Fact]
        public void DueDate_ShouldSetAndGetValue()
        {
            // Arrange
            var testEntry = new TestEntry();
            DateTime expectedDueDate = new DateTime(2024, 12, 31);

            // Act
            testEntry.DueDate = expectedDueDate;

            // Assert
            Assert.Equal(expectedDueDate, testEntry.DueDate);
        }

        [Fact]
        public void PaidAmount_ShouldSetAndGetValue()
        {
            // Arrange
            var testEntry = new TestEntry();
            decimal expectedPaidAmount = 500.25m;

            // Act
            testEntry.PaidAmount = expectedPaidAmount;

            // Assert
            Assert.Equal(expectedPaidAmount, testEntry.PaidAmount);
        }

        [Fact]
        public void TestId_ShouldSetAndGetValue()
        {
            // Arrange
            var testEntry = new TestEntry();
            int expectedTestId = 1;

            // Act
            testEntry.TestId = expectedTestId;

            // Assert
            Assert.Equal(expectedTestId, testEntry.TestId);
        }

        [Fact]
        public void TestName_ShouldSetAndGetValue()
        {
            // Arrange
            var testEntry = new TestEntry();
            string expectedTestName = "Blood Test";

            // Act
            testEntry.TestName = expectedTestName;

            // Assert
            Assert.Equal(expectedTestName, testEntry.TestName);
        }

        [Fact]
        public void TestFee_ShouldSetAndGetValue()
        {
            // Arrange
            var testEntry = new TestEntry();
            string expectedTestFee = "500";

            // Act
            testEntry.TestFee = expectedTestFee;

            // Assert
            Assert.Equal(expectedTestFee, testEntry.TestFee);
        }

        [Fact]
        public void Name_ShouldAcceptNullValue()
        {
            // Arrange
            var testEntry = new TestEntry();

            // Act
            testEntry.Name = null;

            // Assert
            Assert.Null(testEntry.Name);
        }

        [Fact]
        public void MobileNo_ShouldAcceptEmptyValue()
        {
            // Arrange
            var testEntry = new TestEntry();

            // Act
            testEntry.MobileNo = "";

            // Assert
            Assert.Equal("", testEntry.MobileNo);
        }

        [Fact]
        public void BillNo_ShouldAcceptNullValue()
        {
            // Arrange
            var testEntry = new TestEntry();

            // Act
            testEntry.BillNo = null;

            // Assert
            Assert.Null(testEntry.BillNo);
        }

        [Fact]
        public void TotalAmount_ShouldAcceptZeroValue()
        {
            // Arrange
            var testEntry = new TestEntry();

            // Act
            testEntry.TotalAmount = 0m;

            // Assert
            Assert.Equal(0m, testEntry.TotalAmount);
        }

        [Fact]
        public void PaidAmount_ShouldAcceptZeroValue()
        {
            // Arrange
            var testEntry = new TestEntry();

            // Act
            testEntry.PaidAmount = 0m;

            // Assert
            Assert.Equal(0m, testEntry.PaidAmount);
        }

        [Fact]
        public void TestId_ShouldAcceptZeroValue()
        {
            // Arrange
            var testEntry = new TestEntry();

            // Act
            testEntry.TestId = 0;

            // Assert
            Assert.Equal(0, testEntry.TestId);
        }

        [Fact]
        public void TestId_ShouldAcceptNegativeValue()
        {
            // Arrange
            var testEntry = new TestEntry();

            // Act
            testEntry.TestId = -1;

            // Assert
            Assert.Equal(-1, testEntry.TestId);
        }

        [Fact]
        public void TestName_ShouldAcceptNullValue()
        {
            // Arrange
            var testEntry = new TestEntry();

            // Act
            testEntry.TestName = null;

            // Assert
            Assert.Null(testEntry.TestName);
        }

        [Fact]
        public void TestFee_ShouldAcceptNullValue()
        {
            // Arrange
            var testEntry = new TestEntry();

            // Act
            testEntry.TestFee = null;

            // Assert
            Assert.Null(testEntry.TestFee);
        }

        [Fact]
        public void DOB_ShouldAcceptMinValue()
        {
            // Arrange
            var testEntry = new TestEntry();

            // Act
            testEntry.DOB = DateTime.MinValue;

            // Assert
            Assert.Equal(DateTime.MinValue, testEntry.DOB);
        }

        [Fact]
        public void DueDate_ShouldAcceptMaxValue()
        {
            // Arrange
            var testEntry = new TestEntry();

            // Act
            testEntry.DueDate = DateTime.MaxValue;

            // Assert
            Assert.Equal(DateTime.MaxValue, testEntry.DueDate);
        }

        [Fact]
        public void TotalAmount_ShouldAcceptLargeValue()
        {
            // Arrange
            var testEntry = new TestEntry();
            decimal largeAmount = 999999.99m;

            // Act
            testEntry.TotalAmount = largeAmount;

            // Assert
            Assert.Equal(largeAmount, testEntry.TotalAmount);
        }

        [Fact]
        public void PaidAmount_ShouldAcceptLargeValue()
        {
            // Arrange
            var testEntry = new TestEntry();
            decimal largeAmount = 999999.99m;

            // Act
            testEntry.PaidAmount = largeAmount;

            // Assert
            Assert.Equal(largeAmount, testEntry.PaidAmount);
        }
    }
}
