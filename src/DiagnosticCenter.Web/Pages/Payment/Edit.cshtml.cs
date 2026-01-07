using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DiagnosticCenter.Application.Interfaces;
using DiagnosticCenter.Application.DTOs;
using System.ComponentModel.DataAnnotations;

namespace DiagnosticCenter.Web.Pages.Payment;

public class EditModel : PageModel
{
    private readonly IPaymentService _paymentService;
    private readonly ITestEntryService _testEntryService;
    private readonly ILogger<EditModel> _logger;

    public EditModel(
        IPaymentService paymentService,
        ITestEntryService testEntryService,
        ILogger<EditModel> logger)
    {
        _paymentService = paymentService;
        _testEntryService = testEntryService;
        _logger = logger;
    }

    [BindProperty]
    [Required(ErrorMessage = "Test entry is required")]
    public int TestEntryId { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Amount is required")]
    [Range(0.01, 999999.99, ErrorMessage = "Amount must be between 0.01 and 999999.99")]
    public decimal Amount { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Payment date is required")]
    public DateTime PaymentDate { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Payment mode is required")]
    [StringLength(50, ErrorMessage = "Payment mode cannot exceed 50 characters")]
    public string PaymentMode { get; set; } = string.Empty;

    [BindProperty]
    [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters")]
    public string? Notes { get; set; }

    public List<SelectListItem> TestEntries { get; set; } = new();
    public string? ErrorMessage { get; set; }
    public string? SuccessMessage { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToPage("/Index");
            }

            await LoadTestEntriesAsync();

            var payment = await _paymentService.GetByIdAsync(id);
            if (payment == null)
            {
                ErrorMessage = "Payment not found.";
                return Page();
            }

            TestEntryId = payment.TestEntryId;
            Amount = payment.Amount;
            PaymentDate = payment.PaymentDate;
            PaymentMode = payment.PaymentMode;
            Notes = payment.Notes;

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading payment for editing, ID: {Id}", id);
            ErrorMessage = "An error occurred while loading the payment.";
            return Page();
        }
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        await LoadTestEntriesAsync();

        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToPage("/Index");
            }

            var updateDto = new PaymentUpdateDto
            {
                TestEntryId = TestEntryId,
                Amount = Amount,
                PaymentDate = PaymentDate,
                PaymentMode = PaymentMode,
                Notes = Notes
            };

            await _paymentService.UpdateAsync(id, updateDto);
            SuccessMessage = "Payment updated successfully.";
            return RedirectToPage("./Index");
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Payment update validation failed for ID: {Id}", id);
            ErrorMessage = ex.Message;
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating payment with ID: {Id}", id);
            ErrorMessage = "An error occurred while updating the payment.";
            return Page();
        }
    }

    private async Task LoadTestEntriesAsync()
    {
        var entries = await _testEntryService.GetAllAsync();
        TestEntries = entries.Select(e => new SelectListItem
        {
            Value = e.Id.ToString(),
            Text = $"{e.BillNo} - {e.PatientName} ({e.TotalFee:C})"
        }).ToList();
    }
}
