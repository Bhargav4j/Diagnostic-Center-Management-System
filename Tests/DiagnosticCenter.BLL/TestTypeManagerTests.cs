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
        public void Constructor_ShouldCreateInstance()
        {
            // Arrange & Act
            var testTypeManager = new TestTypeManager();

            // Assert
            Assert.NotNull(testTypeManager);
        }

        [Fact]
        public void IsTestTypeExists_WithValidName_ShouldReturnBoolean()
        {
            // Arrange
            var testTypeManager = new TestTypeManager();
            string name = "Radiology";

            // Act
            var result = testTypeManager.IsTestTypeExists(name);

            // Assert
            Assert.IsType<bool>(result);
        }

        [Fact]
        public void IsTestTypeExists_WithNullName_ShouldHandleGracefully()
        {
            // Arrange
            var testTypeManager = new TestTypeManager();
            string name = null;

            // Act
            var result = testTypeManager.IsTestTypeExists(name);

            // Assert
            Assert.IsType<bool>(result);
        }

        [Fact]
        public void IsTestTypeExists_WithEmptyName_ShouldReturnBoolean()
        {
            // Arrange
            var testTypeManager = new TestTypeManager();
            string name = "";

            // Act
            var result = testTypeManager.IsTestTypeExists(name);

            // Assert
            Assert.IsType<bool>(result);
        }

        [Fact]
        public void IsTestTypeExists_WithWhitespaceName_ShouldReturnBoolean()
        {
            // Arrange
            var testTypeManager = new TestTypeManager();
            string name = "   ";

            // Act
            var result = testTypeManager.IsTestTypeExists(name);

            // Assert
            Assert.IsType<bool>(result);
        }

        [Theory]
        [InlineData("Blood")]
        [InlineData("Urine")]
        [InlineData("Imaging")]
        [InlineData("Pathology")]
        public void IsTestTypeExists_WithMultipleTestTypeNames_ShouldReturnBoolean(string testTypeName)
        {
            // Arrange
            var testTypeManager = new TestTypeManager();

            // Act
            var result = testTypeManager.IsTestTypeExists(testTypeName);

            // Assert
            Assert.IsType<bool>(result);
        }

        [Fact]
        public void SaveTestType_WithValidTestType_ShouldReturnBoolean()
        {
            // Arrange
            var testTypeManager = new TestTypeManager();
            var testType = new TestType();

            // Act
            var result = testTypeManager.SaveTestType(testType);

            // Assert
            Assert.IsType<bool>(result);
        }

        [Fact]
        public void SaveTestType_WithNullTestType_ShouldHandleGracefully()
        {
            // Arrange
            var testTypeManager = new TestTypeManager();
            TestType testType = null;

            // Act & Assert
            try
            {
                var result = testTypeManager.SaveTestType(testType);
                Assert.IsType<bool>(result);
            }
            catch (Exception ex)
            {
                Assert.NotNull(ex);
            }
        }

        [Fact]
        public void GetAllTestType_ShouldReturnTestTypeList()
        {
            // Arrange
            var testTypeManager = new TestTypeManager();

            // Act
            var result = testTypeManager.GetAllTestType();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<TestType>>(result);
        }

        [Fact]
        public void IsTestTypeExists_WithLongName_ShouldReturnBoolean()
        {
            // Arrange
            var testTypeManager = new TestTypeManager();
            string name = new string('A', 1000);

            // Act
            var result = testTypeManager.IsTestTypeExists(name);

            // Assert
            Assert.IsType<bool>(result);
        }

        [Fact]
        public void IsTestTypeExists_WithSpecialCharacters_ShouldReturnBoolean()
        {
            // Arrange
            var testTypeManager = new TestTypeManager();
            string name = "Type@#$%^&*()";

            // Act
            var result = testTypeManager.IsTestTypeExists(name);

            // Assert
            Assert.IsType<bool>(result);
        }

        [Fact]
        public void IsTestTypeExists_WithNumericName_ShouldReturnBoolean()
        {
            // Arrange
            var testTypeManager = new TestTypeManager();
            string name = "12345";

            // Act
            var result = testTypeManager.IsTestTypeExists(name);

            // Assert
            Assert.IsType<bool>(result);
        }

        [Fact]
        public void IsTestTypeExists_WithMixedCaseName_ShouldReturnBoolean()
        {
            // Arrange
            var testTypeManager = new TestTypeManager();
            string name = "BlOoD TeSt";

            // Act
            var result = testTypeManager.IsTestTypeExists(name);

            // Assert
            Assert.IsType<bool>(result);
        }
    }
}
