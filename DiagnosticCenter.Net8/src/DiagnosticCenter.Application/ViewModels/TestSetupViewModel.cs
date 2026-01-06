using System.ComponentModel.DataAnnotations;

namespace DiagnosticCenter.Application.ViewModels;

/// <summary>
/// View model for TestSetup entity
/// </summary>
public class TestSetupViewModel
{
    /// <summary>
    /// Gets or sets the unique identifier
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the test name
    /// </summary>
    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the test fee
    /// </summary>
    [Required(ErrorMessage = "Fee is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Fee must be greater than zero")]
    public decimal Fee { get; set; }

    /// <summary>
    /// Gets or sets the test type identifier
    /// </summary>
    [Required(ErrorMessage = "Type is required")]
    public int TypeId { get; set; }

    /// <summary>
    /// Gets or sets the test type name (for display purposes)
    /// </summary>
    public string? TypeName { get; set; }

    /// <summary>
    /// Gets or sets the description
    /// </summary>
    [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters")]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the creation date
    /// </summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// Gets or sets the last modified date
    /// </summary>
    public DateTime? ModifiedDate { get; set; }

    /// <summary>
    /// Gets or sets whether this test setup is active
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Gets or sets the user who created this record
    /// </summary>
    public string CreatedBy { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user who last modified this record
    /// </summary>
    public string? ModifiedBy { get; set; }
}
