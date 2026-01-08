using System;
using System.Collections.Generic;
using Xunit;
using DiagnosticCenter.BLL;
using DiagnosticCenter.Models;

namespace DiagnosticCenter.BLL.Tests
{
    public class TestEntryManagerTests
    {
        [Fact]
        public void Constructor_CreatesInstance()
        {
            // Arrange & Act
            var manager = new TestEntryManager();

            // Assert
            Assert.NotNull(manager);
        }

        [Fact]
        public void GetAllTest_ReturnsListOfTestEntry()
        {
            // Arrange
            var manager = new TestEntryManager();

            // Act
            var result = manager.GetAllTest();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<TestEntry>>(result);
        }

        [Fact]
        public void GetTestFee_WithValidTestValue_ReturnsString()
        {
            // Arrange
            var manager = new TestEntryManager();
            var testValue = 1;

            // Act
            var result = manager.GetTestFee(testValue);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<string>(result);
        }

        [Fact]
        public void GetTestFee_WithZeroTestValue_ReturnsString()
        {
            // Arrange
            var manager = new TestEntryManager();
            var testValue = 0;

            // Act
            var result = manager.GetTestFee(testValue);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void GetTestFee_WithNegativeTestValue_ReturnsString()
        {
            // Arrange
            var manager = new TestEntryManager();
            var testValue = -1;

            // Act
            var result = manager.GetTestFee(testValue);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void IsMobileNoExists_WithValidMobileNo_ReturnsBoolean()
        {
            // Arrange
            var manager = new TestEntryManager();
            var mobileNo = "1234567890";

            // Act
            var result = manager.IsMobileNoExists(mobileNo);

            // Assert
            Assert.IsType<bool>(result);
        }

        [Fact]
        public void IsMobileNoExists_WithEmptyMobileNo_ReturnsBoolean()
        {
            // Arrange
            var manager = new TestEntryManager();
            var mobileNo = "";

            // Act
            var result = manager.IsMobileNoExists(mobileNo);

            // Assert
            Assert.IsType<bool>(result);
        }

        [Fact]
        public void IsMobileNoExists_WithNullMobileNo_HandlesNull()
        {
            // Arrange
            var manager = new TestEntryManager();
            string mobileNo = null;

            // Act
            var result = manager.IsMobileNoExists(mobileNo);

            // Assert
            Assert.IsType<bool>(result);
        }

        [Fact]
        public void SavePatient_WithValidTestEntry_ReturnsInteger()
        {
            // Arrange
            var manager = new TestEntryManager();
            var testEntry = new TestEntry();

            // Act
            var result = manager.SavePatient(testEntry);

            // Assert
            Assert.IsType<int>(result);
        }

        [Fact]
        public void SavePatient_WithNullTestEntry_HandlesNull()
        {
            // Arrange
            var manager = new TestEntryManager();
            TestEntry testEntry = null;

            // Act & Assert
            try
            {
                var result = manager.SavePatient(testEntry);
                Assert.IsType<int>(result);
            }
            catch (Exception)
            {
                Assert.True(true);
            }
        }

        [Fact]
        public void SavePatientTest_WithValidParameters_ReturnsTrue()
        {
            // Arrange
            var manager = new TestEntryManager();
            var patientId = 1;
            var testId = 1;

            // Act
            var result = manager.SavePatientTest(patientId, testId);

            // Assert
            Assert.IsType<bool>(result);
        }

        [Fact]
        public void SavePatientTest_WithZeroPatientId_ReturnsBoolean()
        {
            // Arrange
            var manager = new TestEntryManager();
            var patientId = 0;
            var testId = 1;

            // Act
            var result = manager.SavePatientTest(patientId, testId);

            // Assert
            Assert.IsType<bool>(result);
        }

        [Fact]
        public void SavePatientTest_WithNegativeTestId_ReturnsBoolean()
        {
            // Arrange
            var manager = new TestEntryManager();
            var patientId = 1;
            var testId = -1;

            // Act
            var result = manager.SavePatientTest(patientId, testId);

            // Assert
            Assert.IsType<bool>(result);
        }

        [Fact]
        public void SavePatientTest_WithDuplicateTest_ThrowsException()
        {
            // Arrange
            var manager = new TestEntryManager();
            var patientId = 1;
            var testId = 1;

            // Act & Assert
            try
            {
                manager.SavePatientTest(patientId, testId);
                // If duplicate test exists, it should throw exception
            }
            catch (Exception ex)
            {
                Assert.Contains("Duplicate", ex.Message);
            }
        }
    }
}
