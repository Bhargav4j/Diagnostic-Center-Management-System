using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DiagnosticCenter.Domain.Interfaces.Services;
using DiagnosticCenter.Application.ViewModels;
using DiagnosticCenter.Domain.Entities;
using System.Threading.Tasks;
using System.Linq;

namespace DiagnosticCenter.Web.Pages.TestSetup
{
    [Authorize(Policy = "AdminOnly")]
    public class CreateModel : PageModel
    {
        private readonly ITestSetupService _testSetupService;
        private readonly ITestTypeService _testTypeService;

        [BindProperty]
        public TestSetupViewModel TestSetup { get; set; }

        public SelectList TestTypes { get; set; }

        public CreateModel(
            ITestSetupService testSetupService,
            ITestTypeService testTypeService)
        {
            _testSetupService = testSetupService;
            _testTypeService = testTypeService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            await PopulateTestTypesDropdownAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await PopulateTestTypesDropdownAsync();
                return Page();
            }

            try
            {
                var testSetup = new Domain.Entities.TestSetup
                {
                    Name = TestSetup.Name,
                    Fee = TestSetup.Fee,
                    TypeId = TestSetup.TypeId,
                    Description = TestSetup.Description
                };
                await _testSetupService.CreateAsync(testSetup);
                TempData["SuccessMessage"] = "Test Setup created successfully.";
                return RedirectToPage("./Index");
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while creating the test setup.");
                await PopulateTestTypesDropdownAsync();
                return Page();
            }
        }

        private async Task PopulateTestTypesDropdownAsync()
        {
            var testTypes = await _testTypeService.GetAllAsync();
            TestTypes = new SelectList(testTypes, "Id", "Name");
        }
    }
}