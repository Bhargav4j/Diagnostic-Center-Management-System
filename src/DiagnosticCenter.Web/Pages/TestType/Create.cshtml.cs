using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DiagnosticCenter.Web.ViewModels;
using DiagnosticCenter.Domain.Interfaces.Services;

namespace DiagnosticCenter.Web.Pages.TestType;

[Authorize]
public class CreateModel : PageModel
{
    private readonly ITestTypeService _testTypeService;

    [BindProperty]
    public TestTypeViewModel TestType { get; set; } = new TestTypeViewModel();

    public CreateModel(ITestTypeService testTypeService)
    {
        _testTypeService = testTypeService;
    }

    public IActionResult OnGet()
    {
        return Page();
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
            var createDto = new TestTypeCreateDto
            {
                Name = TestType.Name,
                Description = TestType.Description,
                CreatedBy = User.Identity?.Name ?? "System"
            };

            var createdTestType = await _testTypeService.CreateAsync(createDto, cancellationToken);

            TempData["SuccessMessage"] = $"Test Type '{createdTestType.Name}' created successfully.";

            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, $"Error creating test type: {ex.Message}");
            return Page();
        }
    }
}