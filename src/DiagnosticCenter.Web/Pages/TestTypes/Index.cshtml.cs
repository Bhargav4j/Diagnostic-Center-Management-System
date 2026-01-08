using DiagnosticCenter.Application.DTOs;
using DiagnosticCenter.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DiagnosticCenter.Web.Pages.TestTypes;

/// <summary>
/// Page model for listing all test types.
/// </summary>
[Authorize]
public class IndexModel : PageModel
{
    private readonly ITestTypeService _testTypeService;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ITestTypeService testTypeService, ILogger<IndexModel> logger)
    {
        _testTypeService = testTypeService ?? throw new ArgumentNullException(nameof(testTypeService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public IEnumerable<TestTypeDto> TestTypes { get; set; } = new List<TestTypeDto>();

    [BindProperty(SupportsGet = true)]
    public string? SearchTerm { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                _logger.LogInformation("Searching test types with term: {SearchTerm}", SearchTerm);
                TestTypes = await _testTypeService.SearchAsync(SearchTerm);
            }
            else
            {
                _logger.LogInformation("Loading all test types");
                TestTypes = await _testTypeService.GetAllAsync();
            }

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while loading test types");
            TempData["ErrorMessage"] = "An error occurred while loading test types.";
            TestTypes = new List<TestTypeDto>();
            return Page();
        }
    }
}
