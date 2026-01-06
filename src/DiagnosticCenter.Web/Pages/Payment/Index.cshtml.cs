using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DiagnosticCenter.Domain.Interfaces.Services;

namespace DiagnosticCenter.Web.Pages.Payment;

[Authorize]
public class IndexModel : PageModel
{
    private readonly IPaymentService _paymentService;

    public List<PaymentIndexViewModel> Payments { get; set; } = new();

    public IndexModel(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    public async Task OnGetAsync(CancellationToken cancellationToken)
    {
        try
        {
            var paymentDtos = await _paymentService.GetAllAsync(cancellationToken);

            // Manually map DTOs to ViewModels
            Payments = paymentDtos.Select(p => new PaymentIndexViewModel
            {
                Id = p.Id,
                BillNo = p.BillNo,
                MobileNo = p.MobileNo,
                TotalAmount = p.TotalAmount,
                PaidAmount = p.PaidAmount,
                PaymentDate = p.PaymentDate,
                TestEntryId = p.TestEntryId,
                DueAmount = p.DueAmount,
                IsActive = p.IsActive
            }).ToList();
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Error loading payments: {ex.Message}";
        }
    }
}

public class PaymentIndexViewModel
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
}
