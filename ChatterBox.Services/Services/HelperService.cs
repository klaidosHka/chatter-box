using ChatterBox.Interfaces.Services;

namespace ChatterBox.Services.Services;

public class HelperService : IHelperService
{
    private static readonly string ChatPrefixDirect = "drct__";
    private static readonly string ChatPrefixGroup = "grp__";
    private static readonly string ChatIdJoiner = "@";

    public string ResolveDirectChatId(string userId, string targetId)
    {
        return ChatPrefixDirect + String.Join(
            ChatIdJoiner,
            new[] { userId, targetId }
                .OrderBy(u => u)
                .ToArray()
        );
    }

    public string ResolveGroupChatId(string groupId)
    {
        return ChatPrefixGroup + groupId;
    }
}