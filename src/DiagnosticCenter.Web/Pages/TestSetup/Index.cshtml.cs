using DiagnosticCenter.Application.DTOs;
using DiagnosticCenter.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DiagnosticCenter.Web.Pages.TestSetup;

/// <summary>
/// Page model for listing all test setups.
/// </summary>
[Authorize]
public class IndexModel : PageModel
{
    private readonly ITestSetupService _testSetupService;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ITestSetupService testSetupService, ILogger<IndexModel> logger)
    {
        _testSetupService = testSetupService ?? throw new ArgumentNullException(nameof(testSetupService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public IEnumerable<TestSetupDto> TestSetups { get; set; } = new List<TestSetupDto>();

    [BindProperty(SupportsGet = true)]
    public string? SearchTerm { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                _logger.LogInformation("Searching test setups with term: {SearchTerm}", SearchTerm);
                TestSetups = await _testSetupService.SearchAsync(SearchTerm);
            }
            else
            {
                _logger.LogInformation("Loading all test setups");
                TestSetups = await _testSetupService.GetAllAsync();
            }

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while loading test setups");
            TempData["ErrorMessage"] = "An error occurred while loading test setups.";
            TestSetups = new List<TestSetupDto>();
            return Page();
        }
    }
}
