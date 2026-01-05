using DiagnosticCenter.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DiagnosticCenter.Web.Pages;

public class IndexModel : PageModel
{
    private readonly IAuthenticationService _authenticationService;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(IAuthenticationService authenticationService, ILogger<IndexModel> logger)
    {
        _authenticationService = authenticationService;
        _logger = logger;
    }

    [BindProperty]
    public string Email { get; set; } = string.Empty;

    [BindProperty]
    public string Password { get; set; } = string.Empty;

    public string? ErrorMessage { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Email and password are required";
                return Page();
            }

            var user = await _authenticationService.AuthenticateAsync(Email, Password);

            if (user == null)
            {
                ErrorMessage = "Invalid email or password";
                _logger.LogWarning("Failed login attempt for email: {Email}", Email);
                return Page();
            }

            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetString("UserRole", user.AccountType);
            HttpContext.Session.SetInt32("UserId", user.Id);

            _logger.LogInformation("User {Email} logged in successfully with role {Role}", user.Email, user.AccountType);

            return user.AccountType switch
            {
                "Admin" => RedirectToPage("/Admin/Home"),
                "Accountant" => RedirectToPage("/Accountant/Home"),
                "Receptionist" => RedirectToPage("/Receptionist/Home"),
                _ => RedirectToPage("/TestTypes/Index")
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for email: {Email}", Email);
            ErrorMessage = "An error occurred during login. Please try again.";
            return Page();
        }
    }
}
