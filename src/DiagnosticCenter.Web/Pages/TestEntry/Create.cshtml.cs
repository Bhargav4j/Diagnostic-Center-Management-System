using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DiagnosticCenter.Application.Interfaces;
using DiagnosticCenter.Application.DTOs;
using System.ComponentModel.DataAnnotations;

namespace DiagnosticCenter.Web.Pages.TestEntry;

public class CreateModel : PageModel
{
    private readonly ITestEntryService _testEntryService;
    private readonly ITestSetupService _testSetupService;
    private readonly ILogger<CreateModel> _logger;

    public CreateModel(
        ITestEntryService testEntryService,
        ITestSetupService testSetupService,
        ILogger<CreateModel> logger)
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
    public DateTime TestDate { get; set; } = DateTime.Today;

    [BindProperty]
    [Required(ErrorMessage = "Total fee is required")]
    [Range(0, 999999.99, ErrorMessage = "Total fee must be between 0 and 999999.99")]
    public decimal TotalFee { get; set; }

    [BindProperty]
    public bool IsPaid { get; set; } = false;

    public List<SelectListItem> TestSetups { get; set; } = new();
    public string? ErrorMessage { get; set; }
    public string? SuccessMessage { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var userEmail = HttpContext.Session.GetString("UserEmail");
        if (string.IsNullOrEmpty(userEmail))
        {
            return RedirectToPage("/Index");
        }

        await LoadTestSetupsAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
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

            var createDto = new TestEntryCreateDto
            {
                BillNo = BillNo,
                PatientName = PatientName,
                PatientAge = PatientAge,
                PatientGender = PatientGender,
                ContactNumber = ContactNumber,
                TestSetupId = TestSetupId,
                TestDate = TestDate,
                TotalFee = TotalFee,
                IsPaid = IsPaid
            };

            await _testEntryService.CreateAsync(createDto);
            SuccessMessage = "Test entry created successfully.";
            return RedirectToPage("./Index");
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Test entry creation validation failed");
            ErrorMessage = ex.Message;
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating test entry");
            ErrorMessage = "An error occurred while creating the test entry.";
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
