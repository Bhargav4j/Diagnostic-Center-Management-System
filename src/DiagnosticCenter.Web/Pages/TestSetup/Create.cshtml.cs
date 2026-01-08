using DiagnosticCenter.Application.DTOs;
using DiagnosticCenter.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace DiagnosticCenter.Web.Pages.TestSetup;

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
        _testSetupService = testSetupService;
        _testTypeService = testTypeService;
        _logger = logger;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new InputModel();

    public SelectList TestTypes { get; set; } = new SelectList(new List<TestTypeDto>(), "Id", "Name");

    public string? ErrorMessage { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        await LoadTestTypes();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await LoadTestTypes();
            return Page();
        }

        try
        {
            if (await _testSetupService.ExistsByNameAsync(Input.Name))
            {
                ErrorMessage = $"Test setup with name '{Input.Name}' already exists";
                await LoadTestTypes();
                return Page();
            }

            var dto = new TestSetupCreateDto
            {
                Name = Input.Name,
                Fee = Input.Fee,
                TypeId = Input.TypeId,
                CreatedBy = HttpContext.Session.GetString("admin") ?? "System"
            };

            await _testSetupService.CreateAsync(dto);

            TempData["SuccessMessage"] = "Test setup created successfully";
            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating test setup");
            ErrorMessage = "An error occurred while creating the test setup";
            await LoadTestTypes();
            return Page();
        }
    }

    private async Task LoadTestTypes()
    {
        var testTypes = await _testTypeService.GetAllAsync();
        TestTypes = new SelectList(testTypes, "Id", "Name");
    }

    public class InputModel
    {
        [Required(ErrorMessage = "Test name is required")]
        [StringLength(200, ErrorMessage = "Test name must not exceed 200 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Fee is required")]
        [Range(0.01, 999999.99, ErrorMessage = "Fee must be between 0.01 and 999999.99")]
        public decimal Fee { get; set; }

        [Required(ErrorMessage = "Test type is required")]
        public int TypeId { get; set; }
    }
}
