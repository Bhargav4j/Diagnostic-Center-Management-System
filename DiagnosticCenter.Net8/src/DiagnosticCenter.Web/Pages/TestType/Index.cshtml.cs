using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DiagnosticCenter.Web.ViewModels.TestType;
using DiagnosticCenter.Domain.Interfaces.Services;

namespace DiagnosticCenter.Web.Pages.TestType
{
    [Authorize(Policy = "AdminOnly")]
    public class IndexModel : PageModel
    {
        private readonly ITestTypeService _testTypeService;

        public IndexModel(ITestTypeService testTypeService)
        {
            _testTypeService = testTypeService;
        }

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        public List<TestTypeViewModel> TestTypes { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var testTypes = string.IsNullOrEmpty(SearchTerm)
                    ? await _testTypeService.GetAllAsync()
                    : await _testTypeService.SearchAsync(SearchTerm);

                TestTypes = testTypes.Select(t => new TestTypeViewModel
                {
                    Id = t.Id,
                    Name = t.Name,
                    Description = t.Description
                }).ToList();

                return Page();
            }
            catch (System.Exception ex)
            {
                TempData["ErrorMessage"] = $"Error retrieving test types: {ex.Message}";
                return Page();
            }
        }
    }
}