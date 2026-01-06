using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using DiagnosticCenter.Domain.Interfaces.Services;
using DiagnosticCenter.Web.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiagnosticCenter.Web.Pages.Payment;

[Authorize(Policy = "AccountantOnly")]
public class IndexModel : PageModel
{
    private readonly IPaymentService _paymentService;

    public IndexModel(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    public List<PaymentViewModel> Payments { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var payments = await _paymentService.GetAllAsync();
        Payments = payments.Select(p => new PaymentViewModel
        {
            Id = p.Id,
            BillNo = p.BillNo,
            Amount = p.Amount,
            PaymentDate = p.PaymentDate,
            TestEntryId = p.TestEntryId,
            CreatedDate = p.CreatedDate,
            ModifiedDate = p.ModifiedDate
        }).ToList();

        return Page();
    }
}