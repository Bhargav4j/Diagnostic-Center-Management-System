using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DiagnosticCenter.Web.ViewModels;
using DiagnosticCenter.Domain.Interfaces.Services;

namespace DiagnosticCenter.Web.Pages.TestEntry;

[Authorize]
public class CreateModel : PageModel
{
    private readonly ITestEntryService _testEntryService;
    private readonly ITestSetupService _testSetupService;

    [BindProperty]
    public TestEntryViewModel TestEntry { get; set; } = new TestEntryViewModel();

    public CreateModel(ITestEntryService testEntryService, ITestSetupService testSetupService)
    {
        _testEntryService = testEntryService;
        _testSetupService = testSetupService;
    }

    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken)
    {
        try
        {
            // Load test setups for dropdown
            var testSetups = await _testSetupService.GetAllAsync(cancellationToken);
            TestEntry.TestSetups = testSetups.Select(ts => new TestSetupDropdownViewModel
            {
                Id = ts.Id,
                Name = ts.Name
            }).ToList();

            // Set default values
            TestEntry.DOB = DateTime.Today.AddYears(-30);
            TestEntry.DueDate = DateTime.Today.AddDays(7);
            TestEntry.PaidAmount = 0;

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
            // Reload test setups for dropdown
            var testSetups = await _testSetupService.GetAllAsync(cancellationToken);
            TestEntry.TestSetups = testSetups.Select(ts => new TestSetupDropdownViewModel
            {
                Id = ts.Id,
                Name = ts.Name
            }).ToList();
            return Page();
        }

        try
        {
            // Manually map ViewModel to DTO
            var createDto = new TestEntryCreateDto
            {
                Name = TestEntry.Name,
                DOB = TestEntry.DOB,
                MobileNo = TestEntry.MobileNo,
                BillNo = TestEntry.BillNo,
                TotalAmount = TestEntry.TotalAmount,
                DueDate = TestEntry.DueDate,
                PaidAmount = TestEntry.PaidAmount,
                TestId = TestEntry.TestId,
                CreatedBy = User.Identity?.Name ?? "System"
            };

            await _testEntryService.CreateAsync(createDto, cancellationToken);

            TempData["SuccessMessage"] = $"Test Entry for '{TestEntry.Name}' created successfully.";

            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, $"Error creating test entry: {ex.Message}");

            // Reload test setups for dropdown
            var testSetups = await _testSetupService.GetAllAsync(cancellationToken);
            TestEntry.TestSetups = testSetups.Select(ts => new TestSetupDropdownViewModel
            {
                Id = ts.Id,
                Name = ts.Name
            }).ToList();

            return Page();
        }
    }
}
