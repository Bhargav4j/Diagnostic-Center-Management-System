using DiagnosticCenter.Application.Services;
using DiagnosticCenter.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        _testSetupService = testSetupService ?? throw new ArgumentNullException(nameof(testSetupService));
        _testTypeService = testTypeService ?? throw new ArgumentNullException(nameof(testTypeService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [BindProperty]
    public TestSetupViewModel TestSetup { get; set; } = new();

    public List<SelectListItem> TestTypes { get; set; } = new();

    public async Task<IActionResult> OnGetAsync()
    {
        if (HttpContext.Session.GetString("UserEmail") == null)
        {
            return RedirectToPage("/Index");
        }

        await LoadTestTypesAsync();
        return Page();
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
            var userEmail = HttpContext.Session.GetString("UserEmail") ?? "System";

            var dto = new Application.DTOs.TestSetupCreateDto
            {
                Name = TestSetup.Name,
                Fee = TestSetup.Fee,
                TypeId = TestSetup.TypeId
            };

            await _testSetupService.CreateAsync(dto, userEmail);

            TempData["SuccessMessage"] = "Test setup created successfully!";
            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating test setup");
            ModelState.AddModelError(string.Empty, "An error occurred while creating the test setup.");
            await LoadTestTypesAsync();
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
