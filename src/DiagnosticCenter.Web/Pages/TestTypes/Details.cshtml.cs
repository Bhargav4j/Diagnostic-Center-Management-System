using DiagnosticCenter.Application.DTOs;
using DiagnosticCenter.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DiagnosticCenter.Web.Pages.TestTypes;

/// <summary>
/// Page model for displaying test type details.
/// </summary>
[Authorize]
public class DetailsModel : PageModel
{
    private readonly ITestTypeService _testTypeService;
    private readonly ILogger<DetailsModel> _logger;

    public DetailsModel(ITestTypeService testTypeService, ILogger<DetailsModel> logger)
    {
        _testTypeService = testTypeService ?? throw new ArgumentNullException(nameof(testTypeService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public TestTypeDto? TestType { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        try
        {
            if (id == null)
            {
                _logger.LogWarning("Test type details requested without ID");
                TempData["ErrorMessage"] = "Invalid test type ID.";
                return RedirectToPage("Index");
            }

            _logger.LogInformation("Loading details for test type ID: {Id}", id);
            TestType = await _testTypeService.GetByIdAsync(id.Value);

            if (TestType == null)
            {
                _logger.LogWarning("Test type not found with ID: {Id}", id);
                TempData["ErrorMessage"] = $"Test type with ID {id} not found.";
                return RedirectToPage("Index");
            }

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while loading test type details for ID: {Id}", id);
            TempData["ErrorMessage"] = "An error occurred while loading test type details.";
            return RedirectToPage("Index");
        }
    }
}
