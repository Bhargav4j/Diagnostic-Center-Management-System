using DiagnosticCenter.Application.DTOs;
using DiagnosticCenter.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DiagnosticCenter.Web.Pages.TestEntry;

[Authorize]
public class IndexModel : PageModel
{
    private readonly ITestEntryService _testEntryService;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ITestEntryService testEntryService, ILogger<IndexModel> logger)
    {
        _testEntryService = testEntryService ?? throw new ArgumentNullException(nameof(testEntryService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public IEnumerable<TestEntryDto> TestEntries { get; set; } = new List<TestEntryDto>();

    [BindProperty(SupportsGet = true)]
    public string? SearchTerm { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                _logger.LogInformation("Searching test entries with term: {SearchTerm}", SearchTerm);
                TestEntries = await _testEntryService.SearchAsync(SearchTerm);
            }
            else
            {
                _logger.LogInformation("Loading all test entries");
                TestEntries = await _testEntryService.GetAllAsync();
            }

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while loading test entries");
            TempData["ErrorMessage"] = "An error occurred while loading test entries.";
            TestEntries = new List<TestEntryDto>();
            return Page();
        }
    }
}
