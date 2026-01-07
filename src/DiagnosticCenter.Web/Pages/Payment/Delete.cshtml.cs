using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DiagnosticCenter.Application.Interfaces;
using DiagnosticCenter.Application.DTOs;

namespace DiagnosticCenter.Web.Pages.Payment;

public class DeleteModel : PageModel
{
    private readonly IPaymentService _paymentService;
    private readonly ILogger<DeleteModel> _logger;

    public DeleteModel(IPaymentService paymentService, ILogger<DeleteModel> logger)
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
            _logger.LogError(ex, "Error loading payment for deletion, ID: {Id}", id);
            ErrorMessage = "An error occurred while loading the payment.";
            return Page();
        }
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        try
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToPage("/Index");
            }

            await _paymentService.DeleteAsync(id);
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting payment with ID: {Id}", id);
            Payment = await _paymentService.GetByIdAsync(id);
            ErrorMessage = "An error occurred while deleting the payment.";
            return Page();
        }
    }
}
