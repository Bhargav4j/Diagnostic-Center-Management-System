using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DiagnosticCenter.Application.Interfaces;
using DiagnosticCenter.Application.DTOs;

namespace DiagnosticCenter.Web.Pages.TestSetup;

public class DeleteModel : PageModel
{
    private readonly ITestSetupService _testSetupService;
    private readonly ILogger<DeleteModel> _logger;

    public DeleteModel(ITestSetupService testSetupService, ILogger<DeleteModel> logger)
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
            _logger.LogError(ex, "Error loading test setup for deletion, ID: {Id}", id);
            ErrorMessage = "An error occurred while loading the test setup.";
            return Page();
        }
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        try
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToPage("/Index");
            }

            await _testSetupService.DeleteAsync(id);
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting test setup with ID: {Id}", id);
            TestSetup = await _testSetupService.GetByIdAsync(id);
            ErrorMessage = "An error occurred while deleting the test setup. It may be referenced by other records.";
            return Page();
        }
    }
}
