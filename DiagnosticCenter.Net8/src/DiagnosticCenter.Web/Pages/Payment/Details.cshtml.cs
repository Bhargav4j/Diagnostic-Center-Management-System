using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using DiagnosticCenter.Domain.Interfaces.Services;
using DiagnosticCenter.Web.ViewModels;
using System.Threading.Tasks;

namespace DiagnosticCenter.Web.Pages.Payment;

[Authorize(Policy = "AccountantOnly")]
public class DetailsModel : PageModel
{
    private readonly IPaymentService _paymentService;

    public DetailsModel(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    public PaymentViewModel Payment { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var payment = await _paymentService.GetByIdAsync(id);

        if (payment == null)
        {
            return NotFound();
        }

        Payment = new PaymentViewModel
        {
            Id = payment.Id,
            BillNo = payment.BillNo,
            Amount = payment.Amount,
            PaymentDate = payment.PaymentDate
        };

        return Page();
    }
}