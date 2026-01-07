using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DiagnosticCenter.Application.Interfaces;
using DiagnosticCenter.Application.DTOs;
using System.ComponentModel.DataAnnotations;

namespace DiagnosticCenter.Web.Pages.Payment;

public class CreateModel : PageModel
{
    private readonly IPaymentService _paymentService;
    private readonly ITestEntryService _testEntryService;
    private readonly ILogger<CreateModel> _logger;

    public CreateModel(
        IPaymentService paymentService,
        ITestEntryService testEntryService,
        ILogger<CreateModel> logger)
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
    public DateTime PaymentDate { get; set; } = DateTime.Today;

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

    public async Task<IActionResult> OnGetAsync()
    {
        var userEmail = HttpContext.Session.GetString("UserEmail");
        if (string.IsNullOrEmpty(userEmail))
        {
            return RedirectToPage("/Index");
        }

        await LoadTestEntriesAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
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

            var createDto = new PaymentCreateDto
            {
                TestEntryId = TestEntryId,
                Amount = Amount,
                PaymentDate = PaymentDate,
                PaymentMode = PaymentMode,
                Notes = Notes
            };

            await _paymentService.CreateAsync(createDto);
            SuccessMessage = "Payment created successfully.";
            return RedirectToPage("./Index");
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Payment creation validation failed");
            ErrorMessage = ex.Message;
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating payment");
            ErrorMessage = "An error occurred while creating the payment.";
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
