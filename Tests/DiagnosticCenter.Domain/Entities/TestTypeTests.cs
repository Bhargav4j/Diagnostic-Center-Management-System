using System;
using System.Collections.Generic;
using Xunit;
using DiagnosticCenter.Domain.Entities;

namespace DiagnosticCenter.Domain.Entities.Tests
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
        public void Name_DefaultsToEmptyString()
        {
            // Arrange & Act
            var testType = new TestType();

            // Assert
            Assert.Equal(string.Empty, testType.Name);
        }

        [Fact]
        public void Name_CanBeSetAndRetrieved()
        {
            // Arrange
            var testType = new TestType();
            var name = "Laboratory Tests";

            // Act
            testType.Name = name;

            // Assert
            Assert.Equal(name, testType.Name);
        }

        [Fact]
        public void Description_CanBeNull()
        {
            // Arrange
            var testType = new TestType();

            // Assert
            Assert.Null(testType.Description);
        }

        [Fact]
        public void Description_CanBeSetAndRetrieved()
        {
            // Arrange
            var testType = new TestType();
            var description = "Comprehensive laboratory test category";

            // Act
            testType.Description = description;

            // Assert
            Assert.Equal(description, testType.Description);
        }

        [Fact]
        public void CreatedDate_CanBeSetAndRetrieved()
        {
            // Arrange
            var testType = new TestType();
            var createdDate = new DateTime(2024, 1, 1);

            // Act
            testType.CreatedDate = createdDate;

            // Assert
            Assert.Equal(createdDate, testType.CreatedDate);
        }

        [Fact]
        public void ModifiedDate_CanBeNull()
        {
            // Arrange
            var testType = new TestType();

            // Assert
            Assert.Null(testType.ModifiedDate);
        }

        [Fact]
        public void ModifiedDate_CanBeSetAndRetrieved()
        {
            // Arrange
            var testType = new TestType();
            var modifiedDate = new DateTime(2024, 6, 15);

            // Act
            testType.ModifiedDate = modifiedDate;

            // Assert
            Assert.Equal(modifiedDate, testType.ModifiedDate);
        }

        [Fact]
        public void IsActive_DefaultsToFalse()
        {
            // Arrange & Act
            var testType = new TestType();

            // Assert
            Assert.False(testType.IsActive);
        }

        [Fact]
        public void IsActive_CanBeSetToTrue()
        {
            // Arrange
            var testType = new TestType();

            // Act
            testType.IsActive = true;

            // Assert
            Assert.True(testType.IsActive);
        }

        [Fact]
        public void CreatedBy_CanBeNull()
        {
            // Arrange
            var testType = new TestType();

            // Assert
            Assert.Null(testType.CreatedBy);
        }

        [Fact]
        public void CreatedBy_CanBeSetAndRetrieved()
        {
            // Arrange
            var testType = new TestType();
            var createdBy = 1;

            // Act
            testType.CreatedBy = createdBy;

            // Assert
            Assert.Equal(createdBy, testType.CreatedBy);
        }

        [Fact]
        public void ModifiedBy_CanBeNull()
        {
            // Arrange
            var testType = new TestType();

            // Assert
            Assert.Null(testType.ModifiedBy);
        }

        [Fact]
        public void ModifiedBy_CanBeSetAndRetrieved()
        {
            // Arrange
            var testType = new TestType();
            var modifiedBy = 2;

            // Act
            testType.ModifiedBy = modifiedBy;

            // Assert
            Assert.Equal(modifiedBy, testType.ModifiedBy);
        }

        [Fact]
        public void TestSetups_InitializesAsEmptyList()
        {
            // Arrange & Act
            var testType = new TestType();

            // Assert
            Assert.NotNull(testType.TestSetups);
            Assert.Empty(testType.TestSetups);
        }

        [Fact]
        public void TestSetups_CanAddItems()
        {
            // Arrange
            var testType = new TestType();
            var testSetup = new TestSetup { Id = 1, Name = "Blood Test" };

            // Act
            testType.TestSetups.Add(testSetup);

            // Assert
            Assert.Single(testType.TestSetups);
            Assert.Contains(testSetup, testType.TestSetups);
        }

        [Fact]
        public void AllProperties_CanBeSetTogether()
        {
            // Arrange
            var testType = new TestType
            {
                Id = 5,
                Name = "Radiology",
                Description = "Imaging and scanning tests",
                CreatedDate = new DateTime(2024, 1, 1),
                ModifiedDate = new DateTime(2024, 6, 15),
                IsActive = true,
                CreatedBy = 1,
                ModifiedBy = 2
            };

            // Assert
            Assert.Equal(5, testType.Id);
            Assert.Equal("Radiology", testType.Name);
            Assert.Equal("Imaging and scanning tests", testType.Description);
            Assert.Equal(new DateTime(2024, 1, 1), testType.CreatedDate);
            Assert.Equal(new DateTime(2024, 6, 15), testType.ModifiedDate);
            Assert.True(testType.IsActive);
            Assert.Equal(1, testType.CreatedBy);
            Assert.Equal(2, testType.ModifiedBy);
        }
    }
}
