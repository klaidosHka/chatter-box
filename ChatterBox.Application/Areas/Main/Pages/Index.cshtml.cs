using ChatterBox.Context;
using ChatterBox.Interfaces.Dto;
using ChatterBox.Interfaces.Entities;
using ChatterBox.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ChatterBox.Application.Areas.Main.Pages
{
    [IgnoreAntiforgeryToken]
    public class IndexModel : PageModel
    {
        private readonly IChatGroupService _chatGroupService;
        private readonly IChatGroupMessageService _chatGroupMessageService;
        private readonly IChatMessageService _chatMessageService;
        private readonly SignInManager<ChatUser> _signInManager;
        private readonly UserManager<ChatUser> _userManager;

        public IndexModel(
            IChatGroupService chatGroupService,
            IChatGroupMessageService chatGroupMessageService,
            IChatMessageService chatMessageService,
            SignInManager<ChatUser> signInManager,
            UserManager<ChatUser> userManager
        )
        {
            _chatGroupService = chatGroupService;
            _chatGroupMessageService = chatGroupMessageService;
            _chatMessageService = chatMessageService;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public IQueryable<ChatMessage> GetChatMessages()
        {
            return _chatMessageService
                .GetMessagesAsNoTracking()
                .AsQueryable();
        }

        public async Task<UserMapped> GetCurrentUserAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            
            return new UserMapped
            {
                Online = ChatHub.IsOnline(user.Id),
                Principal = await _signInManager.CreateUserPrincipalAsync(user),
                User = user
            };
        }

        public IQueryable<ChatGroup> GetGroups(bool joined)
        {
            return _chatGroupService
                .GetGroupsAsNoTracking()
                .AsQueryable();
        }

        public IQueryable<UserMapped> GetUsers()
        {
            return _userManager.Users
                .AsNoTracking()
                .ToList()
                .Select(u => new UserMapped
                {
                    Online = ChatHub.IsOnline(u.Id),
                    Principal = _signInManager.CreateUserPrincipalAsync(u).Result,
                    User = u
                })
                .AsQueryable();
        }

        public async Task OnGetAsync()
        {
            if (User is null || await GetCurrentUserAsync() is null)
            {
                HttpContext.Response.Cookies.Append("ChatterBoxCookie", "", new CookieOptions
                {
                    Expires = DateTimeOffset.Now.AddMonths(-1)
                });
                
                HttpContext.Response.Redirect("Index");
            }
        }

        public JsonResult OnGetMessages(string userId, string targetId)
        {
            return new JsonResult(
                _chatMessageService
                    .GetMessagesAsNoTracking()
                    .Where(m => 
                        m.SenderId == userId && m.ReceiverId == targetId ||
                        m.SenderId == targetId && m.ReceiverId == userId
                    )
                    .Select(m => new Message
                    {
                        DateSent = m.DateSent,
                        UserId = m.SenderId,
                        UserName = m.Sender.UserName,
                        Text = m.Text
                    })
                    .OrderBy(m => m.DateSent)
                    .ToList()
            );
        }
    }
}