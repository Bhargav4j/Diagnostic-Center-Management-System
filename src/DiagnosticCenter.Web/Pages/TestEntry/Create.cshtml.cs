using System.ComponentModel.DataAnnotations;
using DiagnosticCenter.Application.DTOs;
using DiagnosticCenter.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DiagnosticCenter.Web.Pages.TestEntry;

[Authorize]
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
        _testEntryService = testEntryService ?? throw new ArgumentNullException(nameof(testEntryService));
        _testSetupService = testSetupService ?? throw new ArgumentNullException(nameof(testSetupService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [BindProperty]
    public InputModel Input { get; set; } = new InputModel();

    public SelectList Tests { get; set; } = new SelectList(Enumerable.Empty<SelectListItem>());

    public class InputModel
    {
        [Required(ErrorMessage = "Patient name is required.")]
        [StringLength(200, MinimumLength = 2)]
        [Display(Name = "Patient Name")]
        public string PatientName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Date of birth is required.")]
        [Display(Name = "Date of Birth")]
        public DateTime DateOfBirth { get; set; } = DateTime.Now.AddYears(-30);

        [Required(ErrorMessage = "Mobile number is required.")]
        [StringLength(20, MinimumLength = 10)]
        [Display(Name = "Mobile Number")]
        public string MobileNo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bill number is required.")]
        [StringLength(50)]
        [Display(Name = "Bill Number")]
        public string BillNo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Total amount is required.")]
        [Range(0.01, 999999.99)]
        [Display(Name = "Total Amount")]
        public decimal TotalAmount { get; set; }

        [Required(ErrorMessage = "Due date is required.")]
        [Display(Name = "Due Date")]
        public DateTime DueDate { get; set; } = DateTime.Now.AddDays(7);

        [Range(0, 999999.99)]
        [Display(Name = "Paid Amount")]
        public decimal PaidAmount { get; set; }

        [Required(ErrorMessage = "Test selection is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a test.")]
        [Display(Name = "Test")]
        public int TestId { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            await LoadTestsAsync();
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
                await LoadTestsAsync();
                return Page();
            }

            var userIdString = HttpContext.Session.GetString("UserId");
            int? userId = null;
            if (!string.IsNullOrEmpty(userIdString) && int.TryParse(userIdString, out var parsedUserId))
            {
                userId = parsedUserId;
            }

            var createDto = new TestEntryCreateDto
            {
                PatientName = Input.PatientName,
                DateOfBirth = Input.DateOfBirth,
                MobileNo = Input.MobileNo,
                BillNo = Input.BillNo,
                TotalAmount = Input.TotalAmount,
                DueDate = Input.DueDate,
                PaidAmount = Input.PaidAmount,
                TestId = Input.TestId,
                IsActive = Input.IsActive,
                CreatedBy = userId
            };

            _logger.LogInformation("Creating new test entry for patient: {PatientName}", Input.PatientName);
            var result = await _testEntryService.CreateAsync(createDto);

            _logger.LogInformation("Test entry created successfully with ID: {Id}", result.Id);
            TempData["SuccessMessage"] = $"Test entry for '{result.PatientName}' created successfully.";

            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating test entry");
            TempData["ErrorMessage"] = "An error occurred while creating the test entry.";
            await LoadTestsAsync();
            return Page();
        }
    }

    private async Task LoadTestsAsync()
    {
        var tests = await _testSetupService.GetAllAsync();
        var activeTests = tests.Where(t => t.IsActive).ToList();
        Tests = new SelectList(activeTests, nameof(TestSetupDto.Id), nameof(TestSetupDto.Name));
    }
}
