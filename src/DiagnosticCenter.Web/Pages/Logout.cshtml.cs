using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DiagnosticCenter.Web.Pages;

/// <summary>
/// Page model for user logout.
/// </summary>
public class LogoutModel : PageModel
{
    private readonly ILogger<LogoutModel> _logger;

    public LogoutModel(ILogger<LogoutModel> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public IActionResult OnGet()
    {
        return RedirectToPage("/Index");
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            var userEmail = User.Identity?.Name ?? "Unknown";
            _logger.LogInformation("User logging out: {Email}", userEmail);

            // Sign out from authentication
            await HttpContext.SignOutAsync("DiagnosticCenterAuth");

            // Clear session
            HttpContext.Session.Clear();

            _logger.LogInformation("User logged out successfully: {Email}", userEmail);
            TempData["InfoMessage"] = "You have been successfully logged out.";

            return RedirectToPage("/Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during logout");
            TempData["ErrorMessage"] = "An error occurred during logout.";
            return RedirectToPage("/Index");
        }
    }
}
