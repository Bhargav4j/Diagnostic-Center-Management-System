using System;
using System.Collections.Generic;
using Xunit;
using DiagnosticCenter.DAL;
using DiagnosticCenter.Models;

namespace Tests.DiagnosticCenter.DAL
{
    public class TestEntryGateWayTests
    {
        private readonly TestEntryGateWay _testEntryGateWay;

        public TestEntryGateWayTests()
        {
            _testEntryGateWay = new TestEntryGateWay();
        }

        [Fact]
        public void Constructor_ShouldCreateInstance()
        {
            // Arrange & Act
            var gateway = new TestEntryGateWay();

            // Assert
            Assert.NotNull(gateway);
        }

        [Fact]
        public void GetAllTest_ShouldReturnTestEntryList()
        {
            // Act
            var result = _testEntryGateWay.GetAllTest();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<TestEntry>>(result);
        }

        [Fact]
        public void GetTestFee_WithValidTestValue_ShouldReturnString()
        {
            // Arrange
            int testValue = 1;

            // Act
            var result = _testEntryGateWay.GetTestFee(testValue);

            // Assert
            Assert.IsType<string>(result);
        }

        [Fact]
        public void GetTestFee_WithZeroTestValue_ShouldReturnString()
        {
            // Arrange
            int testValue = 0;

            // Act
            var result = _testEntryGateWay.GetTestFee(testValue);

            // Assert
            Assert.IsType<string>(result);
        }

        [Fact]
        public void GetTestFee_WithNegativeTestValue_ShouldReturnString()
        {
            // Arrange
            int testValue = -1;

            // Act
            var result = _testEntryGateWay.GetTestFee(testValue);

            // Assert
            Assert.IsType<string>(result);
        }

        [Fact]
        public void SavePatient_WithValidTestEntry_ShouldReturnInteger()
        {
            // Arrange
            var testEntry = new TestEntry
            {
                Name = "Test Patient",
                DOB = new DateTime(1990, 1, 1),
                MobileNo = "1234567890",
                BillNo = "BILL" + Guid.NewGuid().ToString().Substring(0, 8),
                TotalAmount = 1000m,
                DueDate = DateTime.Now.AddDays(7),
                PaidAmount = 0m
            };

            // Act
            var result = _testEntryGateWay.SavePatient(testEntry);

            // Assert
            Assert.IsType<int>(result);
        }

        [Fact]
        public void SavePatient_WithNullTestEntry_ShouldThrowException()
        {
            // Arrange
            TestEntry testEntry = null;

            // Act & Assert
            Assert.Throws<NullReferenceException>(() => _testEntryGateWay.SavePatient(testEntry));
        }

        [Fact]
        public void SavePatient_WithEmptyName_ShouldReturnInteger()
        {
            // Arrange
            var testEntry = new TestEntry
            {
                Name = "",
                DOB = new DateTime(1990, 1, 1),
                MobileNo = "9876543210",
                BillNo = "BILL" + Guid.NewGuid().ToString().Substring(0, 8),
                TotalAmount = 500m,
                DueDate = DateTime.Now,
                PaidAmount = 0m
            };

            // Act
            var result = _testEntryGateWay.SavePatient(testEntry);

            // Assert
            Assert.IsType<int>(result);
        }

        [Fact]
        public void SavePatient_WithNegativeAmounts_ShouldReturnInteger()
        {
            // Arrange
            var testEntry = new TestEntry
            {
                Name = "Patient",
                DOB = new DateTime(1985, 5, 15),
                MobileNo = "5555555555",
                BillNo = "BILL" + Guid.NewGuid().ToString().Substring(0, 8),
                TotalAmount = -100m,
                DueDate = DateTime.Now,
                PaidAmount = -50m
            };

            // Act
            var result = _testEntryGateWay.SavePatient(testEntry);

            // Assert
            Assert.IsType<int>(result);
        }

        [Fact]
        public void IsMobileNoExists_WithValidMobileNo_ShouldReturnBoolean()
        {
            // Arrange
            string mobileNo = "1234567890";

            // Act
            var result = _testEntryGateWay.IsMobileNoExists(mobileNo);

            // Assert
            Assert.IsType<bool>(result);
        }

        [Fact]
        public void IsMobileNoExists_WithEmptyMobileNo_ShouldReturnBoolean()
        {
            // Arrange
            string mobileNo = "";

            // Act
            var result = _testEntryGateWay.IsMobileNoExists(mobileNo);

            // Assert
            Assert.IsType<bool>(result);
        }

        [Fact]
        public void IsMobileNoExists_WithNullMobileNo_ShouldHandleGracefully()
        {
            // Arrange
            string mobileNo = null;

            // Act & Assert
            var exception = Record.Exception(() => _testEntryGateWay.IsMobileNoExists(mobileNo));
            Assert.NotNull(exception);
        }

        [Fact]
        public void IsPatientTestExists_WithValidIds_ShouldReturnBoolean()
        {
            // Arrange
            int patientId = 1;
            int testId = 1;

            // Act
            var result = _testEntryGateWay.IsPatientTestExists(patientId, testId);

            // Assert
            Assert.IsType<bool>(result);
        }

        [Fact]
        public void IsPatientTestExists_WithZeroPatientId_ShouldReturnBoolean()
        {
            // Arrange
            int patientId = 0;
            int testId = 1;

            // Act
            var result = _testEntryGateWay.IsPatientTestExists(patientId, testId);

            // Assert
            Assert.IsType<bool>(result);
        }

        [Fact]
        public void IsPatientTestExists_WithZeroTestId_ShouldReturnBoolean()
        {
            // Arrange
            int patientId = 1;
            int testId = 0;

            // Act
            var result = _testEntryGateWay.IsPatientTestExists(patientId, testId);

            // Assert
            Assert.IsType<bool>(result);
        }

        [Fact]
        public void SavePatientTest_WithValidIds_ShouldReturnBoolean()
        {
            // Arrange
            int patientId = 1;
            int testId = 1;

            // Act
            var result = _testEntryGateWay.SavePatientTest(patientId, testId);

            // Assert
            Assert.IsType<bool>(result);
        }

        [Fact]
        public void SavePatientTest_WithZeroPatientId_ShouldReturnBoolean()
        {
            // Arrange
            int patientId = 0;
            int testId = 1;

            // Act
            var result = _testEntryGateWay.SavePatientTest(patientId, testId);

            // Assert
            Assert.IsType<bool>(result);
        }

        [Fact]
        public void SavePatientTest_WithNegativeIds_ShouldReturnBoolean()
        {
            // Arrange
            int patientId = -1;
            int testId = -1;

            // Act
            var result = _testEntryGateWay.SavePatientTest(patientId, testId);

            // Assert
            Assert.IsType<bool>(result);
        }
    }
}
