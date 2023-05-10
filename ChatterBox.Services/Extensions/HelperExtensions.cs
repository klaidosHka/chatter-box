using ChatterBox.Interfaces.Entities;
using Microsoft.AspNetCore.Identity;

namespace ChatterBox.Services.Extensions;

public static class HelperExtensions
{
    public static async Task<bool> IsOnlineAsync(this ChatUser chatUser, SignInManager<ChatUser> signInManager)
    {
        return (await signInManager.CreateUserPrincipalAsync(chatUser))?.Identity?.IsAuthenticated ?? false;
    }
}