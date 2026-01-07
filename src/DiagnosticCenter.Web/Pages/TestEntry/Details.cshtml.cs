using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DiagnosticCenter.Application.Interfaces;
using DiagnosticCenter.Application.DTOs;

namespace DiagnosticCenter.Web.Pages.TestEntry;

public class DetailsModel : PageModel
{
    private readonly ITestEntryService _testEntryService;
    private readonly ILogger<DetailsModel> _logger;

    public DetailsModel(ITestEntryService testEntryService, ILogger<DetailsModel> logger)
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
            _logger.LogError(ex, "Error loading test entry details for ID: {Id}", id);
            ErrorMessage = "An error occurred while loading the test entry details.";
            return Page();
        }
    }
}
