using System.ComponentModel.DataAnnotations;

namespace DiagnosticCenter.Domain.DTOs;

/// <summary>
/// Data transfer object for updating an existing test setup.
/// </summary>
public class TestSetupUpdateDto
{
    /// <summary>
    /// Gets or sets the name of the test.
    /// </summary>
    [Required(ErrorMessage = "Test name is required.")]
    [StringLength(200, MinimumLength = 2, ErrorMessage = "Test name must be between 2 and 200 characters.")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the fee charged for the test.
    /// </summary>
    [Required(ErrorMessage = "Test fee is required.")]
    [Range(0.01, 999999.99, ErrorMessage = "Fee must be between 0.01 and 999,999.99.")]
    public decimal Fee { get; set; }

    /// <summary>
    /// Gets or sets the foreign key to the test type.
    /// </summary>
    [Required(ErrorMessage = "Test type is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Invalid test type ID.")]
    public int TypeId { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the test setup is active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user who is modifying the test setup.
    /// </summary>
    public int? ModifiedBy { get; set; }
}
