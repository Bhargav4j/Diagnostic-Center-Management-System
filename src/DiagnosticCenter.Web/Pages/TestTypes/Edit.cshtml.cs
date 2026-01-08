using System.ComponentModel.DataAnnotations;
using DiagnosticCenter.Application.DTOs;
using DiagnosticCenter.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DiagnosticCenter.Web.Pages.TestTypes;

/// <summary>
/// Page model for editing an existing test type.
/// </summary>
[Authorize]
public class EditModel : PageModel
{
    private readonly ITestTypeService _testTypeService;
    private readonly ILogger<EditModel> _logger;

    public EditModel(ITestTypeService testTypeService, ILogger<EditModel> logger)
    {
        _testTypeService = testTypeService ?? throw new ArgumentNullException(nameof(testTypeService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [BindProperty]
    public InputModel? Input { get; set; }

    public class InputModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Test type name is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Test type name must be between 2 and 100 characters.")]
        [Display(Name = "Test Type Name")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        [Display(Name = "Description")]
        public string? Description { get; set; }

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
                TempData["ErrorMessage"] = "Invalid test type ID.";
                return RedirectToPage("Index");
            }

            _logger.LogInformation("Loading test type for editing, ID: {Id}", id);
            var testType = await _testTypeService.GetByIdAsync(id.Value);

            if (testType == null)
            {
                _logger.LogWarning("Test type not found with ID: {Id}", id);
                TempData["ErrorMessage"] = $"Test type with ID {id} not found.";
                return RedirectToPage("Index");
            }

            // Manual mapping from DTO to InputModel
            Input = new InputModel
            {
                Id = testType.Id,
                Name = testType.Name,
                Description = testType.Description,
                IsActive = testType.IsActive
            };

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while loading test type for editing, ID: {Id}", id);
            TempData["ErrorMessage"] = "An error occurred while loading the test type.";
            return RedirectToPage("Index");
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            if (!ModelState.IsValid || Input == null)
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

            // Manual mapping from InputModel to UpdateDto
            var updateDto = new TestTypeUpdateDto
            {
                Name = Input.Name,
                Description = Input.Description,
                IsActive = Input.IsActive,
                ModifiedBy = userId
            };

            _logger.LogInformation("Updating test type with ID: {Id}", Input.Id);
            await _testTypeService.UpdateAsync(Input.Id, updateDto);

            _logger.LogInformation("Test type updated successfully, ID: {Id}", Input.Id);
            TempData["SuccessMessage"] = $"Test type '{Input.Name}' updated successfully.";

            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating test type with ID: {Id}", Input?.Id);
            TempData["ErrorMessage"] = "An error occurred while updating the test type. Please try again.";
            return Page();
        }
    }
}
