using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DiagnosticCenter.Application.Interfaces;
using DiagnosticCenter.Application.DTOs;

namespace DiagnosticCenter.Web.Pages.Payment;

public class DetailsModel : PageModel
{
    private readonly IPaymentService _paymentService;
    private readonly ILogger<DetailsModel> _logger;

    public DetailsModel(IPaymentService paymentService, ILogger<DetailsModel> logger)
    {
        _paymentService = paymentService;
        _logger = logger;
    }

    public PaymentDto? Payment { get; set; }
    public string? ErrorMessage { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToPage("/Index");
            }

            Payment = await _paymentService.GetByIdAsync(id);
            if (Payment == null)
            {
                ErrorMessage = "Payment not found.";
                return Page();
            }

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading payment details for ID: {Id}", id);
            ErrorMessage = "An error occurred while loading the payment details.";
            return Page();
        }
    }
}
