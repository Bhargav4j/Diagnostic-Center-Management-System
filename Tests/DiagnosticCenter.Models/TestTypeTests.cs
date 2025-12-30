using System;
using Xunit;
using DiagnosticCenter.Models;

namespace DiagnosticCenter.Models.Tests
{
    public class TestTypeTests
    {
        [Fact]
        public void Constructor_ShouldCreateInstance()
        {
            // Arrange & Act
            var testType = new TestType();

            // Assert
            Assert.NotNull(testType);
        }

        [Fact]
        public void Id_ShouldSetAndGetValue()
        {
            // Arrange
            var testType = new TestType();
            int expectedId = 1;

            // Act
            testType.Id = expectedId;

            // Assert
            Assert.Equal(expectedId, testType.Id);
        }

        [Fact]
        public void Name_ShouldSetAndGetValue()
        {
            // Arrange
            var testType = new TestType();
            string expectedName = "Radiology";

            // Act
            testType.Name = expectedName;

            // Assert
            Assert.Equal(expectedName, testType.Name);
        }

        [Fact]
        public void Id_ShouldAcceptZeroValue()
        {
            // Arrange
            var testType = new TestType();

            // Act
            testType.Id = 0;

            // Assert
            Assert.Equal(0, testType.Id);
        }

        [Fact]
        public void Id_ShouldAcceptNegativeValue()
        {
            // Arrange
            var testType = new TestType();

            // Act
            testType.Id = -1;

            // Assert
            Assert.Equal(-1, testType.Id);
        }

        [Fact]
        public void Name_ShouldAcceptNullValue()
        {
            // Arrange
            var testType = new TestType();

            // Act
            testType.Name = null;

            // Assert
            Assert.Null(testType.Name);
        }

        [Fact]
        public void Name_ShouldAcceptEmptyValue()
        {
            // Arrange
            var testType = new TestType();

            // Act
            testType.Name = "";

            // Assert
            Assert.Equal("", testType.Name);
        }

        [Fact]
        public void Id_ShouldAcceptLargeValue()
        {
            // Arrange
            var testType = new TestType();

            // Act
            testType.Id = 999999;

            // Assert
            Assert.Equal(999999, testType.Id);
        }

        [Fact]
        public void Name_ShouldAcceptLongValue()
        {
            // Arrange
            var testType = new TestType();
            string expectedName = new string('A', 1000);

            // Act
            testType.Name = expectedName;

            // Assert
            Assert.Equal(expectedName, testType.Name);
        }

        [Theory]
        [InlineData(1, "Blood")]
        [InlineData(2, "Urine")]
        [InlineData(3, "Imaging")]
        [InlineData(4, "Pathology")]
        public void TestType_ShouldAcceptMultipleValues(int id, string name)
        {
            // Arrange
            var testType = new TestType();

            // Act
            testType.Id = id;
            testType.Name = name;

            // Assert
            Assert.Equal(id, testType.Id);
            Assert.Equal(name, testType.Name);
        }

        [Fact]
        public void Name_ShouldAcceptSpecialCharacters()
        {
            // Arrange
            var testType = new TestType();
            string expectedName = "Type@#$%^&*()";

            // Act
            testType.Name = expectedName;

            // Assert
            Assert.Equal(expectedName, testType.Name);
        }

        [Fact]
        public void Name_ShouldAcceptWhitespace()
        {
            // Arrange
            var testType = new TestType();
            string expectedName = "   ";

            // Act
            testType.Name = expectedName;

            // Assert
            Assert.Equal(expectedName, testType.Name);
        }

        [Fact]
        public void Name_ShouldAcceptNumericString()
        {
            // Arrange
            var testType = new TestType();
            string expectedName = "12345";

            // Act
            testType.Name = expectedName;

            // Assert
            Assert.Equal(expectedName, testType.Name);
        }

        [Fact]
        public void Id_ShouldAcceptMaxValue()
        {
            // Arrange
            var testType = new TestType();

            // Act
            testType.Id = int.MaxValue;

            // Assert
            Assert.Equal(int.MaxValue, testType.Id);
        }

        [Fact]
        public void Id_ShouldAcceptMinValue()
        {
            // Arrange
            var testType = new TestType();

            // Act
            testType.Id = int.MinValue;

            // Assert
            Assert.Equal(int.MinValue, testType.Id);
        }
    }
}
