using System.ComponentModel.DataAnnotations;

namespace DiagnosticCenter.Application.DTOs;

/// <summary>
/// Data transfer object for updating an existing test type.
/// </summary>
public class TestTypeUpdateDto
{
    /// <summary>
    /// Gets or sets the name of the test type.
    /// </summary>
    [Required(ErrorMessage = "Test type name is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Test type name must be between 2 and 100 characters.")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the test type.
    /// </summary>
    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the test type is active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user who is modifying the test type.
    /// </summary>
    public int? ModifiedBy { get; set; }
}
