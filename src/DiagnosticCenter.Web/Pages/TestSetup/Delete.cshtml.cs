using DiagnosticCenter.Application.DTOs;
using DiagnosticCenter.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DiagnosticCenter.Web.Pages.TestSetup;

/// <summary>
/// Page model for deleting a test setup.
/// </summary>
[Authorize]
public class DeleteModel : PageModel
{
    private readonly ITestSetupService _testSetupService;
    private readonly ILogger<DeleteModel> _logger;

    public DeleteModel(ITestSetupService testSetupService, ILogger<DeleteModel> logger)
    {
        _testSetupService = testSetupService ?? throw new ArgumentNullException(nameof(testSetupService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [BindProperty]
    public TestSetupDto? TestSetup { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        try
        {
            if (id == null)
            {
                _logger.LogWarning("Delete page requested without ID");
                TempData["ErrorMessage"] = "Invalid test ID.";
                return RedirectToPage("Index");
            }

            _logger.LogInformation("Loading test setup for deletion, ID: {Id}", id);
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
            _logger.LogError(ex, "Error occurred while loading test setup for deletion, ID: {Id}", id);
            TempData["ErrorMessage"] = "An error occurred while loading the test.";
            return RedirectToPage("Index");
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            if (TestSetup == null || TestSetup.Id <= 0)
            {
                _logger.LogWarning("Delete attempted without valid test setup");
                TempData["ErrorMessage"] = "Invalid test.";
                return RedirectToPage("Index");
            }

            _logger.LogInformation("Deleting test setup with ID: {Id}", TestSetup.Id);
            await _testSetupService.DeleteAsync(TestSetup.Id);

            _logger.LogInformation("Test setup deleted successfully, ID: {Id}", TestSetup.Id);
            TempData["SuccessMessage"] = $"Test '{TestSetup.Name}' deleted successfully.";

            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting test setup with ID: {Id}", TestSetup?.Id);
            TempData["ErrorMessage"] = "An error occurred while deleting the test. It may be in use by other records.";
            return Page();
        }
    }
}
