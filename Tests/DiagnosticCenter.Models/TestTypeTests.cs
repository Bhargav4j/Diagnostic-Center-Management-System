using System;
using Xunit;
using DiagnosticCenter.Models;

namespace DiagnosticCenter.Models.Tests
{
    public class TestTypeTests
    {
        [Fact]
        public void Constructor_CreatesInstance()
        {
            // Arrange & Act
            var testType = new TestType();

            // Assert
            Assert.NotNull(testType);
        }

        [Fact]
        public void Id_CanBeSetAndRetrieved()
        {
            // Arrange
            var testType = new TestType();
            var id = 1;

            // Act
            testType.Id = id;

            // Assert
            Assert.Equal(id, testType.Id);
        }

        [Fact]
        public void Name_CanBeSetAndRetrieved()
        {
            // Arrange
            var testType = new TestType();
            var name = "Laboratory";

            // Act
            testType.Name = name;

            // Assert
            Assert.Equal(name, testType.Name);
        }

        [Fact]
        public void AllProperties_CanBeSetTogether()
        {
            // Arrange
            var testType = new TestType
            {
                Id = 5,
                Name = "Radiology"
            };

            // Assert
            Assert.Equal(5, testType.Id);
            Assert.Equal("Radiology", testType.Name);
        }

        [Fact]
        public void Id_WithZero_CanBeSet()
        {
            // Arrange
            var testType = new TestType();
            var id = 0;

            // Act
            testType.Id = id;

            // Assert
            Assert.Equal(0, testType.Id);
        }

        [Fact]
        public void Id_WithNegative_CanBeSet()
        {
            // Arrange
            var testType = new TestType();
            var id = -1;

            // Act
            testType.Id = id;

            // Assert
            Assert.Equal(-1, testType.Id);
        }

        [Fact]
        public void Name_WithEmpty_CanBeSet()
        {
            // Arrange
            var testType = new TestType();
            var name = "";

            // Act
            testType.Name = name;

            // Assert
            Assert.Equal("", testType.Name);
        }

        [Fact]
        public void Name_WithNull_CanBeSet()
        {
            // Arrange
            var testType = new TestType();
            string name = null;

            // Act
            testType.Name = name;

            // Assert
            Assert.Null(testType.Name);
        }

        [Fact]
        public void Name_WithLongString_CanBeSet()
        {
            // Arrange
            var testType = new TestType();
            var name = new string('A', 1000);

            // Act
            testType.Name = name;

            // Assert
            Assert.Equal(name, testType.Name);
        }

        [Fact]
        public void Id_WithMaxValue_CanBeSet()
        {
            // Arrange
            var testType = new TestType();
            var id = int.MaxValue;

            // Act
            testType.Id = id;

            // Assert
            Assert.Equal(int.MaxValue, testType.Id);
        }

        [Fact]
        public void Id_WithMinValue_CanBeSet()
        {
            // Arrange
            var testType = new TestType();
            var id = int.MinValue;

            // Act
            testType.Id = id;

            // Assert
            Assert.Equal(int.MinValue, testType.Id);
        }

        [Fact]
        public void Name_WithSpecialCharacters_CanBeSet()
        {
            // Arrange
            var testType = new TestType();
            var name = "Test@#$%^&*()";

            // Act
            testType.Name = name;

            // Assert
            Assert.Equal("Test@#$%^&*()", testType.Name);
        }
    }
}
