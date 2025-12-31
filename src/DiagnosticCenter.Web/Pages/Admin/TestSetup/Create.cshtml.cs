using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using DiagnosticCenter.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DiagnosticCenter.Web.Pages.Admin.TestSetup;

[Authorize(Policy = "AdminOnly")]
public class CreateModel : PageModel
{
    private readonly ITestSetupService _testSetupService;
    private readonly ITestTypeService _testTypeService;

    public CreateModel(ITestSetupService testSetupService, ITestTypeService testTypeService)
    {
        _testSetupService = testSetupService;
        _testTypeService = testTypeService;
    }

    [BindProperty]
    [Required(ErrorMessage = "Test name is required")]
    public string Name { get; set; } = string.Empty;

    [BindProperty]
    [Required(ErrorMessage = "Fee is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Fee must be greater than 0")]
    public decimal Fee { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Test type is required")]
    public int TypeId { get; set; }

    public SelectList TestTypes { get; set; } = new SelectList(Enumerable.Empty<object>());

    public async Task OnGetAsync()
    {
        var testTypes = await _testTypeService.GetAllAsync();
        TestTypes = new SelectList(testTypes, "Id", "Name");
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            var testTypes = await _testTypeService.GetAllAsync();
            TestTypes = new SelectList(testTypes, "Id", "Name");
            return Page();
        }

        var testSetup = new Domain.Entities.TestSetup
        {
            Name = Name,
            Fee = Fee,
            TypeId = TypeId,
            CreatedBy = User.FindFirstValue(ClaimTypes.Email) ?? "System",
            CreatedDate = DateTime.UtcNow,
            IsActive = true
        };

        await _testSetupService.CreateAsync(testSetup);

        return RedirectToPage("Index");
    }
}
