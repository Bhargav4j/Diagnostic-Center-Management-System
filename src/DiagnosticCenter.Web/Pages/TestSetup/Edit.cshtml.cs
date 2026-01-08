using System.ComponentModel.DataAnnotations;
using DiagnosticCenter.Application.DTOs;
using DiagnosticCenter.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DiagnosticCenter.Web.Pages.TestSetup;

/// <summary>
/// Page model for editing an existing test setup.
/// </summary>
[Authorize]
public class EditModel : PageModel
{
    private readonly ITestSetupService _testSetupService;
    private readonly ITestTypeService _testTypeService;
    private readonly ILogger<EditModel> _logger;

    public EditModel(
        ITestSetupService testSetupService,
        ITestTypeService testTypeService,
        ILogger<EditModel> logger)
    {
        _testSetupService = testSetupService ?? throw new ArgumentNullException(nameof(testSetupService));
        _testTypeService = testTypeService ?? throw new ArgumentNullException(nameof(testTypeService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [BindProperty]
    public InputModel? Input { get; set; }

    public SelectList TestTypes { get; set; } = new SelectList(Enumerable.Empty<SelectListItem>());

    public class InputModel
    {
        public int Id { get; set; }

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
        public bool IsActive { get; set; }
    }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        try
        {
            if (id == null)
            {
                _logger.LogWarning("Edit page requested without ID");
                TempData["ErrorMessage"] = "Invalid test ID.";
                return RedirectToPage("Index");
            }

            _logger.LogInformation("Loading test setup for editing, ID: {Id}", id);
            var testSetup = await _testSetupService.GetByIdAsync(id.Value);

            if (testSetup == null)
            {
                _logger.LogWarning("Test setup not found with ID: {Id}", id);
                TempData["ErrorMessage"] = $"Test with ID {id} not found.";
                return RedirectToPage("Index");
            }

            // Manual mapping from DTO to InputModel
            Input = new InputModel
            {
                Id = testSetup.Id,
                Name = testSetup.Name,
                Fee = testSetup.Fee,
                TypeId = testSetup.TypeId,
                IsActive = testSetup.IsActive
            };

            await LoadTestTypesAsync();
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while loading test setup for editing, ID: {Id}", id);
            TempData["ErrorMessage"] = "An error occurred while loading the test.";
            return RedirectToPage("Index");
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            if (!ModelState.IsValid || Input == null)
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

            // Manual mapping from InputModel to UpdateDto
            var updateDto = new TestSetupUpdateDto
            {
                Name = Input.Name,
                Fee = Input.Fee,
                TypeId = Input.TypeId,
                IsActive = Input.IsActive,
                ModifiedBy = userId
            };

            _logger.LogInformation("Updating test setup with ID: {Id}", Input.Id);
            await _testSetupService.UpdateAsync(Input.Id, updateDto);

            _logger.LogInformation("Test setup updated successfully, ID: {Id}", Input.Id);
            TempData["SuccessMessage"] = $"Test '{Input.Name}' updated successfully.";

            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating test setup with ID: {Id}", Input?.Id);
            TempData["ErrorMessage"] = "An error occurred while updating the test. Please try again.";
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
