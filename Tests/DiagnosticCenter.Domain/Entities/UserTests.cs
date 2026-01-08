using System;
using Xunit;
using DiagnosticCenter.Domain.Entities;

namespace DiagnosticCenter.Domain.Entities.Tests
{
    public class UserTests
    {
        [Fact]
        public void Constructor_CreatesInstance()
        {
            // Arrange & Act
            var user = new User();

            // Assert
            Assert.NotNull(user);
        }

        [Fact]
        public void Id_CanBeSetAndRetrieved()
        {
            // Arrange
            var user = new User();
            var id = 1;

            // Act
            user.Id = id;

            // Assert
            Assert.Equal(id, user.Id);
        }

        [Fact]
        public void Email_DefaultsToEmptyString()
        {
            // Arrange & Act
            var user = new User();

            // Assert
            Assert.Equal(string.Empty, user.Email);
        }

        [Fact]
        public void Email_CanBeSetAndRetrieved()
        {
            // Arrange
            var user = new User();
            var email = "user@example.com";

            // Act
            user.Email = email;

            // Assert
            Assert.Equal(email, user.Email);
        }

        [Fact]
        public void PasswordHash_DefaultsToEmptyString()
        {
            // Arrange & Act
            var user = new User();

            // Assert
            Assert.Equal(string.Empty, user.PasswordHash);
        }

        [Fact]
        public void PasswordHash_CanBeSetAndRetrieved()
        {
            // Arrange
            var user = new User();
            var passwordHash = "hashedpassword123";

            // Act
            user.PasswordHash = passwordHash;

            // Assert
            Assert.Equal(passwordHash, user.PasswordHash);
        }

        [Fact]
        public void AccountType_DefaultsToEmptyString()
        {
            // Arrange & Act
            var user = new User();

            // Assert
            Assert.Equal(string.Empty, user.AccountType);
        }

        [Fact]
        public void AccountType_CanBeSetAndRetrieved()
        {
            // Arrange
            var user = new User();
            var accountType = "Admin";

            // Act
            user.AccountType = accountType;

            // Assert
            Assert.Equal(accountType, user.AccountType);
        }

        [Fact]
        public void CreatedDate_CanBeSetAndRetrieved()
        {
            // Arrange
            var user = new User();
            var createdDate = new DateTime(2024, 1, 1);

            // Act
            user.CreatedDate = createdDate;

            // Assert
            Assert.Equal(createdDate, user.CreatedDate);
        }

        [Fact]
        public void ModifiedDate_CanBeNull()
        {
            // Arrange
            var user = new User();

            // Assert
            Assert.Null(user.ModifiedDate);
        }

        [Fact]
        public void ModifiedDate_CanBeSetAndRetrieved()
        {
            // Arrange
            var user = new User();
            var modifiedDate = new DateTime(2024, 6, 15);

            // Act
            user.ModifiedDate = modifiedDate;

            // Assert
            Assert.Equal(modifiedDate, user.ModifiedDate);
        }

        [Fact]
        public void IsActive_DefaultsToFalse()
        {
            // Arrange & Act
            var user = new User();

            // Assert
            Assert.False(user.IsActive);
        }

        [Fact]
        public void IsActive_CanBeSetToTrue()
        {
            // Arrange
            var user = new User();

            // Act
            user.IsActive = true;

            // Assert
            Assert.True(user.IsActive);
        }

        [Fact]
        public void CreatedBy_CanBeNull()
        {
            // Arrange
            var user = new User();

            // Assert
            Assert.Null(user.CreatedBy);
        }

        [Fact]
        public void CreatedBy_CanBeSetAndRetrieved()
        {
            // Arrange
            var user = new User();
            var createdBy = 1;

            // Act
            user.CreatedBy = createdBy;

            // Assert
            Assert.Equal(createdBy, user.CreatedBy);
        }

        [Fact]
        public void ModifiedBy_CanBeNull()
        {
            // Arrange
            var user = new User();

            // Assert
            Assert.Null(user.ModifiedBy);
        }

        [Fact]
        public void ModifiedBy_CanBeSetAndRetrieved()
        {
            // Arrange
            var user = new User();
            var modifiedBy = 2;

            // Act
            user.ModifiedBy = modifiedBy;

            // Assert
            Assert.Equal(modifiedBy, user.ModifiedBy);
        }

        [Fact]
        public void AllProperties_CanBeSetTogether()
        {
            // Arrange
            var user = new User
            {
                Id = 10,
                Email = "admin@example.com",
                PasswordHash = "hashedpass456",
                AccountType = "Staff",
                CreatedDate = new DateTime(2024, 1, 1),
                ModifiedDate = new DateTime(2024, 6, 15),
                IsActive = true,
                CreatedBy = 1,
                ModifiedBy = 2
            };

            // Assert
            Assert.Equal(10, user.Id);
            Assert.Equal("admin@example.com", user.Email);
            Assert.Equal("hashedpass456", user.PasswordHash);
            Assert.Equal("Staff", user.AccountType);
            Assert.Equal(new DateTime(2024, 1, 1), user.CreatedDate);
            Assert.Equal(new DateTime(2024, 6, 15), user.ModifiedDate);
            Assert.True(user.IsActive);
            Assert.Equal(1, user.CreatedBy);
            Assert.Equal(2, user.ModifiedBy);
        }

        [Fact]
        public void Email_WithEmpty_CanBeSet()
        {
            // Arrange
            var user = new User();
            var email = "";

            // Act
            user.Email = email;

            // Assert
            Assert.Equal("", user.Email);
        }

        [Fact]
        public void AccountType_WithDifferentValues_CanBeSet()
        {
            // Arrange
            var user = new User();

            // Act & Assert - Admin
            user.AccountType = "Admin";
            Assert.Equal("Admin", user.AccountType);

            // Act & Assert - Receptionist
            user.AccountType = "Receptionist";
            Assert.Equal("Receptionist", user.AccountType);

            // Act & Assert - Accountant
            user.AccountType = "Accountant";
            Assert.Equal("Accountant", user.AccountType);
        }

        [Fact]
        public void Id_WithZero_CanBeSet()
        {
            // Arrange
            var user = new User();
            var id = 0;

            // Act
            user.Id = id;

            // Assert
            Assert.Equal(0, user.Id);
        }

        [Fact]
        public void CreatedDate_WithMinValue_CanBeSet()
        {
            // Arrange
            var user = new User();
            var createdDate = DateTime.MinValue;

            // Act
            user.CreatedDate = createdDate;

            // Assert
            Assert.Equal(DateTime.MinValue, user.CreatedDate);
        }
    }
}
