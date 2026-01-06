using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DiagnosticCenter.Web.ViewModels;
using DiagnosticCenter.Domain.Interfaces.Services;

namespace DiagnosticCenter.Web.Pages.TestSetup;

[Authorize]
public class DetailsModel : PageModel
{
    private readonly ITestSetupService _testSetupService;

    public TestSetupDetailsViewModel TestSetup { get; set; } = new TestSetupDetailsViewModel();

    public DetailsModel(ITestSetupService testSetupService)
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
            TestSetup = new TestSetupDetailsViewModel
            {
                Id = testSetupDto.Id,
                Name = testSetupDto.Name,
                Fee = testSetupDto.Fee,
                TypeId = testSetupDto.TypeId,
                TypeName = testSetupDto.TypeName,
                Description = testSetupDto.Description,
                IsActive = testSetupDto.IsActive,
                CreatedDate = testSetupDto.CreatedDate
            };

            return Page();
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Error retrieving test setup details: {ex.Message}";
            return RedirectToPage("Index");
        }
    }
}

public class TestSetupDetailsViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Fee { get; set; }
    public int TypeId { get; set; }
    public string TypeName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
}
