using DiagnosticCenter.Application.DTOs;
using DiagnosticCenter.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DiagnosticCenter.Web.Pages.Payments;

[Authorize]
public class DeleteModel : PageModel
{
    private readonly IPaymentService _paymentService;
    private readonly ILogger<DeleteModel> _logger;

    public DeleteModel(IPaymentService paymentService, ILogger<DeleteModel> logger)
    {
        _paymentService = paymentService ?? throw new ArgumentNullException(nameof(paymentService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [BindProperty]
    public PaymentDto? Payment { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        try
        {
            if (id == null)
            {
                _logger.LogWarning("Delete page requested without ID");
                TempData["ErrorMessage"] = "Invalid payment ID.";
                return RedirectToPage("Index");
            }

            _logger.LogInformation("Loading payment for deletion, ID: {Id}", id);
            Payment = await _paymentService.GetByIdAsync(id.Value);

            if (Payment == null)
            {
                _logger.LogWarning("Payment not found with ID: {Id}", id);
                TempData["ErrorMessage"] = $"Payment with ID {id} not found.";
                return RedirectToPage("Index");
            }

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading payment for deletion, ID: {Id}", id);
            TempData["ErrorMessage"] = "An error occurred while loading the payment.";
            return RedirectToPage("Index");
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            if (Payment == null || Payment.Id <= 0)
            {
                _logger.LogWarning("Delete attempted without valid payment");
                TempData["ErrorMessage"] = "Invalid payment.";
                return RedirectToPage("Index");
            }

            _logger.LogInformation("Deleting payment with ID: {Id}", Payment.Id);
            await _paymentService.DeleteAsync(Payment.Id);

            _logger.LogInformation("Payment deleted successfully, ID: {Id}", Payment.Id);
            TempData["SuccessMessage"] = $"Payment for bill '{Payment.BillNo}' deleted successfully.";

            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting payment with ID: {Id}", Payment?.Id);
            TempData["ErrorMessage"] = "An error occurred while deleting the payment.";
            return Page();
        }
    }
}
