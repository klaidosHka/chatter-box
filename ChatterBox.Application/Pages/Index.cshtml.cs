using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ChatterBox.Application.Pages
{
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                HttpContext.Response.Redirect("Main/Index");
            }
        }
    }
}