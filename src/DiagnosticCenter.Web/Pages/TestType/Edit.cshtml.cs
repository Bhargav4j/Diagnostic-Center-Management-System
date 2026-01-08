using System.ComponentModel.DataAnnotations;
using DiagnosticCenter.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DiagnosticCenter.Web.Pages.TestType;

[Authorize(Policy = "AdminOnly")]
public class EditModel : PageModel
{
    private readonly ITestTypeService _testTypeService;
    private readonly ILogger<EditModel> _logger;

    public EditModel(ITestTypeService testTypeService, ILogger<EditModel> logger)
    {
        _testTypeService = testTypeService;
        _logger = logger;
    }

    [BindProperty(SupportsGet = true)]
    public int Id { get; set; }

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public string? ErrorMessage { get; set; }

    public class InputModel
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(200, ErrorMessage = "Name cannot exceed 200 characters")]
        public string Name { get; set; } = string.Empty;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var testType = await _testTypeService.GetByIdAsync(Id);

        if (testType == null)
        {
            return NotFound();
        }

        Input.Name = testType.Name;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            var dto = new TestTypeUpdateDto
            {
                Name = Input.Name,
                ModifiedBy = User.Identity?.Name ?? "System"
            };

            await _testTypeService.UpdateAsync(Id, dto);

            _logger.LogInformation("Test type updated: ID={Id}, Name={Name}", Id, Input.Name);

            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating test type: ID={Id}", Id);
            ErrorMessage = "An error occurred while updating the test type.";
            return Page();
        }
    }
}
