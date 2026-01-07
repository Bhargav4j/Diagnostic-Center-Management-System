using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DiagnosticCenter.Application.Interfaces;
using DiagnosticCenter.Application.DTOs;
using System.ComponentModel.DataAnnotations;

namespace DiagnosticCenter.Web.Pages.TestEntry;

public class EditModel : PageModel
{
    private readonly ITestEntryService _testEntryService;
    private readonly ITestSetupService _testSetupService;
    private readonly ILogger<EditModel> _logger;

    public EditModel(
        ITestEntryService testEntryService,
        ITestSetupService testSetupService,
        ILogger<EditModel> logger)
    {
        _testEntryService = testEntryService;
        _testSetupService = testSetupService;
        _logger = logger;
    }

    [BindProperty]
    [Required(ErrorMessage = "Bill number is required")]
    [StringLength(50, ErrorMessage = "Bill number cannot exceed 50 characters")]
    public string BillNo { get; set; } = string.Empty;

    [BindProperty]
    [Required(ErrorMessage = "Patient name is required")]
    [StringLength(200, ErrorMessage = "Patient name cannot exceed 200 characters")]
    public string PatientName { get; set; } = string.Empty;

    [BindProperty]
    [Required(ErrorMessage = "Patient age is required")]
    [Range(0, 150, ErrorMessage = "Patient age must be between 0 and 150")]
    public int PatientAge { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Gender is required")]
    public string PatientGender { get; set; } = string.Empty;

    [BindProperty]
    [StringLength(20, ErrorMessage = "Contact number cannot exceed 20 characters")]
    public string? ContactNumber { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Test is required")]
    public int TestSetupId { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Test date is required")]
    public DateTime TestDate { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Total fee is required")]
    [Range(0, 999999.99, ErrorMessage = "Total fee must be between 0 and 999999.99")]
    public decimal TotalFee { get; set; }

    [BindProperty]
    public bool IsPaid { get; set; } = false;

    public List<SelectListItem> TestSetups { get; set; } = new();
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

            await LoadTestSetupsAsync();

            var testEntry = await _testEntryService.GetByIdAsync(id);
            if (testEntry == null)
            {
                ErrorMessage = "Test entry not found.";
                return Page();
            }

            BillNo = testEntry.BillNo;
            PatientName = testEntry.PatientName;
            PatientAge = testEntry.PatientAge;
            PatientGender = testEntry.PatientGender;
            ContactNumber = testEntry.ContactNumber;
            TestSetupId = testEntry.TestSetupId;
            TestDate = testEntry.TestDate;
            TotalFee = testEntry.TotalFee;
            IsPaid = testEntry.IsPaid;

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading test entry for editing, ID: {Id}", id);
            ErrorMessage = "An error occurred while loading the test entry.";
            return Page();
        }
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        await LoadTestSetupsAsync();

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

            var updateDto = new TestEntryUpdateDto
            {
                PatientName = PatientName,
                PatientAge = PatientAge,
                PatientGender = PatientGender,
                ContactNumber = ContactNumber,
                TestSetupId = TestSetupId,
                TestDate = TestDate,
                TotalFee = TotalFee,
                IsPaid = IsPaid
            };

            await _testEntryService.UpdateAsync(id, updateDto);
            SuccessMessage = "Test entry updated successfully.";
            return RedirectToPage("./Index");
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Test entry update validation failed for ID: {Id}", id);
            ErrorMessage = ex.Message;
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating test entry with ID: {Id}", id);
            ErrorMessage = "An error occurred while updating the test entry.";
            return Page();
        }
    }

    private async Task LoadTestSetupsAsync()
    {
        var setups = await _testSetupService.GetAllAsync();
        TestSetups = setups.Where(s => s.IsActive).Select(s => new SelectListItem
        {
            Value = s.Id.ToString(),
            Text = $"{s.Name} - {s.Fee:C}"
        }).ToList();
    }
}
