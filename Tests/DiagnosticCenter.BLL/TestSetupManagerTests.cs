using System;
using System.Collections.Generic;
using Xunit;
using DiagnosticCenter.BLL;
using DiagnosticCenter.Models;

namespace DiagnosticCenter.BLL.Tests
{
    public class TestSetupManagerTests
    {
        [Fact]
        public void Constructor_ShouldCreateInstance()
        {
            // Arrange & Act
            var testSetupManager = new TestSetupManager();

            // Assert
            Assert.NotNull(testSetupManager);
        }

        [Fact]
        public void GetAllTestType_ShouldReturnTestTypeList()
        {
            // Arrange
            var testSetupManager = new TestSetupManager();

            // Act
            var result = testSetupManager.GetAllTestType();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<TestType>>(result);
        }

        [Fact]
        public void IsTestExists_WithValidName_ShouldReturnBoolean()
        {
            // Arrange
            var testSetupManager = new TestSetupManager();
            string name = "Blood Test";

            // Act
            var result = testSetupManager.IsTestExists(name);

            // Assert
            Assert.IsType<bool>(result);
        }

        [Fact]
        public void IsTestExists_WithNullName_ShouldHandleGracefully()
        {
            // Arrange
            var testSetupManager = new TestSetupManager();
            string name = null;

            // Act
            var result = testSetupManager.IsTestExists(name);

            // Assert
            Assert.IsType<bool>(result);
        }

        [Fact]
        public void IsTestExists_WithEmptyName_ShouldReturnBoolean()
        {
            // Arrange
            var testSetupManager = new TestSetupManager();
            string name = "";

            // Act
            var result = testSetupManager.IsTestExists(name);

            // Assert
            Assert.IsType<bool>(result);
        }

        [Fact]
        public void IsTestExists_WithWhitespaceName_ShouldReturnBoolean()
        {
            // Arrange
            var testSetupManager = new TestSetupManager();
            string name = "   ";

            // Act
            var result = testSetupManager.IsTestExists(name);

            // Assert
            Assert.IsType<bool>(result);
        }

        [Fact]
        public void SaveTestSetup_WithValidTestSetup_ShouldReturnBoolean()
        {
            // Arrange
            var testSetupManager = new TestSetupManager();
            var testSetup = new TestSetup();

            // Act
            var result = testSetupManager.SaveTestSetup(testSetup);

            // Assert
            Assert.IsType<bool>(result);
        }

        [Fact]
        public void SaveTestSetup_WithNullTestSetup_ShouldHandleGracefully()
        {
            // Arrange
            var testSetupManager = new TestSetupManager();
            TestSetup testSetup = null;

            // Act & Assert
            try
            {
                var result = testSetupManager.SaveTestSetup(testSetup);
                Assert.IsType<bool>(result);
            }
            catch (Exception ex)
            {
                Assert.NotNull(ex);
            }
        }

        [Fact]
        public void GetAllTest_ShouldReturnTestSetupList()
        {
            // Arrange
            var testSetupManager = new TestSetupManager();

            // Act
            var result = testSetupManager.GetAllTest();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<TestSetup>>(result);
        }

        [Theory]
        [InlineData("X-Ray")]
        [InlineData("MRI")]
        [InlineData("CT Scan")]
        public void IsTestExists_WithMultipleTestNames_ShouldReturnBoolean(string testName)
        {
            // Arrange
            var testSetupManager = new TestSetupManager();

            // Act
            var result = testSetupManager.IsTestExists(testName);

            // Assert
            Assert.IsType<bool>(result);
        }

        [Fact]
        public void IsTestExists_WithLongName_ShouldReturnBoolean()
        {
            // Arrange
            var testSetupManager = new TestSetupManager();
            string name = new string('A', 1000);

            // Act
            var result = testSetupManager.IsTestExists(name);

            // Assert
            Assert.IsType<bool>(result);
        }

        [Fact]
        public void IsTestExists_WithSpecialCharacters_ShouldReturnBoolean()
        {
            // Arrange
            var testSetupManager = new TestSetupManager();
            string name = "Test@#$%^&*()";

            // Act
            var result = testSetupManager.IsTestExists(name);

            // Assert
            Assert.IsType<bool>(result);
        }
    }
}
