using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DiagnosticCenter.Domain.Interfaces.Services;

namespace DiagnosticCenter.Web.Pages.TestEntry;

[Authorize]
public class EditModel : PageModel
{
    private readonly ITestEntryService _testEntryService;

    [BindProperty]
    public TestEntryEditViewModel TestEntry { get; set; } = new TestEntryEditViewModel();

    public EditModel(ITestEntryService testEntryService)
    {
        _testEntryService = testEntryService;
    }

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            var testEntryDto = await _testEntryService.GetByIdAsync(id, cancellationToken);

            if (testEntryDto == null)
            {
                TempData["ErrorMessage"] = "Test Entry not found.";
                return RedirectToPage("Index");
            }

            // Manually map DTO to ViewModel
            TestEntry = new TestEntryEditViewModel
            {
                Id = testEntryDto.Id,
                Name = testEntryDto.Name,
                DOB = testEntryDto.DOB,
                MobileNo = testEntryDto.MobileNo,
                BillNo = testEntryDto.BillNo,
                TotalAmount = testEntryDto.TotalAmount,
                DueDate = testEntryDto.DueDate,
                PaidAmount = testEntryDto.PaidAmount,
                TestName = testEntryDto.TestName,
                IsActive = testEntryDto.IsActive,
                CreatedDate = testEntryDto.CreatedDate
            };

            return Page();
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Error retrieving test entry details: {ex.Message}";
            return RedirectToPage("Index");
        }
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            // Manually map ViewModel to DTO
            var updateDto = new TestEntryUpdateDto
            {
                Name = TestEntry.Name,
                DOB = TestEntry.DOB,
                MobileNo = TestEntry.MobileNo,
                TotalAmount = TestEntry.TotalAmount,
                DueDate = TestEntry.DueDate,
                PaidAmount = TestEntry.PaidAmount,
                IsActive = TestEntry.IsActive,
                ModifiedBy = User.Identity?.Name ?? "System"
            };

            await _testEntryService.UpdateAsync(TestEntry.Id, updateDto, cancellationToken);

            TempData["SuccessMessage"] = $"Test Entry for '{TestEntry.Name}' updated successfully.";

            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, $"Error updating test entry: {ex.Message}");
            return Page();
        }
    }
}

public class TestEntryEditViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime DOB { get; set; }
    public string MobileNo { get; set; } = string.Empty;
    public string BillNo { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public DateTime DueDate { get; set; }
    public decimal PaidAmount { get; set; }
    public string TestName { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
    public decimal DueAmount => TotalAmount - PaidAmount;
}
