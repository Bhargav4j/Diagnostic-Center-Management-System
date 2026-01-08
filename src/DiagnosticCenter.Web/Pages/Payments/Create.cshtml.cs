using System.ComponentModel.DataAnnotations;
using DiagnosticCenter.Application.DTOs;
using DiagnosticCenter.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DiagnosticCenter.Web.Pages.Payments;

[Authorize]
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
        _paymentService = paymentService ?? throw new ArgumentNullException(nameof(paymentService));
        _testEntryService = testEntryService ?? throw new ArgumentNullException(nameof(testEntryService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [BindProperty]
    public InputModel Input { get; set; } = new InputModel();

    public SelectList TestEntries { get; set; } = new SelectList(Enumerable.Empty<SelectListItem>());

    public class InputModel
    {
        [Required(ErrorMessage = "Bill number is required.")]
        [StringLength(50)]
        [Display(Name = "Bill Number")]
        public string BillNo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Payment amount is required.")]
        [Range(0.01, 999999.99, ErrorMessage = "Amount must be between 0.01 and 999,999.99.")]
        [Display(Name = "Amount")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Payment date is required.")]
        [Display(Name = "Payment Date")]
        public DateTime PaymentDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Test entry is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a test entry.")]
        [Display(Name = "Test Entry")]
        public int TestEntryId { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            await LoadTestEntriesAsync();
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading create page");
            TempData["ErrorMessage"] = "An error occurred while loading the page.";
            return RedirectToPage("Index");
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            if (!ModelState.IsValid)
            {
                await LoadTestEntriesAsync();
                return Page();
            }

            var userIdString = HttpContext.Session.GetString("UserId");
            int? userId = null;
            if (!string.IsNullOrEmpty(userIdString) && int.TryParse(userIdString, out var parsedUserId))
            {
                userId = parsedUserId;
            }

            var createDto = new PaymentCreateDto
            {
                BillNo = Input.BillNo,
                Amount = Input.Amount,
                PaymentDate = Input.PaymentDate,
                TestEntryId = Input.TestEntryId,
                IsActive = Input.IsActive,
                CreatedBy = userId
            };

            _logger.LogInformation("Creating new payment for bill: {BillNo}", Input.BillNo);
            var result = await _paymentService.CreateAsync(createDto);

            _logger.LogInformation("Payment created successfully with ID: {Id}", result.Id);
            TempData["SuccessMessage"] = $"Payment for bill '{result.BillNo}' created successfully.";

            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating payment");
            TempData["ErrorMessage"] = "An error occurred while creating the payment.";
            await LoadTestEntriesAsync();
            return Page();
        }
    }

    private async Task LoadTestEntriesAsync()
    {
        var testEntries = await _testEntryService.GetAllAsync();
        var activeTestEntries = testEntries.Where(t => t.IsActive).ToList();

        var items = activeTestEntries.Select(t => new SelectListItem
        {
            Value = t.Id.ToString(),
            Text = $"{t.BillNo} - {t.PatientName} ({t.TestName})"
        });

        TestEntries = new SelectList(items, "Value", "Text");
    }
}
