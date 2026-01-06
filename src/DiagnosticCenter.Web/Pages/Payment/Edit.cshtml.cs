using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DiagnosticCenter.Domain.Interfaces.Services;

namespace DiagnosticCenter.Web.Pages.Payment;

[Authorize]
public class EditModel : PageModel
{
    private readonly IPaymentService _paymentService;

    [BindProperty]
    public PaymentEditViewModel Payment { get; set; } = new PaymentEditViewModel();

    public EditModel(IPaymentService paymentService)
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
            Payment = new PaymentEditViewModel
            {
                Id = paymentDto.Id,
                BillNo = paymentDto.BillNo,
                MobileNo = paymentDto.MobileNo,
                TotalAmount = paymentDto.TotalAmount,
                PaidAmount = paymentDto.PaidAmount,
                PaymentDate = paymentDto.PaymentDate,
                IsActive = paymentDto.IsActive,
                CreatedDate = paymentDto.CreatedDate
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
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            // Manually map ViewModel to DTO
            var updateDto = new PaymentUpdateDto
            {
                TotalAmount = Payment.TotalAmount,
                PaidAmount = Payment.PaidAmount,
                PaymentDate = Payment.PaymentDate,
                IsActive = Payment.IsActive,
                ModifiedBy = User.Identity?.Name ?? "System"
            };

            await _paymentService.UpdateAsync(Payment.Id, updateDto, cancellationToken);

            TempData["SuccessMessage"] = $"Payment for Bill '{Payment.BillNo}' updated successfully.";

            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, $"Error updating payment: {ex.Message}");
            return Page();
        }
    }
}

public class PaymentEditViewModel
{
    public int Id { get; set; }
    public string BillNo { get; set; } = string.Empty;
    public string MobileNo { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public DateTime PaymentDate { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
    public decimal DueAmount => TotalAmount - PaidAmount;
}
