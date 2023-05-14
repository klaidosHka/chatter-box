using System.Security.Claims;
using ChatterBox.Interfaces.Dto;
using ChatterBox.Interfaces.Entities;

namespace ChatterBox.Interfaces.Services;

public interface IChatUserService
{
    public IQueryable<ChatUser> Get();

    public IEnumerable<UserMapped> GetMapped();

    public IEnumerable<UserMapped> GetWithinGroup(string groupId);

    public Task<UserMapped> GetMappedAsync(ClaimsPrincipal principal);
}