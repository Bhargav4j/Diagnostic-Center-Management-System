using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DiagnosticCenter.Application.Interfaces;
using DiagnosticCenter.Application.DTOs;

namespace DiagnosticCenter.Web.Pages.Payment;

public class IndexModel : PageModel
{
    private readonly IPaymentService _paymentService;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(IPaymentService paymentService, ILogger<IndexModel> logger)
    {
        _paymentService = paymentService;
        _logger = logger;
    }

    public IEnumerable<PaymentDto> Payments { get; set; } = new List<PaymentDto>();
    public string? ErrorMessage { get; set; }

    [BindProperty(SupportsGet = true)]
    public int? TestEntryId { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToPage("/Index");
            }

            if (TestEntryId.HasValue)
            {
                Payments = await _paymentService.GetByTestEntryIdAsync(TestEntryId.Value);
            }
            else
            {
                Payments = await _paymentService.GetAllAsync();
            }

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading payments");
            ErrorMessage = "An error occurred while loading payments.";
            return Page();
        }
    }
}
