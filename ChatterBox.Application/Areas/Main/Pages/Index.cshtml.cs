using ChatterBox.Interfaces.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ChatterBox.Application.Areas.Main.Pages
{
    public class IndexModel : PageModel
    {
        private readonly SignInManager<ChatUser> _signInManager;
        private readonly UserManager<ChatUser> _userManager;

        public IndexModel(SignInManager<ChatUser> signInManager, UserManager<ChatUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public IQueryable<UserViewModel> GetUsers()
        {
            return _userManager.Users
                .AsNoTracking()
                .ToList()
                .Select(u => new UserViewModel
                {
                    Online = _signInManager.IsSignedIn(_signInManager.CreateUserPrincipalAsync(u).Result),
                    Username = u.UserName
                })
                .AsQueryable();
        }

        public async Task<IdentityUser> GetCurrentUserAsync()
        {
            return await _userManager.GetUserAsync(HttpContext.User);
        }
    }

    public class UserViewModel
    {
        public bool Online { get; set; }

        public string Username { get; set; }
    }
}
