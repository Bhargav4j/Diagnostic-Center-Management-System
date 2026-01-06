using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DiagnosticCenter.Web.ViewModels;
using DiagnosticCenter.Domain.Interfaces.Services;

namespace DiagnosticCenter.Web.Pages.TestType;

[Authorize]
public class EditModel : PageModel
{
    private readonly ITestTypeService _testTypeService;

    [BindProperty]
    public TestTypeViewModel TestType { get; set; } = new TestTypeViewModel();

    public EditModel(ITestTypeService testTypeService)
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
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            // Manually map ViewModel to DTO
            var updateDto = new TestTypeUpdateDto
            {
                Name = TestType.Name,
                Description = TestType.Description,
                IsActive = TestType.IsActive,
                ModifiedBy = User.Identity?.Name ?? "System"
            };

            await _testTypeService.UpdateAsync(TestType.Id, updateDto, cancellationToken);

            TempData["SuccessMessage"] = $"Test Type '{TestType.Name}' updated successfully.";

            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, $"Error updating test type: {ex.Message}");
            return Page();
        }
    }
}