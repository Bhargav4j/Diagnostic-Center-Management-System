using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DiagnosticCenter.Web.Pages.Admin;

[Authorize(Roles = "Admin")]
public class IndexModel : PageModel
{
    public void OnGet()
    {
    }
}
