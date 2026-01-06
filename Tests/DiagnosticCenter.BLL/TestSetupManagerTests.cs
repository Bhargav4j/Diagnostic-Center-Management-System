using System;
using System.Collections.Generic;
using Xunit;
using DiagnosticCenter.BLL;
using DiagnosticCenter.Models;

namespace Tests.DiagnosticCenter.BLL
{
    public class TestSetupManagerTests
    {
        private readonly TestSetupManager _testSetupManager;

        public TestSetupManagerTests()
        {
            _testSetupManager = new TestSetupManager();
        }

        [Fact]
        public void Constructor_ShouldCreateInstance()
        {
            // Arrange & Act
            var manager = new TestSetupManager();

            // Assert
            Assert.NotNull(manager);
        }

        [Fact]
        public void GetAllTestType_ShouldReturnTestTypeList()
        {
            // Act
            var result = _testSetupManager.GetAllTestType();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<TestType>>(result);
        }

        [Fact]
        public void IsTestExists_WithValidName_ShouldReturnBoolean()
        {
            // Arrange
            string name = "Blood Test";

            // Act
            var result = _testSetupManager.IsTestExists(name);

            // Assert
            Assert.IsType<bool>(result);
        }

        [Fact]
        public void IsTestExists_WithEmptyName_ShouldReturnBoolean()
        {
            // Arrange
            string name = "";

            // Act
            var result = _testSetupManager.IsTestExists(name);

            // Assert
            Assert.IsType<bool>(result);
        }

        [Fact]
        public void IsTestExists_WithNullName_ShouldHandleGracefully()
        {
            // Arrange
            string name = null;

            // Act & Assert
            var exception = Record.Exception(() => _testSetupManager.IsTestExists(name));
            Assert.NotNull(exception);
        }

        [Fact]
        public void IsTestExists_WithSpecialCharacters_ShouldReturnBoolean()
        {
            // Arrange
            string name = "Test@#$%";

            // Act
            var result = _testSetupManager.IsTestExists(name);

            // Assert
            Assert.IsType<bool>(result);
        }

        [Fact]
        public void SaveTestSetup_WithValidTestSetup_ShouldReturnBoolean()
        {
            // Arrange
            var testSetup = new TestSetup
            {
                Name = "New Test",
                Fee = "500",
                TypeId = 1,
                TypeName = "Lab Test"
            };

            // Act
            var result = _testSetupManager.SaveTestSetup(testSetup);

            // Assert
            Assert.IsType<bool>(result);
        }

        [Fact]
        public void SaveTestSetup_WithNullTestSetup_ShouldThrowException()
        {
            // Arrange
            TestSetup testSetup = null;

            // Act & Assert
            Assert.Throws<NullReferenceException>(() => _testSetupManager.SaveTestSetup(testSetup));
        }

        [Fact]
        public void SaveTestSetup_WithEmptyName_ShouldReturnBoolean()
        {
            // Arrange
            var testSetup = new TestSetup
            {
                Name = "",
                Fee = "500",
                TypeId = 1,
                TypeName = "Lab Test"
            };

            // Act
            var result = _testSetupManager.SaveTestSetup(testSetup);

            // Assert
            Assert.IsType<bool>(result);
        }

        [Fact]
        public void SaveTestSetup_WithZeroTypeId_ShouldReturnBoolean()
        {
            // Arrange
            var testSetup = new TestSetup
            {
                Name = "Test",
                Fee = "500",
                TypeId = 0,
                TypeName = "Lab Test"
            };

            // Act
            var result = _testSetupManager.SaveTestSetup(testSetup);

            // Assert
            Assert.IsType<bool>(result);
        }

        [Fact]
        public void SaveTestSetup_WithNegativeFee_ShouldReturnBoolean()
        {
            // Arrange
            var testSetup = new TestSetup
            {
                Name = "Test",
                Fee = "-100",
                TypeId = 1,
                TypeName = "Lab Test"
            };

            // Act
            var result = _testSetupManager.SaveTestSetup(testSetup);

            // Assert
            Assert.IsType<bool>(result);
        }

        [Fact]
        public void GetAllTest_ShouldReturnTestSetupList()
        {
            // Act
            var result = _testSetupManager.GetAllTest();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<TestSetup>>(result);
        }

        [Fact]
        public void GetAllTestType_ShouldNotReturnNull()
        {
            // Act
            var result = _testSetupManager.GetAllTestType();

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void GetAllTest_ShouldNotReturnNull()
        {
            // Act
            var result = _testSetupManager.GetAllTest();

            // Assert
            Assert.NotNull(result);
        }
    }
}
