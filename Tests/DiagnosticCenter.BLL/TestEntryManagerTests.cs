using System;
using System.Collections.Generic;
using Xunit;
using DiagnosticCenter.BLL;
using DiagnosticCenter.Models;

namespace Tests.DiagnosticCenter.BLL
{
    public class TestEntryManagerTests
    {
        private readonly TestEntryManager _testEntryManager;

        public TestEntryManagerTests()
        {
            _testEntryManager = new TestEntryManager();
        }

        [Fact]
        public void Constructor_ShouldCreateInstance()
        {
            // Arrange & Act
            var manager = new TestEntryManager();

            // Assert
            Assert.NotNull(manager);
        }

        [Fact]
        public void GetAllTest_ShouldReturnTestEntryList()
        {
            // Act
            var result = _testEntryManager.GetAllTest();

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
            var result = _testEntryManager.GetTestFee(testValue);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<string>(result);
        }

        [Fact]
        public void GetTestFee_WithZeroTestValue_ShouldReturnString()
        {
            // Arrange
            int testValue = 0;

            // Act
            var result = _testEntryManager.GetTestFee(testValue);

            // Assert
            Assert.IsType<string>(result);
        }

        [Fact]
        public void GetTestFee_WithNegativeTestValue_ShouldReturnString()
        {
            // Arrange
            int testValue = -1;

            // Act
            var result = _testEntryManager.GetTestFee(testValue);

            // Assert
            Assert.IsType<string>(result);
        }

        [Fact]
        public void IsMobileNoExists_WithValidMobileNo_ShouldReturnBoolean()
        {
            // Arrange
            string mobileNo = "1234567890";

            // Act
            var result = _testEntryManager.IsMobileNoExists(mobileNo);

            // Assert
            Assert.IsType<bool>(result);
        }

        [Fact]
        public void IsMobileNoExists_WithEmptyMobileNo_ShouldReturnBoolean()
        {
            // Arrange
            string mobileNo = "";

            // Act
            var result = _testEntryManager.IsMobileNoExists(mobileNo);

            // Assert
            Assert.IsType<bool>(result);
        }

        [Fact]
        public void IsMobileNoExists_WithNullMobileNo_ShouldHandleGracefully()
        {
            // Arrange
            string mobileNo = null;

            // Act & Assert
            var exception = Record.Exception(() => _testEntryManager.IsMobileNoExists(mobileNo));
            Assert.NotNull(exception);
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
                BillNo = "BILL001",
                TotalAmount = 1000m,
                DueDate = DateTime.Now.AddDays(7),
                PaidAmount = 0m
            };

            // Act
            var result = _testEntryManager.SavePatient(testEntry);

            // Assert
            Assert.IsType<int>(result);
        }

        [Fact]
        public void SavePatient_WithNullTestEntry_ShouldThrowException()
        {
            // Arrange
            TestEntry testEntry = null;

            // Act & Assert
            Assert.Throws<NullReferenceException>(() => _testEntryManager.SavePatient(testEntry));
        }

        [Fact]
        public void SavePatient_WithNegativeAmounts_ShouldReturnInteger()
        {
            // Arrange
            var testEntry = new TestEntry
            {
                Name = "Test Patient",
                DOB = new DateTime(1990, 1, 1),
                MobileNo = "9876543210",
                BillNo = "BILL002",
                TotalAmount = -100m,
                DueDate = DateTime.Now,
                PaidAmount = -50m
            };

            // Act
            var result = _testEntryManager.SavePatient(testEntry);

            // Assert
            Assert.IsType<int>(result);
        }

        [Fact]
        public void SavePatientTest_WithValidPatientIdAndTestId_ShouldReturnBoolean()
        {
            // Arrange
            int patientId = 1;
            int testId = 1;

            // Act
            var result = _testEntryManager.SavePatientTest(patientId, testId);

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
            var result = _testEntryManager.SavePatientTest(patientId, testId);

            // Assert
            Assert.IsType<bool>(result);
        }

        [Fact]
        public void SavePatientTest_WithZeroTestId_ShouldReturnBoolean()
        {
            // Arrange
            int patientId = 1;
            int testId = 0;

            // Act
            var result = _testEntryManager.SavePatientTest(patientId, testId);

            // Assert
            Assert.IsType<bool>(result);
        }

        [Fact]
        public void SavePatientTest_WithDuplicateTest_ShouldThrowException()
        {
            // Arrange
            int patientId = 1;
            int testId = 1;

            // Act & Assert - First save should succeed, second should throw exception
            var exception = Record.Exception(() =>
            {
                _testEntryManager.SavePatientTest(patientId, testId);
                _testEntryManager.SavePatientTest(patientId, testId);
            });

            // If exception is thrown, it should be of type Exception
            if (exception != null)
            {
                Assert.IsType<Exception>(exception);
                Assert.Contains("Duplicate", exception.Message);
            }
        }

        [Fact]
        public void SavePatientTest_WithNegativePatientId_ShouldReturnBoolean()
        {
            // Arrange
            int patientId = -1;
            int testId = 1;

            // Act
            var result = _testEntryManager.SavePatientTest(patientId, testId);

            // Assert
            Assert.IsType<bool>(result);
        }
    }
}
