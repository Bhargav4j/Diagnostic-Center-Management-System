using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DiagnosticCenter.Web.ViewModels.TestType;
using DiagnosticCenter.Domain.Interfaces.Services;

namespace DiagnosticCenter.Web.Pages.TestType
{
    [Authorize(Policy = "AdminOnly")]
    public class CreateModel : PageModel
    {
        private readonly ITestTypeService _testTypeService;

        public CreateModel(ITestTypeService testTypeService)
        {
            _testTypeService = testTypeService;
        }

        [BindProperty]
        public TestTypeViewModel TestType { get; set; }

        public IActionResult OnGet()
        {
            return Page();
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
                    Name = TestType.Name,
                    Description = TestType.Description
                };

                await _testTypeService.CreateAsync(entity);
                TempData["SuccessMessage"] = "Test Type created successfully.";
                return RedirectToPage("./Index");
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error creating test type: {ex.Message}");
                return Page();
            }
        }
    }
}