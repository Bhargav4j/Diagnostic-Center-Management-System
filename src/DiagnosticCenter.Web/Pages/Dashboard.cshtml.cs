using DiagnosticCenter.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DiagnosticCenter.Web.Pages;

[Authorize]
public class DashboardModel : PageModel
{
    private readonly ITestTypeService _testTypeService;
    private readonly ITestSetupService _testSetupService;
    private readonly ITestEntryService _testEntryService;
    private readonly IPaymentService _paymentService;
    private readonly ILogger<DashboardModel> _logger;

    public DashboardModel(
        ITestTypeService testTypeService,
        ITestSetupService testSetupService,
        ITestEntryService testEntryService,
        IPaymentService paymentService,
        ILogger<DashboardModel> logger)
    {
        _testTypeService = testTypeService ?? throw new ArgumentNullException(nameof(testTypeService));
        _testSetupService = testSetupService ?? throw new ArgumentNullException(nameof(testSetupService));
        _testEntryService = testEntryService ?? throw new ArgumentNullException(nameof(testEntryService));
        _paymentService = paymentService ?? throw new ArgumentNullException(nameof(paymentService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public int TotalTestTypes { get; set; }
    public int TotalTestSetups { get; set; }
    public int TotalTestEntries { get; set; }
    public int TotalPayments { get; set; }
    public decimal TotalRevenue { get; set; }
    public decimal TotalPaid { get; set; }
    public decimal TotalDue { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            _logger.LogInformation("Loading dashboard data");

            // Load statistics
            var testTypes = await _testTypeService.GetAllAsync();
            TotalTestTypes = testTypes.Count();

            var testSetups = await _testSetupService.GetAllAsync();
            TotalTestSetups = testSetups.Count();

            var testEntries = await _testEntryService.GetAllAsync();
            TotalTestEntries = testEntries.Count();
            TotalRevenue = testEntries.Sum(t => t.TotalAmount);
            TotalPaid = testEntries.Sum(t => t.PaidAmount);
            TotalDue = testEntries.Sum(t => t.DueAmount);

            var payments = await _paymentService.GetAllAsync();
            TotalPayments = payments.Count();

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while loading dashboard");
            TempData["ErrorMessage"] = "An error occurred while loading dashboard data.";
            return Page();
        }
    }
}
