using System;
using System.Collections.Generic;
using Xunit;
using DiagnosticCenter.BLL;
using DiagnosticCenter.Models;

namespace Tests.DiagnosticCenter.BLL
{
    public class TestTypeManagerTests
    {
        private readonly TestTypeManager _testTypeManager;

        public TestTypeManagerTests()
        {
            _testTypeManager = new TestTypeManager();
        }

        [Fact]
        public void Constructor_ShouldCreateInstance()
        {
            // Arrange & Act
            var manager = new TestTypeManager();

            // Assert
            Assert.NotNull(manager);
        }

        [Fact]
        public void IsTestTypeExists_WithValidName_ShouldReturnBoolean()
        {
            // Arrange
            string name = "Blood Test";

            // Act
            var result = _testTypeManager.IsTestTypeExists(name);

            // Assert
            Assert.IsType<bool>(result);
        }

        [Fact]
        public void IsTestTypeExists_WithEmptyName_ShouldReturnBoolean()
        {
            // Arrange
            string name = "";

            // Act
            var result = _testTypeManager.IsTestTypeExists(name);

            // Assert
            Assert.IsType<bool>(result);
        }

        [Fact]
        public void IsTestTypeExists_WithNullName_ShouldHandleGracefully()
        {
            // Arrange
            string name = null;

            // Act & Assert
            var exception = Record.Exception(() => _testTypeManager.IsTestTypeExists(name));
            Assert.NotNull(exception);
        }

        [Fact]
        public void IsTestTypeExists_WithSpecialCharacters_ShouldReturnBoolean()
        {
            // Arrange
            string name = "Test@#$%";

            // Act
            var result = _testTypeManager.IsTestTypeExists(name);

            // Assert
            Assert.IsType<bool>(result);
        }

        [Fact]
        public void SaveTestType_WithValidTestType_ShouldReturnBoolean()
        {
            // Arrange
            var testType = new TestType
            {
                Name = "New Test Type"
            };

            // Act
            var result = _testTypeManager.SaveTestType(testType);

            // Assert
            Assert.IsType<bool>(result);
        }

        [Fact]
        public void SaveTestType_WithNullTestType_ShouldThrowException()
        {
            // Arrange
            TestType testType = null;

            // Act & Assert
            Assert.Throws<NullReferenceException>(() => _testTypeManager.SaveTestType(testType));
        }

        [Fact]
        public void SaveTestType_WithEmptyName_ShouldReturnBoolean()
        {
            // Arrange
            var testType = new TestType
            {
                Name = ""
            };

            // Act
            var result = _testTypeManager.SaveTestType(testType);

            // Assert
            Assert.IsType<bool>(result);
        }

        [Fact]
        public void SaveTestType_WithLongName_ShouldReturnBoolean()
        {
            // Arrange
            var testType = new TestType
            {
                Name = new string('A', 500)
            };

            // Act
            var result = _testTypeManager.SaveTestType(testType);

            // Assert
            Assert.IsType<bool>(result);
        }

        [Fact]
        public void GetAllTestType_ShouldReturnTestTypeList()
        {
            // Act
            var result = _testTypeManager.GetAllTestType();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<TestType>>(result);
        }

        [Fact]
        public void GetAllTestType_ShouldNotReturnNull()
        {
            // Act
            var result = _testTypeManager.GetAllTestType();

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void IsTestTypeExists_WithWhitespaceName_ShouldReturnBoolean()
        {
            // Arrange
            string name = "   ";

            // Act
            var result = _testTypeManager.IsTestTypeExists(name);

            // Assert
            Assert.IsType<bool>(result);
        }
    }
}
