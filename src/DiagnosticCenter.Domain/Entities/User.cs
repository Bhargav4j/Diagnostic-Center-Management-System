using Microsoft.AspNetCore.Identity;

namespace DiagnosticCenter.Domain.Entities;

/// <summary>
/// Represents a user in the diagnostic center system
/// </summary>
public class User : IdentityUser
{
    public string AccountType { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? ModifiedDate { get; set; }
    public bool IsActive { get; set; } = true;
}
