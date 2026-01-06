using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DiagnosticCenter.Web.ViewModels;
using DiagnosticCenter.Domain.Interfaces.Services;

namespace DiagnosticCenter.Web.Pages.TestSetup;

[Authorize]
public class DeleteModel : PageModel
{
    private readonly ITestSetupService _testSetupService;

    [BindProperty]
    public TestSetupDeleteViewModel TestSetup { get; set; } = new TestSetupDeleteViewModel();

    public DeleteModel(ITestSetupService testSetupService)
    {
        _testSetupService = testSetupService;
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

            // Manually map DTO to ViewModel
            TestSetup = new TestSetupDeleteViewModel
            {
                Id = testSetupDto.Id,
                Name = testSetupDto.Name,
                Fee = testSetupDto.Fee,
                TypeName = testSetupDto.TypeName,
                Description = testSetupDto.Description,
                IsActive = testSetupDto.IsActive
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
        try
        {
            await _testSetupService.DeleteAsync(TestSetup.Id, cancellationToken);

            TempData["SuccessMessage"] = $"Test Setup '{TestSetup.Name}' deleted successfully.";

            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Error deleting test setup: {ex.Message}";
            return RedirectToPage("Index");
        }
    }
}

public class TestSetupDeleteViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Fee { get; set; }
    public string TypeName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}
