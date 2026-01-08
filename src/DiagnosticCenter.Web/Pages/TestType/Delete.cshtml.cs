using DiagnosticCenter.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DiagnosticCenter.Web.Pages.TestType;

[Authorize(Policy = "AdminOnly")]
public class DeleteModel : PageModel
{
    private readonly ITestTypeService _testTypeService;
    private readonly ILogger<DeleteModel> _logger;

    public DeleteModel(ITestTypeService testTypeService, ILogger<DeleteModel> logger)
    {
        _testTypeService = testTypeService;
        _logger = logger;
    }

    [BindProperty(SupportsGet = true)]
    public int Id { get; set; }

    public TestTypeDto? TestType { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        TestType = await _testTypeService.GetByIdAsync(Id);

        if (TestType == null)
        {
            return NotFound();
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            await _testTypeService.DeleteAsync(Id);

            _logger.LogInformation("Test type deleted: ID={Id}", Id);

            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting test type: ID={Id}", Id);
            return RedirectToPage("Index");
        }
    }
}
