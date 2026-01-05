using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DiagnosticCenter.Web.Pages.TestTypes;

public class DetailsModel : PageModel
{
    private readonly ITestTypeService _testTypeService;
    private readonly ILogger<DetailsModel> _logger;

    public DetailsModel(ITestTypeService testTypeService, ILogger<DetailsModel> logger)
    {
        _testTypeService = testTypeService;
        _logger = logger;
    }

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

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving test type details for ID: {Id}", id);
            return NotFound();
        }
    }
}
