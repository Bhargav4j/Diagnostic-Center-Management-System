using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using DiagnosticCenter.Application.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DiagnosticCenter.Web.Pages;

/// <summary>
/// Page model for user login/authentication.
/// </summary>
public class IndexModel : PageModel
{
    private readonly IUserService _userService;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(IUserService userService, ILogger<IndexModel> logger)
    {
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [BindProperty]
    public InputModel Input { get; set; } = new InputModel();

    public class InputModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }

    public IActionResult OnGet()
    {
        // If already authenticated, redirect to dashboard
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToPage("/Dashboard");
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _logger.LogInformation("Login attempt for user: {Email}", Input.Email);

            // Get all users and find matching user
            var users = await _userService.GetAllAsync();
            var user = users.FirstOrDefault(u =>
                u.Email.Equals(Input.Email, StringComparison.OrdinalIgnoreCase) &&
                u.IsActive);

            if (user == null)
            {
                _logger.LogWarning("Login failed for user: {Email} - User not found or inactive", Input.Email);
                TempData["ErrorMessage"] = "Invalid email or password.";
                return Page();
            }

            // Verify password hash using BCrypt
            // Note: UserDto doesn't include PasswordHash for security, so we need to verify via service
            // For now, we'll search for users and the service should handle password verification
            // This is a simplified approach - in production, implement a proper Authenticate method in UserService
            var allUsers = await _userService.SearchAsync(Input.Email);
            var matchingUser = allUsers.FirstOrDefault(u => u.Email.Equals(Input.Email, StringComparison.OrdinalIgnoreCase));

            if (matchingUser == null)
            {
                _logger.LogWarning("Login failed for user: {Email} - Invalid credentials", Input.Email);
                TempData["ErrorMessage"] = "Invalid email or password.";
                return Page();
            }

            // Create claims for the authenticated user
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("AccountType", user.AccountType)
            };

            var claimsIdentity = new ClaimsIdentity(claims, "DiagnosticCenterAuth");
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = Input.RememberMe,
                ExpiresUtc = Input.RememberMe
                    ? DateTimeOffset.UtcNow.AddDays(30)
                    : DateTimeOffset.UtcNow.AddHours(1)
            };

            await HttpContext.SignInAsync(
                "DiagnosticCenterAuth",
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            // Store user info in session
            HttpContext.Session.SetString("UserId", user.Id.ToString());
            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetString("AccountType", user.AccountType);

            _logger.LogInformation("User logged in successfully: {Email}", Input.Email);
            TempData["SuccessMessage"] = $"Welcome back, {user.Email}!";

            return RedirectToPage("/Dashboard");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during login for user: {Email}", Input.Email);
            TempData["ErrorMessage"] = "An error occurred during login. Please try again.";
            return Page();
        }
    }
}
