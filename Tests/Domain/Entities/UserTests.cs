using Xunit;
using DiagnosticCenter.Domain.Entities;

namespace DiagnosticCenterTests.Domain.Entities;

public class UserTests
{
    [Fact]
    public void User_Constructor_SetsDefaultValues()
    {
        var user = new User();

        Assert.Equal(0, user.Id);
        Assert.Equal(string.Empty, user.Email);
        Assert.Equal(string.Empty, user.PasswordHash);
        Assert.Equal("System", user.CreatedBy);
        Assert.True(user.IsActive);
    }

    [Fact]
    public void User_SetEmail_UpdatesEmailProperty()
    {
        var user = new User();
        user.Email = "test@example.com";

        Assert.Equal("test@example.com", user.Email);
    }

    [Theory]
    [InlineData("Admin")]
    [InlineData("User")]
    [InlineData("Manager")]
    public void User_SetAccountType_AcceptsVariousTypes(string accountType)
    {
        var user = new User();
        user.AccountType = accountType;

        Assert.Equal(accountType, user.AccountType);
    }
}
