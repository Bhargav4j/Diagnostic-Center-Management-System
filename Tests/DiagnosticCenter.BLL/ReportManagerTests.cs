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
        public void Constructor_CreatesInstance()
        {
            // Arrange & Act
            var reportManager = new ReportManager();

            // Assert
            Assert.NotNull(reportManager);
        }

        [Fact]
        public void GetTestWiseReport_WithValidDates_ReturnsReportList()
        {
            // Arrange
            var reportManager = new ReportManager();
            var fromDate = new DateTime(2024, 1, 1);
            var toDate = new DateTime(2024, 12, 31);

            // Act
            var result = reportManager.GetTestWiseReport(fromDate, toDate);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Report>>(result);
        }

        [Fact]
        public void GetTestWiseReport_WithSameDates_ReturnsReportList()
        {
            // Arrange
            var reportManager = new ReportManager();
            var fromDate = new DateTime(2024, 6, 15);
            var toDate = new DateTime(2024, 6, 15);

            // Act
            var result = reportManager.GetTestWiseReport(fromDate, toDate);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void GetTestWiseReport_WithFromDateAfterToDate_ReturnsReportList()
        {
            // Arrange
            var reportManager = new ReportManager();
            var fromDate = new DateTime(2024, 12, 31);
            var toDate = new DateTime(2024, 1, 1);

            // Act
            var result = reportManager.GetTestWiseReport(fromDate, toDate);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void GetTypeWiseReport_WithValidDates_ReturnsReportList()
        {
            // Arrange
            var reportManager = new ReportManager();
            var fromDate = new DateTime(2024, 1, 1);
            var toDate = new DateTime(2024, 12, 31);

            // Act
            var result = reportManager.GetTypeWiseReport(fromDate, toDate);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Report>>(result);
        }

        [Fact]
        public void GetTypeWiseReport_WithSameDates_ReturnsReportList()
        {
            // Arrange
            var reportManager = new ReportManager();
            var fromDate = new DateTime(2024, 6, 15);
            var toDate = new DateTime(2024, 6, 15);

            // Act
            var result = reportManager.GetTypeWiseReport(fromDate, toDate);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void GetTypeWiseReport_WithFromDateAfterToDate_ReturnsReportList()
        {
            // Arrange
            var reportManager = new ReportManager();
            var fromDate = new DateTime(2024, 12, 31);
            var toDate = new DateTime(2024, 1, 1);

            // Act
            var result = reportManager.GetTypeWiseReport(fromDate, toDate);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void GetUnpaidBillReport_WithValidDates_ReturnsReportList()
        {
            // Arrange
            var reportManager = new ReportManager();
            var fromDate = new DateTime(2024, 1, 1);
            var toDate = new DateTime(2024, 12, 31);

            // Act
            var result = reportManager.GetUnpaidBillReport(fromDate, toDate);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Report>>(result);
        }

        [Fact]
        public void GetUnpaidBillReport_WithSameDates_ReturnsReportList()
        {
            // Arrange
            var reportManager = new ReportManager();
            var fromDate = new DateTime(2024, 6, 15);
            var toDate = new DateTime(2024, 6, 15);

            // Act
            var result = reportManager.GetUnpaidBillReport(fromDate, toDate);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void GetUnpaidBillReport_WithFromDateAfterToDate_ReturnsReportList()
        {
            // Arrange
            var reportManager = new ReportManager();
            var fromDate = new DateTime(2024, 12, 31);
            var toDate = new DateTime(2024, 1, 1);

            // Act
            var result = reportManager.GetUnpaidBillReport(fromDate, toDate);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void GetTestWiseReport_WithMinDate_ReturnsReportList()
        {
            // Arrange
            var reportManager = new ReportManager();
            var fromDate = DateTime.MinValue;
            var toDate = DateTime.Now;

            // Act
            var result = reportManager.GetTestWiseReport(fromDate, toDate);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void GetTypeWiseReport_WithMaxDate_ReturnsReportList()
        {
            // Arrange
            var reportManager = new ReportManager();
            var fromDate = DateTime.Now;
            var toDate = DateTime.MaxValue;

            // Act
            var result = reportManager.GetTypeWiseReport(fromDate, toDate);

            // Assert
            Assert.NotNull(result);
        }
    }
}
