using System.ComponentModel.DataAnnotations;
using DiagnosticCenter.Application.DTOs;
using DiagnosticCenter.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DiagnosticCenter.Web.Pages.TestTypes;

/// <summary>
/// Page model for creating a new test type.
/// </summary>
[Authorize]
public class CreateModel : PageModel
{
    private readonly ITestTypeService _testTypeService;
    private readonly ILogger<CreateModel> _logger;

    public CreateModel(ITestTypeService testTypeService, ILogger<CreateModel> logger)
    {
        _testTypeService = testTypeService ?? throw new ArgumentNullException(nameof(testTypeService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [BindProperty]
    public InputModel Input { get; set; } = new InputModel();

    public class InputModel
    {
        [Required(ErrorMessage = "Test type name is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Test type name must be between 2 and 100 characters.")]
        [Display(Name = "Test Type Name")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;
    }

    public IActionResult OnGet()
    {
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Get current user ID from session
            var userIdString = HttpContext.Session.GetString("UserId");
            int? userId = null;
            if (!string.IsNullOrEmpty(userIdString) && int.TryParse(userIdString, out var parsedUserId))
            {
                userId = parsedUserId;
            }

            // Manual mapping from InputModel to CreateDto
            var createDto = new TestTypeCreateDto
            {
                Name = Input.Name,
                Description = Input.Description,
                IsActive = Input.IsActive,
                CreatedBy = userId
            };

            _logger.LogInformation("Creating new test type: {Name}", Input.Name);
            var result = await _testTypeService.CreateAsync(createDto);

            _logger.LogInformation("Test type created successfully with ID: {Id}", result.Id);
            TempData["SuccessMessage"] = $"Test type '{result.Name}' created successfully.";

            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating test type: {Name}", Input.Name);
            TempData["ErrorMessage"] = "An error occurred while creating the test type. Please try again.";
            return Page();
        }
    }
}
