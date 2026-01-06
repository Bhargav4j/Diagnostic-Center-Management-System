using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DiagnosticCenter.Web.ViewModels;
using DiagnosticCenter.Domain.Interfaces.Services;

namespace DiagnosticCenter.Web.Pages.Payment;

[Authorize]
public class CreateModel : PageModel
{
    private readonly IPaymentService _paymentService;
    private readonly ITestEntryService _testEntryService;

    [BindProperty]
    public PaymentViewModel Payment { get; set; } = new PaymentViewModel();

    public CreateModel(IPaymentService paymentService, ITestEntryService testEntryService)
    {
        _paymentService = paymentService;
        _testEntryService = testEntryService;
    }

    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken)
    {
        try
        {
            // Load test entries for dropdown
            var testEntries = await _testEntryService.GetAllAsync(cancellationToken);
            Payment.TestEntries = testEntries.Select(te => new TestEntryDropdownViewModel
            {
                Id = te.Id,
                BillNo = te.BillNo
            }).ToList();

            // Set default values
            Payment.PaymentDate = DateTime.Today;
            Payment.PaidAmount = 0;
            Payment.TotalAmount = 0;

            return Page();
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Error loading page: {ex.Message}";
            return RedirectToPage("Index");
        }
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            // Reload test entries for dropdown
            var testEntries = await _testEntryService.GetAllAsync(cancellationToken);
            Payment.TestEntries = testEntries.Select(te => new TestEntryDropdownViewModel
            {
                Id = te.Id,
                BillNo = te.BillNo
            }).ToList();
            return Page();
        }

        try
        {
            // Manually map ViewModel to DTO
            var createDto = new PaymentCreateDto
            {
                BillNo = Payment.BillNo,
                MobileNo = Payment.MobileNo,
                TotalAmount = Payment.TotalAmount,
                PaidAmount = Payment.PaidAmount,
                PaymentDate = Payment.PaymentDate,
                TestEntryId = Payment.TestEntryId,
                CreatedBy = User.Identity?.Name ?? "System"
            };

            await _paymentService.CreateAsync(createDto, cancellationToken);

            TempData["SuccessMessage"] = $"Payment for Bill '{Payment.BillNo}' created successfully.";

            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, $"Error creating payment: {ex.Message}");

            // Reload test entries for dropdown
            var testEntries = await _testEntryService.GetAllAsync(cancellationToken);
            Payment.TestEntries = testEntries.Select(te => new TestEntryDropdownViewModel
            {
                Id = te.Id,
                BillNo = te.BillNo
            }).ToList();

            return Page();
        }
    }
}
