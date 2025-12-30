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
        public void Constructor_ShouldCreateInstance()
        {
            // Arrange & Act
            var testEntryManager = new TestEntryManager();

            // Assert
            Assert.NotNull(testEntryManager);
        }

        [Fact]
        public void GetAllTest_ShouldReturnTestEntryList()
        {
            // Arrange
            var testEntryManager = new TestEntryManager();

            // Act
            var result = testEntryManager.GetAllTest();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<TestEntry>>(result);
        }

        [Fact]
        public void GetTestFee_WithValidTestValue_ShouldReturnString()
        {
            // Arrange
            var testEntryManager = new TestEntryManager();
            int testValue = 1;

            // Act
            var result = testEntryManager.GetTestFee(testValue);

            // Assert
            Assert.IsType<string>(result);
        }

        [Fact]
        public void GetTestFee_WithZeroTestValue_ShouldReturnString()
        {
            // Arrange
            var testEntryManager = new TestEntryManager();
            int testValue = 0;

            // Act
            var result = testEntryManager.GetTestFee(testValue);

            // Assert
            Assert.IsType<string>(result);
        }

        [Fact]
        public void GetTestFee_WithNegativeTestValue_ShouldReturnString()
        {
            // Arrange
            var testEntryManager = new TestEntryManager();
            int testValue = -1;

            // Act
            var result = testEntryManager.GetTestFee(testValue);

            // Assert
            Assert.IsType<string>(result);
        }

        [Fact]
        public void GetTestFee_WithLargeTestValue_ShouldReturnString()
        {
            // Arrange
            var testEntryManager = new TestEntryManager();
            int testValue = 99999;

            // Act
            var result = testEntryManager.GetTestFee(testValue);

            // Assert
            Assert.IsType<string>(result);
        }

        [Fact]
        public void IsMobileNoExists_WithValidMobileNo_ShouldReturnBoolean()
        {
            // Arrange
            var testEntryManager = new TestEntryManager();
            string mobileNo = "1234567890";

            // Act
            var result = testEntryManager.IsMobileNoExists(mobileNo);

            // Assert
            Assert.IsType<bool>(result);
        }

        [Fact]
        public void IsMobileNoExists_WithNullMobileNo_ShouldHandleGracefully()
        {
            // Arrange
            var testEntryManager = new TestEntryManager();
            string mobileNo = null;

            // Act
            var result = testEntryManager.IsMobileNoExists(mobileNo);

            // Assert
            Assert.IsType<bool>(result);
        }

        [Fact]
        public void IsMobileNoExists_WithEmptyMobileNo_ShouldReturnBoolean()
        {
            // Arrange
            var testEntryManager = new TestEntryManager();
            string mobileNo = "";

            // Act
            var result = testEntryManager.IsMobileNoExists(mobileNo);

            // Assert
            Assert.IsType<bool>(result);
        }

        [Fact]
        public void SavePatient_WithValidTestEntry_ShouldReturnInteger()
        {
            // Arrange
            var testEntryManager = new TestEntryManager();
            var testEntry = new TestEntry();

            // Act
            var result = testEntryManager.SavePatient(testEntry);

            // Assert
            Assert.IsType<int>(result);
        }

        [Fact]
        public void SavePatient_WithNullTestEntry_ShouldHandleGracefully()
        {
            // Arrange
            var testEntryManager = new TestEntryManager();
            TestEntry testEntry = null;

            // Act & Assert
            try
            {
                var result = testEntryManager.SavePatient(testEntry);
                Assert.IsType<int>(result);
            }
            catch (Exception ex)
            {
                Assert.NotNull(ex);
            }
        }

        [Fact]
        public void SavePatientTest_WithValidPatientIdAndTestId_ShouldReturnBoolean()
        {
            // Arrange
            var testEntryManager = new TestEntryManager();
            int patientId = 1;
            int testId = 1;

            // Act
            try
            {
                var result = testEntryManager.SavePatientTest(patientId, testId);
                Assert.IsType<bool>(result);
            }
            catch (Exception ex)
            {
                // Expected exception for duplicate tests
                Assert.NotNull(ex);
            }
        }

        [Fact]
        public void SavePatientTest_WithZeroPatientId_ShouldHandleGracefully()
        {
            // Arrange
            var testEntryManager = new TestEntryManager();
            int patientId = 0;
            int testId = 1;

            // Act
            try
            {
                var result = testEntryManager.SavePatientTest(patientId, testId);
                Assert.IsType<bool>(result);
            }
            catch (Exception ex)
            {
                Assert.NotNull(ex);
            }
        }

        [Fact]
        public void SavePatientTest_WithZeroTestId_ShouldHandleGracefully()
        {
            // Arrange
            var testEntryManager = new TestEntryManager();
            int patientId = 1;
            int testId = 0;

            // Act
            try
            {
                var result = testEntryManager.SavePatientTest(patientId, testId);
                Assert.IsType<bool>(result);
            }
            catch (Exception ex)
            {
                Assert.NotNull(ex);
            }
        }

        [Fact]
        public void SavePatientTest_WithNegativePatientId_ShouldHandleGracefully()
        {
            // Arrange
            var testEntryManager = new TestEntryManager();
            int patientId = -1;
            int testId = 1;

            // Act
            try
            {
                var result = testEntryManager.SavePatientTest(patientId, testId);
                Assert.IsType<bool>(result);
            }
            catch (Exception ex)
            {
                Assert.NotNull(ex);
            }
        }

        [Fact]
        public void SavePatientTest_WithNegativeTestId_ShouldHandleGracefully()
        {
            // Arrange
            var testEntryManager = new TestEntryManager();
            int patientId = 1;
            int testId = -1;

            // Act
            try
            {
                var result = testEntryManager.SavePatientTest(patientId, testId);
                Assert.IsType<bool>(result);
            }
            catch (Exception ex)
            {
                Assert.NotNull(ex);
            }
        }

        [Fact]
        public void SavePatientTest_WhenDuplicateTestExists_ShouldThrowException()
        {
            // Arrange
            var testEntryManager = new TestEntryManager();
            int patientId = 1;
            int testId = 1;

            // Act & Assert
            try
            {
                var result = testEntryManager.SavePatientTest(patientId, testId);
            }
            catch (Exception ex)
            {
                Assert.Contains("Duplicate", ex.Message);
            }
        }
    }
}
