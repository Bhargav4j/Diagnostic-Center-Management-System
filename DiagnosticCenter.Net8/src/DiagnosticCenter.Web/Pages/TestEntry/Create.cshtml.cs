using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DiagnosticCenter.Domain.Interfaces.Services;
using DiagnosticCenter.Application.ViewModels;
using System.Threading.Tasks;
using System.Linq;

namespace DiagnosticCenter.Web.Pages.TestEntry
{
    [Authorize(Policy = "ReceptionistOnly")]
    public class CreateModel : PageModel
    {
        private readonly ITestEntryService _testEntryService;
        private readonly ITestSetupService _testSetupService;

        public CreateModel(
            ITestEntryService testEntryService,
            ITestSetupService testSetupService)
        {
            _testEntryService = testEntryService;
            _testSetupService = testSetupService;
        }

        [BindProperty]
        public TestEntryViewModel TestEntry { get; set; }

        public SelectList TestSetups { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var testSetups = await _testSetupService.GetAllAsync();
            TestSetups = new SelectList(testSetups, "Id", "Name");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var testSetups = await _testSetupService.GetAllAsync();
                TestSetups = new SelectList(testSetups, "Id", "Name");
                return Page();
            }

            var entity = new Domain.Entities.TestEntry
            {
                Name = TestEntry.Name,
                DateOfBirth = TestEntry.DateOfBirth,
                MobileNo = TestEntry.MobileNo,
                BillNo = TestEntry.BillNo,
                TotalAmount = TestEntry.TotalAmount,
                PaidAmount = TestEntry.PaidAmount,
                DueDate = TestEntry.DueDate,
                TestId = TestEntry.TestId
            };

            await _testEntryService.CreateAsync(entity);
            return RedirectToPage("./Index");
        }
    }
}