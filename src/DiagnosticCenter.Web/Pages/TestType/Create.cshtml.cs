using System.ComponentModel.DataAnnotations;
using DiagnosticCenter.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DiagnosticCenter.Web.Pages.TestType;

[Authorize(Policy = "AdminOnly")]
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
    public InputModel Input { get; set; } = new();

    public string? ErrorMessage { get; set; }
    public string? SuccessMessage { get; set; }

    public class InputModel
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(200, ErrorMessage = "Name cannot exceed 200 characters")]
        public string Name { get; set; } = string.Empty;
    }

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
            if (await _testTypeService.ExistsAsync(Input.Name))
            {
                ErrorMessage = $"Test type '{Input.Name}' already exists.";
                return Page();
            }

            var dto = new TestTypeCreateDto
            {
                Name = Input.Name,
                CreatedBy = User.Identity?.Name ?? "System"
            };

            await _testTypeService.CreateAsync(dto);

            _logger.LogInformation("Test type created: {Name}", Input.Name);

            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating test type: {Name}", Input.Name);
            ErrorMessage = "An error occurred while creating the test type.";
            return Page();
        }
    }
}
