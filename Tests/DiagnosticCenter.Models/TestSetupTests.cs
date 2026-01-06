using System;
using Xunit;
using DiagnosticCenter.Models;

namespace Tests.DiagnosticCenter.Models
{
    public class TestSetupTests
    {
        [Fact]
        public void Constructor_ShouldCreateInstance()
        {
            // Arrange & Act
            var testSetup = new TestSetup();

            // Assert
            Assert.NotNull(testSetup);
        }

        [Fact]
        public void Id_SetAndGet_ShouldWork()
        {
            // Arrange
            var testSetup = new TestSetup();
            int expectedId = 1;

            // Act
            testSetup.Id = expectedId;

            // Assert
            Assert.Equal(expectedId, testSetup.Id);
        }

        [Fact]
        public void Name_SetAndGet_ShouldWork()
        {
            // Arrange
            var testSetup = new TestSetup();
            string expectedName = "Blood Test";

            // Act
            testSetup.Name = expectedName;

            // Assert
            Assert.Equal(expectedName, testSetup.Name);
        }

        [Fact]
        public void Fee_SetAndGet_ShouldWork()
        {
            // Arrange
            var testSetup = new TestSetup();
            string expectedFee = "500";

            // Act
            testSetup.Fee = expectedFee;

            // Assert
            Assert.Equal(expectedFee, testSetup.Fee);
        }

        [Fact]
        public void TypeId_SetAndGet_ShouldWork()
        {
            // Arrange
            var testSetup = new TestSetup();
            int expectedTypeId = 1;

            // Act
            testSetup.TypeId = expectedTypeId;

            // Assert
            Assert.Equal(expectedTypeId, testSetup.TypeId);
        }

        [Fact]
        public void TypeName_SetAndGet_ShouldWork()
        {
            // Arrange
            var testSetup = new TestSetup();
            string expectedTypeName = "Lab Test";

            // Act
            testSetup.TypeName = expectedTypeName;

            // Assert
            Assert.Equal(expectedTypeName, testSetup.TypeName);
        }

        [Fact]
        public void AllProperties_SetAndGet_ShouldWork()
        {
            // Arrange
            var testSetup = new TestSetup();
            int id = 2;
            string name = "X-Ray";
            string fee = "1500";
            int typeId = 3;
            string typeName = "Imaging";

            // Act
            testSetup.Id = id;
            testSetup.Name = name;
            testSetup.Fee = fee;
            testSetup.TypeId = typeId;
            testSetup.TypeName = typeName;

            // Assert
            Assert.Equal(id, testSetup.Id);
            Assert.Equal(name, testSetup.Name);
            Assert.Equal(fee, testSetup.Fee);
            Assert.Equal(typeId, testSetup.TypeId);
            Assert.Equal(typeName, testSetup.TypeName);
        }

        [Fact]
        public void Id_WithZeroValue_ShouldWork()
        {
            // Arrange
            var testSetup = new TestSetup();

            // Act
            testSetup.Id = 0;

            // Assert
            Assert.Equal(0, testSetup.Id);
        }

        [Fact]
        public void TypeId_WithZeroValue_ShouldWork()
        {
            // Arrange
            var testSetup = new TestSetup();

            // Act
            testSetup.TypeId = 0;

            // Assert
            Assert.Equal(0, testSetup.TypeId);
        }

        [Fact]
        public void Name_WithEmptyString_ShouldWork()
        {
            // Arrange
            var testSetup = new TestSetup();

            // Act
            testSetup.Name = "";

            // Assert
            Assert.Equal("", testSetup.Name);
        }

        [Fact]
        public void Fee_WithEmptyString_ShouldWork()
        {
            // Arrange
            var testSetup = new TestSetup();

            // Act
            testSetup.Fee = "";

            // Assert
            Assert.Equal("", testSetup.Fee);
        }

        [Fact]
        public void TypeName_WithEmptyString_ShouldWork()
        {
            // Arrange
            var testSetup = new TestSetup();

            // Act
            testSetup.TypeName = "";

            // Assert
            Assert.Equal("", testSetup.TypeName);
        }

        [Fact]
        public void Id_WithNegativeValue_ShouldWork()
        {
            // Arrange
            var testSetup = new TestSetup();

            // Act
            testSetup.Id = -1;

            // Assert
            Assert.Equal(-1, testSetup.Id);
        }
    }
}
