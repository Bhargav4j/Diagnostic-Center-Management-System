using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using DiagnosticCenter.Domain.Interfaces.Repositories;

namespace DiagnosticCenter.Web.Pages;

public class IndexModel : PageModel
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(IUserRepository userRepository, ILogger<IndexModel> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    [BindProperty]
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = string.Empty;

    [BindProperty]
    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = string.Empty;

    public string? ErrorMessage { get; set; }

    public void OnGet()
    {
        var userEmail = HttpContext.Session.GetString("UserEmail");
        if (!string.IsNullOrEmpty(userEmail))
        {
            var accountType = HttpContext.Session.GetString("AccountType");
            RedirectToHomePage(accountType);
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            var user = await _userRepository.GetByEmailAsync(Email);

            if (user == null)
            {
                ErrorMessage = "Email or password is incorrect";
                return Page();
            }

            // Simple password comparison (in production, use proper password hashing)
            if (user.PasswordHash.Trim() != Password)
            {
                ErrorMessage = "Email or password is incorrect";
                return Page();
            }

            // Set session
            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetString("AccountType", user.AccountType);

            _logger.LogInformation("User logged in: {Email}", user.Email);

            return RedirectToHomePage(user.AccountType);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for email: {Email}", Email);
            ErrorMessage = "An error occurred during login. Please try again.";
            return Page();
        }
    }

    private IActionResult RedirectToHomePage(string? accountType)
    {
        return accountType switch
        {
            "Admin" => RedirectToPage("/TestSetup/Index"),
            "Receptionist" => RedirectToPage("/TestEntry/Index"),
            "Accountant" => RedirectToPage("/Payment/Index"),
            _ => Page()
        };
    }
}
