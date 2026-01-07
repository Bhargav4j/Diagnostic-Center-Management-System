using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DiagnosticCenter.Application.Interfaces;
using DiagnosticCenter.Application.DTOs;

namespace DiagnosticCenter.Web.Pages.TestType;

public class IndexModel : PageModel
{
    private readonly ITestTypeService _testTypeService;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ITestTypeService testTypeService, ILogger<IndexModel> logger)
    {
        _testTypeService = testTypeService;
        _logger = logger;
    }

    public IEnumerable<TestTypeDto> TestTypes { get; set; } = new List<TestTypeDto>();
    public string? ErrorMessage { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToPage("/Index");
            }

            TestTypes = await _testTypeService.GetAllAsync();
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading test types");
            ErrorMessage = "An error occurred while loading test types.";
            return Page();
        }
    }
}
