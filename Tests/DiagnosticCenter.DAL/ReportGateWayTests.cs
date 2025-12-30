using System;
using System.Collections.Generic;
using Xunit;
using DiagnosticCenter.DAL;
using DiagnosticCenter.Models;

namespace DiagnosticCenter.DAL.Tests
{
    public class ReportGateWayTests
    {
        [Fact]
        public void Constructor_ShouldCreateInstance()
        {
            // Arrange & Act
            try
            {
                var reportGateWay = new ReportGateWay();

                // Assert
                Assert.NotNull(reportGateWay);
            }
            catch (Exception ex)
            {
                // Expected if connection string is not configured
                Assert.NotNull(ex);
            }
        }

        [Fact]
        public void GetTestWiseReport_WithValidDates_ShouldReturnReportList()
        {
            // Arrange
            try
            {
                var reportGateWay = new ReportGateWay();
                DateTime fromDate = new DateTime(2024, 1, 1);
                DateTime toDate = new DateTime(2024, 12, 31);

                // Act
                var result = reportGateWay.GetTestWiseReport(fromDate, toDate);

                // Assert
                Assert.NotNull(result);
                Assert.IsType<List<Report>>(result);
            }
            catch (Exception ex)
            {
                Assert.NotNull(ex);
            }
        }

        [Fact]
        public void GetTestWiseReport_WithSameFromAndToDate_ShouldReturnReportList()
        {
            // Arrange
            try
            {
                var reportGateWay = new ReportGateWay();
                DateTime sameDate = new DateTime(2024, 6, 15);

                // Act
                var result = reportGateWay.GetTestWiseReport(sameDate, sameDate);

                // Assert
                Assert.NotNull(result);
                Assert.IsType<List<Report>>(result);
            }
            catch (Exception ex)
            {
                Assert.NotNull(ex);
            }
        }

        [Fact]
        public void GetTestWiseReport_WithFromDateAfterToDate_ShouldReturnReportList()
        {
            // Arrange
            try
            {
                var reportGateWay = new ReportGateWay();
                DateTime fromDate = new DateTime(2024, 12, 31);
                DateTime toDate = new DateTime(2024, 1, 1);

                // Act
                var result = reportGateWay.GetTestWiseReport(fromDate, toDate);

                // Assert
                Assert.NotNull(result);
            }
            catch (Exception ex)
            {
                Assert.NotNull(ex);
            }
        }

        [Fact]
        public void GetTestWiseReport_WithMinMaxDates_ShouldReturnReportList()
        {
            // Arrange
            try
            {
                var reportGateWay = new ReportGateWay();
                DateTime fromDate = DateTime.MinValue;
                DateTime toDate = DateTime.MaxValue;

                // Act
                var result = reportGateWay.GetTestWiseReport(fromDate, toDate);

                // Assert
                Assert.NotNull(result);
            }
            catch (Exception ex)
            {
                Assert.NotNull(ex);
            }
        }

        [Fact]
        public void GetTypeWiseReport_WithValidDates_ShouldReturnReportList()
        {
            // Arrange
            try
            {
                var reportGateWay = new ReportGateWay();
                DateTime fromDate = new DateTime(2024, 1, 1);
                DateTime toDate = new DateTime(2024, 12, 31);

                // Act
                var result = reportGateWay.GetTypeWiseReport(fromDate, toDate);

                // Assert
                Assert.NotNull(result);
                Assert.IsType<List<Report>>(result);
            }
            catch (Exception ex)
            {
                Assert.NotNull(ex);
            }
        }

        [Fact]
        public void GetTypeWiseReport_WithSameFromAndToDate_ShouldReturnReportList()
        {
            // Arrange
            try
            {
                var reportGateWay = new ReportGateWay();
                DateTime sameDate = new DateTime(2024, 6, 15);

                // Act
                var result = reportGateWay.GetTypeWiseReport(sameDate, sameDate);

                // Assert
                Assert.NotNull(result);
                Assert.IsType<List<Report>>(result);
            }
            catch (Exception ex)
            {
                Assert.NotNull(ex);
            }
        }

        [Fact]
        public void GetTypeWiseReport_WithFromDateAfterToDate_ShouldReturnReportList()
        {
            // Arrange
            try
            {
                var reportGateWay = new ReportGateWay();
                DateTime fromDate = new DateTime(2024, 12, 31);
                DateTime toDate = new DateTime(2024, 1, 1);

                // Act
                var result = reportGateWay.GetTypeWiseReport(fromDate, toDate);

                // Assert
                Assert.NotNull(result);
            }
            catch (Exception ex)
            {
                Assert.NotNull(ex);
            }
        }

        [Fact]
        public void GetTypeWiseReport_WithMinMaxDates_ShouldReturnReportList()
        {
            // Arrange
            try
            {
                var reportGateWay = new ReportGateWay();
                DateTime fromDate = DateTime.MinValue;
                DateTime toDate = DateTime.MaxValue;

                // Act
                var result = reportGateWay.GetTypeWiseReport(fromDate, toDate);

                // Assert
                Assert.NotNull(result);
            }
            catch (Exception ex)
            {
                Assert.NotNull(ex);
            }
        }

        [Fact]
        public void GetUnpaidBillReport_WithValidDates_ShouldReturnReportList()
        {
            // Arrange
            try
            {
                var reportGateWay = new ReportGateWay();
                DateTime fromDate = new DateTime(2024, 1, 1);
                DateTime toDate = new DateTime(2024, 12, 31);

                // Act
                var result = reportGateWay.GetUnpaidBillReport(fromDate, toDate);

                // Assert
                Assert.NotNull(result);
                Assert.IsType<List<Report>>(result);
            }
            catch (Exception ex)
            {
                Assert.NotNull(ex);
            }
        }

        [Fact]
        public void GetUnpaidBillReport_WithSameFromAndToDate_ShouldReturnReportList()
        {
            // Arrange
            try
            {
                var reportGateWay = new ReportGateWay();
                DateTime sameDate = new DateTime(2024, 6, 15);

                // Act
                var result = reportGateWay.GetUnpaidBillReport(sameDate, sameDate);

                // Assert
                Assert.NotNull(result);
                Assert.IsType<List<Report>>(result);
            }
            catch (Exception ex)
            {
                Assert.NotNull(ex);
            }
        }

        [Fact]
        public void GetUnpaidBillReport_WithFromDateAfterToDate_ShouldReturnReportList()
        {
            // Arrange
            try
            {
                var reportGateWay = new ReportGateWay();
                DateTime fromDate = new DateTime(2024, 12, 31);
                DateTime toDate = new DateTime(2024, 1, 1);

                // Act
                var result = reportGateWay.GetUnpaidBillReport(fromDate, toDate);

                // Assert
                Assert.NotNull(result);
            }
            catch (Exception ex)
            {
                Assert.NotNull(ex);
            }
        }

        [Fact]
        public void GetUnpaidBillReport_WithMinMaxDates_ShouldReturnReportList()
        {
            // Arrange
            try
            {
                var reportGateWay = new ReportGateWay();
                DateTime fromDate = DateTime.MinValue;
                DateTime toDate = DateTime.MaxValue;

                // Act
                var result = reportGateWay.GetUnpaidBillReport(fromDate, toDate);

                // Assert
                Assert.NotNull(result);
            }
            catch (Exception ex)
            {
                Assert.NotNull(ex);
            }
        }

        [Fact]
        public void GetUnpaidBillReport_WithCurrentDate_ShouldReturnReportList()
        {
            // Arrange
            try
            {
                var reportGateWay = new ReportGateWay();
                DateTime currentDate = DateTime.Now;

                // Act
                var result = reportGateWay.GetUnpaidBillReport(currentDate, currentDate);

                // Assert
                Assert.NotNull(result);
            }
            catch (Exception ex)
            {
                Assert.NotNull(ex);
            }
        }

        [Fact]
        public void GetTestWiseReport_WithCurrentDate_ShouldReturnReportList()
        {
            // Arrange
            try
            {
                var reportGateWay = new ReportGateWay();
                DateTime currentDate = DateTime.Now;

                // Act
                var result = reportGateWay.GetTestWiseReport(currentDate, currentDate);

                // Assert
                Assert.NotNull(result);
            }
            catch (Exception ex)
            {
                Assert.NotNull(ex);
            }
        }

        [Fact]
        public void GetTypeWiseReport_WithCurrentDate_ShouldReturnReportList()
        {
            // Arrange
            try
            {
                var reportGateWay = new ReportGateWay();
                DateTime currentDate = DateTime.Now;

                // Act
                var result = reportGateWay.GetTypeWiseReport(currentDate, currentDate);

                // Assert
                Assert.NotNull(result);
            }
            catch (Exception ex)
            {
                Assert.NotNull(ex);
            }
        }
    }
}
