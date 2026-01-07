using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DiagnosticCenter.Application.Interfaces;
using DiagnosticCenter.Application.DTOs;

namespace DiagnosticCenter.Web.Pages.TestSetup;

public class DetailsModel : PageModel
{
    private readonly ITestSetupService _testSetupService;
    private readonly ILogger<DetailsModel> _logger;

    public DetailsModel(ITestSetupService testSetupService, ILogger<DetailsModel> logger)
    {
        _testSetupService = testSetupService;
        _logger = logger;
    }

    public TestSetupDto? TestSetup { get; set; }
    public string? ErrorMessage { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToPage("/Index");
            }

            TestSetup = await _testSetupService.GetByIdAsync(id);
            if (TestSetup == null)
            {
                ErrorMessage = "Test setup not found.";
                return Page();
            }

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading test setup details for ID: {Id}", id);
            ErrorMessage = "An error occurred while loading the test setup details.";
            return Page();
        }
    }
}
