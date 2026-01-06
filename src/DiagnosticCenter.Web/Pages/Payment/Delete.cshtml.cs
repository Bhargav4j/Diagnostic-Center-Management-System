using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DiagnosticCenter.Domain.Interfaces.Services;

namespace DiagnosticCenter.Web.Pages.Payment;

[Authorize]
public class DeleteModel : PageModel
{
    private readonly IPaymentService _paymentService;

    [BindProperty]
    public PaymentDeleteViewModel Payment { get; set; } = new PaymentDeleteViewModel();

    public DeleteModel(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            var paymentDto = await _paymentService.GetByIdAsync(id, cancellationToken);

            if (paymentDto == null)
            {
                TempData["ErrorMessage"] = "Payment not found.";
                return RedirectToPage("Index");
            }

            // Manually map DTO to ViewModel
            Payment = new PaymentDeleteViewModel
            {
                Id = paymentDto.Id,
                BillNo = paymentDto.BillNo,
                MobileNo = paymentDto.MobileNo,
                TotalAmount = paymentDto.TotalAmount,
                PaidAmount = paymentDto.PaidAmount,
                PaymentDate = paymentDto.PaymentDate,
                DueAmount = paymentDto.DueAmount,
                IsActive = paymentDto.IsActive
            };

            return Page();
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Error retrieving payment details: {ex.Message}";
            return RedirectToPage("Index");
        }
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _paymentService.DeleteAsync(Payment.Id, cancellationToken);

            TempData["SuccessMessage"] = $"Payment for Bill '{Payment.BillNo}' deleted successfully.";

            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Error deleting payment: {ex.Message}";
            return RedirectToPage("Index");
        }
    }
}

public class PaymentDeleteViewModel
{
    public int Id { get; set; }
    public string BillNo { get; set; } = string.Empty;
    public string MobileNo { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public DateTime PaymentDate { get; set; }
    public decimal DueAmount { get; set; }
    public bool IsActive { get; set; }
}
