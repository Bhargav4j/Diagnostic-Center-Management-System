using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using DiagnosticCenter.Application.DTOs;
using DiagnosticCenter.Application.Interfaces;

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

    public SelectList TestTypes { get; set; } = new SelectList(new List<TestTypeDto>(), "Id", "Name");

    [BindProperty]
    public TestSetupCreateViewModel TestSetup { get; set; } = new();

    public async Task OnGetAsync()
    {
        await LoadTestTypesAsync();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await LoadTestTypesAsync();
            return Page();
        }

        try
        {
            if (await _testSetupService.ExistsByNameAsync(TestSetup.Name))
            {
                ModelState.AddModelError("TestSetup.Name", "A test with this name already exists");
                await LoadTestTypesAsync();
                return Page();
            }

            var createDto = new TestSetupCreateDto
            {
                Name = TestSetup.Name,
                Fee = TestSetup.Fee,
                TypeId = TestSetup.TypeId
            };

            await _testSetupService.CreateAsync(createDto);

            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating test setup");
            ModelState.AddModelError(string.Empty, "An error occurred while creating the test setup");
            await LoadTestTypesAsync();
            return Page();
        }
    }

    private async Task LoadTestTypesAsync()
    {
        var testTypes = await _testTypeService.GetAllAsync();
        TestTypes = new SelectList(testTypes, "Id", "Name");
    }

    public class TestSetupCreateViewModel
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(200, ErrorMessage = "Name cannot exceed 200 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Fee is required")]
        [Range(0.01, 999999.99, ErrorMessage = "Fee must be between 0.01 and 999999.99")]
        public decimal Fee { get; set; }

        [Required(ErrorMessage = "Type is required")]
        public int TypeId { get; set; }
    }
}
