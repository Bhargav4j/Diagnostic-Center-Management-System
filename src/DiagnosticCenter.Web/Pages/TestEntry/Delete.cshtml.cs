using DiagnosticCenter.Application.DTOs;
using DiagnosticCenter.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DiagnosticCenter.Web.Pages.TestEntry;

[Authorize]
public class DeleteModel : PageModel
{
    private readonly ITestEntryService _testEntryService;
    private readonly ILogger<DeleteModel> _logger;

    public DeleteModel(ITestEntryService testEntryService, ILogger<DeleteModel> logger)
    {
        _testEntryService = testEntryService ?? throw new ArgumentNullException(nameof(testEntryService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [BindProperty]
    public TestEntryDto? TestEntry { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        try
        {
            if (id == null)
            {
                _logger.LogWarning("Delete page requested without ID");
                TempData["ErrorMessage"] = "Invalid test entry ID.";
                return RedirectToPage("Index");
            }

            _logger.LogInformation("Loading test entry for deletion, ID: {Id}", id);
            TestEntry = await _testEntryService.GetByIdAsync(id.Value);

            if (TestEntry == null)
            {
                _logger.LogWarning("Test entry not found with ID: {Id}", id);
                TempData["ErrorMessage"] = $"Test entry with ID {id} not found.";
                return RedirectToPage("Index");
            }

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading test entry for deletion, ID: {Id}", id);
            TempData["ErrorMessage"] = "An error occurred while loading the test entry.";
            return RedirectToPage("Index");
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            if (TestEntry == null || TestEntry.Id <= 0)
            {
                _logger.LogWarning("Delete attempted without valid test entry");
                TempData["ErrorMessage"] = "Invalid test entry.";
                return RedirectToPage("Index");
            }

            _logger.LogInformation("Deleting test entry with ID: {Id}", TestEntry.Id);
            await _testEntryService.DeleteAsync(TestEntry.Id);

            _logger.LogInformation("Test entry deleted successfully, ID: {Id}", TestEntry.Id);
            TempData["SuccessMessage"] = $"Test entry for '{TestEntry.PatientName}' deleted successfully.";

            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting test entry with ID: {Id}", TestEntry?.Id);
            TempData["ErrorMessage"] = "An error occurred while deleting the test entry. It may be in use by other records.";
            return Page();
        }
    }
}
