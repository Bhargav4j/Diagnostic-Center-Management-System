using System;
using Xunit;
using DiagnosticCenter.Models;

namespace Tests.DiagnosticCenter.Models
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
        public void Name_SetAndGet_ShouldWork()
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
        public void DOB_SetAndGet_ShouldWork()
        {
            // Arrange
            var testEntry = new TestEntry();
            DateTime expectedDOB = new DateTime(1990, 1, 1);

            // Act
            testEntry.DOB = expectedDOB;

            // Assert
            Assert.Equal(expectedDOB, testEntry.DOB);
        }

        [Fact]
        public void MobileNo_SetAndGet_ShouldWork()
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
        public void BillNo_SetAndGet_ShouldWork()
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
        public void TotalAmount_SetAndGet_ShouldWork()
        {
            // Arrange
            var testEntry = new TestEntry();
            decimal expectedTotalAmount = 1000m;

            // Act
            testEntry.TotalAmount = expectedTotalAmount;

            // Assert
            Assert.Equal(expectedTotalAmount, testEntry.TotalAmount);
        }

        [Fact]
        public void DueDate_SetAndGet_ShouldWork()
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
        public void PaidAmount_SetAndGet_ShouldWork()
        {
            // Arrange
            var testEntry = new TestEntry();
            decimal expectedPaidAmount = 500m;

            // Act
            testEntry.PaidAmount = expectedPaidAmount;

            // Assert
            Assert.Equal(expectedPaidAmount, testEntry.PaidAmount);
        }

        [Fact]
        public void TestId_SetAndGet_ShouldWork()
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
        public void TestName_SetAndGet_ShouldWork()
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
        public void TestFee_SetAndGet_ShouldWork()
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
        public void AllProperties_SetAndGet_ShouldWork()
        {
            // Arrange
            var testEntry = new TestEntry();
            string name = "Jane Smith";
            DateTime dob = new DateTime(1985, 5, 15);
            string mobileNo = "9876543210";
            string billNo = "BILL002";
            decimal totalAmount = 2000m;
            DateTime dueDate = new DateTime(2024, 6, 30);
            decimal paidAmount = 1000m;
            int testId = 2;
            string testName = "X-Ray";
            string testFee = "1500";

            // Act
            testEntry.Name = name;
            testEntry.DOB = dob;
            testEntry.MobileNo = mobileNo;
            testEntry.BillNo = billNo;
            testEntry.TotalAmount = totalAmount;
            testEntry.DueDate = dueDate;
            testEntry.PaidAmount = paidAmount;
            testEntry.TestId = testId;
            testEntry.TestName = testName;
            testEntry.TestFee = testFee;

            // Assert
            Assert.Equal(name, testEntry.Name);
            Assert.Equal(dob, testEntry.DOB);
            Assert.Equal(mobileNo, testEntry.MobileNo);
            Assert.Equal(billNo, testEntry.BillNo);
            Assert.Equal(totalAmount, testEntry.TotalAmount);
            Assert.Equal(dueDate, testEntry.DueDate);
            Assert.Equal(paidAmount, testEntry.PaidAmount);
            Assert.Equal(testId, testEntry.TestId);
            Assert.Equal(testName, testEntry.TestName);
            Assert.Equal(testFee, testEntry.TestFee);
        }

        [Fact]
        public void TotalAmount_WithZeroValue_ShouldWork()
        {
            // Arrange
            var testEntry = new TestEntry();

            // Act
            testEntry.TotalAmount = 0m;

            // Assert
            Assert.Equal(0m, testEntry.TotalAmount);
        }

        [Fact]
        public void PaidAmount_WithNegativeValue_ShouldWork()
        {
            // Arrange
            var testEntry = new TestEntry();

            // Act
            testEntry.PaidAmount = -100m;

            // Assert
            Assert.Equal(-100m, testEntry.PaidAmount);
        }

        [Fact]
        public void TestId_WithZeroValue_ShouldWork()
        {
            // Arrange
            var testEntry = new TestEntry();

            // Act
            testEntry.TestId = 0;

            // Assert
            Assert.Equal(0, testEntry.TestId);
        }
    }
}
