using System;
using System.Collections.Generic;
using Xunit;
using DiagnosticCenter.BLL;
using DiagnosticCenter.Models;

namespace Tests.DiagnosticCenter.BLL
{
    public class ReportManagerTests
    {
        private readonly ReportManager _reportManager;

        public ReportManagerTests()
        {
            _reportManager = new ReportManager();
        }

        [Fact]
        public void Constructor_ShouldCreateInstance()
        {
            // Arrange & Act
            var manager = new ReportManager();

            // Assert
            Assert.NotNull(manager);
        }

        [Fact]
        public void GetTestWiseReport_WithValidDateRange_ShouldReturnReportList()
        {
            // Arrange
            DateTime fromDate = new DateTime(2024, 1, 1);
            DateTime toDate = new DateTime(2024, 12, 31);

            // Act
            var result = _reportManager.GetTestWiseReport(fromDate, toDate);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Report>>(result);
        }

        [Fact]
        public void GetTestWiseReport_WithSameDateRange_ShouldReturnReportList()
        {
            // Arrange
            DateTime fromDate = new DateTime(2024, 6, 1);
            DateTime toDate = new DateTime(2024, 6, 1);

            // Act
            var result = _reportManager.GetTestWiseReport(fromDate, toDate);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Report>>(result);
        }

        [Fact]
        public void GetTestWiseReport_WithReverseDateRange_ShouldReturnReportList()
        {
            // Arrange
            DateTime fromDate = new DateTime(2024, 12, 31);
            DateTime toDate = new DateTime(2024, 1, 1);

            // Act
            var result = _reportManager.GetTestWiseReport(fromDate, toDate);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Report>>(result);
        }

        [Fact]
        public void GetTypeWiseReport_WithValidDateRange_ShouldReturnReportList()
        {
            // Arrange
            DateTime fromDate = new DateTime(2024, 1, 1);
            DateTime toDate = new DateTime(2024, 12, 31);

            // Act
            var result = _reportManager.GetTypeWiseReport(fromDate, toDate);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Report>>(result);
        }

        [Fact]
        public void GetTypeWiseReport_WithSameDateRange_ShouldReturnReportList()
        {
            // Arrange
            DateTime fromDate = new DateTime(2024, 6, 1);
            DateTime toDate = new DateTime(2024, 6, 1);

            // Act
            var result = _reportManager.GetTypeWiseReport(fromDate, toDate);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Report>>(result);
        }

        [Fact]
        public void GetTypeWiseReport_WithFutureDates_ShouldReturnReportList()
        {
            // Arrange
            DateTime fromDate = new DateTime(2025, 1, 1);
            DateTime toDate = new DateTime(2025, 12, 31);

            // Act
            var result = _reportManager.GetTypeWiseReport(fromDate, toDate);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Report>>(result);
        }

        [Fact]
        public void GetUnpaidBillReport_WithValidDateRange_ShouldReturnReportList()
        {
            // Arrange
            DateTime fromDate = new DateTime(2024, 1, 1);
            DateTime toDate = new DateTime(2024, 12, 31);

            // Act
            var result = _reportManager.GetUnpaidBillReport(fromDate, toDate);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Report>>(result);
        }

        [Fact]
        public void GetUnpaidBillReport_WithSameDateRange_ShouldReturnReportList()
        {
            // Arrange
            DateTime fromDate = new DateTime(2024, 6, 1);
            DateTime toDate = new DateTime(2024, 6, 1);

            // Act
            var result = _reportManager.GetUnpaidBillReport(fromDate, toDate);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Report>>(result);
        }

        [Fact]
        public void GetUnpaidBillReport_WithMinDateValue_ShouldReturnReportList()
        {
            // Arrange
            DateTime fromDate = DateTime.MinValue;
            DateTime toDate = DateTime.Now;

            // Act
            var result = _reportManager.GetUnpaidBillReport(fromDate, toDate);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Report>>(result);
        }
    }
}
