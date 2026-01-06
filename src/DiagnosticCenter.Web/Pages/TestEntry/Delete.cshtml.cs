using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DiagnosticCenter.Domain.Interfaces.Services;

namespace DiagnosticCenter.Web.Pages.TestEntry;

[Authorize]
public class DeleteModel : PageModel
{
    private readonly ITestEntryService _testEntryService;

    [BindProperty]
    public TestEntryDeleteViewModel TestEntry { get; set; } = new TestEntryDeleteViewModel();

    public DeleteModel(ITestEntryService testEntryService)
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
            TestEntry = new TestEntryDeleteViewModel
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
                DueAmount = testEntryDto.DueAmount,
                IsActive = testEntryDto.IsActive
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
        try
        {
            await _testEntryService.DeleteAsync(TestEntry.Id, cancellationToken);

            TempData["SuccessMessage"] = $"Test Entry for '{TestEntry.Name}' deleted successfully.";

            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Error deleting test entry: {ex.Message}";
            return RedirectToPage("Index");
        }
    }
}

public class TestEntryDeleteViewModel
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
    public decimal DueAmount { get; set; }
    public bool IsActive { get; set; }
}
