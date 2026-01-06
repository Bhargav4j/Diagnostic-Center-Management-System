using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DiagnosticCenter.Domain.Interfaces.Services;
using DiagnosticCenter.Application.ViewModels;
using System.Threading.Tasks;

namespace DiagnosticCenter.Web.Pages.TestEntry
{
    [Authorize(Policy = "ReceptionistOnly")]
    public class DetailsModel : PageModel
    {
        private readonly ITestEntryService _testEntryService;

        public DetailsModel(ITestEntryService testEntryService)
        {
            _testEntryService = testEntryService;
        }

        public TestEntryViewModel TestEntry { get; set; }

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

            return Page();
        }
    }
}