using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DiagnosticCenter.Domain.Interfaces.Services;
using DiagnosticCenter.Application.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiagnosticCenter.Web.Pages.TestSetup
{
    [Authorize(Policy = "AdminOnly")]
    public class IndexModel : PageModel
    {
        private readonly ITestSetupService _testSetupService;

        public IndexModel(ITestSetupService testSetupService)
        {
            _testSetupService = testSetupService;
        }

        public IEnumerable<TestSetupViewModel> TestSetups { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var testSetups = await _testSetupService.GetAllAsync();
                TestSetups = testSetups.Select(ts => new TestSetupViewModel
                {
                    Id = ts.Id,
                    Name = ts.Name,
                    Fee = ts.Fee,
                    TypeId = ts.TypeId,
                    TypeName = ts.TestType?.Name,
                    Description = ts.Description,
                    CreatedDate = ts.CreatedDate,
                    ModifiedDate = ts.ModifiedDate,
                    IsActive = ts.IsActive,
                    CreatedBy = ts.CreatedBy,
                    ModifiedBy = ts.ModifiedBy
                }).ToList();
                return Page();
            }
            catch (System.Exception ex)
            {
                // Log the exception
                ModelState.AddModelError(string.Empty, "An error occurred while retrieving test setups.");
                return Page();
            }
        }
    }
}