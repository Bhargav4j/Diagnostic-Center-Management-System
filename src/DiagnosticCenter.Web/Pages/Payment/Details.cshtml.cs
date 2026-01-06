using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DiagnosticCenter.Domain.Interfaces.Services;

namespace DiagnosticCenter.Web.Pages.Payment;

[Authorize]
public class DetailsModel : PageModel
{
    private readonly IPaymentService _paymentService;

    public PaymentDetailsViewModel Payment { get; set; } = new PaymentDetailsViewModel();

    public DetailsModel(IPaymentService paymentService)
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
            Payment = new PaymentDetailsViewModel
            {
                Id = paymentDto.Id,
                BillNo = paymentDto.BillNo,
                MobileNo = paymentDto.MobileNo,
                TotalAmount = paymentDto.TotalAmount,
                PaidAmount = paymentDto.PaidAmount,
                PaymentDate = paymentDto.PaymentDate,
                TestEntryId = paymentDto.TestEntryId,
                DueAmount = paymentDto.DueAmount,
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
}

public class PaymentDetailsViewModel
{
    public int Id { get; set; }
    public string BillNo { get; set; } = string.Empty;
    public string MobileNo { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public DateTime PaymentDate { get; set; }
    public int TestEntryId { get; set; }
    public decimal DueAmount { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
}
