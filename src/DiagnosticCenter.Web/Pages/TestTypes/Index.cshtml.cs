using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DiagnosticCenter.Web.Pages.TestTypes;

public class IndexModel : PageModel
{
    private readonly ITestTypeService _testTypeService;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ITestTypeService testTypeService, ILogger<IndexModel> logger)
    {
        _testTypeService = testTypeService;
        _logger = logger;
    }

    public IEnumerable<TestType> TestTypes { get; set; } = new List<TestType>();

    public async Task OnGetAsync()
    {
        try
        {
            TestTypes = await _testTypeService.GetAllAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving test types");
            TestTypes = new List<TestType>();
        }
    }
}
