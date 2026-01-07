using DiagnosticCenter.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace DiagnosticCenter.Web.Pages.Admin.TestType;

[Authorize(Roles = "Admin")]
public class CreateModel : PageModel
{
    private readonly ITestTypeService _testTypeService;
    private readonly ILogger<CreateModel> _logger;

    public CreateModel(ITestTypeService testTypeService, ILogger<CreateModel> logger)
    {
        _testTypeService = testTypeService;
        _logger = logger;
    }

    [BindProperty]
    [Required(ErrorMessage = "Test type name is required")]
    [StringLength(200, ErrorMessage = "Name cannot exceed 200 characters")]
    public string Name { get; set; } = string.Empty;

    [BindProperty]
    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
    public string? Description { get; set; }

    public string? ErrorMessage { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            var testType = new Domain.Entities.TestType
            {
                Name = Name,
                Description = Description,
                CreatedBy = User.Identity?.Name ?? "Admin",
                CreatedDate = DateTime.UtcNow,
                IsActive = true
            };

            await _testTypeService.CreateAsync(testType);
            return RedirectToPage("Index");
        }
        catch (InvalidOperationException ex)
        {
            ErrorMessage = ex.Message;
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating test type");
            ErrorMessage = "An error occurred while creating the test type. Please try again.";
            return Page();
        }
    }
}
