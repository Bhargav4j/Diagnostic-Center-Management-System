using Microsoft.AspNetCore.Mvc.RazorPages;
using DiagnosticCenter.Application.DTOs;
using DiagnosticCenter.Application.Interfaces;

namespace DiagnosticCenter.Web.Pages.TestSetup;

public class IndexModel : PageModel
{
    private readonly ITestSetupService _testSetupService;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ITestSetupService testSetupService, ILogger<IndexModel> logger)
    {
        _testSetupService = testSetupService;
        _logger = logger;
    }

    public IEnumerable<TestSetupDto> TestSetups { get; set; } = new List<TestSetupDto>();

    public async Task OnGetAsync()
    {
        try
        {
            TestSetups = await _testSetupService.GetAllAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving test setups");
            TestSetups = new List<TestSetupDto>();
        }
    }
}
