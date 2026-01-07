using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DiagnosticCenter.Application.Interfaces;
using DiagnosticCenter.Application.DTOs;

namespace DiagnosticCenter.Web.Pages.TestType;

public class DeleteModel : PageModel
{
    private readonly ITestTypeService _testTypeService;
    private readonly ILogger<DeleteModel> _logger;

    public DeleteModel(ITestTypeService testTypeService, ILogger<DeleteModel> logger)
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
            _logger.LogError(ex, "Error loading test type for deletion, ID: {Id}", id);
            ErrorMessage = "An error occurred while loading the test type.";
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

            await _testTypeService.DeleteAsync(id);
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting test type with ID: {Id}", id);
            TestType = await _testTypeService.GetByIdAsync(id);
            ErrorMessage = "An error occurred while deleting the test type. It may be referenced by test setups.";
            return Page();
        }
    }
}
