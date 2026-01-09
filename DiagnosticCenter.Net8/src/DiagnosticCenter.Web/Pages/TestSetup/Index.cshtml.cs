using DiagnosticCenter.Domain.Interfaces.Services;
using DiagnosticCenter.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DiagnosticCenter.Web.Pages.TestSetup;

[Authorize(Roles = "Admin")]
public class IndexModel : PageModel
{
    private readonly ITestSetupService _testSetupService;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(
        ITestSetupService testSetupService,
        ILogger<IndexModel> logger)
    {
        _testSetupService = testSetupService;
        _logger = logger;
    }

    public IEnumerable<TestSetupViewModel> TestSetups { get; set; } = new List<TestSetupViewModel>();

    [BindProperty(SupportsGet = true)]
    public string? SearchTerm { get; set; }

    public async Task OnGetAsync(CancellationToken cancellationToken)
    {
        try
        {
            var testSetups = string.IsNullOrWhiteSpace(SearchTerm)
                ? await _testSetupService.GetAllAsync(cancellationToken)
                : await _testSetupService.SearchAsync(SearchTerm, cancellationToken);

            TestSetups = testSetups.Select(t => new TestSetupViewModel
            {
                Id = t.Id,
                Name = t.Name,
                Fee = t.Fee,
                TypeId = t.TypeId,
                TypeName = t.Type?.Name ?? string.Empty,
                CreatedDate = t.CreatedDate,
                ModifiedDate = t.ModifiedDate,
                IsActive = t.IsActive
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading test setups");
            ModelState.AddModelError(string.Empty, "An error occurred while loading test setups.");
        }
    }
}
