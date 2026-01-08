using System;
using System.Collections.Generic;
using Xunit;
using DiagnosticCenter.Domain.Entities;

namespace DiagnosticCenter.Domain.Entities.Tests
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
        public void Name_DefaultsToEmptyString()
        {
            // Arrange & Act
            var testSetup = new TestSetup();

            // Assert
            Assert.Equal(string.Empty, testSetup.Name);
        }

        [Fact]
        public void Name_CanBeSetAndRetrieved()
        {
            // Arrange
            var testSetup = new TestSetup();
            var name = "Complete Blood Count";

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
            var fee = 1500.50m;

            // Act
            testSetup.Fee = fee;

            // Assert
            Assert.Equal(fee, testSetup.Fee);
        }

        [Fact]
        public void Fee_WithZero_CanBeSet()
        {
            // Arrange
            var testSetup = new TestSetup();
            var fee = 0m;

            // Act
            testSetup.Fee = fee;

            // Assert
            Assert.Equal(0m, testSetup.Fee);
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
        public void CreatedDate_CanBeSetAndRetrieved()
        {
            // Arrange
            var testSetup = new TestSetup();
            var createdDate = new DateTime(2024, 1, 1);

            // Act
            testSetup.CreatedDate = createdDate;

            // Assert
            Assert.Equal(createdDate, testSetup.CreatedDate);
        }

        [Fact]
        public void ModifiedDate_CanBeNull()
        {
            // Arrange
            var testSetup = new TestSetup();

            // Assert
            Assert.Null(testSetup.ModifiedDate);
        }

        [Fact]
        public void ModifiedDate_CanBeSetAndRetrieved()
        {
            // Arrange
            var testSetup = new TestSetup();
            var modifiedDate = new DateTime(2024, 6, 15);

            // Act
            testSetup.ModifiedDate = modifiedDate;

            // Assert
            Assert.Equal(modifiedDate, testSetup.ModifiedDate);
        }

        [Fact]
        public void IsActive_DefaultsToFalse()
        {
            // Arrange & Act
            var testSetup = new TestSetup();

            // Assert
            Assert.False(testSetup.IsActive);
        }

        [Fact]
        public void IsActive_CanBeSetToTrue()
        {
            // Arrange
            var testSetup = new TestSetup();

            // Act
            testSetup.IsActive = true;

            // Assert
            Assert.True(testSetup.IsActive);
        }

        [Fact]
        public void CreatedBy_CanBeNull()
        {
            // Arrange
            var testSetup = new TestSetup();

            // Assert
            Assert.Null(testSetup.CreatedBy);
        }

        [Fact]
        public void CreatedBy_CanBeSetAndRetrieved()
        {
            // Arrange
            var testSetup = new TestSetup();
            var createdBy = 1;

            // Act
            testSetup.CreatedBy = createdBy;

            // Assert
            Assert.Equal(createdBy, testSetup.CreatedBy);
        }

        [Fact]
        public void ModifiedBy_CanBeNull()
        {
            // Arrange
            var testSetup = new TestSetup();

            // Assert
            Assert.Null(testSetup.ModifiedBy);
        }

        [Fact]
        public void ModifiedBy_CanBeSetAndRetrieved()
        {
            // Arrange
            var testSetup = new TestSetup();
            var modifiedBy = 2;

            // Act
            testSetup.ModifiedBy = modifiedBy;

            // Assert
            Assert.Equal(modifiedBy, testSetup.ModifiedBy);
        }

        [Fact]
        public void TestType_CanBeNull()
        {
            // Arrange
            var testSetup = new TestSetup();

            // Assert
            Assert.Null(testSetup.TestType);
        }

        [Fact]
        public void TestType_CanBeSetAndRetrieved()
        {
            // Arrange
            var testSetup = new TestSetup();
            var testType = new TestType { Id = 1, Name = "Laboratory" };

            // Act
            testSetup.TestType = testType;

            // Assert
            Assert.Equal(testType, testSetup.TestType);
        }

        [Fact]
        public void TestEntries_InitializesAsEmptyList()
        {
            // Arrange & Act
            var testSetup = new TestSetup();

            // Assert
            Assert.NotNull(testSetup.TestEntries);
            Assert.Empty(testSetup.TestEntries);
        }

        [Fact]
        public void TestEntries_CanAddItems()
        {
            // Arrange
            var testSetup = new TestSetup();
            var testEntry = new TestEntry { Id = 1, PatientName = "John Doe" };

            // Act
            testSetup.TestEntries.Add(testEntry);

            // Assert
            Assert.Single(testSetup.TestEntries);
            Assert.Contains(testEntry, testSetup.TestEntries);
        }

        [Fact]
        public void AllProperties_CanBeSetTogether()
        {
            // Arrange
            var testSetup = new TestSetup
            {
                Id = 5,
                Name = "Lipid Panel",
                Fee = 2500.75m,
                TypeId = 3,
                CreatedDate = new DateTime(2024, 1, 1),
                ModifiedDate = new DateTime(2024, 6, 15),
                IsActive = true,
                CreatedBy = 1,
                ModifiedBy = 2
            };

            // Assert
            Assert.Equal(5, testSetup.Id);
            Assert.Equal("Lipid Panel", testSetup.Name);
            Assert.Equal(2500.75m, testSetup.Fee);
            Assert.Equal(3, testSetup.TypeId);
            Assert.Equal(new DateTime(2024, 1, 1), testSetup.CreatedDate);
            Assert.Equal(new DateTime(2024, 6, 15), testSetup.ModifiedDate);
            Assert.True(testSetup.IsActive);
            Assert.Equal(1, testSetup.CreatedBy);
            Assert.Equal(2, testSetup.ModifiedBy);
        }
    }
}
