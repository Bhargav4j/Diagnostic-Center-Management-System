using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DiagnosticCenter.Web.ViewModels.TestType;
using DiagnosticCenter.Domain.Interfaces.Services;

namespace DiagnosticCenter.Web.Pages.TestType
{
    [Authorize(Policy = "AdminOnly")]
    public class DetailsModel : PageModel
    {
        private readonly ITestTypeService _testTypeService;

        public DetailsModel(ITestTypeService testTypeService)
        {
            _testTypeService = testTypeService;
        }

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
    }
}