using System;
using Xunit;
using DiagnosticCenter.Models;

namespace DiagnosticCenter.Models.Tests
{
    public class ReportTests
    {
        [Fact]
        public void Constructor_ShouldCreateInstance()
        {
            // Arrange & Act
            var report = new Report();

            // Assert
            Assert.NotNull(report);
        }

        [Fact]
        public void TestName_ShouldSetAndGetValue()
        {
            // Arrange
            var report = new Report();
            string expectedTestName = "Blood Test";

            // Act
            report.TestName = expectedTestName;

            // Assert
            Assert.Equal(expectedTestName, report.TestName);
        }

        [Fact]
        public void TotalFee_ShouldSetAndGetValue()
        {
            // Arrange
            var report = new Report();
            int expectedTotalFee = 5000;

            // Act
            report.TotalFee = expectedTotalFee;

            // Assert
            Assert.Equal(expectedTotalFee, report.TotalFee);
        }

        [Fact]
        public void TestCount_ShouldSetAndGetValue()
        {
            // Arrange
            var report = new Report();
            int expectedTestCount = 10;

            // Act
            report.TestCount = expectedTestCount;

            // Assert
            Assert.Equal(expectedTestCount, report.TestCount);
        }

        [Fact]
        public void TestTypeName_ShouldSetAndGetValue()
        {
            // Arrange
            var report = new Report();
            string expectedTestTypeName = "Radiology";

            // Act
            report.TestTypeName = expectedTestTypeName;

            // Assert
            Assert.Equal(expectedTestTypeName, report.TestTypeName);
        }

        [Fact]
        public void BillNo_ShouldSetAndGetValue()
        {
            // Arrange
            var report = new Report();
            string expectedBillNo = "BILL001";

            // Act
            report.BillNo = expectedBillNo;

            // Assert
            Assert.Equal(expectedBillNo, report.BillNo);
        }

        [Fact]
        public void MobileNo_ShouldSetAndGetValue()
        {
            // Arrange
            var report = new Report();
            string expectedMobileNo = "1234567890";

            // Act
            report.MobileNo = expectedMobileNo;

            // Assert
            Assert.Equal(expectedMobileNo, report.MobileNo);
        }

        [Fact]
        public void TotalAmount_ShouldSetAndGetValue()
        {
            // Arrange
            var report = new Report();
            decimal expectedTotalAmount = 1000.50m;

            // Act
            report.TotalAmount = expectedTotalAmount;

            // Assert
            Assert.Equal(expectedTotalAmount, report.TotalAmount);
        }

        [Fact]
        public void PaidAmount_ShouldSetAndGetValue()
        {
            // Arrange
            var report = new Report();
            decimal expectedPaidAmount = 500.25m;

            // Act
            report.PaidAmount = expectedPaidAmount;

            // Assert
            Assert.Equal(expectedPaidAmount, report.PaidAmount);
        }

        [Fact]
        public void PatientName_ShouldSetAndGetValue()
        {
            // Arrange
            var report = new Report();
            string expectedPatientName = "John Doe";

            // Act
            report.PatientName = expectedPatientName;

            // Assert
            Assert.Equal(expectedPatientName, report.PatientName);
        }

        [Fact]
        public void TestName_ShouldAcceptNullValue()
        {
            // Arrange
            var report = new Report();

            // Act
            report.TestName = null;

            // Assert
            Assert.Null(report.TestName);
        }

        [Fact]
        public void TestTypeName_ShouldAcceptEmptyValue()
        {
            // Arrange
            var report = new Report();

            // Act
            report.TestTypeName = "";

            // Assert
            Assert.Equal("", report.TestTypeName);
        }

        [Fact]
        public void TotalFee_ShouldAcceptZeroValue()
        {
            // Arrange
            var report = new Report();

            // Act
            report.TotalFee = 0;

            // Assert
            Assert.Equal(0, report.TotalFee);
        }

        [Fact]
        public void TotalFee_ShouldAcceptNegativeValue()
        {
            // Arrange
            var report = new Report();

            // Act
            report.TotalFee = -500;

            // Assert
            Assert.Equal(-500, report.TotalFee);
        }

        [Fact]
        public void TestCount_ShouldAcceptZeroValue()
        {
            // Arrange
            var report = new Report();

            // Act
            report.TestCount = 0;

            // Assert
            Assert.Equal(0, report.TestCount);
        }

        [Fact]
        public void TestCount_ShouldAcceptLargeValue()
        {
            // Arrange
            var report = new Report();

            // Act
            report.TestCount = 999999;

            // Assert
            Assert.Equal(999999, report.TestCount);
        }

        [Fact]
        public void TotalAmount_ShouldAcceptZeroValue()
        {
            // Arrange
            var report = new Report();

            // Act
            report.TotalAmount = 0m;

            // Assert
            Assert.Equal(0m, report.TotalAmount);
        }

        [Fact]
        public void PaidAmount_ShouldAcceptZeroValue()
        {
            // Arrange
            var report = new Report();

            // Act
            report.PaidAmount = 0m;

            // Assert
            Assert.Equal(0m, report.PaidAmount);
        }

        [Fact]
        public void BillNo_ShouldAcceptNullValue()
        {
            // Arrange
            var report = new Report();

            // Act
            report.BillNo = null;

            // Assert
            Assert.Null(report.BillNo);
        }

        [Fact]
        public void MobileNo_ShouldAcceptNullValue()
        {
            // Arrange
            var report = new Report();

            // Act
            report.MobileNo = null;

            // Assert
            Assert.Null(report.MobileNo);
        }

        [Fact]
        public void PatientName_ShouldAcceptNullValue()
        {
            // Arrange
            var report = new Report();

            // Act
            report.PatientName = null;

            // Assert
            Assert.Null(report.PatientName);
        }
    }
}
