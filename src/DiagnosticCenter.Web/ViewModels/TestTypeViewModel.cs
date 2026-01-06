using System.ComponentModel.DataAnnotations;

namespace DiagnosticCenter.Web.ViewModels;

public class TestTypeViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Test Type Name is required")]
    [StringLength(100, ErrorMessage = "Test Type Name cannot exceed 100 characters")]
    [Display(Name = "Test Type Name")]
    public string Name { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
    [Display(Name = "Description")]
    public string? Description { get; set; }

    [Display(Name = "Is Active")]
    public bool IsActive { get; set; } = true;

    [Display(Name = "Created Date")]
    public DateTime CreatedDate { get; set; }
}