using System.ComponentModel.DataAnnotations;

namespace DiagnosticCenter.Domain.DTOs;

/// <summary>
/// Data transfer object representing a test type.
/// </summary>
public class TestTypeDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the test type.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the test type.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the test type.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the test type was created.
    /// </summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the test type was last modified.
    /// </summary>
    public DateTime? ModifiedDate { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the test type is active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user who created the test type.
    /// </summary>
    public int? CreatedBy { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user who last modified the test type.
    /// </summary>
    public int? ModifiedBy { get; set; }
}
