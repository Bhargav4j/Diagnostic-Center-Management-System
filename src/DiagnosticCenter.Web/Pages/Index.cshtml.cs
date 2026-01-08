using DiagnosticCenter.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

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
            var result = await _authenticationService.AuthenticateAsync(Email, Password);

            if (result == null || !result.IsAuthenticated)
            {
                ErrorMessage = "Invalid email or password";
                return Page();
            }

            switch (result.AccountType.Trim())
            {
                case "Admin":
                    HttpContext.Session.SetString("admin", result.Email);
                    return RedirectToPage("/TestSetup/Index");
                case "Accountant":
                    HttpContext.Session.SetString("accountant", result.Email);
                    return RedirectToPage("/Payment/Index");
                case "Receptionist":
                    HttpContext.Session.SetString("receptionist", result.Email);
                    return RedirectToPage("/TestSetup/Index");
                default:
                    ErrorMessage = "Invalid account type";
                    return Page();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during authentication for email: {Email}", Email);
            ErrorMessage = "An error occurred during login. Please try again.";
            return Page();
        }
    }
}
