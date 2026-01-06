using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DiagnosticCenter.Domain.Interfaces.Services;

namespace DiagnosticCenter.Web.Pages.TestEntry;

[Authorize]
public class DetailsModel : PageModel
{
    private readonly ITestEntryService _testEntryService;

    public TestEntryDetailsViewModel TestEntry { get; set; } = new TestEntryDetailsViewModel();

    public DetailsModel(ITestEntryService testEntryService)
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
            TestEntry = new TestEntryDetailsViewModel
            {
                Id = testEntryDto.Id,
                Name = testEntryDto.Name,
                DOB = testEntryDto.DOB,
                MobileNo = testEntryDto.MobileNo,
                BillNo = testEntryDto.BillNo,
                TotalAmount = testEntryDto.TotalAmount,
                DueDate = testEntryDto.DueDate,
                PaidAmount = testEntryDto.PaidAmount,
                TestId = testEntryDto.TestId,
                TestName = testEntryDto.TestName,
                DueAmount = testEntryDto.DueAmount,
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
}

public class TestEntryDetailsViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime DOB { get; set; }
    public string MobileNo { get; set; } = string.Empty;
    public string BillNo { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public DateTime DueDate { get; set; }
    public decimal PaidAmount { get; set; }
    public int TestId { get; set; }
    public string TestName { get; set; } = string.Empty;
    public decimal DueAmount { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
}
