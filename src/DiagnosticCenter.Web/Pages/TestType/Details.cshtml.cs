using DiagnosticCenter.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DiagnosticCenter.Web.Pages.TestType;

[Authorize(Policy = "AdminOnly")]
public class DetailsModel : PageModel
{
    private readonly ITestTypeService _testTypeService;

    public DetailsModel(ITestTypeService testTypeService)
    {
        _testTypeService = testTypeService;
    }

    public TestTypeDto? TestType { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        TestType = await _testTypeService.GetByIdAsync(id);

        if (TestType == null)
        {
            return NotFound();
        }

        return Page();
    }
}
