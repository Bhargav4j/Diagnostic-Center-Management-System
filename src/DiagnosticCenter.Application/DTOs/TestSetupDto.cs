using System.ComponentModel.DataAnnotations;

namespace DiagnosticCenter.Application.DTOs;

/// <summary>
/// Data transfer object representing a test setup.
/// </summary>
public class TestSetupDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the test setup.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the test.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the fee charged for the test.
    /// </summary>
    public decimal Fee { get; set; }

    /// <summary>
    /// Gets or sets the foreign key to the test type.
    /// </summary>
    public int TypeId { get; set; }

    /// <summary>
    /// Gets or sets the name of the test type.
    /// </summary>
    public string? TypeName { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the test setup was created.
    /// </summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the test setup was last modified.
    /// </summary>
    public DateTime? ModifiedDate { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the test setup is active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user who created the test setup.
    /// </summary>
    public int? CreatedBy { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user who last modified the test setup.
    /// </summary>
    public int? ModifiedBy { get; set; }
}
