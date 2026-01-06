using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using DiagnosticCenter.Domain.Interfaces.Services;
using DiagnosticCenter.Web.ViewModels;
using DiagnosticCenter.Domain.Entities;
using System.Threading.Tasks;

namespace DiagnosticCenter.Web.Pages.Payment;

[Authorize(Policy = "AccountantOnly")]
public class CreateModel : PageModel
{
    private readonly IPaymentService _paymentService;
    private readonly ITestEntryService _testEntryService;

    [BindProperty]
    public PaymentViewModel Payment { get; set; }

    public CreateModel(IPaymentService paymentService, ITestEntryService testEntryService)
    {
        _paymentService = paymentService;
        _testEntryService = testEntryService;
        Payment = new PaymentViewModel();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var bill = await _testEntryService.GetByBillNoAsync(Payment.BillNo);
        if (bill == null)
        {
            ModelState.AddModelError("Payment.BillNo", "Invalid bill number");
            return Page();
        }

        if (Payment.Amount > bill.DueAmount)
        {
            ModelState.AddModelError("Payment.Amount", "Payment amount cannot exceed the due amount");
            return Page();
        }

        var payment = new Domain.Entities.Payment
        {
            BillNo = Payment.BillNo,
            Amount = Payment.Amount,
            PaymentDate = DateTime.UtcNow
        };

        await _paymentService.CreateAsync(payment);

        return RedirectToPage("Index");
    }
}