﻿using ChatterBox.Interfaces.Dto;
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

        public IndexModel(
            IChatGroupService chatGroupService,
            IChatGroupMessageService chatGroupMessageService,
            IChatMessageService chatMessageService,
            IChatUserService chatUserService
        )
        {
            _chatGroupService = chatGroupService;
            _chatGroupMessageService = chatGroupMessageService;
            _chatMessageService = chatMessageService;
            _chatUserService = chatUserService;
        }

        public async Task<UserMapped> GetCurrentUserAsync()
        {
            return await _chatUserService.GetMappedAsync(User);
        }

        public IQueryable<ChatGroup> GetGroupsJoined()
        {
            return _chatGroupService
                .GetUserGroups(
                    GetCurrentUserAsync().Result.User.Id
                )
                .AsQueryable();
        }

        public IQueryable<GroupMapped> GetGroups()
        {
            return _chatGroupService
                .GetMapped(GetCurrentUserAsync().Result.User.Id)
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

        public async Task OnGetAsync()
        {
            if (User is null || await GetCurrentUserAsync() is null)
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
                UserId = GetCurrentUserAsync().Result.User.Id,
                GroupName = groupName
            };

            var response = _chatGroupService.CreateAsync(request).Result;

            return StatusCode(response.Success ? 200 : 400, response);
        }
    }
}