using System;
using Xunit;
using DiagnosticCenter.Models;

namespace DiagnosticCenter.Models.Tests
{
    public class TestSetupTests
    {
        [Fact]
        public void Constructor_CreatesInstance()
        {
            // Arrange & Act
            var testSetup = new TestSetup();

            // Assert
            Assert.NotNull(testSetup);
        }

        [Fact]
        public void Id_CanBeSetAndRetrieved()
        {
            // Arrange
            var testSetup = new TestSetup();
            var id = 1;

            // Act
            testSetup.Id = id;

            // Assert
            Assert.Equal(id, testSetup.Id);
        }

        [Fact]
        public void Name_CanBeSetAndRetrieved()
        {
            // Arrange
            var testSetup = new TestSetup();
            var name = "Blood Test";

            // Act
            testSetup.Name = name;

            // Assert
            Assert.Equal(name, testSetup.Name);
        }

        [Fact]
        public void Fee_CanBeSetAndRetrieved()
        {
            // Arrange
            var testSetup = new TestSetup();
            var fee = "1000";

            // Act
            testSetup.Fee = fee;

            // Assert
            Assert.Equal(fee, testSetup.Fee);
        }

        [Fact]
        public void TypeId_CanBeSetAndRetrieved()
        {
            // Arrange
            var testSetup = new TestSetup();
            var typeId = 2;

            // Act
            testSetup.TypeId = typeId;

            // Assert
            Assert.Equal(typeId, testSetup.TypeId);
        }

        [Fact]
        public void TypeName_CanBeSetAndRetrieved()
        {
            // Arrange
            var testSetup = new TestSetup();
            var typeName = "Laboratory";

            // Act
            testSetup.TypeName = typeName;

            // Assert
            Assert.Equal(typeName, testSetup.TypeName);
        }

        [Fact]
        public void AllProperties_CanBeSetTogether()
        {
            // Arrange
            var testSetup = new TestSetup
            {
                Id = 5,
                Name = "X-Ray",
                Fee = "2000",
                TypeId = 3,
                TypeName = "Radiology"
            };

            // Assert
            Assert.Equal(5, testSetup.Id);
            Assert.Equal("X-Ray", testSetup.Name);
            Assert.Equal("2000", testSetup.Fee);
            Assert.Equal(3, testSetup.TypeId);
            Assert.Equal("Radiology", testSetup.TypeName);
        }

        [Fact]
        public void Id_WithZero_CanBeSet()
        {
            // Arrange
            var testSetup = new TestSetup();
            var id = 0;

            // Act
            testSetup.Id = id;

            // Assert
            Assert.Equal(0, testSetup.Id);
        }

        [Fact]
        public void Id_WithNegative_CanBeSet()
        {
            // Arrange
            var testSetup = new TestSetup();
            var id = -1;

            // Act
            testSetup.Id = id;

            // Assert
            Assert.Equal(-1, testSetup.Id);
        }

        [Fact]
        public void TypeId_WithZero_CanBeSet()
        {
            // Arrange
            var testSetup = new TestSetup();
            var typeId = 0;

            // Act
            testSetup.TypeId = typeId;

            // Assert
            Assert.Equal(0, testSetup.TypeId);
        }

        [Fact]
        public void TypeId_WithNegative_CanBeSet()
        {
            // Arrange
            var testSetup = new TestSetup();
            var typeId = -5;

            // Act
            testSetup.TypeId = typeId;

            // Assert
            Assert.Equal(-5, testSetup.TypeId);
        }

        [Fact]
        public void Name_WithEmpty_CanBeSet()
        {
            // Arrange
            var testSetup = new TestSetup();
            var name = "";

            // Act
            testSetup.Name = name;

            // Assert
            Assert.Equal("", testSetup.Name);
        }

        [Fact]
        public void Fee_WithZero_CanBeSet()
        {
            // Arrange
            var testSetup = new TestSetup();
            var fee = "0";

            // Act
            testSetup.Fee = fee;

            // Assert
            Assert.Equal("0", testSetup.Fee);
        }

        [Fact]
        public void TypeName_WithNull_CanBeSet()
        {
            // Arrange
            var testSetup = new TestSetup();
            string typeName = null;

            // Act
            testSetup.TypeName = typeName;

            // Assert
            Assert.Null(testSetup.TypeName);
        }
    }
}
