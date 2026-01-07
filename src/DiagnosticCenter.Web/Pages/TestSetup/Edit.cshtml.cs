using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DiagnosticCenter.Application.Interfaces;
using DiagnosticCenter.Application.DTOs;
using System.ComponentModel.DataAnnotations;

namespace DiagnosticCenter.Web.Pages.TestSetup;

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
        _testSetupService = testSetupService;
        _testTypeService = testTypeService;
        _logger = logger;
    }

    [BindProperty]
    [Required(ErrorMessage = "Test name is required")]
    [StringLength(100, ErrorMessage = "Test name cannot exceed 100 characters")]
    public string Name { get; set; } = string.Empty;

    [BindProperty]
    [Required(ErrorMessage = "Fee is required")]
    [Range(0, 999999.99, ErrorMessage = "Fee must be between 0 and 999999.99")]
    public decimal Fee { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Test type is required")]
    public int TypeId { get; set; }

    [BindProperty]
    public bool IsActive { get; set; } = true;

    public List<SelectListItem> TestTypes { get; set; } = new();
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

            await LoadTestTypesAsync();

            var testSetup = await _testSetupService.GetByIdAsync(id);
            if (testSetup == null)
            {
                ErrorMessage = "Test setup not found.";
                return Page();
            }

            Name = testSetup.Name;
            Fee = testSetup.Fee;
            TypeId = testSetup.TypeId;
            IsActive = testSetup.IsActive;

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading test setup for editing, ID: {Id}", id);
            ErrorMessage = "An error occurred while loading the test setup.";
            return Page();
        }
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        await LoadTestTypesAsync();

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

            var updateDto = new TestSetupUpdateDto
            {
                Name = Name,
                Fee = Fee,
                TypeId = TypeId,
                IsActive = IsActive
            };

            await _testSetupService.UpdateAsync(id, updateDto);
            SuccessMessage = "Test setup updated successfully.";
            return RedirectToPage("./Index");
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Test setup update validation failed for ID: {Id}", id);
            ErrorMessage = ex.Message;
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating test setup with ID: {Id}", id);
            ErrorMessage = "An error occurred while updating the test setup.";
            return Page();
        }
    }

    private async Task LoadTestTypesAsync()
    {
        var types = await _testTypeService.GetAllAsync();
        TestTypes = types.Select(t => new SelectListItem
        {
            Value = t.Id.ToString(),
            Text = t.Name
        }).ToList();
    }
}
