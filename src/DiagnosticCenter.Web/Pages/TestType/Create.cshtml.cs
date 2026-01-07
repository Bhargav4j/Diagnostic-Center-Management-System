using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DiagnosticCenter.Application.Interfaces;
using DiagnosticCenter.Application.DTOs;
using System.ComponentModel.DataAnnotations;

namespace DiagnosticCenter.Web.Pages.TestType;

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
    [Required(ErrorMessage = "Type name is required")]
    [StringLength(100, ErrorMessage = "Type name cannot exceed 100 characters")]
    public string Name { get; set; } = string.Empty;

    [BindProperty]
    public bool IsActive { get; set; } = true;

    public string? ErrorMessage { get; set; }
    public string? SuccessMessage { get; set; }

    public IActionResult OnGet()
    {
        var userEmail = HttpContext.Session.GetString("UserEmail");
        if (string.IsNullOrEmpty(userEmail))
        {
            return RedirectToPage("/Index");
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
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

            var createDto = new TestTypeCreateDto
            {
                Name = Name
            };

            await _testTypeService.CreateAsync(createDto);
            SuccessMessage = "Test type created successfully.";
            return RedirectToPage("./Index");
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Test type creation validation failed");
            ErrorMessage = ex.Message;
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating test type");
            ErrorMessage = "An error occurred while creating the test type.";
            return Page();
        }
    }
}
