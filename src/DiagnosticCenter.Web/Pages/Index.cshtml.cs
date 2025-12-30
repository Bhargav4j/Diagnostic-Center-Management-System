using DiagnosticCenter.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace DiagnosticCenter.Web.Pages;

public class IndexModel : PageModel
{
    private readonly IAuthenticationService _authService;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(IAuthenticationService authService, ILogger<IndexModel> logger)
    {
        _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [BindProperty]
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email { get; set; } = string.Empty;

    [BindProperty]
    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = string.Empty;

    public string? ErrorMessage { get; set; }

    public IActionResult OnGet()
    {
        if (HttpContext.Session.GetString("UserEmail") != null)
        {
            var role = HttpContext.Session.GetString("UserRole");
            return RedirectToPage(GetHomePageByRole(role));
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            var (success, role, message) = await _authService.AuthenticateAsync(Email, Password);

            if (success && role != null)
            {
                HttpContext.Session.SetString("UserEmail", Email);
                HttpContext.Session.SetString("UserRole", role);

                _logger.LogInformation("User {Email} logged in successfully with role {Role}", Email, role);

                return RedirectToPage(GetHomePageByRole(role));
            }

            ErrorMessage = message ?? "Invalid email or password";
            _logger.LogWarning("Failed login attempt for {Email}", Email);
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for {Email}", Email);
            ErrorMessage = "An error occurred during login. Please try again.";
            return Page();
        }
    }

    private string GetHomePageByRole(string? role)
    {
        return role switch
        {
            "Admin" => "/TestSetup/Index",
            "Accountant" => "/Payment/Index",
            "Receptionist" => "/TestEntry/Index",
            _ => "/Index"
        };
    }
}
