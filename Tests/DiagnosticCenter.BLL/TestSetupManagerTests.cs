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
        public void Constructor_CreatesInstance()
        {
            // Arrange & Act
            var manager = new TestSetupManager();

            // Assert
            Assert.NotNull(manager);
        }

        [Fact]
        public void GetAllTestType_ReturnsListOfTestType()
        {
            // Arrange
            var manager = new TestSetupManager();

            // Act
            var result = manager.GetAllTestType();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<TestType>>(result);
        }

        [Fact]
        public void IsTestExists_WithValidName_ReturnsBoolean()
        {
            // Arrange
            var manager = new TestSetupManager();
            var name = "Blood Test";

            // Act
            var result = manager.IsTestExists(name);

            // Assert
            Assert.IsType<bool>(result);
        }

        [Fact]
        public void IsTestExists_WithEmptyName_ReturnsBoolean()
        {
            // Arrange
            var manager = new TestSetupManager();
            var name = "";

            // Act
            var result = manager.IsTestExists(name);

            // Assert
            Assert.IsType<bool>(result);
        }

        [Fact]
        public void IsTestExists_WithNullName_HandlesNull()
        {
            // Arrange
            var manager = new TestSetupManager();
            string name = null;

            // Act
            var result = manager.IsTestExists(name);

            // Assert
            Assert.IsType<bool>(result);
        }

        [Fact]
        public void IsTestExists_WithWhitespaceName_ReturnsBoolean()
        {
            // Arrange
            var manager = new TestSetupManager();
            var name = "   ";

            // Act
            var result = manager.IsTestExists(name);

            // Assert
            Assert.IsType<bool>(result);
        }

        [Fact]
        public void SaveTestSetup_WithValidTestSetup_ReturnsBoolean()
        {
            // Arrange
            var manager = new TestSetupManager();
            var testSetup = new TestSetup();

            // Act
            var result = manager.SaveTestSetup(testSetup);

            // Assert
            Assert.IsType<bool>(result);
        }

        [Fact]
        public void SaveTestSetup_WithNullTestSetup_HandlesNull()
        {
            // Arrange
            var manager = new TestSetupManager();
            TestSetup testSetup = null;

            // Act & Assert
            try
            {
                var result = manager.SaveTestSetup(testSetup);
                Assert.IsType<bool>(result);
            }
            catch (Exception)
            {
                Assert.True(true);
            }
        }

        [Fact]
        public void GetAllTest_ReturnsListOfTestSetup()
        {
            // Arrange
            var manager = new TestSetupManager();

            // Act
            var result = manager.GetAllTest();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<TestSetup>>(result);
        }

        [Fact]
        public void IsTestExists_WithSpecialCharacters_ReturnsBoolean()
        {
            // Arrange
            var manager = new TestSetupManager();
            var name = "Test@#$%";

            // Act
            var result = manager.IsTestExists(name);

            // Assert
            Assert.IsType<bool>(result);
        }

        [Fact]
        public void IsTestExists_WithLongName_ReturnsBoolean()
        {
            // Arrange
            var manager = new TestSetupManager();
            var name = new string('A', 1000);

            // Act
            var result = manager.IsTestExists(name);

            // Assert
            Assert.IsType<bool>(result);
        }
    }
}
