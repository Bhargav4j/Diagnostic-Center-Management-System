using DiagnosticCenter.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DiagnosticCenter.Web.Pages.TestType;

[Authorize(Policy = "AdminOnly")]
public class IndexModel : PageModel
{
    private readonly ITestTypeService _testTypeService;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ITestTypeService testTypeService, ILogger<IndexModel> logger)
    {
        _testTypeService = testTypeService;
        _logger = logger;
    }

    public IEnumerable<TestTypeDto> TestTypes { get; set; } = new List<TestTypeDto>();

    public async Task OnGetAsync()
    {
        try
        {
            TestTypes = await _testTypeService.GetAllAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading test types");
            TestTypes = new List<TestTypeDto>();
        }
    }
}
