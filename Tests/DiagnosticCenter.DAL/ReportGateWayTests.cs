using System;
using System.Collections.Generic;
using Xunit;
using DiagnosticCenter.DAL;
using DiagnosticCenter.Models;

namespace Tests.DiagnosticCenter.DAL
{
    public class ReportGateWayTests
    {
        private readonly ReportGateWay _reportGateWay;

        public ReportGateWayTests()
        {
            _reportGateWay = new ReportGateWay();
        }

        [Fact]
        public void Constructor_ShouldCreateInstance()
        {
            // Arrange & Act
            var gateway = new ReportGateWay();

            // Assert
            Assert.NotNull(gateway);
        }

        [Fact]
        public void GetTestWiseReport_WithValidDateRange_ShouldReturnReportList()
        {
            // Arrange
            DateTime fromDate = new DateTime(2024, 1, 1);
            DateTime toDate = new DateTime(2024, 12, 31);

            // Act
            var result = _reportGateWay.GetTestWiseReport(fromDate, toDate);

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
            var result = _reportGateWay.GetTestWiseReport(fromDate, toDate);

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
            var result = _reportGateWay.GetTestWiseReport(fromDate, toDate);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Report>>(result);
        }

        [Fact]
        public void GetTestWiseReport_WithMinDateValue_ShouldReturnReportList()
        {
            // Arrange
            DateTime fromDate = DateTime.MinValue;
            DateTime toDate = DateTime.Now;

            // Act
            var result = _reportGateWay.GetTestWiseReport(fromDate, toDate);

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
            var result = _reportGateWay.GetTypeWiseReport(fromDate, toDate);

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
            var result = _reportGateWay.GetTypeWiseReport(fromDate, toDate);

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
            var result = _reportGateWay.GetTypeWiseReport(fromDate, toDate);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Report>>(result);
        }

        [Fact]
        public void GetTypeWiseReport_WithReverseDateRange_ShouldReturnReportList()
        {
            // Arrange
            DateTime fromDate = new DateTime(2024, 12, 31);
            DateTime toDate = new DateTime(2024, 1, 1);

            // Act
            var result = _reportGateWay.GetTypeWiseReport(fromDate, toDate);

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
            var result = _reportGateWay.GetUnpaidBillReport(fromDate, toDate);

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
            var result = _reportGateWay.GetUnpaidBillReport(fromDate, toDate);

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
            var result = _reportGateWay.GetUnpaidBillReport(fromDate, toDate);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Report>>(result);
        }

        [Fact]
        public void GetUnpaidBillReport_WithFutureDates_ShouldReturnReportList()
        {
            // Arrange
            DateTime fromDate = DateTime.Now.AddDays(1);
            DateTime toDate = DateTime.Now.AddDays(30);

            // Act
            var result = _reportGateWay.GetUnpaidBillReport(fromDate, toDate);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Report>>(result);
        }
    }
}
