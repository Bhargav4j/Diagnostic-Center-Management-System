using Microsoft.AspNetCore.Identity;

namespace DiagnosticCenter.Domain.Entities;

/// <summary>
/// Represents a user in the diagnostic center system
/// </summary>
public class User : IdentityUser
{
    /// <summary>
    /// Gets or sets the account type (Admin, Accountant, Receptionist)
    /// </summary>
    public string AccountType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the creation date
    /// </summary>
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the last modified date
    /// </summary>
    public DateTime? ModifiedDate { get; set; }

    /// <summary>
    /// Gets or sets whether this user is active
    /// </summary>
    public bool IsActive { get; set; } = true;
}
