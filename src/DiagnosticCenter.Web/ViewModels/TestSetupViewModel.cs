using System.ComponentModel.DataAnnotations;

namespace DiagnosticCenter.Web.ViewModels;

public class TestSetupViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Test name is required")]
    [StringLength(200, ErrorMessage = "Test name cannot exceed 200 characters")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Fee is required")]
    [Range(0.01, 999999.99, ErrorMessage = "Fee must be between 0.01 and 999999.99")]
    public decimal Fee { get; set; }

    [Required(ErrorMessage = "Test type is required")]
    public int TypeId { get; set; }

    public string TypeName { get; set; } = string.Empty;
}
