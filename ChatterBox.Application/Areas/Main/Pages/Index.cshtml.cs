using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ChatterBox.Application.Areas.Main.Pages
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;

        public IndexModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public IEnumerable<UserViewModel> GetUsers()
        {
            return _userManager.Users
                .Select(u => new UserViewModel
                {
                    Username = u.UserName.Split('@', StringSplitOptions.RemoveEmptyEntries)[0],
                    Status = "Offline"
                })
                .ToList();
        }

        public async Task<IdentityUser> GetCurrentUserAsync()
        {
            return await _userManager.GetUserAsync(HttpContext.User);
        }
    }

    public class UserViewModel
    {
        public string Username { get; set; }

        public string Status { get; set; }
    }
}
