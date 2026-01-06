using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DiagnosticCenter.Domain.Interfaces.Services;
using DiagnosticCenter.Application.ViewModels;
using System.Threading.Tasks;

namespace DiagnosticCenter.Web.Pages.TestSetup
{
    [Authorize(Policy = "AdminOnly")]
    public class DetailsModel : PageModel
    {
        private readonly ITestSetupService _testSetupService;

        public TestSetupViewModel TestSetup { get; set; }

        public DetailsModel(ITestSetupService testSetupService)
        {
            _testSetupService = testSetupService;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var testSetup = await _testSetupService.GetByIdAsync(id);

            if (testSetup == null)
            {
                return NotFound();
            }

            TestSetup = new TestSetupViewModel
            {
                Id = testSetup.Id,
                Name = testSetup.Name,
                Fee = testSetup.Fee,
                TypeId = testSetup.TypeId,
                TypeName = testSetup.TestType?.Name,
                Description = testSetup.Description,
                CreatedDate = testSetup.CreatedDate,
                ModifiedDate = testSetup.ModifiedDate,
                IsActive = testSetup.IsActive,
                CreatedBy = testSetup.CreatedBy,
                ModifiedBy = testSetup.ModifiedBy
            };

            return Page();
        }
    }
}