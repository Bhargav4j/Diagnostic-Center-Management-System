using DiagnosticCenter.Application.DTOs;
using DiagnosticCenter.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DiagnosticCenter.Web.Pages.TestEntry;

[Authorize]
public class DetailsModel : PageModel
{
    private readonly ITestEntryService _testEntryService;
    private readonly ILogger<DetailsModel> _logger;

    public DetailsModel(ITestEntryService testEntryService, ILogger<DetailsModel> logger)
    {
        _testEntryService = testEntryService ?? throw new ArgumentNullException(nameof(testEntryService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public TestEntryDto? TestEntry { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        try
        {
            if (id == null)
            {
                _logger.LogWarning("Test entry details requested without ID");
                TempData["ErrorMessage"] = "Invalid test entry ID.";
                return RedirectToPage("Index");
            }

            _logger.LogInformation("Loading details for test entry ID: {Id}", id);
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
            _logger.LogError(ex, "Error occurred while loading test entry details for ID: {Id}", id);
            TempData["ErrorMessage"] = "An error occurred while loading test entry details.";
            return RedirectToPage("Index");
        }
    }
}
