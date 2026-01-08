using DiagnosticCenter.Application.DTOs;
using DiagnosticCenter.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DiagnosticCenter.Web.Pages.Payments;

[Authorize]
public class DetailsModel : PageModel
{
    private readonly IPaymentService _paymentService;
    private readonly ILogger<DetailsModel> _logger;

    public DetailsModel(IPaymentService paymentService, ILogger<DetailsModel> logger)
    {
        _paymentService = paymentService ?? throw new ArgumentNullException(nameof(paymentService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public PaymentDto? Payment { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        try
        {
            if (id == null)
            {
                _logger.LogWarning("Payment details requested without ID");
                TempData["ErrorMessage"] = "Invalid payment ID.";
                return RedirectToPage("Index");
            }

            _logger.LogInformation("Loading details for payment ID: {Id}", id);
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
            _logger.LogError(ex, "Error loading payment details for ID: {Id}", id);
            TempData["ErrorMessage"] = "An error occurred while loading payment details.";
            return RedirectToPage("Index");
        }
    }
}
