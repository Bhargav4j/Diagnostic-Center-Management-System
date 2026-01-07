using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DiagnosticCenter.Web.Pages.Admin.TestType;

[Authorize(Roles = "Admin")]
public class IndexModel : PageModel
{
    private readonly ITestTypeService _testTypeService;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ITestTypeService testTypeService, ILogger<IndexModel> logger)
    {
        _testTypeService = testTypeService;
        _logger = logger;
    }

    public IEnumerable<Domain.Entities.TestType>? TestTypes { get; set; }
    public string? SuccessMessage { get; set; }
    public string? ErrorMessage { get; set; }

    public async Task OnGetAsync()
    {
        try
        {
            TestTypes = await _testTypeService.GetAllAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading test types");
            ErrorMessage = "Error loading test types. Please try again.";
        }
    }
}
