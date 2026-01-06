using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using DiagnosticCenter.Domain.Interfaces.Services;
using DiagnosticCenter.Web.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiagnosticCenter.Web.Pages.Payment;

[Authorize(Policy = "AccountantOnly")]
public class UnpaidBillsModel : PageModel
{
    private readonly ITestEntryService _testEntryService;

    public UnpaidBillsModel(ITestEntryService testEntryService)
    {
        _testEntryService = testEntryService;
    }

    public List<UnpaidBillViewModel> UnpaidBills { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var unpaidEntries = await _testEntryService.GetUnpaidAsync();
        UnpaidBills = unpaidEntries.Select(e => new UnpaidBillViewModel
        {
            Id = e.Id,
            BillNo = e.BillNo,
            PatientName = e.Name,
            MobileNo = e.MobileNo,
            TotalAmount = e.TotalAmount,
            PaidAmount = e.PaidAmount,
            DueAmount = e.DueAmount,
            DueDate = e.DueDate,
            TestName = e.TestSetup?.Name ?? string.Empty
        }).ToList();
        return Page();
    }
}