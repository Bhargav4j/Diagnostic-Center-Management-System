using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DiagnosticCenter.Web.ViewModels;
using DiagnosticCenter.Domain.Interfaces.Services;

namespace DiagnosticCenter.Web.Pages.TestType;

[Authorize]
public class DetailsModel : PageModel
{
    private readonly ITestTypeService _testTypeService;

    public TestTypeViewModel TestType { get; set; } = new TestTypeViewModel();

    public DetailsModel(ITestTypeService testTypeService)
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
}