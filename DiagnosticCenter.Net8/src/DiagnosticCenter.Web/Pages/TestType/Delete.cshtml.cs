using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DiagnosticCenter.Web.ViewModels.TestType;
using DiagnosticCenter.Domain.Interfaces.Services;

namespace DiagnosticCenter.Web.Pages.TestType
{
    [Authorize(Policy = "AdminOnly")]
    public class DeleteModel : PageModel
    {
        private readonly ITestTypeService _testTypeService;

        public DeleteModel(ITestTypeService testTypeService)
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
            try
            {
                await _testTypeService.DeleteAsync(TestType.Id);
                TempData["SuccessMessage"] = "Test Type deleted successfully.";
                return RedirectToPage("./Index");
            }
            catch (System.Exception ex)
            {
                TempData["ErrorMessage"] = $"Error deleting test type: {ex.Message}";
                return RedirectToPage("./Index");
            }
        }
    }
}