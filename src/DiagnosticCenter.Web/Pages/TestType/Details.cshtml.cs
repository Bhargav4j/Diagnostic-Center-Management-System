using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DiagnosticCenter.Application.Interfaces;
using DiagnosticCenter.Application.DTOs;

namespace DiagnosticCenter.Web.Pages.TestType;

public class DetailsModel : PageModel
{
    private readonly ITestTypeService _testTypeService;
    private readonly ILogger<DetailsModel> _logger;

    public DetailsModel(ITestTypeService testTypeService, ILogger<DetailsModel> logger)
    {
        _testTypeService = testTypeService;
        _logger = logger;
    }

    public TestTypeDto? TestType { get; set; }
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

            TestType = await _testTypeService.GetByIdAsync(id);
            if (TestType == null)
            {
                ErrorMessage = "Test type not found.";
                return Page();
            }

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading test type details for ID: {Id}", id);
            ErrorMessage = "An error occurred while loading the test type details.";
            return Page();
        }
    }
}
