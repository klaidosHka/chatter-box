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
    private readonly IChatGroupRegistrarService _registrarService;
    private readonly SignInManager<ChatUser> _signInManager;
    private readonly UserManager<ChatUser> _userManager;

    public ChatUserService(
        IChatGroupRegistrarService registrarService,
        SignInManager<ChatUser> signInManager,
        UserManager<ChatUser> userManager
    )
    {
        _registrarService = registrarService;
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
            .Select(u => GetMappedAsync(_signInManager.CreateUserPrincipalAsync(u).Result).Result);
    }

    public IEnumerable<UserMapped> GetWithinGroup(string groupId)
    {
        return _registrarService
            .GetAsNoTracking()
            .Where(r => r.GroupId == groupId)
            .ToList()
            .Select(r => GetMappedAsync(_signInManager.CreateUserPrincipalAsync(r.User).Result).Result);
    }

    public async Task<UserMapped> GetMappedAsync(ClaimsPrincipal principal)
    {
        var avatar = principal.FindFirstValue("urn:google:picture");
        var user = await _userManager.GetUserAsync(principal);

        if (String.IsNullOrEmpty(avatar))
        {
            avatar = "/img/profile-circle.svg";
        }

        return new UserMapped
        {
            AvatarLink = avatar,
            Online = user is not null && ChatHub.IsOnline(user.Id),
            Principal = principal,
            User = user
        };
    }
}