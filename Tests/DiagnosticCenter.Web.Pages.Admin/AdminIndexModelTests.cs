using Xunit;
using DiagnosticCenter.Web.Pages.Admin;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;

namespace DiagnosticCenter.Web.Pages.Admin.Tests;

public class AdminIndexModelTests
{
    private readonly IndexModel _pageModel;

    public AdminIndexModelTests()
    {
        _pageModel = new IndexModel();
        _pageModel.PageContext = new PageContext
        {
            HttpContext = new DefaultHttpContext()
        };
    }

    [Fact]
    public void Constructor_ShouldInitializePageModel()
    {
        // Assert
        Assert.NotNull(_pageModel);
    }

    [Fact]
    public void OnGet_ShouldExecuteWithoutErrors()
    {
        // Act
        _pageModel.OnGet();

        // Assert
        Assert.NotNull(_pageModel);
    }

    [Fact]
    public void PageModel_ShouldInheritFromPageModel()
    {
        // Assert
        Assert.IsAssignableFrom<PageModel>(_pageModel);
    }
}
