using Microsoft.AspNetCore.Mvc.RazorPages;
using DiagnosticCenter.Web.ViewModels;
using DiagnosticCenter.Domain.Interfaces.Services;

namespace DiagnosticCenter.Web.Pages.TestSetup
{
    public class IndexModel : PageModel
    {
        private readonly ITestSetupService _testSetupService;
        private readonly ITestTypeService _testTypeService;

        public IndexModel(ITestSetupService testSetupService, ITestTypeService testTypeService)
        {
            _testSetupService = testSetupService;
            _testTypeService = testTypeService;
        }

        public List<TestSetupViewModel> TestSetups { get; set; } = new();

        public async Task OnGetAsync()
        {
            var testSetups = await _testSetupService.GetAllAsync();
            var testTypes = await _testTypeService.GetAllAsync();

            TestSetups = testSetups.Select(ts => new TestSetupViewModel
            {
                Id = ts.Id,
                Name = ts.Name,
                Fee = ts.Fee,
                TypeId = ts.TypeId,
                Description = ts.Description,
                TestTypes = testTypes.Select(tt => new TestTypeDropdownViewModel
                {
                    Id = tt.Id,
                    Name = tt.Name
                }).ToList()
            }).ToList();
        }
    }
}