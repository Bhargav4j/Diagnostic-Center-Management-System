using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace DiagnosticCenter.Web.Pages.TestTypes;

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
    [Required(ErrorMessage = "Name is required")]
    [StringLength(200, ErrorMessage = "Name cannot exceed 200 characters")]
    public string Name { get; set; } = string.Empty;

    [BindProperty]
    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
    public string? Description { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var exists = await _testTypeService.ExistsByNameAsync(Name);
            if (exists)
            {
                ModelState.AddModelError(nameof(Name), "A test type with this name already exists");
                return Page();
            }

            var testType = new TestType
            {
                Name = Name,
                Description = Description,
                CreatedBy = HttpContext.Session.GetString("UserEmail") ?? "System"
            };

            await _testTypeService.CreateAsync(testType);

            _logger.LogInformation("Test type created: {Name}", Name);

            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating test type");
            ModelState.AddModelError(string.Empty, "An error occurred while creating the test type");
            return Page();
        }
    }
}
