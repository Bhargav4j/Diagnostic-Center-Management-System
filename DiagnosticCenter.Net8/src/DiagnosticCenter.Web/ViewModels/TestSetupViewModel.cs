using System.ComponentModel.DataAnnotations;

namespace DiagnosticCenter.Web.ViewModels;

public class TestSetupViewModel
{
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [Range(0.01, 999999.99)]
    public decimal Fee { get; set; }

    [Required]
    public int TypeId { get; set; }

    public string TypeName { get; set; } = string.Empty;

    public DateTime CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public bool IsActive { get; set; }
}
