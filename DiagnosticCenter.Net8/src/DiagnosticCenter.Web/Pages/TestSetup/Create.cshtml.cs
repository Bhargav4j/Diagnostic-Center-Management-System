using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Interfaces.Services;
using DiagnosticCenter.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DiagnosticCenter.Web.Pages.TestSetup;

[Authorize(Roles = "Admin")]
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
    public TestSetupViewModel Input { get; set; } = new();

    public IEnumerable<SelectListItem> TestTypes { get; set; } = new List<SelectListItem>();

    public async Task OnGetAsync(CancellationToken cancellationToken)
    {
        await LoadTestTypesAsync(cancellationToken);
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            await LoadTestTypesAsync(cancellationToken);
            return Page();
        }

        try
        {
            var testSetup = new Domain.Entities.TestSetup
            {
                Name = Input.Name,
                Fee = Input.Fee,
                TypeId = Input.TypeId,
                CreatedBy = User.Identity?.Name ?? "System",
                CreatedDate = DateTime.UtcNow,
                IsActive = true
            };

            await _testSetupService.CreateAsync(testSetup, cancellationToken);

            TempData["SuccessMessage"] = "Test setup created successfully.";
            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating test setup");
            ModelState.AddModelError(string.Empty, "An error occurred while creating the test setup.");
            await LoadTestTypesAsync(cancellationToken);
            return Page();
        }
    }

    private async Task LoadTestTypesAsync(CancellationToken cancellationToken)
    {
        var types = await _testTypeService.GetAllAsync(cancellationToken);
        TestTypes = types.Select(t => new SelectListItem
        {
            Value = t.Id.ToString(),
            Text = t.Name
        });
    }
}
