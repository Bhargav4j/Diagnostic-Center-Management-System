using System.ComponentModel.DataAnnotations;
using DiagnosticCenter.Application.DTOs;
using DiagnosticCenter.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DiagnosticCenter.Web.Pages.Payments;

[Authorize]
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
        _paymentService = paymentService ?? throw new ArgumentNullException(nameof(paymentService));
        _testEntryService = testEntryService ?? throw new ArgumentNullException(nameof(testEntryService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [BindProperty]
    public InputModel? Input { get; set; }

    public SelectList TestEntries { get; set; } = new SelectList(Enumerable.Empty<SelectListItem>());

    public class InputModel
    {
        public int Id { get; set; }

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
        public DateTime PaymentDate { get; set; }

        [Required(ErrorMessage = "Test entry is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a test entry.")]
        [Display(Name = "Test Entry")]
        public int TestEntryId { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; }
    }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        try
        {
            if (id == null)
            {
                _logger.LogWarning("Edit page requested without ID");
                TempData["ErrorMessage"] = "Invalid payment ID.";
                return RedirectToPage("Index");
            }

            _logger.LogInformation("Loading payment for editing, ID: {Id}", id);
            var payment = await _paymentService.GetByIdAsync(id.Value);

            if (payment == null)
            {
                _logger.LogWarning("Payment not found with ID: {Id}", id);
                TempData["ErrorMessage"] = $"Payment with ID {id} not found.";
                return RedirectToPage("Index");
            }

            Input = new InputModel
            {
                Id = payment.Id,
                BillNo = payment.BillNo,
                Amount = payment.Amount,
                PaymentDate = payment.PaymentDate,
                TestEntryId = payment.TestEntryId,
                IsActive = payment.IsActive
            };

            await LoadTestEntriesAsync();
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading payment for editing, ID: {Id}", id);
            TempData["ErrorMessage"] = "An error occurred while loading the payment.";
            return RedirectToPage("Index");
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            if (!ModelState.IsValid || Input == null)
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

            var updateDto = new PaymentUpdateDto
            {
                BillNo = Input.BillNo,
                Amount = Input.Amount,
                PaymentDate = Input.PaymentDate,
                TestEntryId = Input.TestEntryId,
                IsActive = Input.IsActive,
                ModifiedBy = userId
            };

            _logger.LogInformation("Updating payment with ID: {Id}", Input.Id);
            await _paymentService.UpdateAsync(Input.Id, updateDto);

            _logger.LogInformation("Payment updated successfully, ID: {Id}", Input.Id);
            TempData["SuccessMessage"] = $"Payment for bill '{Input.BillNo}' updated successfully.";

            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating payment with ID: {Id}", Input?.Id);
            TempData["ErrorMessage"] = "An error occurred while updating the payment.";
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
