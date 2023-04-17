using ChatterBox.Interfaces.Entities;
using ChatterBox.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ChatterBox.Application.Areas.Main.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IChatGroupMessageService _chatGroupMessageService;
        private readonly IChatMessageService _chatMessageService;
        private readonly SignInManager<ChatUser> _signInManager;
        private readonly UserManager<ChatUser> _userManager;

        public ChatUser? ChatUser { get; set; }

        public IndexModel(
            IChatGroupMessageService chatGroupMessageService,
            IChatMessageService chatMessageService,
            SignInManager<ChatUser> signInManager,
            UserManager<ChatUser> userManager
        )
        {
            _chatGroupMessageService = chatGroupMessageService;
            _chatMessageService = chatMessageService;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> CreateGroupMessageAsync(ChatGroupMessage message)
        {
            if (ModelState.IsValid)
            {
                var user = await GetCurrentUserAsync();

                message.SenderId = user.Id;

                await _chatGroupMessageService.ImportAsync(message);

                return Page();
            }

            return BadRequest();
        }

        public async Task<IActionResult> CreateMessageAsync(ChatMessage message)
        {
            if (ModelState.IsValid)
            {
                var user = await GetCurrentUserAsync();

                message.SenderId = user.Id;

                await _chatMessageService.ImportAsync(message);

                return Page();
            }

            return BadRequest();
        }

        public IQueryable<ChatMessage> GetChatMessages()
        {
            return _chatMessageService
                .GetMessagesAsNoTracking()
                .AsQueryable();
        }

        public async Task<ChatUser> GetCurrentUserAsync()
        {
            return await _userManager.GetUserAsync(User);
        }

        public IQueryable<UserViewModel> GetUsers()
        {
            return _userManager.Users
                .AsNoTracking()
                .ToList()
                .Select(u => new UserViewModel
                {
                    Online = _signInManager.IsSignedIn(_signInManager.CreateUserPrincipalAsync(u).Result),
                    UserName = u.UserName
                })
                .AsQueryable();
        }

        public async Task<bool> IsOnline(ChatUser user)
        {
            return _signInManager.IsSignedIn(await _signInManager.CreateUserPrincipalAsync(user));
        }
    }

    public class UserViewModel
    {
        public bool Online { get; set; }

        public string UserName { get; set; } = string.Empty;
    }
}
