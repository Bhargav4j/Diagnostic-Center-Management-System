using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Interfaces.Services;

namespace DiagnosticCenter.Web.Pages.TestTypes;

public class CreateModel : PageModel
{
    private readonly ITestTypeService _testTypeService;
    private readonly ILogger<CreateModel> _logger;

    public CreateModel(ITestTypeService testTypeService, ILogger<CreateModel> logger)
    {
        _testTypeService = testTypeService;
        _logger = logger;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new InputModel();

    public class InputModel
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            if (await _testTypeService.ExistsByNameAsync(Input.Name, cancellationToken))
            {
                ModelState.AddModelError("Input.Name", "A test type with this name already exists.");
                return Page();
            }

            var testType = new TestType
            {
                Name = Input.Name,
                Description = Input.Description,
                CreatedBy = "System",
                CreatedDate = DateTime.UtcNow,
                IsActive = true
            };

            await _testTypeService.CreateAsync(testType, cancellationToken);

            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating test type");
            ModelState.AddModelError(string.Empty, "An error occurred while creating the test type.");
            return Page();
        }
    }
}
