using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DiagnosticCenter.Web.ViewModels;
using DiagnosticCenter.Domain.Interfaces.Services;

namespace DiagnosticCenter.Web.Pages.TestSetup;

[Authorize]
public class EditModel : PageModel
{
    private readonly ITestSetupService _testSetupService;
    private readonly ITestTypeService _testTypeService;

    [BindProperty]
    public TestSetupEditViewModel TestSetup { get; set; } = new TestSetupEditViewModel();

    public EditModel(ITestSetupService testSetupService, ITestTypeService testTypeService)
    {
        _testSetupService = testSetupService;
        _testTypeService = testTypeService;
    }

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            var testSetupDto = await _testSetupService.GetByIdAsync(id, cancellationToken);

            if (testSetupDto == null)
            {
                TempData["ErrorMessage"] = "Test Setup not found.";
                return RedirectToPage("Index");
            }

            // Load test types for dropdown
            var testTypes = await _testTypeService.GetAllAsync(cancellationToken);

            // Manually map DTO to ViewModel
            TestSetup = new TestSetupEditViewModel
            {
                Id = testSetupDto.Id,
                Name = testSetupDto.Name,
                Fee = testSetupDto.Fee,
                TypeId = testSetupDto.TypeId,
                Description = testSetupDto.Description,
                IsActive = testSetupDto.IsActive,
                CreatedDate = testSetupDto.CreatedDate,
                TestTypes = testTypes.Select(tt => new TestTypeDropdownViewModel
                {
                    Id = tt.Id,
                    Name = tt.Name
                }).ToList()
            };

            return Page();
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Error retrieving test setup details: {ex.Message}";
            return RedirectToPage("Index");
        }
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            // Reload test types for dropdown
            var testTypes = await _testTypeService.GetAllAsync(cancellationToken);
            TestSetup.TestTypes = testTypes.Select(tt => new TestTypeDropdownViewModel
            {
                Id = tt.Id,
                Name = tt.Name
            }).ToList();
            return Page();
        }

        try
        {
            // Manually map ViewModel to DTO
            var updateDto = new TestSetupUpdateDto
            {
                Name = TestSetup.Name,
                Fee = TestSetup.Fee,
                TypeId = TestSetup.TypeId,
                Description = TestSetup.Description,
                IsActive = TestSetup.IsActive,
                ModifiedBy = User.Identity?.Name ?? "System"
            };

            await _testSetupService.UpdateAsync(TestSetup.Id, updateDto, cancellationToken);

            TempData["SuccessMessage"] = $"Test Setup '{TestSetup.Name}' updated successfully.";

            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, $"Error updating test setup: {ex.Message}");

            // Reload test types for dropdown
            var testTypes = await _testTypeService.GetAllAsync(cancellationToken);
            TestSetup.TestTypes = testTypes.Select(tt => new TestTypeDropdownViewModel
            {
                Id = tt.Id,
                Name = tt.Name
            }).ToList();

            return Page();
        }
    }
}

public class TestSetupEditViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Fee { get; set; }
    public int TypeId { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
    public List<TestTypeDropdownViewModel>? TestTypes { get; set; }
}
