using System.ComponentModel.DataAnnotations;
using DiagnosticCenter.Application.DTOs;
using DiagnosticCenter.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DiagnosticCenter.Web.Pages.TestSetup;

/// <summary>
/// Page model for creating a new test setup.
/// </summary>
[Authorize]
public class CreateModel : PageModel
{
    private readonly ITestSetupService _testSetupService;
    private readonly ITestTypeService _testTypeService;
    private readonly ILogger<CreateModel> _logger;

    public CreateModel(
        ITestSetupService testSetupService,
        ITestTypeService testTypeService,
        ILogger<CreateModel> logger)
    {
        _testSetupService = testSetupService ?? throw new ArgumentNullException(nameof(testSetupService));
        _testTypeService = testTypeService ?? throw new ArgumentNullException(nameof(testTypeService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [BindProperty]
    public InputModel Input { get; set; } = new InputModel();

    public SelectList TestTypes { get; set; } = new SelectList(Enumerable.Empty<SelectListItem>());

    public class InputModel
    {
        [Required(ErrorMessage = "Test name is required.")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Test name must be between 2 and 200 characters.")]
        [Display(Name = "Test Name")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Test fee is required.")]
        [Range(0.01, 999999.99, ErrorMessage = "Fee must be between 0.01 and 999,999.99.")]
        [Display(Name = "Fee")]
        public decimal Fee { get; set; }

        [Required(ErrorMessage = "Test type is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a test type.")]
        [Display(Name = "Test Type")]
        public int TypeId { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            await LoadTestTypesAsync();
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while loading create page");
            TempData["ErrorMessage"] = "An error occurred while loading the page.";
            return RedirectToPage("Index");
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            if (!ModelState.IsValid)
            {
                await LoadTestTypesAsync();
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
            var createDto = new TestSetupCreateDto
            {
                Name = Input.Name,
                Fee = Input.Fee,
                TypeId = Input.TypeId,
                IsActive = Input.IsActive,
                CreatedBy = userId
            };

            _logger.LogInformation("Creating new test setup: {Name}", Input.Name);
            var result = await _testSetupService.CreateAsync(createDto);

            _logger.LogInformation("Test setup created successfully with ID: {Id}", result.Id);
            TempData["SuccessMessage"] = $"Test '{result.Name}' created successfully.";

            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating test setup: {Name}", Input.Name);
            TempData["ErrorMessage"] = "An error occurred while creating the test. Please try again.";
            await LoadTestTypesAsync();
            return Page();
        }
    }

    private async Task LoadTestTypesAsync()
    {
        var testTypes = await _testTypeService.GetAllAsync();
        var activeTestTypes = testTypes.Where(t => t.IsActive).ToList();
        TestTypes = new SelectList(activeTestTypes, nameof(TestTypeDto.Id), nameof(TestTypeDto.Name));
    }
}
