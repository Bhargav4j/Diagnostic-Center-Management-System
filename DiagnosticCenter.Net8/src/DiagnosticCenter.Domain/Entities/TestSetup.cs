namespace DiagnosticCenter.Domain.Entities;

/// <summary>
/// Represents a specific diagnostic test setup with pricing
/// </summary>
public class TestSetup
{
    /// <summary>
    /// Gets or sets the unique identifier
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the test name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the test fee
    /// </summary>
    public decimal Fee { get; set; }

    /// <summary>
    /// Gets or sets the test type identifier
    /// </summary>
    public int TypeId { get; set; }

    /// <summary>
    /// Gets or sets the description
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the creation date
    /// </summary>
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

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

    /// <summary>
    /// Navigation property for test type
    /// </summary>
    public virtual TestType? TestType { get; set; }

    /// <summary>
    /// Navigation property for test entries
    /// </summary>
    public virtual ICollection<TestEntry> TestEntries { get; set; } = new List<TestEntry>();
}
