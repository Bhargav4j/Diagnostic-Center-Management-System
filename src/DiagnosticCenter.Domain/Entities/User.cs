using Microsoft.AspNetCore.Identity;

namespace DiagnosticCenter.Domain.Entities;

/// <summary>
/// Represents an application user (Admin, Receptionist, Accountant)
/// </summary>
public class User : IdentityUser<int>
{
    public string FullName { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public bool IsActive { get; set; } = true;
}
