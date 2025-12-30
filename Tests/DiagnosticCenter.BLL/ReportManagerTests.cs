using System;
using System.Collections.Generic;
using Xunit;
using DiagnosticCenter.BLL;
using DiagnosticCenter.Models;

namespace DiagnosticCenter.BLL.Tests
{
    public class ReportManagerTests
    {
        [Fact]
        public void Constructor_ShouldCreateInstance()
        {
            // Arrange & Act
            var reportManager = new ReportManager();

            // Assert
            Assert.NotNull(reportManager);
        }

        [Fact]
        public void GetTestWiseReport_WithValidDates_ShouldReturnReportList()
        {
            // Arrange
            var reportManager = new ReportManager();
            DateTime fromDate = new DateTime(2024, 1, 1);
            DateTime toDate = new DateTime(2024, 12, 31);

            // Act
            var result = reportManager.GetTestWiseReport(fromDate, toDate);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Report>>(result);
        }

        [Fact]
        public void GetTestWiseReport_WithSameFromAndToDate_ShouldReturnReportList()
        {
            // Arrange
            var reportManager = new ReportManager();
            DateTime sameDate = new DateTime(2024, 6, 15);

            // Act
            var result = reportManager.GetTestWiseReport(sameDate, sameDate);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Report>>(result);
        }

        [Fact]
        public void GetTestWiseReport_WithFromDateAfterToDate_ShouldReturnReportList()
        {
            // Arrange
            var reportManager = new ReportManager();
            DateTime fromDate = new DateTime(2024, 12, 31);
            DateTime toDate = new DateTime(2024, 1, 1);

            // Act
            var result = reportManager.GetTestWiseReport(fromDate, toDate);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void GetTestWiseReport_WithMinMaxDates_ShouldReturnReportList()
        {
            // Arrange
            var reportManager = new ReportManager();
            DateTime fromDate = DateTime.MinValue;
            DateTime toDate = DateTime.MaxValue;

            // Act
            var result = reportManager.GetTestWiseReport(fromDate, toDate);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void GetTypeWiseReport_WithValidDates_ShouldReturnReportList()
        {
            // Arrange
            var reportManager = new ReportManager();
            DateTime fromDate = new DateTime(2024, 1, 1);
            DateTime toDate = new DateTime(2024, 12, 31);

            // Act
            var result = reportManager.GetTypeWiseReport(fromDate, toDate);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Report>>(result);
        }

        [Fact]
        public void GetTypeWiseReport_WithSameFromAndToDate_ShouldReturnReportList()
        {
            // Arrange
            var reportManager = new ReportManager();
            DateTime sameDate = new DateTime(2024, 6, 15);

            // Act
            var result = reportManager.GetTypeWiseReport(sameDate, sameDate);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Report>>(result);
        }

        [Fact]
        public void GetTypeWiseReport_WithFromDateAfterToDate_ShouldReturnReportList()
        {
            // Arrange
            var reportManager = new ReportManager();
            DateTime fromDate = new DateTime(2024, 12, 31);
            DateTime toDate = new DateTime(2024, 1, 1);

            // Act
            var result = reportManager.GetTypeWiseReport(fromDate, toDate);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void GetTypeWiseReport_WithMinMaxDates_ShouldReturnReportList()
        {
            // Arrange
            var reportManager = new ReportManager();
            DateTime fromDate = DateTime.MinValue;
            DateTime toDate = DateTime.MaxValue;

            // Act
            var result = reportManager.GetTypeWiseReport(fromDate, toDate);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void GetUnpaidBillReport_WithValidDates_ShouldReturnReportList()
        {
            // Arrange
            var reportManager = new ReportManager();
            DateTime fromDate = new DateTime(2024, 1, 1);
            DateTime toDate = new DateTime(2024, 12, 31);

            // Act
            var result = reportManager.GetUnpaidBillReport(fromDate, toDate);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Report>>(result);
        }

        [Fact]
        public void GetUnpaidBillReport_WithSameFromAndToDate_ShouldReturnReportList()
        {
            // Arrange
            var reportManager = new ReportManager();
            DateTime sameDate = new DateTime(2024, 6, 15);

            // Act
            var result = reportManager.GetUnpaidBillReport(sameDate, sameDate);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Report>>(result);
        }

        [Fact]
        public void GetUnpaidBillReport_WithFromDateAfterToDate_ShouldReturnReportList()
        {
            // Arrange
            var reportManager = new ReportManager();
            DateTime fromDate = new DateTime(2024, 12, 31);
            DateTime toDate = new DateTime(2024, 1, 1);

            // Act
            var result = reportManager.GetUnpaidBillReport(fromDate, toDate);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void GetUnpaidBillReport_WithMinMaxDates_ShouldReturnReportList()
        {
            // Arrange
            var reportManager = new ReportManager();
            DateTime fromDate = DateTime.MinValue;
            DateTime toDate = DateTime.MaxValue;

            // Act
            var result = reportManager.GetUnpaidBillReport(fromDate, toDate);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void GetUnpaidBillReport_WithCurrentDate_ShouldReturnReportList()
        {
            // Arrange
            var reportManager = new ReportManager();
            DateTime currentDate = DateTime.Now;

            // Act
            var result = reportManager.GetUnpaidBillReport(currentDate, currentDate);

            // Assert
            Assert.NotNull(result);
        }
    }
}
