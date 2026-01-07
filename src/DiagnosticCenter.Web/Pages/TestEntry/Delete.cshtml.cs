using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DiagnosticCenter.Application.Interfaces;
using DiagnosticCenter.Application.DTOs;

namespace DiagnosticCenter.Web.Pages.TestEntry;

public class DeleteModel : PageModel
{
    private readonly ITestEntryService _testEntryService;
    private readonly ILogger<DeleteModel> _logger;

    public DeleteModel(ITestEntryService testEntryService, ILogger<DeleteModel> logger)
    {
        _testEntryService = testEntryService;
        _logger = logger;
    }

    public TestEntryDto? TestEntry { get; set; }
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

            TestEntry = await _testEntryService.GetByIdAsync(id);
            if (TestEntry == null)
            {
                ErrorMessage = "Test entry not found.";
                return Page();
            }

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading test entry for deletion, ID: {Id}", id);
            ErrorMessage = "An error occurred while loading the test entry.";
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

            await _testEntryService.DeleteAsync(id);
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting test entry with ID: {Id}", id);
            TestEntry = await _testEntryService.GetByIdAsync(id);
            ErrorMessage = "An error occurred while deleting the test entry. It may be referenced by payment records.";
            return Page();
        }
    }
}
