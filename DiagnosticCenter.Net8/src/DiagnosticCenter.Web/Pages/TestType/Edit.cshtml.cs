using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DiagnosticCenter.Web.ViewModels.TestType;
using DiagnosticCenter.Domain.Interfaces.Services;

namespace DiagnosticCenter.Web.Pages.TestType
{
    [Authorize(Policy = "AdminOnly")]
    public class EditModel : PageModel
    {
        private readonly ITestTypeService _testTypeService;

        public EditModel(ITestTypeService testTypeService)
        {
            _testTypeService = testTypeService;
        }

        [BindProperty]
        public TestTypeViewModel TestType { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                var entity = await _testTypeService.GetByIdAsync(id);
                if (entity == null)
                {
                    TempData["ErrorMessage"] = "Test Type not found.";
                    return RedirectToPage("./Index");
                }

                TestType = new TestTypeViewModel
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Description = entity.Description
                };

                return Page();
            }
            catch (System.Exception ex)
            {
                TempData["ErrorMessage"] = $"Error retrieving test type: {ex.Message}";
                return RedirectToPage("./Index");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var entity = new Domain.Entities.TestType
                {
                    Id = TestType.Id,
                    Name = TestType.Name,
                    Description = TestType.Description
                };

                await _testTypeService.UpdateAsync(TestType.Id, entity);
                TempData["SuccessMessage"] = "Test Type updated successfully.";
                return RedirectToPage("./Index");
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error updating test type: {ex.Message}");
                return Page();
            }
        }
    }
}