using System;
using Xunit;
using DiagnosticCenter.Models;

namespace DiagnosticCenter.Models.Tests
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
        public void Id_ShouldSetAndGetValue()
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
        public void Name_ShouldSetAndGetValue()
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
        public void Fee_ShouldSetAndGetValue()
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
        public void TypeId_ShouldSetAndGetValue()
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
        public void TypeName_ShouldSetAndGetValue()
        {
            // Arrange
            var testSetup = new TestSetup();
            string expectedTypeName = "Radiology";

            // Act
            testSetup.TypeName = expectedTypeName;

            // Assert
            Assert.Equal(expectedTypeName, testSetup.TypeName);
        }

        [Fact]
        public void Id_ShouldAcceptZeroValue()
        {
            // Arrange
            var testSetup = new TestSetup();

            // Act
            testSetup.Id = 0;

            // Assert
            Assert.Equal(0, testSetup.Id);
        }

        [Fact]
        public void Id_ShouldAcceptNegativeValue()
        {
            // Arrange
            var testSetup = new TestSetup();

            // Act
            testSetup.Id = -1;

            // Assert
            Assert.Equal(-1, testSetup.Id);
        }

        [Fact]
        public void Name_ShouldAcceptNullValue()
        {
            // Arrange
            var testSetup = new TestSetup();

            // Act
            testSetup.Name = null;

            // Assert
            Assert.Null(testSetup.Name);
        }

        [Fact]
        public void Name_ShouldAcceptEmptyValue()
        {
            // Arrange
            var testSetup = new TestSetup();

            // Act
            testSetup.Name = "";

            // Assert
            Assert.Equal("", testSetup.Name);
        }

        [Fact]
        public void Fee_ShouldAcceptNullValue()
        {
            // Arrange
            var testSetup = new TestSetup();

            // Act
            testSetup.Fee = null;

            // Assert
            Assert.Null(testSetup.Fee);
        }

        [Fact]
        public void Fee_ShouldAcceptEmptyValue()
        {
            // Arrange
            var testSetup = new TestSetup();

            // Act
            testSetup.Fee = "";

            // Assert
            Assert.Equal("", testSetup.Fee);
        }

        [Fact]
        public void TypeId_ShouldAcceptZeroValue()
        {
            // Arrange
            var testSetup = new TestSetup();

            // Act
            testSetup.TypeId = 0;

            // Assert
            Assert.Equal(0, testSetup.TypeId);
        }

        [Fact]
        public void TypeId_ShouldAcceptNegativeValue()
        {
            // Arrange
            var testSetup = new TestSetup();

            // Act
            testSetup.TypeId = -1;

            // Assert
            Assert.Equal(-1, testSetup.TypeId);
        }

        [Fact]
        public void TypeName_ShouldAcceptNullValue()
        {
            // Arrange
            var testSetup = new TestSetup();

            // Act
            testSetup.TypeName = null;

            // Assert
            Assert.Null(testSetup.TypeName);
        }

        [Fact]
        public void TypeName_ShouldAcceptEmptyValue()
        {
            // Arrange
            var testSetup = new TestSetup();

            // Act
            testSetup.TypeName = "";

            // Assert
            Assert.Equal("", testSetup.TypeName);
        }

        [Fact]
        public void Id_ShouldAcceptLargeValue()
        {
            // Arrange
            var testSetup = new TestSetup();

            // Act
            testSetup.Id = 999999;

            // Assert
            Assert.Equal(999999, testSetup.Id);
        }

        [Fact]
        public void TypeId_ShouldAcceptLargeValue()
        {
            // Arrange
            var testSetup = new TestSetup();

            // Act
            testSetup.TypeId = 999999;

            // Assert
            Assert.Equal(999999, testSetup.TypeId);
        }

        [Fact]
        public void Fee_ShouldAcceptDecimalStringValue()
        {
            // Arrange
            var testSetup = new TestSetup();
            string expectedFee = "1000.50";

            // Act
            testSetup.Fee = expectedFee;

            // Assert
            Assert.Equal(expectedFee, testSetup.Fee);
        }

        [Fact]
        public void Name_ShouldAcceptLongValue()
        {
            // Arrange
            var testSetup = new TestSetup();
            string expectedName = new string('A', 1000);

            // Act
            testSetup.Name = expectedName;

            // Assert
            Assert.Equal(expectedName, testSetup.Name);
        }

        [Fact]
        public void TypeName_ShouldAcceptLongValue()
        {
            // Arrange
            var testSetup = new TestSetup();
            string expectedTypeName = new string('B', 1000);

            // Act
            testSetup.TypeName = expectedTypeName;

            // Assert
            Assert.Equal(expectedTypeName, testSetup.TypeName);
        }
    }
}
