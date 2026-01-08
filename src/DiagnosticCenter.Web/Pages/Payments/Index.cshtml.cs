using DiagnosticCenter.Application.DTOs;
using DiagnosticCenter.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DiagnosticCenter.Web.Pages.Payments;

[Authorize]
public class IndexModel : PageModel
{
    private readonly IPaymentService _paymentService;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(IPaymentService paymentService, ILogger<IndexModel> logger)
    {
        _paymentService = paymentService ?? throw new ArgumentNullException(nameof(paymentService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public IEnumerable<PaymentDto> Payments { get; set; } = new List<PaymentDto>();

    [BindProperty(SupportsGet = true)]
    public string? SearchTerm { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                _logger.LogInformation("Searching payments with term: {SearchTerm}", SearchTerm);
                Payments = await _paymentService.SearchAsync(SearchTerm);
            }
            else
            {
                _logger.LogInformation("Loading all payments");
                Payments = await _paymentService.GetAllAsync();
            }

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while loading payments");
            TempData["ErrorMessage"] = "An error occurred while loading payments.";
            Payments = new List<PaymentDto>();
            return Page();
        }
    }
}
