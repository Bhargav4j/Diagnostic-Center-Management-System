using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DiagnosticCenter.Domain.Interfaces.Services;
using DiagnosticCenter.Application.ViewModels;
using System.Threading.Tasks;

namespace DiagnosticCenter.Web.Pages.TestEntry
{
    [Authorize(Policy = "ReceptionistOnly")]
    public class EditModel : PageModel
    {
        private readonly ITestEntryService _testEntryService;
        private readonly ITestSetupService _testSetupService;

        public EditModel(
            ITestEntryService testEntryService,
            ITestSetupService testSetupService)
        {
            _testEntryService = testEntryService;
            _testSetupService = testSetupService;
        }

        [BindProperty]
        public TestEntryViewModel TestEntry { get; set; }

        public SelectList TestSetups { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var entity = await _testEntryService.GetByIdAsync(id);

            if (entity == null)
            {
                return NotFound();
            }

            TestEntry = new TestEntryViewModel
            {
                Id = entity.Id,
                Name = entity.Name,
                DateOfBirth = entity.DateOfBirth,
                MobileNo = entity.MobileNo,
                BillNo = entity.BillNo,
                TotalAmount = entity.TotalAmount,
                PaidAmount = entity.PaidAmount,
                DueAmount = entity.DueAmount,
                DueDate = entity.DueDate,
                TestId = entity.TestId,
                TestName = entity.TestSetup?.Name,
                CreatedDate = entity.CreatedDate,
                ModifiedDate = entity.ModifiedDate,
                IsActive = entity.IsActive,
                CreatedBy = entity.CreatedBy,
                ModifiedBy = entity.ModifiedBy
            };

            var testSetups = await _testSetupService.GetAllAsync();
            TestSetups = new SelectList(testSetups, "Id", "Name", TestEntry.TestId);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var testSetups = await _testSetupService.GetAllAsync();
                TestSetups = new SelectList(testSetups, "Id", "Name", TestEntry.TestId);
                return Page();
            }

            var entity = new Domain.Entities.TestEntry
            {
                Id = TestEntry.Id,
                Name = TestEntry.Name,
                DateOfBirth = TestEntry.DateOfBirth,
                MobileNo = TestEntry.MobileNo,
                BillNo = TestEntry.BillNo,
                TotalAmount = TestEntry.TotalAmount,
                PaidAmount = TestEntry.PaidAmount,
                DueDate = TestEntry.DueDate,
                TestId = TestEntry.TestId
            };

            await _testEntryService.UpdateAsync(TestEntry.Id, entity);
            return RedirectToPage("./Index");
        }
    }
}