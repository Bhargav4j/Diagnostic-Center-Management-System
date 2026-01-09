using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Threading.Tasks;

namespace DiagnosticCenter.UnitTests.Web;

public class ProgramTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public ProgramTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Application_ShouldStart_Successfully()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act & Assert - If the application fails to start, this test will fail
        Assert.NotNull(client);
    }

    [Fact]
    public void WebApplicationFactory_ShouldCreateInstance()
    {
        // Arrange & Act & Assert
        Assert.NotNull(_factory);
        Assert.NotNull(_factory.Services);
    }
}
