using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DiagnosticCenter.Web.Pages.TestTypes;

public class DeleteModel : PageModel
{
    private readonly ITestTypeService _testTypeService;
    private readonly ILogger<DeleteModel> _logger;

    public DeleteModel(ITestTypeService testTypeService, ILogger<DeleteModel> logger)
    {
        _testTypeService = testTypeService;
        _logger = logger;
    }

    [BindProperty]
    public int Id { get; set; }

    public TestType? TestType { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            TestType = await _testTypeService.GetByIdAsync(id);

            if (TestType == null)
            {
                return NotFound();
            }

            Id = TestType.Id;
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading test type for delete: {Id}", id);
            return NotFound();
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            await _testTypeService.DeleteAsync(Id);

            _logger.LogInformation("Test type deleted: {Id}", Id);

            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting test type: {Id}", Id);
            ModelState.AddModelError(string.Empty, "An error occurred while deleting the test type");
            return Page();
        }
    }
}
