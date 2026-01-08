using DiagnosticCenter.Application.DTOs;
using DiagnosticCenter.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DiagnosticCenter.Web.Pages.TestTypes;

/// <summary>
/// Page model for deleting a test type.
/// </summary>
[Authorize]
public class DeleteModel : PageModel
{
    private readonly ITestTypeService _testTypeService;
    private readonly ILogger<DeleteModel> _logger;

    public DeleteModel(ITestTypeService testTypeService, ILogger<DeleteModel> logger)
    {
        _testTypeService = testTypeService ?? throw new ArgumentNullException(nameof(testTypeService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [BindProperty]
    public TestTypeDto? TestType { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        try
        {
            if (id == null)
            {
                _logger.LogWarning("Delete page requested without ID");
                TempData["ErrorMessage"] = "Invalid test type ID.";
                return RedirectToPage("Index");
            }

            _logger.LogInformation("Loading test type for deletion, ID: {Id}", id);
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
            _logger.LogError(ex, "Error occurred while loading test type for deletion, ID: {Id}", id);
            TempData["ErrorMessage"] = "An error occurred while loading the test type.";
            return RedirectToPage("Index");
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            if (TestType == null || TestType.Id <= 0)
            {
                _logger.LogWarning("Delete attempted without valid test type");
                TempData["ErrorMessage"] = "Invalid test type.";
                return RedirectToPage("Index");
            }

            _logger.LogInformation("Deleting test type with ID: {Id}", TestType.Id);
            await _testTypeService.DeleteAsync(TestType.Id);

            _logger.LogInformation("Test type deleted successfully, ID: {Id}", TestType.Id);
            TempData["SuccessMessage"] = $"Test type '{TestType.Name}' deleted successfully.";

            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting test type with ID: {Id}", TestType?.Id);
            TempData["ErrorMessage"] = "An error occurred while deleting the test type. It may be in use by other records.";
            return Page();
        }
    }
}
