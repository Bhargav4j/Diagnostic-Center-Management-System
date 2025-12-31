using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DiagnosticCenter.Web.Pages.Admin.TestSetup;

[Authorize(Policy = "AdminOnly")]
public class IndexModel : PageModel
{
    private readonly ITestSetupService _testSetupService;

    public IndexModel(ITestSetupService testSetupService)
    {
        _testSetupService = testSetupService;
    }

    public IEnumerable<Domain.Entities.TestSetup>? TestSetups { get; set; }

    public async Task OnGetAsync()
    {
        TestSetups = await _testSetupService.GetAllAsync();
    }
}
