using System;
using Xunit;
using DiagnosticCenter.Models;

namespace Tests.DiagnosticCenter.Models
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
        public void TestName_SetAndGet_ShouldWork()
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
        public void TotalFee_SetAndGet_ShouldWork()
        {
            // Arrange
            var report = new Report();
            int expectedTotalFee = 1000;

            // Act
            report.TotalFee = expectedTotalFee;

            // Assert
            Assert.Equal(expectedTotalFee, report.TotalFee);
        }

        [Fact]
        public void TestCount_SetAndGet_ShouldWork()
        {
            // Arrange
            var report = new Report();
            int expectedTestCount = 5;

            // Act
            report.TestCount = expectedTestCount;

            // Assert
            Assert.Equal(expectedTestCount, report.TestCount);
        }

        [Fact]
        public void TestTypeName_SetAndGet_ShouldWork()
        {
            // Arrange
            var report = new Report();
            string expectedTestTypeName = "Lab Test";

            // Act
            report.TestTypeName = expectedTestTypeName;

            // Assert
            Assert.Equal(expectedTestTypeName, report.TestTypeName);
        }

        [Fact]
        public void BillNo_SetAndGet_ShouldWork()
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
        public void MobileNo_SetAndGet_ShouldWork()
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
        public void TotalAmount_SetAndGet_ShouldWork()
        {
            // Arrange
            var report = new Report();
            decimal expectedTotalAmount = 1500m;

            // Act
            report.TotalAmount = expectedTotalAmount;

            // Assert
            Assert.Equal(expectedTotalAmount, report.TotalAmount);
        }

        [Fact]
        public void PaidAmount_SetAndGet_ShouldWork()
        {
            // Arrange
            var report = new Report();
            decimal expectedPaidAmount = 750m;

            // Act
            report.PaidAmount = expectedPaidAmount;

            // Assert
            Assert.Equal(expectedPaidAmount, report.PaidAmount);
        }

        [Fact]
        public void PatientName_SetAndGet_ShouldWork()
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
        public void AllProperties_SetAndGet_ShouldWork()
        {
            // Arrange
            var report = new Report();
            string testName = "X-Ray";
            int totalFee = 2000;
            int testCount = 10;
            string testTypeName = "Imaging";
            string billNo = "BILL002";
            string mobileNo = "9876543210";
            decimal totalAmount = 5000m;
            decimal paidAmount = 2500m;
            string patientName = "Jane Smith";

            // Act
            report.TestName = testName;
            report.TotalFee = totalFee;
            report.TestCount = testCount;
            report.TestTypeName = testTypeName;
            report.BillNo = billNo;
            report.MobileNo = mobileNo;
            report.TotalAmount = totalAmount;
            report.PaidAmount = paidAmount;
            report.PatientName = patientName;

            // Assert
            Assert.Equal(testName, report.TestName);
            Assert.Equal(totalFee, report.TotalFee);
            Assert.Equal(testCount, report.TestCount);
            Assert.Equal(testTypeName, report.TestTypeName);
            Assert.Equal(billNo, report.BillNo);
            Assert.Equal(mobileNo, report.MobileNo);
            Assert.Equal(totalAmount, report.TotalAmount);
            Assert.Equal(paidAmount, report.PaidAmount);
            Assert.Equal(patientName, report.PatientName);
        }

        [Fact]
        public void TotalFee_WithZeroValue_ShouldWork()
        {
            // Arrange
            var report = new Report();

            // Act
            report.TotalFee = 0;

            // Assert
            Assert.Equal(0, report.TotalFee);
        }

        [Fact]
        public void TestCount_WithNegativeValue_ShouldWork()
        {
            // Arrange
            var report = new Report();

            // Act
            report.TestCount = -1;

            // Assert
            Assert.Equal(-1, report.TestCount);
        }
    }
}
