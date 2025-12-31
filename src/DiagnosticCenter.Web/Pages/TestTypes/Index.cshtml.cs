using Microsoft.AspNetCore.Mvc.RazorPages;
using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Interfaces.Services;

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
    public string? SuccessMessage { get; set; }

    public async Task OnGetAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            TestTypes = await _testTypeService.GetAllAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching test types");
            TestTypes = new List<TestType>();
        }
    }
}
