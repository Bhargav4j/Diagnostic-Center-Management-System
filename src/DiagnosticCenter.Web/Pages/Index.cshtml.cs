using DiagnosticCenter.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace DiagnosticCenter.Web.Pages;

public class IndexModel : PageModel
{
    private readonly IAuthService _authService;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(IAuthService authService, ILogger<IndexModel> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [BindProperty]
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email { get; set; } = string.Empty;

    [BindProperty]
    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = string.Empty;

    public string? ErrorMessage { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            var (success, user, message) = await _authService.AuthenticateAsync(Email, Password);

            if (!success || user == null)
            {
                ErrorMessage = message;
                return Page();
            }

            // Create claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.AccountType),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, "CookieAuth");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync("CookieAuth", claimsPrincipal);

            // Set session
            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetString("AccountType", user.AccountType);

            // Redirect based on account type
            return user.AccountType switch
            {
                "Admin" => RedirectToPage("/Admin/Index"),
                "Accountant" => RedirectToPage("/Accountant/Index"),
                "Receptionist" => RedirectToPage("/Receptionist/Index"),
                _ => RedirectToPage("/Index")
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during authentication for user {Email}", Email);
            ErrorMessage = "An error occurred during login. Please try again.";
            return Page();
        }
    }
}
