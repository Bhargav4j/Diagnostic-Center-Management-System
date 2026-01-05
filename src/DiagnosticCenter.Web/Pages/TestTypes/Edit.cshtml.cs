using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace DiagnosticCenter.Web.Pages.TestTypes;

public class EditModel : PageModel
{
    private readonly ITestTypeService _testTypeService;
    private readonly ILogger<EditModel> _logger;

    public EditModel(ITestTypeService testTypeService, ILogger<EditModel> logger)
    {
        _testTypeService = testTypeService;
        _logger = logger;
    }

    [BindProperty]
    public int Id { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Name is required")]
    [StringLength(200, ErrorMessage = "Name cannot exceed 200 characters")]
    public string Name { get; set; } = string.Empty;

    [BindProperty]
    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
    public string? Description { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            var testType = await _testTypeService.GetByIdAsync(id);

            if (testType == null)
            {
                return NotFound();
            }

            Id = testType.Id;
            Name = testType.Name;
            Description = testType.Description;

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading test type for edit: {Id}", id);
            return NotFound();
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var testType = new TestType
            {
                Id = Id,
                Name = Name,
                Description = Description,
                ModifiedBy = HttpContext.Session.GetString("UserEmail") ?? "System"
            };

            await _testTypeService.UpdateAsync(Id, testType);

            _logger.LogInformation("Test type updated: {Id}", Id);

            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating test type: {Id}", Id);
            ModelState.AddModelError(string.Empty, "An error occurred while updating the test type");
            return Page();
        }
    }
}
