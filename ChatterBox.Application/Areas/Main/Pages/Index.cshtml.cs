using ChatterBox.Interfaces.Dto;
using ChatterBox.Interfaces.Entities;
using ChatterBox.Interfaces.Services;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ChatterBox.Application.Areas.Main.Pages
{
    [IgnoreAntiforgeryToken]
    public class IndexModel : PageModel
    {
        private readonly IChatGroupService _chatGroupService;
        private readonly IChatGroupMessageService _chatGroupMessageService;
        private readonly IChatMessageService _chatMessageService;
        private readonly IChatUserService _chatUserService;
        private readonly IChatGroupRegistrarService _registrarService;
        private UserMapped? _currentUser;

        public UserMapped CurrentUser
        {
            get => _currentUser ??= _chatUserService.GetMappedAsync(User).Result;
            set => _currentUser = value;
        }

        public IndexModel(
            IChatGroupService chatGroupService,
            IChatGroupMessageService chatGroupMessageService,
            IChatMessageService chatMessageService,
            IChatUserService chatUserService,
            IChatGroupRegistrarService registrarService
        )
        {
            _chatGroupService = chatGroupService;
            _chatGroupMessageService = chatGroupMessageService;
            _chatMessageService = chatMessageService;
            _chatUserService = chatUserService;
            _registrarService = registrarService;
        }

        public IQueryable<ChatGroup> GetGroupsJoined()
        {
            return _chatGroupService
                .GetUserGroups(CurrentUser.User.Id)
                .AsQueryable();
        }

        public IQueryable<GroupMapped> GetGroups()
        {
            return _chatGroupService
                .GetMapped(CurrentUser.User.Id)
                .AsQueryable();
        }

        public IQueryable<UserMapped> GetUsers()
        {
            return _chatUserService
                .GetMapped()
                .OrderBy(u => u.Online)
                .ThenBy(u => u.User.UserName)
                .AsQueryable();
        }

        public void OnGet()
        {
            if (User is null || CurrentUser is null)
            {
                HttpContext.Response.Cookies.Append("ChatterBoxCookie", "", new CookieOptions
                {
                    Expires = DateTimeOffset.Now.AddMonths(-1)
                });

                HttpContext.Response.Redirect("/Index");
            }
            else if (!HttpContext.Request.GetDisplayUrl().EndsWith("Index", StringComparison.OrdinalIgnoreCase))
            {
                HttpContext.Response.Redirect("/Main/Index");
            }
        }

        public JsonResult OnGetGroupMessages(string groupId)
        {
            return new JsonResult(_chatGroupMessageService.GetMapped(groupId));
        }

        public JsonResult OnGetGroupUsers(string groupId)
        {
            if (String.IsNullOrEmpty(groupId))
            {
                return new JsonResult(
                    _chatUserService
                        .GetMapped()
                        .Select(u => new UserMappedBasic
                        {
                            AvatarLink = u.AvatarLink,
                            Id = u.User.Id,
                            Online = u.Online,
                            UserName = u.User.UserName
                        })
                        .ToList()
                );
            }

            return new JsonResult(
                _chatUserService
                    .GetWithinGroup(groupId)
                    .Select(u => new UserMappedBasic
                    {
                        AvatarLink = u.AvatarLink,
                        Id = u.User.Id,
                        Online = u.Online,
                        UserName = u.User.UserName
                    })
                    .ToList()
            );
        }

        public JsonResult OnGetMessages(string userId, string targetId)
        {
            return new JsonResult(_chatMessageService.GetMapped(userId, targetId));
        }

        public IActionResult OnPostCreateGroup(string groupName)
        {
            var request = new CreateGroupRequest
            {
                UserId = CurrentUser.User.Id,
                GroupName = groupName
            };

            var response = _chatGroupService.CreateAsync(request).Result;

            return StatusCode(response.Success ? 200 : 400, response);
        }

        public IActionResult OnPostDeleteGroup(string groupId)
        {
            return StatusCode(
                _chatGroupService.DeleteGroupAsync(groupId).Result
                    ? 200
                    : 400
            );
        }

        public IActionResult OnPostJoinGroup(string groupId)
        {
            return StatusCode(
                _registrarService.JoinGroupAsync(CurrentUser.User.Id, groupId).Result
                    ? 200
                    : 400
            );
        }

        public IActionResult OnPostLeaveGroup(string groupId)
        {
            return StatusCode(
                _registrarService.LeaveGroupAsync(CurrentUser.User.Id, groupId).Result
                    ? 200
                    : 400
            );
        }

        public IActionResult OnPostRenameGroup(string groupId, string name)
        {
            var response = _chatGroupService.RenameGroupAsync(groupId, name).Result;

            return StatusCode(response.Success ? 200 : 400, response);
        }
    }
}