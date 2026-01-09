using Microsoft.AspNetCore.Identity;

namespace DiagnosticCenter.Domain.Entities;

/// <summary>
/// Represents an application user
/// </summary>
public class User : IdentityUser
{
    public string FullName { get; set; } = string.Empty;

    public DateTime CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public bool IsActive { get; set; }
}
