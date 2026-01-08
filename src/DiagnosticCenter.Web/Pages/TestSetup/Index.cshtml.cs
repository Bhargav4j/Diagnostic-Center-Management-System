using DiagnosticCenter.Application.DTOs;
using DiagnosticCenter.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DiagnosticCenter.Web.Pages.TestSetup;

public class IndexModel : PageModel
{
    private readonly ITestSetupService _testSetupService;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ITestSetupService testSetupService, ILogger<IndexModel> logger)
    {
        _testSetupService = testSetupService;
        _logger = logger;
    }

    public IEnumerable<TestSetupDto> TestSetups { get; set; } = new List<TestSetupDto>();

    [TempData]
    public string? SuccessMessage { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            TestSetups = await _testSetupService.GetAllAsync();
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving test setups");
            return RedirectToPage("/Error");
        }
    }
}
