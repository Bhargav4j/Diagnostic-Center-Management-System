using System.ComponentModel.DataAnnotations;
using DiagnosticCenter.Application.DTOs;
using DiagnosticCenter.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DiagnosticCenter.Web.Pages.TestEntry;

[Authorize]
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
        _testEntryService = testEntryService ?? throw new ArgumentNullException(nameof(testEntryService));
        _testSetupService = testSetupService ?? throw new ArgumentNullException(nameof(testSetupService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [BindProperty]
    public InputModel? Input { get; set; }

    public SelectList Tests { get; set; } = new SelectList(Enumerable.Empty<SelectListItem>());

    public class InputModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Patient name is required.")]
        [StringLength(200, MinimumLength = 2)]
        [Display(Name = "Patient Name")]
        public string PatientName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Date of birth is required.")]
        [Display(Name = "Date of Birth")]
        public DateTime DateOfBirth { get; set; }

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
        public DateTime DueDate { get; set; }

        [Range(0, 999999.99)]
        [Display(Name = "Paid Amount")]
        public decimal PaidAmount { get; set; }

        [Required(ErrorMessage = "Test selection is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a test.")]
        [Display(Name = "Test")]
        public int TestId { get; set; }

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
                TempData["ErrorMessage"] = "Invalid test entry ID.";
                return RedirectToPage("Index");
            }

            _logger.LogInformation("Loading test entry for editing, ID: {Id}", id);
            var testEntry = await _testEntryService.GetByIdAsync(id.Value);

            if (testEntry == null)
            {
                _logger.LogWarning("Test entry not found with ID: {Id}", id);
                TempData["ErrorMessage"] = $"Test entry with ID {id} not found.";
                return RedirectToPage("Index");
            }

            Input = new InputModel
            {
                Id = testEntry.Id,
                PatientName = testEntry.PatientName,
                DateOfBirth = testEntry.DateOfBirth,
                MobileNo = testEntry.MobileNo,
                BillNo = testEntry.BillNo,
                TotalAmount = testEntry.TotalAmount,
                DueDate = testEntry.DueDate,
                PaidAmount = testEntry.PaidAmount,
                TestId = testEntry.TestId,
                IsActive = testEntry.IsActive
            };

            await LoadTestsAsync();
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading test entry for editing, ID: {Id}", id);
            TempData["ErrorMessage"] = "An error occurred while loading the test entry.";
            return RedirectToPage("Index");
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            if (!ModelState.IsValid || Input == null)
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

            var updateDto = new TestEntryUpdateDto
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
                ModifiedBy = userId
            };

            _logger.LogInformation("Updating test entry with ID: {Id}", Input.Id);
            await _testEntryService.UpdateAsync(Input.Id, updateDto);

            _logger.LogInformation("Test entry updated successfully, ID: {Id}", Input.Id);
            TempData["SuccessMessage"] = $"Test entry for '{Input.PatientName}' updated successfully.";

            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating test entry with ID: {Id}", Input?.Id);
            TempData["ErrorMessage"] = "An error occurred while updating the test entry.";
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
