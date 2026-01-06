using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DiagnosticCenter.Domain.Interfaces.Services;

namespace DiagnosticCenter.Web.Pages.TestEntry;

[Authorize]
public class IndexModel : PageModel
{
    private readonly ITestEntryService _testEntryService;

    public List<TestEntryIndexViewModel> TestEntries { get; set; } = new();

    public IndexModel(ITestEntryService testEntryService)
    {
        _testEntryService = testEntryService;
    }

    public async Task OnGetAsync(CancellationToken cancellationToken)
    {
        try
        {
            var testEntryDtos = await _testEntryService.GetAllAsync(cancellationToken);

            // Manually map DTOs to ViewModels
            TestEntries = testEntryDtos.Select(te => new TestEntryIndexViewModel
            {
                Id = te.Id,
                Name = te.Name,
                DOB = te.DOB,
                MobileNo = te.MobileNo,
                BillNo = te.BillNo,
                TotalAmount = te.TotalAmount,
                DueDate = te.DueDate,
                PaidAmount = te.PaidAmount,
                TestId = te.TestId,
                TestName = te.TestName,
                IsActive = te.IsActive,
                DueAmount = te.DueAmount
            }).ToList();
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Error loading test entries: {ex.Message}";
        }
    }
}

public class TestEntryIndexViewModel
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
    public bool IsActive { get; set; }
    public decimal DueAmount { get; set; }
}
