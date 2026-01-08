using DiagnosticCenter.Application.DTOs;
using DiagnosticCenter.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DiagnosticCenter.Web.Pages.TestSetup;

/// <summary>
/// Page model for displaying test setup details.
/// </summary>
[Authorize]
public class DetailsModel : PageModel
{
    private readonly ITestSetupService _testSetupService;
    private readonly ILogger<DetailsModel> _logger;

    public DetailsModel(ITestSetupService testSetupService, ILogger<DetailsModel> logger)
    {
        _testSetupService = testSetupService ?? throw new ArgumentNullException(nameof(testSetupService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public TestSetupDto? TestSetup { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        try
        {
            if (id == null)
            {
                _logger.LogWarning("Test setup details requested without ID");
                TempData["ErrorMessage"] = "Invalid test ID.";
                return RedirectToPage("Index");
            }

            _logger.LogInformation("Loading details for test setup ID: {Id}", id);
            TestSetup = await _testSetupService.GetByIdAsync(id.Value);

            if (TestSetup == null)
            {
                _logger.LogWarning("Test setup not found with ID: {Id}", id);
                TempData["ErrorMessage"] = $"Test with ID {id} not found.";
                return RedirectToPage("Index");
            }

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while loading test setup details for ID: {Id}", id);
            TempData["ErrorMessage"] = "An error occurred while loading test details.";
            return RedirectToPage("Index");
        }
    }
}
