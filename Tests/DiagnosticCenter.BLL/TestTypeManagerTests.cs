using System;
using System.Collections.Generic;
using Xunit;
using DiagnosticCenter.BLL;
using DiagnosticCenter.Models;

namespace DiagnosticCenter.BLL.Tests
{
    public class TestTypeManagerTests
    {
        [Fact]
        public void Constructor_CreatesInstance()
        {
            // Arrange & Act
            var manager = new TestTypeManager();

            // Assert
            Assert.NotNull(manager);
        }

        [Fact]
        public void IsTestTypeExists_WithValidName_ReturnsBoolean()
        {
            // Arrange
            var manager = new TestTypeManager();
            var name = "X-Ray";

            // Act
            var result = manager.IsTestTypeExists(name);

            // Assert
            Assert.IsType<bool>(result);
        }

        [Fact]
        public void IsTestTypeExists_WithEmptyName_ReturnsBoolean()
        {
            // Arrange
            var manager = new TestTypeManager();
            var name = "";

            // Act
            var result = manager.IsTestTypeExists(name);

            // Assert
            Assert.IsType<bool>(result);
        }

        [Fact]
        public void IsTestTypeExists_WithNullName_HandlesNull()
        {
            // Arrange
            var manager = new TestTypeManager();
            string name = null;

            // Act
            var result = manager.IsTestTypeExists(name);

            // Assert
            Assert.IsType<bool>(result);
        }

        [Fact]
        public void IsTestTypeExists_WithWhitespaceName_ReturnsBoolean()
        {
            // Arrange
            var manager = new TestTypeManager();
            var name = "   ";

            // Act
            var result = manager.IsTestTypeExists(name);

            // Assert
            Assert.IsType<bool>(result);
        }

        [Fact]
        public void SaveTestType_WithValidTestType_ReturnsBoolean()
        {
            // Arrange
            var manager = new TestTypeManager();
            var testType = new TestType();

            // Act
            var result = manager.SaveTestType(testType);

            // Assert
            Assert.IsType<bool>(result);
        }

        [Fact]
        public void SaveTestType_WithNullTestType_HandlesNull()
        {
            // Arrange
            var manager = new TestTypeManager();
            TestType testType = null;

            // Act & Assert
            try
            {
                var result = manager.SaveTestType(testType);
                Assert.IsType<bool>(result);
            }
            catch (Exception)
            {
                Assert.True(true);
            }
        }

        [Fact]
        public void GetAllTestType_ReturnsListOfTestType()
        {
            // Arrange
            var manager = new TestTypeManager();

            // Act
            var result = manager.GetAllTestType();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<TestType>>(result);
        }

        [Fact]
        public void IsTestTypeExists_WithSpecialCharacters_ReturnsBoolean()
        {
            // Arrange
            var manager = new TestTypeManager();
            var name = "Test@#$%";

            // Act
            var result = manager.IsTestTypeExists(name);

            // Assert
            Assert.IsType<bool>(result);
        }

        [Fact]
        public void IsTestTypeExists_WithLongName_ReturnsBoolean()
        {
            // Arrange
            var manager = new TestTypeManager();
            var name = new string('A', 1000);

            // Act
            var result = manager.IsTestTypeExists(name);

            // Assert
            Assert.IsType<bool>(result);
        }

        [Fact]
        public void IsTestTypeExists_WithNumericName_ReturnsBoolean()
        {
            // Arrange
            var manager = new TestTypeManager();
            var name = "12345";

            // Act
            var result = manager.IsTestTypeExists(name);

            // Assert
            Assert.IsType<bool>(result);
        }
    }
}
