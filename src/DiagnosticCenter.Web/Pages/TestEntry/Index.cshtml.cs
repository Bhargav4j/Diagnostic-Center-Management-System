using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DiagnosticCenter.Application.Interfaces;
using DiagnosticCenter.Application.DTOs;

namespace DiagnosticCenter.Web.Pages.TestEntry;

public class IndexModel : PageModel
{
    private readonly ITestEntryService _testEntryService;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ITestEntryService testEntryService, ILogger<IndexModel> logger)
    {
        _testEntryService = testEntryService;
        _logger = logger;
    }

    public IEnumerable<TestEntryDto> TestEntries { get; set; } = new List<TestEntryDto>();
    public string? ErrorMessage { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? SearchTerm { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToPage("/Index");
            }

            if (!string.IsNullOrEmpty(SearchTerm))
            {
                TestEntries = await _testEntryService.SearchAsync(SearchTerm);
            }
            else
            {
                TestEntries = await _testEntryService.GetAllAsync();
            }

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading test entries");
            ErrorMessage = "An error occurred while loading test entries.";
            return Page();
        }
    }
}
