using System;
using Xunit;
using DiagnosticCenter.Models;

namespace Tests.DiagnosticCenter.Models
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
        public void Id_SetAndGet_ShouldWork()
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
        public void Name_SetAndGet_ShouldWork()
        {
            // Arrange
            var testType = new TestType();
            string expectedName = "Lab Test";

            // Act
            testType.Name = expectedName;

            // Assert
            Assert.Equal(expectedName, testType.Name);
        }

        [Fact]
        public void AllProperties_SetAndGet_ShouldWork()
        {
            // Arrange
            var testType = new TestType();
            int id = 2;
            string name = "Imaging Test";

            // Act
            testType.Id = id;
            testType.Name = name;

            // Assert
            Assert.Equal(id, testType.Id);
            Assert.Equal(name, testType.Name);
        }

        [Fact]
        public void Id_WithZeroValue_ShouldWork()
        {
            // Arrange
            var testType = new TestType();

            // Act
            testType.Id = 0;

            // Assert
            Assert.Equal(0, testType.Id);
        }

        [Fact]
        public void Id_WithNegativeValue_ShouldWork()
        {
            // Arrange
            var testType = new TestType();

            // Act
            testType.Id = -1;

            // Assert
            Assert.Equal(-1, testType.Id);
        }

        [Fact]
        public void Name_WithEmptyString_ShouldWork()
        {
            // Arrange
            var testType = new TestType();

            // Act
            testType.Name = "";

            // Assert
            Assert.Equal("", testType.Name);
        }

        [Fact]
        public void Name_WithNull_ShouldWork()
        {
            // Arrange
            var testType = new TestType();

            // Act
            testType.Name = null;

            // Assert
            Assert.Null(testType.Name);
        }

        [Fact]
        public void Name_WithLongString_ShouldWork()
        {
            // Arrange
            var testType = new TestType();
            string longName = new string('A', 500);

            // Act
            testType.Name = longName;

            // Assert
            Assert.Equal(longName, testType.Name);
        }

        [Fact]
        public void Name_WithSpecialCharacters_ShouldWork()
        {
            // Arrange
            var testType = new TestType();
            string specialName = "Test@#$%^&*()";

            // Act
            testType.Name = specialName;

            // Assert
            Assert.Equal(specialName, testType.Name);
        }

        [Fact]
        public void Id_WithLargeValue_ShouldWork()
        {
            // Arrange
            var testType = new TestType();
            int largeId = int.MaxValue;

            // Act
            testType.Id = largeId;

            // Assert
            Assert.Equal(largeId, testType.Id);
        }

        [Fact]
        public void DefaultValues_ShouldBeCorrect()
        {
            // Arrange & Act
            var testType = new TestType();

            // Assert
            Assert.Equal(0, testType.Id);
            Assert.Null(testType.Name);
        }
    }
}
