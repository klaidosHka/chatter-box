using System.Security.Claims;
using ChatterBox.Context;
using ChatterBox.Interfaces.Dto;
using ChatterBox.Interfaces.Entities;
using ChatterBox.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ChatterBox.Services.Services;

public class ChatUserService : IChatUserService
{
    private readonly SignInManager<ChatUser> _signInManager;
    private readonly UserManager<ChatUser> _userManager;

    public ChatUserService(SignInManager<ChatUser> signInManager, UserManager<ChatUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    public IQueryable<ChatUser> Get()
    {
        return _userManager.Users;
    }

    public IEnumerable<UserMapped> GetMapped()
    {
        return _userManager.Users
            .AsNoTracking()
            .ToList()
            .Select(u => new UserMapped
            {
                Online = ChatHub.IsOnline(u.Id),
                Principal = _signInManager.CreateUserPrincipalAsync(u).Result,
                User = u
            });
    }

    public async Task<UserMapped> GetMappedAsync(ClaimsPrincipal principal)
    {
        var user = await _userManager.GetUserAsync(principal);

        return new UserMapped
        {
            Online = ChatHub.IsOnline(user.Id),
            Principal = await _signInManager.CreateUserPrincipalAsync(user),
            User = user
        };
    }
}