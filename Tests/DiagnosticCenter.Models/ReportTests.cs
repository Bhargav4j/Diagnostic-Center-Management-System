using System;
using Xunit;
using DiagnosticCenter.Models;

namespace DiagnosticCenter.Models.Tests
{
    public class ReportTests
    {
        [Fact]
        public void Constructor_CreatesInstance()
        {
            // Arrange & Act
            var report = new Report();

            // Assert
            Assert.NotNull(report);
        }

        [Fact]
        public void TestName_CanBeSetAndRetrieved()
        {
            // Arrange
            var report = new Report();
            var testName = "Blood Test";

            // Act
            report.TestName = testName;

            // Assert
            Assert.Equal(testName, report.TestName);
        }

        [Fact]
        public void TotalFee_CanBeSetAndRetrieved()
        {
            // Arrange
            var report = new Report();
            var totalFee = 1000;

            // Act
            report.TotalFee = totalFee;

            // Assert
            Assert.Equal(totalFee, report.TotalFee);
        }

        [Fact]
        public void TestCount_CanBeSetAndRetrieved()
        {
            // Arrange
            var report = new Report();
            var testCount = 5;

            // Act
            report.TestCount = testCount;

            // Assert
            Assert.Equal(testCount, report.TestCount);
        }

        [Fact]
        public void TestTypeName_CanBeSetAndRetrieved()
        {
            // Arrange
            var report = new Report();
            var testTypeName = "Laboratory";

            // Act
            report.TestTypeName = testTypeName;

            // Assert
            Assert.Equal(testTypeName, report.TestTypeName);
        }

        [Fact]
        public void BillNo_CanBeSetAndRetrieved()
        {
            // Arrange
            var report = new Report();
            var billNo = "BILL001";

            // Act
            report.BillNo = billNo;

            // Assert
            Assert.Equal(billNo, report.BillNo);
        }

        [Fact]
        public void MobileNo_CanBeSetAndRetrieved()
        {
            // Arrange
            var report = new Report();
            var mobileNo = "1234567890";

            // Act
            report.MobileNo = mobileNo;

            // Assert
            Assert.Equal(mobileNo, report.MobileNo);
        }

        [Fact]
        public void TotalAmount_CanBeSetAndRetrieved()
        {
            // Arrange
            var report = new Report();
            var totalAmount = 1500.50m;

            // Act
            report.TotalAmount = totalAmount;

            // Assert
            Assert.Equal(totalAmount, report.TotalAmount);
        }

        [Fact]
        public void PaidAmount_CanBeSetAndRetrieved()
        {
            // Arrange
            var report = new Report();
            var paidAmount = 750.25m;

            // Act
            report.PaidAmount = paidAmount;

            // Assert
            Assert.Equal(paidAmount, report.PaidAmount);
        }

        [Fact]
        public void PatientName_CanBeSetAndRetrieved()
        {
            // Arrange
            var report = new Report();
            var patientName = "John Doe";

            // Act
            report.PatientName = patientName;

            // Assert
            Assert.Equal(patientName, report.PatientName);
        }

        [Fact]
        public void AllProperties_CanBeSetTogether()
        {
            // Arrange
            var report = new Report
            {
                TestName = "X-Ray",
                TotalFee = 2000,
                TestCount = 3,
                TestTypeName = "Radiology",
                BillNo = "BILL002",
                MobileNo = "9876543210",
                TotalAmount = 6000m,
                PaidAmount = 3000m,
                PatientName = "Jane Smith"
            };

            // Assert
            Assert.Equal("X-Ray", report.TestName);
            Assert.Equal(2000, report.TotalFee);
            Assert.Equal(3, report.TestCount);
            Assert.Equal("Radiology", report.TestTypeName);
            Assert.Equal("BILL002", report.BillNo);
            Assert.Equal("9876543210", report.MobileNo);
            Assert.Equal(6000m, report.TotalAmount);
            Assert.Equal(3000m, report.PaidAmount);
            Assert.Equal("Jane Smith", report.PatientName);
        }

        [Fact]
        public void TotalFee_WithZero_CanBeSet()
        {
            // Arrange
            var report = new Report();
            var totalFee = 0;

            // Act
            report.TotalFee = totalFee;

            // Assert
            Assert.Equal(0, report.TotalFee);
        }

        [Fact]
        public void TestCount_WithNegative_CanBeSet()
        {
            // Arrange
            var report = new Report();
            var testCount = -5;

            // Act
            report.TestCount = testCount;

            // Assert
            Assert.Equal(-5, report.TestCount);
        }

        [Fact]
        public void TotalAmount_WithZero_CanBeSet()
        {
            // Arrange
            var report = new Report();
            var totalAmount = 0m;

            // Act
            report.TotalAmount = totalAmount;

            // Assert
            Assert.Equal(0m, report.TotalAmount);
        }

        [Fact]
        public void PaidAmount_WithNegative_CanBeSet()
        {
            // Arrange
            var report = new Report();
            var paidAmount = -100m;

            // Act
            report.PaidAmount = paidAmount;

            // Assert
            Assert.Equal(-100m, report.PaidAmount);
        }
    }
}
