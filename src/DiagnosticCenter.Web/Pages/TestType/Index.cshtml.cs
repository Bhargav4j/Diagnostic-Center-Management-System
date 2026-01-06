using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DiagnosticCenter.Web.ViewModels;
using DiagnosticCenter.Domain.Interfaces.Services;

namespace DiagnosticCenter.Web.Pages.TestType;

[Authorize]
public class IndexModel : PageModel
{
    private readonly ITestTypeService _testTypeService;

    [BindProperty(SupportsGet = true)]
    public string? SearchTerm { get; set; }

    public IEnumerable<TestTypeViewModel>? TestTypes { get; set; }

    public IndexModel(ITestTypeService testTypeService)
    {
        _testTypeService = testTypeService;
    }

    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken)
    {
        try
        {
            // Manually map DTOs to ViewModels
            var testTypeDtos = string.IsNullOrWhiteSpace(SearchTerm)
                ? await _testTypeService.GetAllAsync(cancellationToken)
                : await _testTypeService.SearchAsync(SearchTerm, cancellationToken);
            TestTypes = testTypeDtos.Select(dto => new TestTypeViewModel
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                IsActive = dto.IsActive,
                CreatedDate = dto.CreatedDate
            });

            return Page();
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Error retrieving test types: {ex.Message}";
            return Page();
        }
    }
}