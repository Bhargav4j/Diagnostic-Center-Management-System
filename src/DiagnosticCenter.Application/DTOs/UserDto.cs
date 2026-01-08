namespace DiagnosticCenter.Application.DTOs;

public class UserDto
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string AccountType { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public bool IsActive { get; set; }
}

public class UserCreateDto
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string AccountType { get; set; } = string.Empty;
    public string CreatedBy { get; set; } = string.Empty;
}

public class AuthenticationResultDto
{
    public int UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string AccountType { get; set; } = string.Empty;
    public bool IsAuthenticated { get; set; }
}
