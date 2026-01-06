using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DiagnosticCenter.Web.ViewModels;
using DiagnosticCenter.Domain.Interfaces.Services;

namespace DiagnosticCenter.Web.Pages.TestType;

[Authorize]
public class DeleteModel : PageModel
{
    private readonly ITestTypeService _testTypeService;

    [BindProperty]
    public TestTypeViewModel TestType { get; set; } = new TestTypeViewModel();

    public DeleteModel(ITestTypeService testTypeService)
    {
        _testTypeService = testTypeService;
    }

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            var testTypeDto = await _testTypeService.GetByIdAsync(id, cancellationToken);

            if (testTypeDto == null)
            {
                TempData["ErrorMessage"] = "Test Type not found.";
                return RedirectToPage("Index");
            }

            // Manually map DTO to ViewModel
            TestType = new TestTypeViewModel
            {
                Id = testTypeDto.Id,
                Name = testTypeDto.Name,
                Description = testTypeDto.Description,
                IsActive = testTypeDto.IsActive,
                CreatedDate = testTypeDto.CreatedDate
            };

            return Page();
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Error retrieving test type details: {ex.Message}";
            return RedirectToPage("Index");
        }
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _testTypeService.DeleteAsync(TestType.Id, cancellationToken);

            TempData["SuccessMessage"] = $"Test Type '{TestType.Name}' deleted successfully.";

            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Error deleting test type: {ex.Message}";
            return RedirectToPage("Index");
        }
    }
}