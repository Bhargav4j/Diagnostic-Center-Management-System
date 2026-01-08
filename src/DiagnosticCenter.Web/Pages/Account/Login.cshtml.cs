using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using DiagnosticCenter.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DiagnosticCenter.Web.Pages.Account;

public class LoginModel : PageModel
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<LoginModel> _logger;

    public LoginModel(IUserRepository userRepository, ILogger<LoginModel> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public string? ErrorMessage { get; set; }

    public class InputModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }

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
            var user = await _userRepository.GetByEmailAsync(Input.Email);

            if (user == null)
            {
                ErrorMessage = "Email or password is incorrect";
                _logger.LogWarning("Login attempt failed for email: {Email}", Input.Email);
                return Page();
            }

            var passwordMatches = user.PasswordHash.Trim() == Input.Password.Trim();

            if (!passwordMatches)
            {
                ErrorMessage = "Email or password is incorrect";
                _logger.LogWarning("Login attempt failed - invalid password for email: {Email}", Input.Email);
                return Page();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("AccountType", user.AccountType),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            _logger.LogInformation("User logged in: {Email}", user.Email);

            return user.AccountType.Trim() switch
            {
                "Admin" => RedirectToPage("/TestType/Index"),
                "Receptionist" => RedirectToPage("/TestEntry/Index"),
                "Accountant" => RedirectToPage("/Payment/Index"),
                _ => RedirectToPage("/Index")
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for email: {Email}", Input.Email);
            ErrorMessage = "An error occurred during login. Please try again.";
            return Page();
        }
    }
}
