using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DiagnosticCenter.Web.Pages.TestTypes;

[Authorize(Roles = "Admin")]
public class IndexModel : PageModel
{
    private readonly ITestTypeService _service;

    public IndexModel(ITestTypeService service)
    {
        _service = service;
    }

    public IEnumerable<TestType> TestTypes { get; set; } = new List<TestType>();

    public async Task OnGetAsync()
    {
        TestTypes = await _service.GetAllAsync();
    }
}
