using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DiagnosticCenter.Domain.Interfaces.Services;
using DiagnosticCenter.Application.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiagnosticCenter.Web.Pages.TestEntry
{
    [Authorize(Policy = "ReceptionistOnly")]
    public class IndexModel : PageModel
    {
        private readonly ITestEntryService _testEntryService;

        public IndexModel(ITestEntryService testEntryService)
        {
            _testEntryService = testEntryService;
        }

        public IList<TestEntryViewModel> TestEntries { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var entries = await _testEntryService.GetAllAsync();
            TestEntries = entries.Select(e => new TestEntryViewModel
            {
                Id = e.Id,
                Name = e.Name,
                DateOfBirth = e.DateOfBirth,
                MobileNo = e.MobileNo,
                BillNo = e.BillNo,
                TotalAmount = e.TotalAmount,
                PaidAmount = e.PaidAmount,
                DueAmount = e.DueAmount,
                DueDate = e.DueDate,
                TestId = e.TestId,
                TestName = e.TestSetup?.Name,
                CreatedDate = e.CreatedDate,
                ModifiedDate = e.ModifiedDate,
                IsActive = e.IsActive,
                CreatedBy = e.CreatedBy,
                ModifiedBy = e.ModifiedBy
            }).ToList();
            return Page();
        }
    }
}