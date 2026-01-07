using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DiagnosticCenter.Application.Interfaces;
using DiagnosticCenter.Application.DTOs;
using System.ComponentModel.DataAnnotations;

namespace DiagnosticCenter.Web.Pages.TestType;

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
    [Required(ErrorMessage = "Type name is required")]
    [StringLength(100, ErrorMessage = "Type name cannot exceed 100 characters")]
    public string Name { get; set; } = string.Empty;

    [BindProperty]
    public bool IsActive { get; set; } = true;

    public string? ErrorMessage { get; set; }
    public string? SuccessMessage { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToPage("/Index");
            }

            var testType = await _testTypeService.GetByIdAsync(id);
            if (testType == null)
            {
                ErrorMessage = "Test type not found.";
                return Page();
            }

            Name = testType.Name;
            IsActive = testType.IsActive;

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading test type for editing, ID: {Id}", id);
            ErrorMessage = "An error occurred while loading the test type.";
            return Page();
        }
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToPage("/Index");
            }

            var updateDto = new TestTypeUpdateDto
            {
                Name = Name,
                IsActive = IsActive
            };

            await _testTypeService.UpdateAsync(id, updateDto);
            SuccessMessage = "Test type updated successfully.";
            return RedirectToPage("./Index");
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Test type update validation failed for ID: {Id}", id);
            ErrorMessage = ex.Message;
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating test type with ID: {Id}", id);
            ErrorMessage = "An error occurred while updating the test type.";
            return Page();
        }
    }
}
