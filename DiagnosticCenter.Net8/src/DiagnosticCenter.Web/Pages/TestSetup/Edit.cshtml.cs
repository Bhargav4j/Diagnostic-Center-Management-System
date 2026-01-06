using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DiagnosticCenter.Domain.Interfaces.Services;
using DiagnosticCenter.Application.ViewModels;
using System.Threading.Tasks;

namespace DiagnosticCenter.Web.Pages.TestSetup
{
    [Authorize(Policy = "AdminOnly")]
    public class EditModel : PageModel
    {
        private readonly ITestSetupService _testSetupService;
        private readonly ITestTypeService _testTypeService;

        [BindProperty]
        public TestSetupViewModel TestSetup { get; set; }

        public SelectList TestTypes { get; set; }

        public EditModel(
            ITestSetupService testSetupService,
            ITestTypeService testTypeService)
        {
            _testSetupService = testSetupService;
            _testTypeService = testTypeService;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var entity = await _testSetupService.GetByIdAsync(id);

            if (entity == null)
            {
                return NotFound();
            }

            TestSetup = new TestSetupViewModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Fee = entity.Fee,
                TypeId = entity.TypeId,
                TypeName = entity.TestType?.Name,
                Description = entity.Description,
                CreatedDate = entity.CreatedDate,
                ModifiedDate = entity.ModifiedDate,
                IsActive = entity.IsActive,
                CreatedBy = entity.CreatedBy,
                ModifiedBy = entity.ModifiedBy
            };

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
                var entity = new Domain.Entities.TestSetup
                {
                    Id = TestSetup.Id,
                    Name = TestSetup.Name,
                    Fee = TestSetup.Fee,
                    TypeId = TestSetup.TypeId,
                    Description = TestSetup.Description
                };

                await _testSetupService.UpdateAsync(TestSetup.Id, entity);
                TempData["SuccessMessage"] = "Test Setup updated successfully.";
                return RedirectToPage("./Index");
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while updating the test setup.");
                await PopulateTestTypesDropdownAsync();
                return Page();
            }
        }

        private async Task PopulateTestTypesDropdownAsync()
        {
            var testTypes = await _testTypeService.GetAllAsync();
            TestTypes = new SelectList(testTypes.Select(t => new { t.Id, t.Name }), "Id", "Name", TestSetup.TypeId);
        }
    }
}