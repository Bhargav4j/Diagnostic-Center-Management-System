using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DiagnosticCenter.Web.ViewModels;
using DiagnosticCenter.Domain.Interfaces.Services;

namespace DiagnosticCenter.Web.Pages.TestSetup
{
    public class CreateModel : PageModel
    {
        private readonly ITestSetupService _testSetupService;
        private readonly ITestTypeService _testTypeService;

        public CreateModel(ITestSetupService testSetupService, ITestTypeService testTypeService)
        {
            _testSetupService = testSetupService;
            _testTypeService = testTypeService;
        }

        [BindProperty]
        public TestSetupViewModel TestSetup { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            TestSetup.TestTypes = (await _testTypeService.GetAllAsync())
                .Select(tt => new TestTypeDropdownViewModel
                {
                    Id = tt.Id,
                    Name = tt.Name
                }).ToList();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // Reload test types dropdown
                TestSetup.TestTypes = (await _testTypeService.GetAllAsync())
                    .Select(tt => new TestTypeDropdownViewModel
                    {
                        Id = tt.Id,
                        Name = tt.Name
                    }).ToList();
                return Page();
            }

            try
            {
                var createDto = new TestSetupCreateDto
                {
                    Name = TestSetup.Name,
                    Fee = TestSetup.Fee,
                    TypeId = TestSetup.TypeId,
                    Description = TestSetup.Description,
                    CreatedBy = User.Identity?.Name ?? "System"
                };

                await _testSetupService.CreateAsync(createDto);
                TempData["SuccessMessage"] = "Test Setup created successfully.";
                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while creating the test setup.");
                TempData["ErrorMessage"] = ex.Message;

                // Reload test types dropdown
                TestSetup.TestTypes = (await _testTypeService.GetAllAsync())
                    .Select(tt => new TestTypeDropdownViewModel
                    {
                        Id = tt.Id,
                        Name = tt.Name
                    }).ToList();
                return Page();
            }
        }
    }
}