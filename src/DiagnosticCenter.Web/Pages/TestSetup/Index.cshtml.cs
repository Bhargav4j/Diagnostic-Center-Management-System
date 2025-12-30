using DiagnosticCenter.Application.DTOs;
using DiagnosticCenter.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DiagnosticCenter.Web.Pages.TestSetup;

public class IndexModel : PageModel
{
    private readonly ITestSetupService _testSetupService;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ITestSetupService testSetupService, ILogger<IndexModel> logger)
    {
        _testSetupService = testSetupService ?? throw new ArgumentNullException(nameof(testSetupService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public IEnumerable<TestSetupDto> TestSetups { get; set; } = new List<TestSetupDto>();
    public string? SuccessMessage { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        if (HttpContext.Session.GetString("UserEmail") == null)
        {
            return RedirectToPage("/Index");
        }

        try
        {
            TestSetups = await _testSetupService.GetAllAsync();

            if (TempData["SuccessMessage"] != null)
            {
                SuccessMessage = TempData["SuccessMessage"]?.ToString();
            }

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving test setups");
            return RedirectToPage("/Error");
        }
    }
}
