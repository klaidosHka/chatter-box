namespace ChatterBox.Interfaces.Services;

public interface IHelperService
{
    public string ResolveDirectChatId(string userId, string targetId);

    public string ResolveGroupChatId(string groupId);
}